using System;
using System.Configuration;
using System.IO;
using ProjectReference;
using ProjectReferences.Models;
using ProjectReferences.Shared;

namespace ProjectReferences.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 
             * Example of command line args
             * 
             * -rootfile "D:\Work\Aerdata\StreamInteractive\Dev-2.6\Shared\Stream2.JobQueuePersistence\Stream2.JobQueuePersistence.csproj" -outputfolder "C:\temp\projectReferences" -outputeachitem true -outputtype YumlReferenceList -loglevel High
             * 
             * -rootfile "D:\Work\Aerdata\StreamInteractive\Dev-2.6\Shared\Stream2.JobQueuePersistence\Stream2.JobQueuePersistence.csproj" 
             * -outputfolder "C:\temp\projectReferences" 
             * -outputeachitem true 
             * -outputtype YumlReferenceList 
             * -loglevel High
             * 
             */


            Logger.Log("parsing args");
            AnalysisRequest request = new ParseCommandLineArgs().Process(GetAppSettingValues(), args);

            Logger.SetupLogger(request);            

            //set the current directory to ensure all relative file paths workout correctly.
            string dir = new FileInfo(request.RootFile).Directory.FullName;
            Logger.Log("setting current / working directory to: " + dir);
            Directory.SetCurrentDirectory(dir);

            Logger.Log("Creating project reference collection for root file: " + request.RootFile);
            RootNode rootNode = Manager.CreateRootNode(request);

            Logger.Log("Processing rootNode to fill all project references");
            Manager.Process(rootNode);

            Logger.Log("Creating output for rootNode", LogLevel.High);
            var outputResponse = Manager.CreateOutPut(request, rootNode);

            Logger.Log(string.Format("output creation result: {0}", outputResponse.Success));
            Logger.Log(string.Format("output creation path: {0}", outputResponse.Path));
        }

        private static AnalysisRequest GetAppSettingValues()
        {
            var request = new AnalysisRequest();

            var outputFolder = ConfigurationManager.AppSettings["OutputFolder"];
            if (!string.IsNullOrWhiteSpace(outputFolder))
            {
                request.LogOutputFolderLocation = outputFolder;
            }

            var outputFile = ConfigurationManager.AppSettings["OutputFile"];
            if (!string.IsNullOrWhiteSpace(outputFile))
            {
                request.LogOutputFileLocation = outputFile;
            }

            var loggerType = ConfigurationManager.AppSettings["LoggerType"];
            if (!string.IsNullOrWhiteSpace(loggerType))
            {
                if (Enum.IsDefined(typeof(LogType), loggerType))
                {
                    request.LogType = (LogType)Enum.Parse(typeof(LogType), loggerType, true);
                }
            }

            var loggerLevel = ConfigurationManager.AppSettings["LoggerLevel"];
            if (!string.IsNullOrWhiteSpace(loggerLevel))
            {
                if (Enum.IsDefined(typeof(LogLevel), loggerLevel))
                {
                    request.LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), loggerLevel, true);
                }
            }

            return request;
        }
    }
}
