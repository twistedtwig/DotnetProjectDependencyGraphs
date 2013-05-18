using System.Collections.Generic;
using System.IO;
using System.Text;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies.Output.YumlOutputTypes
{
    public class YumlUrl : IOutputStrategy
    {
        public void Generate(AnalysisRequest request, IList<ProjectReferenceMapCollection> collections)
        {
            foreach (var collection in collections)
            {
                string filePath = Path.Combine(Path.GetFullPath(request.OutPutFolder), "UrlReferences.yuml");
                File.WriteAllText(filePath, UrlBuilder(collection));    
            }            
        }

        public string UrlBuilder(ProjectReferenceMapCollection collection, bool newlines = true, bool stripWhiteSpace = false)
        {
            Logger.Log("building URL for: " + collection.RootFileName);
            var builder = new StringBuilder();
            builder.Append(YumlGenerator.YumlUrl);
            builder.Append(YumlGenerator.Generate(collection, newlines, stripWhiteSpace));
            return builder.ToString();
        }

    }
}