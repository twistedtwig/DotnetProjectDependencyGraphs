using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies.Output.YumlOutputTypes
{
    public class YumlHtmlDocumentGenerator : IOutputStrategy
    {
        public void Generate(AnalysisRequest request, IList<ProjectReferenceMapCollection> collections)
        {
            //need to get all project reference items
                //need to generate a dedupped list of items that contains a list of everything it references and everything that references it
                //need to create an image for that item and populate property

            var referenceCollection = GenerateProjectReferenceItemCollection(request, collections);

            //once we have a unique list with all images created then generate HTML page containing all the links.
            GenerateHtmlPage(request, referenceCollection);
        }

        private void GenerateHtmlPage(AnalysisRequest request, ProjectReferenceItemCollection references)
        {
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
            
            builder.AppendLine(string.Format(@"<h1>All references for: {0}</h1>", request.RootFile));

            builder.AppendLine(@"<div id='accordian'>");

            foreach (var item in references.Items)
            {
                builder.AppendLine(string.Format(@"<h2>{0}</h2>", item.FileName));

                builder.AppendLine(string.Format(@"<div class='projectReference' id='{0}'>", item.Id.ToString()));

                if(!string.IsNullOrWhiteSpace(item.ImageName) && File.Exists(Path.Combine(request.OutPutFolder, item.ImageName)))
                {
                    builder.AppendLine(string.Format(@"<h2>{0} - <a href='{1}' target='_blank'>View Yuml Image</a></h2>", item.FileName, item.ImageName));
                }

                if (item.References.Any())
                {
                    builder.AppendLine(@"<p>This project references:</p>");
                    builder.AppendLine(@"<ul>");
                    foreach (var reference in item.References)
                    {
                        SetupReferenceItem(builder, reference, references);
                    }
                    builder.AppendLine(@"</ul>");    
                }
                else
                {
                    builder.AppendLine("<p>This project does not reference any other projects</p>");
                }

                if (item.ReferencedBy.Any())
                {
                    builder.AppendLine(@"<p>This project is referenced by:</p>");
                    builder.AppendLine(@"<ul>");
                    foreach (var reference in item.ReferencedBy)
                    {
                        SetupReferenceItem(builder, reference, references);
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

            File.WriteAllText(Path.Combine(request.OutPutFolder, "references.html"), builder.ToString());
        }

        private void SetupReferenceItem(StringBuilder builder, Guid linkId, ProjectReferenceItemCollection references)
        {
            ProjectReferenceItem item = references.Items.FirstOrDefault(i => i.Id.Equals(linkId));
            if (item == null)
            {
                throw new ArgumentNullException(linkId.ToString());
            }

            builder.AppendLine(string.Format(@"<li><a href='#{0}'>{1}</a></li>", item.Id.ToString(), item.FileName));
        }

        private ProjectReferenceItemCollection GenerateProjectReferenceItemCollection(AnalysisRequest request, IList<ProjectReferenceMapCollection> collections)
        {
            //get all project files, figure out what is referencing it and what it references.
            var referenceItemCollection = new ProjectReferenceItemCollection();

            foreach (var referenceMapCollection in collections)
            {
                if (referenceMapCollection == null || !referenceMapCollection.ProjectMappings.Any())
                {
                    Logger.Log("no mappings had been found so ignoring: " + referenceMapCollection.RootFileName, LogLevel.High);
                    continue;
                }

                foreach (var mapping in referenceMapCollection.ProjectMappings)
                {
                    var parentItem = SetupReferenceItem(referenceItemCollection, mapping.Parent);

                    if (!parentItem.References.Contains(mapping.Child.Id))
                    {
                        parentItem.References.Add(mapping.Child.Id);
                    }


                    var childItem = SetupReferenceItem(referenceItemCollection, mapping.Child);

                    if (!childItem.ReferencedBy.Contains(mapping.Parent.Id))
                    {
                        childItem.ReferencedBy.Add(mapping.Parent.Id);
                    }
                }
                
                var image = new YumlImage().ProduceImage(request, referenceMapCollection);
                if (string.IsNullOrWhiteSpace(image))
                {
                    throw new ArgumentNullException(image);
                }

                var item = referenceItemCollection.GetReferenceItem(referenceMapCollection.ProjectId);                
                item.ImageName = image;
            }

            return referenceItemCollection;
        }

        private ProjectReferenceItem SetupReferenceItem(ProjectReferenceItemCollection collection, ProjectMapItem mappingItem)
        {
            var item = collection.GetReferenceItem(mappingItem.Id) ?? new ProjectReferenceItem
            {
                Id = mappingItem.Id,
                FileName = mappingItem.ProjectName
            };

            collection.AddIfNewReferenceItem(item);

            return item;
        }
    }
}
