using System;
using System.Text;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies.Output.YumlOutputTypes
{
    public class YumlGenerator
    {
        public static string Generate(ProjectReferenceMapCollection collection, bool newlines = true, bool stripWhiteSpace = false)
        {
            var builder = new StringBuilder();

            foreach (var mapping in collection.ProjectMappings)
            {
                builder.Append(string.Format("[{0}]->[{1}]",
                        stripWhiteSpace ? mapping.Parent.ToString().Replace(" ", "") : mapping.Parent.ToString(),
                        stripWhiteSpace ? mapping.Child.ToString().Replace(" ", "") : mapping.Child.ToString())
                    );

                if (newlines)
                {
                    builder.Append(Environment.NewLine);
                }
            }

            return builder.ToString();
        }

        public static string GenerateForPostEncoding(ProjectReferenceMapCollection collection)
        {
            string value = Generate(collection);

            value = value.Replace("[", "%5B");
            value = value.Replace("]", "%5D");
            value = value.Replace(">", "%3E");
            value = value.Replace(" ", "+");
            value = value.Replace("\r\n", "%2C+++++");
            
            return value;
        }

        public static string YumlUrl { get { return @"http://yuml.me/diagram/nofunky;/class/"; } }
    }
}