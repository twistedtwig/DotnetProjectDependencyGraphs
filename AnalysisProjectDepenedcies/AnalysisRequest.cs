using System.Collections.Generic;

namespace AnalysisProjectDepenedcies
{
    public class AnalysisRequest
    {
        public AnalysisRequest()
        {
            NumberOfLevelsToDig = int.MaxValue;
            OutPutFolder = @"C:\ProjectDependencies";
            ExcludeNames = new List<string>();
            CreateOutputForEachItem = false;
            OutPutType = OutPutType.YumlReferenceList;
            LogLevel = LogLevel.Low;
        }

        public string RootFile { get; set; }
        public int NumberOfLevelsToDig { get; set; }

        public IList<string> ExcludeNames { get; set; }
        public string OutPutFolder { get; set; }

        public bool CreateOutputForEachItem { get; set; }
        public OutPutType OutPutType { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}