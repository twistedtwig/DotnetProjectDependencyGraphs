using System;
using System.IO;
using ProjectReferences.Interfaces;
using ProjectReferences.Models;
using ProjectReferences.Shared;

namespace ProjectReferences.Output.Yuml
{
    public class YumlReferenceListOutputProvider : IOutputProvider
    {
        public OutputResponse Create(RootNode rootNode, string outputFolder)
        {
            Logger.Log("Creating instance of YumlReferenceListOutputProvider", LogLevel.High);

            var translator = new RootNodeToYumlClassDiagramTranslator();
            var yumlClassOutput = translator.Translate(rootNode, true);

            string filePath = Path.Combine(Path.GetFullPath(outputFolder), Path.Combine(outputFolder, rootNode.File.Name + ".yuml"));

            FileHandler.WriteToOutputFile(filePath, yumlClassOutput.ClassDiagram.ToString());

            return new OutputResponse { Success = true, Path = filePath };           
        }

    }
}