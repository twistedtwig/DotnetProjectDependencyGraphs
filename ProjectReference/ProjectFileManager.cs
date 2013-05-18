using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ProjectReferences.Models;

namespace ProjectReference
{
    public class ProjectFileManager
    {
        /// <summary>
        /// Creates a project detail object from the link information provided.
        /// </summary>
        /// <param name="linkObject"></param>
        /// <returns></returns>
        public ProjectDetail Create(ProjectLinkObject linkObject)
        {
            return Create(linkObject.FullPath);
        }

        /// <summary>
        /// Creates a project detail object from the path to a CS project file.
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public ProjectDetail Create(string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException(fullFilePath);
            }

            var details = new ProjectDetail();
            details.FullPath = fullFilePath;

            DirectoryInfo projectDirectory = new FileInfo(fullFilePath).Directory;

            //Create an xml doc with correct namespace to analysis project file.
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fullFilePath);

            //fill up all the extra information required for parent project reference
            details.Name = xmlDoc.SelectSingleNode(@"/msb:Project/msb:PropertyGroup/msb:AssemblyName", nsMgr).InnerText;
            details.DotNetVersion = xmlDoc.SelectSingleNode(@"/msb:Project/msb:PropertyGroup/msb:TargetFrameworkVersion", nsMgr).InnerText;
            details.Id = Guid.Parse(xmlDoc.SelectSingleNode(@"/msb:Project/msb:PropertyGroup/msb:ProjectGuid", nsMgr).InnerText);

            //get all child project references
            XmlNodeList projectReferences = xmlDoc.SelectNodes(@"/msb:Project/msb:ItemGroup/msb:ProjectReference", nsMgr);
            IList<ProjectLinkObject> projectReferenceObjects = new List<ProjectLinkObject>();

            foreach (XmlElement reference in projectReferences)
            {
                string subProjectPath = reference.GetAttribute("Include");
                var id = reference.SelectSingleNode("msb:Project", nsMgr).InnerText;

                projectReferenceObjects.Add(new ProjectLinkObject { FullPath = Path.Combine(projectDirectory.FullName, subProjectPath), Id = Guid.Parse(id) });
            }

            details.ChildProjects.AddRange(projectReferenceObjects);
            return details;
        }
    }
}