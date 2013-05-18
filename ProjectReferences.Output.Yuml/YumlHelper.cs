
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ProjectReferences.Output.Yuml.Models;
using ProjectReferences.Shared;

namespace ProjectReferences.Output.Yuml
{
    public class YumlHelper
    {
        public static string EncodeForHttpPost(string message)
        {
            var value = message.Trim();

            value = Regex.Replace(value, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            value = value.Replace("[", "%5B");
            value = value.Replace("]", "%5D");
            value = value.Replace(">", "%3E");
            value = value.Replace(" ", "+");
            value = value.Replace(":", "%3A");
            value = value.Replace(";", "%3B");
            value = value.Replace("|", "%7C");
            value = value.Replace("\r\n", "%2C+");

            return value;
        }

        public static string YumlClassUrl { get { return @"http://yuml.me/diagram/nofunky;/class/"; } }

        public static string ReplaceSpaces(string url)
        {
            return url.Replace(" ", "%20");
        }

        public static string CommaSeperateRelationshipsOnMultipleLines(string relationshsipTree)
        {
            var tree = relationshsipTree.Replace(Environment.NewLine, ",");
            if (tree.EndsWith(","))
            {
                tree = tree.Substring(0, tree.Length - 1);
            }
            return tree;
        }

        /// <summary>
        /// Generates a document on the server and returns the full url to it.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static string GenerateImageOnYumlServer(YumlClassOutput output)
        {
            Logger.Log(string.Format("Generating image on yuml server, class diagram: '{0}'", output.ClassDiagram), LogLevel.High);

            ServicePointManager.Expect100Continue = false;
            WebRequest req = WebRequest.Create(YumlClassUrl);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = Encoding.ASCII.GetBytes("dsl_text=" + EncodeForHttpPost(output.ClassDiagram.ToString()));
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            WebResponse resp = req.GetResponse();

            var sr = new StreamReader(resp.GetResponseStream());
            string pngName = sr.ReadToEnd().Trim();

            var imageOnYumlServer = YumlClassUrl + pngName;
            Logger.Log(string.Format("image has been generated on server with url of :'{0}'", imageOnYumlServer), LogLevel.High);
            return imageOnYumlServer;
        }


        public static void DownloadYumlServerImage(string outputFileName, string url)
        {
            Logger.Log(string.Format("Downloading image from '{0}' saving to '{1}'", url, outputFileName), LogLevel.High);
            FileHandler.EnsureFolderExistsForFullyPathedLink(outputFileName);

            var client = new WebClient();
            client.DownloadFile(url, outputFileName);
        }
    }
}
