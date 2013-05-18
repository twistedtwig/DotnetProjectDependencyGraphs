using System.Collections.Generic;
using System.IO;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies.Output.YumlOutputTypes
{
    public class YumlReferenceFile : IOutputStrategy
    {
        public void Generate(AnalysisRequest request, IList<ProjectReferenceMapCollection> collections)
        {
            foreach (var collection in collections)
            {
                Logger.Log("Generating Yuml text file for: " + collection.RootFileName);
                string filePath = Path.Combine(Path.GetFullPath(request.OutPutFolder), Path.Combine(request.OutPutFolder, collection.RootFileName + ".yuml"));
                string parameters = YumlGenerator.Generate(collection);
                File.WriteAllText(filePath, parameters);
            }
        }
    }
}