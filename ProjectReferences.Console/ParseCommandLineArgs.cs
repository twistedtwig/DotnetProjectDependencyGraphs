using System;
using ProjectReferences.Shared;

namespace ProjectReferences.Console
{
    class ParseCommandLineArgs
    {
        public AnalysisRequest Process(string[] args)
        {
            var request = new AnalysisRequest();
            return Process(request, args);
        }

        public AnalysisRequest Process(AnalysisRequest request, string[] args)
        {
            if (args.Length == 0)
            {
                return request;
            }

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].Trim().ToLower())
                {
                    case "-leveltodig":
                        if (args.Length > i + 1)
                        {
                            request.NumberOfLevelsToDig = int.Parse(args[i + 1]);
                        }
                        break;

                    case "-outputfolder":
                        if (args.Length > i + 1)
                        {
                            if (!string.IsNullOrWhiteSpace(args[i + 1]))
                            {
                                request.OutPutFolder = args[i + 1];
                            }
                        }
                        break;

                    case "-rootfile":
                        if (args.Length > i + 1)
                        {
                            if (!string.IsNullOrWhiteSpace(args[i + 1]))
                            {
                                request.RootFile = args[i + 1];
                            }
                        }
                        break;

                    case "-outputeachitem":
                        if (args.Length > i + 1)
                        {
                            request.CreateOutputForEachItem = bool.Parse(args[i + 1]);
                        }
                        break;

                    case "-outputtype":
                        if (args.Length > i + 1) 
                        {
                            if (Enum.IsDefined(typeof (OutPutType), args[i +1]))
                            {
                                request.OutPutType = (OutPutType)Enum.Parse(typeof(OutPutType), args[i + 1], true);
                            }
                        }
                        break;
                    case "-loglevel":
                        if (args.Length > i + 1) 
                        {
                            if (Enum.IsDefined(typeof (LogLevel), args[i +1]))
                            {
                                request.LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), args[i + 1], true);
                            }
                        }
                        break;
                    case "-logfolder":
                        if (args.Length > i + 1)
                        {
                            if (!string.IsNullOrWhiteSpace(args[i + 1]))
                            {
                                request.LogOutputFolderLocation = args[i + 1];
                            }
                        }
                        break;
                    case "-logfile":
                        if (args.Length > i + 1)
                        {
                            if (!string.IsNullOrWhiteSpace(args[i + 1]))
                            {
                                request.LogOutputFileLocation= args[i + 1];
                            }
                        }
                        break;
                    case "-logtype":
                        if (args.Length > i + 1)
                        {
                            if (Enum.IsDefined(typeof(LogType), args[i + 1]))
                            {
                                request.LogType = (LogType)Enum.Parse(typeof(LogType), args[i + 1], true);
                            }
                        }
                        break;
                }
            }

            //if the output type is OutPutType.HtmlDocument then it needs to produce png's for each item so set the output each item flag to true, regardless of params
            if (request.OutPutType == OutPutType.HtmlDocument)
            {
                request.CreateOutputForEachItem = true;
            }

            return request;
        }
    }
}
