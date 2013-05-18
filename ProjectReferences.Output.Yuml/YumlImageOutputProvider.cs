using System.IO;
using ProjectReferences.Interfaces;
using ProjectReferences.Models;
using ProjectReferences.Shared;

namespace ProjectReferences.Output.Yuml
{
    public class YumlImageOutputProvider : IOutputProvider
    {       
        public OutputResponse Create(RootNode rootNode, string outputFolder)
        {
            Logger.Log("Creating instance of YumlImageOutputProvider", LogLevel.High);

            var translator = new RootNodeToYumlClassDiagramTranslator();
            var yumlClassOutput = translator.Translate(rootNode, true);

            var serverImagePath = YumlHelper.GenerateImageOnYumlServer(yumlClassOutput);

            //have generated the image on the yuml server, now download the file.
            string basePath = Path.GetFullPath(outputFolder);
            var outputFileName = Path.Combine(basePath, rootNode.File.Name + ".png");
            YumlHelper.DownloadYumlServerImage(outputFileName, serverImagePath);
            
            return new OutputResponse { Success = true, Path = outputFileName };      
        }
    }
}