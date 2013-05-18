using System.Collections.Generic;

namespace ProjectReferences.Shared
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
            LogType = LogType.Console;
        }

        public string RootFile { get; set; }
        public int NumberOfLevelsToDig { get; set; }

        public IList<string> ExcludeNames { get; protected set; }
        public string OutPutFolder { get; set; }

        public bool CreateOutputForEachItem { get; set; }
        public OutPutType OutPutType { get; set; }

        public LogLevel LogLevel { get; set; }
        public LogType LogType { get; set; }
        public string LogOutputFolderLocation { get; set; }
        public string LogOutputFileLocation { get; set; }
    }
}
