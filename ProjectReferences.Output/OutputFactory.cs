using System;
using ProjectReferences.Interfaces;
using ProjectReferences.Output.Html;
using ProjectReferences.Output.Yuml;
using ProjectReferences.Shared;

namespace ProjectReferences.Output
{
    public class OutputFactory
    {
        public static IOutputProvider CreateProvider(OutPutType outputType)
        {
            Logger.Log(string.Format("Creating IOutputProvider for type: '{0}'", outputType));
            switch (outputType)
            {
                case OutPutType.YumlReferenceList:
                    return new YumlReferenceListOutputProvider();
                case OutPutType.YumlUrl:
                    return new YumlUrlOutputProvider();
                case OutPutType.YumlImage:
                    return new YumlImageOutputProvider();
                case OutPutType.HtmlDocument:
                    return new SinglePageHtmlDocumentOutputProvider();
                default:
                    throw new ArgumentOutOfRangeException("outputType");
            }
        }
    }
}
