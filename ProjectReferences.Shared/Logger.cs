using System;
using System.IO;

namespace ProjectReferences.Shared
{
    public class Logger
    {
        private static int _MaxloggingLevel = (int) LogLevel.Low;
        private static LogType _logType = LogType.Console;
        private static string _folderLocation = string.Empty;
        private static string _fileLocation = "ProjectReferenceLog.log";


        public static void Log(string message, LogLevel logLevel = LogLevel.Low)
        {
            if ((int) logLevel <= _MaxloggingLevel)
            {
                switch (_logType)
                {
                    case LogType.Console:
                        Console.WriteLine(message);
                        break;
                    case LogType.TextFile:
                        string fullPath = Path.GetFullPath(Path.Combine(_folderLocation, _fileLocation));
                        File.AppendAllText(fullPath, message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }
        }

        public static void SetLogLevel(LogLevel logLevel)
        {
            _MaxloggingLevel = (int) logLevel;
        }

        public static void SetLogType(LogType logType)
        {
            _logType = logType;
        }

        public static void SetOutputFolderLocation(string location)
        {
            _folderLocation = location;
        }

        public static void SetOutputFileLocation(string location)
        {
            _fileLocation = location;
        }

        public static void SetupLogger(AnalysisRequest request)
        {
            SetLogLevel(request.LogLevel);
            SetLogType(request.LogType);

            SetOutputFolderLocation(!string.IsNullOrWhiteSpace(request.LogOutputFolderLocation) ? request.LogOutputFolderLocation : request.OutPutFolder);
            if (!string.IsNullOrWhiteSpace(request.LogOutputFileLocation))
            {
                SetOutputFileLocation(request.LogOutputFileLocation);
            }
        }
    }

    public enum LogLevel
    {
        Low = 1,
        High = 2,
    }

    public enum LogType
    {
        Console = 1,
        TextFile = 2,
    }
}
