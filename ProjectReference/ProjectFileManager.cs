using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public ProjectDetail Create(ProjectLinkObject linkObject, bool includeExternalReferences)
        {
            return Create(linkObject.FullPath, includeExternalReferences);
        }

        /// <summary>
        /// Creates a project detail object from the path to a CS project file.
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public ProjectDetail Create(string fullFilePath, bool includeExternalReferences)
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

            if (includeExternalReferences)
            {
                //get all dll references
                XmlNodeList dllReferences = xmlDoc.SelectNodes(@"/msb:Project/msb:ItemGroup/msb:Reference[not(starts-with(@Include,'System')) and not(starts-with(@Include,'Microsoft.'))]", nsMgr);
                IList<DllReference> dllReferenceObjects = new List<DllReference>();

                if (dllReferences != null)
                {
                    foreach (XmlElement reference in dllReferences)
                    {
                        var include = reference.GetAttribute("Include");
                        var version = reference.GetAttribute("Version");

                        //if not version stored as XML then try and get the hint path.
                        if (string.IsNullOrWhiteSpace(version))
                        {
                            var inner = reference.InnerXml;
                            if (!string.IsNullOrWhiteSpace(inner) && inner.StartsWith("<HintPath"))
                            {
                                var innerHintPathXml = new XmlDocument();
                                innerHintPathXml.LoadXml(inner);

                                var relativehintPath = innerHintPathXml.InnerText;
                                var csprojFile = new FileInfo(fullFilePath);

                                var directory = csprojFile.Directory.FullName  + (relativehintPath.StartsWith(@"\") ? "" : @"\");
                                var dllPath = Path.GetFullPath(directory + relativehintPath);
                                var dllFile = new FileInfo(dllPath);
                                if (dllFile.Exists)
                                {
                                    Assembly assembly = Assembly.LoadFrom(dllFile.FullName);
                                    Version ver = assembly.GetName().Version;
                                    version = ver.ToString();
                                }

                            }
                        }

                        dllReferenceObjects.Add(new DllReference { AssemblyName = include.Split(',')[0], Version = version });
                    }

                    details.References.AddRange(dllReferenceObjects);                    
                }                
            }            

            return details;
        }
    }
}
