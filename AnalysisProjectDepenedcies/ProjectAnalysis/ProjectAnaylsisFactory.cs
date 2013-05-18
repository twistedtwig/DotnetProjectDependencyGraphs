using System;
using System.Linq;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectAnaylsisFactory
    {
        public static ProjectReferenceCollection Create(AnalysisRequest request)
        {
            if (request.RootFile.EndsWith(".sln"))
            {
                Logger.Log("Creating ProjectReferenceCollection for a solution file: " + request.RootFile, LogLevel.High);
                return new SolutionAnalysis().GetProjectReferencesFromSln(request.RootFile, request.ExcludeNames.ToArray(), request.NumberOfLevelsToDig);
            }
            
            if (request.RootFile.EndsWith(".csproj"))
            {
                Logger.Log("Creating ProjectReferenceCollection for a CS project file: " + request.RootFile, LogLevel.High); 
                return new ProjectReferenceAnalysis().FindProjectAndChildren(request.RootFile, request.ExcludeNames.ToArray(), request.NumberOfLevelsToDig, 0);
            }

            throw new ArgumentOutOfRangeException(request.RootFile);
        }
    }
}