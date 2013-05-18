using System;
using System.IO;
using System.Linq;
using System.Text;
using ProjectReferences.Interfaces;
using ProjectReferences.Models;
using ProjectReferences.Output.Yuml;
using ProjectReferences.Shared;

namespace ProjectReferences.Output.Html
{
    public class SinglePageHtmlDocumentOutputProvider : IOutputProvider
    {
        public OutputResponse Create(RootNode rootNode, string outputFolder)
        {
            Logger.Log("Creating instance of SinglePageHtmlDocumentOutputProvider", LogLevel.High);
            
            var translator = new RootNodeToYumlClassDiagramTranslator();
            var yumlClassOutput = translator.Translate(rootNode, true);


            var builder = new StringBuilder();

            builder.AppendLine(@"<html>");
            builder.AppendLine(@"<head>");
            builder.AppendLine(@"<script src='http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js'></script>");
            builder.AppendLine(@"<script src='http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/jquery-ui.min.js'></script>");
            builder.AppendLine(@"<link rel='stylesheet' type='text/css' href='http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/themes/eggplant/jquery-ui.css' />");


            builder.AppendLine(@"
                <script>
                    $(document).ready(function() {
                        $('#accordian').accordion();

                        $('li').click(function() {
                            $('#accordian').accordion( 'option', 'active', $('#accordian .projectReference').index($($(this).find('a').attr('href'))));                            
                            return false;
                        });

                    });
                </script>"
            );

            builder.AppendLine(@"</head>");
            builder.AppendLine(@"<body>");

            builder.AppendLine(string.Format(@"<h1>All references for: {0}</h1>", yumlClassOutput.RootFile));

            var rootNodeOutputFileName = ConvertRootFileFullPathIntoOutputPngFileName(outputFolder, yumlClassOutput.RootFile);
            YumlHelper.DownloadYumlServerImage(rootNodeOutputFileName, YumlHelper.GenerateImageOnYumlServer(yumlClassOutput));
            builder.AppendLine(String.Format(@"<p>Image for whole reference list: <a href='{0}' target='_blank'> View Yuml Image</a></p>", rootNodeOutputFileName));

            builder.AppendLine(@"<div id='accordian'>");


            //then for each project details item in the collection need to generate an image and list of links that it references and what references it

            foreach (var projectDetail in rootNode.ProjectDetails)
            {
                Logger.Log(string.Format("generating HTML output for projectDetail: '{0}'", projectDetail.FullPath), LogLevel.High);

                var projectOutput = translator.Translate(projectDetail, rootNode.ProjectDetails, true);

                builder.AppendLine(string.Format(@"<h2>{0}</h2>", Path.GetFileName(projectOutput.RootFile)));
                builder.AppendLine(string.Format(@"<div class='projectReference' id='{0}'>", projectDetail.Id));
                
                if (!string.IsNullOrWhiteSpace(projectOutput.ClassDiagram.ToString()))
                {
                    var projectOutputFileName = ConvertRootFileFullPathIntoOutputPngFileName(outputFolder, projectOutput.RootFile);
                    YumlHelper.DownloadYumlServerImage(projectOutputFileName, YumlHelper.GenerateImageOnYumlServer(projectOutput));
                    builder.AppendLine(string.Format(@"<h2>{0} - <a href='{1}' target='_blank'>View Yuml Image</a></h2>", Path.GetFileName(projectOutput.RootFile), projectOutputFileName));
                }               

                if (projectDetail.ChildProjects.Any())
                {
                    builder.AppendLine(@"<p>This project references:</p>");
                    builder.AppendLine(@"<ul>");
                    foreach (var reference in projectDetail.ChildProjects)
                    {
                        builder.AppendLine(string.Format(@"<li><a href='#{0}'>{1}</a></li>", reference.Id, Path.GetFileName(reference.FullPath)));
                    }
                    builder.AppendLine(@"</ul>");
                }
                else
                {
                    builder.AppendLine("<p>This project does not reference any other projects</p>");
                }

                if (projectDetail.ParentProjects.Any())
                {
                    builder.AppendLine(@"<p>This project is referenced by:</p>");
                    builder.AppendLine(@"<ul>");
                    foreach (var reference in projectDetail.ParentProjects)
                    {
                        builder.AppendLine(string.Format(@"<li><a href='#{0}'>{1}</a></li>", reference.Id, Path.GetFileName(reference.FullPath)));
                    }
                    builder.AppendLine(@"</ul>");
                }
                else
                {
                    builder.AppendLine("<p>This project is not referenced by any other projects</p>");
                }

                builder.AppendLine(@"</div>");
            }

            builder.AppendLine(@"</div>");

            builder.AppendLine(@"</body>");
            builder.AppendLine(@"</html>");

            var htmlOutputFilePath = Path.Combine(outputFolder, "references.html");
            File.WriteAllText(htmlOutputFilePath, builder.ToString());

            return new OutputResponse { Path = htmlOutputFilePath, Success = true };
        }

        private string ConvertRootFileFullPathIntoOutputPngFileName(string outputFolder, string rootFile)
        {
            return Path.Combine(outputFolder, Path.GetFileName(rootFile)) + ".png";
        }
    }
}
