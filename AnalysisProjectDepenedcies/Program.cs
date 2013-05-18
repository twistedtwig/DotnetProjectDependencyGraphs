using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("parsing args");
            AnalysisRequest request = new ParseCommandLineArgs().Process(args);

            Logger.SetLogLevel(request.LogLevel);
            
            //set the current directory to ensure all relative file paths workout correctly.
            string dir = new FileInfo(request.RootFile).Directory.FullName;
            Logger.Log("setting current / working directory to: " + dir);            
            Directory.SetCurrentDirectory(dir);

            Logger.Log("Creating project reference collection for root file: " + request.RootFile);
            IList<ProjectReferenceCollection> collections = new List<ProjectReferenceCollection>();
            collections.Add(ProjectAnaylsisFactory.Create(request));

            if (request.CreateOutputForEachItem)
            {
                Logger.Log("Creating project reference collection for each child project individually");
                collections.AddRange(collections[0].CreateChildCollections().ToList());
            }

            Logger.Log("outputting project references.");
            //have now got the project information, output the info

            Output.OutputTypeFactory.Create(request.OutPutType).Generate(request, collections.Select(collection => collection.RemoveDuplicateRecords()).ToList());            
        }

        




    }
}
