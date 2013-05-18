using System;
using AnalysisProjectDepenedcies.Output.YumlOutputTypes;

namespace AnalysisProjectDepenedcies.Output
{
    public class OutputTypeFactory
    {
         
        public static IOutputStrategy Create(OutPutType type)
        {
            switch (type)
            {
                case OutPutType.YumlReferenceList:
                    return new YumlReferenceFile();
                case OutPutType.YumlUrl:
                    return new YumlUrl();
                case OutPutType.YumlImage:
                    return new YumlImage();
                    case OutPutType.HtmlDocument:
                    return new YumlHtmlDocumentGenerator();

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}