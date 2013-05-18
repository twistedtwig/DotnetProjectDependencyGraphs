using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies.Output.YumlOutputTypes
{
    public class YumlImage : IOutputStrategy
    {
        public void Generate(AnalysisRequest request, IList<ProjectReferenceMapCollection> collections)
        {
            foreach (var collection in collections)
            {
                ProduceImage(request, collection);
            }
        }

        public string ProduceImage(AnalysisRequest request, ProjectReferenceMapCollection collection)
        {
            Logger.Log("Generating Yuml Post parameter string for project reference map collection with root node: " + collection.RootFileName);
            string parameters = YumlGenerator.GenerateForPostEncoding(collection);

            if (string.IsNullOrWhiteSpace(parameters))
            {
                return string.Empty;
            }

            WebRequest req = WebRequest.Create(YumlGenerator.YumlUrl);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = Encoding.ASCII.GetBytes("dsl_text=" + parameters);
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            WebResponse resp = req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string pngName = sr.ReadToEnd().Trim();

            //have generated the image on the yuml server, no download the file.
            string basePath = Path.GetFullPath(request.OutPutFolder);

            Logger.Log("downloading PNG for project reference map collection with root node: " + collection.RootFileName);
            var client = new WebClient();
            client.DownloadFile(YumlGenerator.YumlUrl + pngName, Path.Combine(basePath, collection.PngFileName));

            return collection.PngFileName;
        }
    }
}