using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectReferenceAnalysis
    {
        public ProjectReferenceCollection FindProjectAndChildren(string projectFilePath, string[] excludeProjectsContaining, int totalDepth, int currentDepth)
        {
            string fullPath = GetProjectFullPath(projectFilePath);
            
            var collection = new ProjectReferenceCollection();
            collection.RootFile = fullPath;

            var proj = new ProjectReference(fullPath);
            collection.Projects.Add(proj);
            
            FindProjects(ProjectFolderPath(fullPath), proj, excludeProjectsContaining, totalDepth, currentDepth);

            return collection;
        }

        public void FindAllChildProjects(string parentFileLocation, ProjectReference projectReference, string[] excludeProjectsContaining, int totalDepth, int currentDepth)
        {
            FindProjects(ProjectFolderPath(parentFileLocation), projectReference, excludeProjectsContaining, totalDepth, currentDepth);
        }

        private void FindProjects(string parentFileLocation, ProjectReference projectReference, string[] excludeProjectsContaining, int totalDepth, int currentDepth)
        {
            string fullPath = GetProjectFullPath(projectReference.ProjectPath, parentFileLocation);
            Logger.Log("Finding all projects for: " + fullPath, LogLevel.High);

            if (!excludeProjectsContaining.Any(fullPath.Contains))
            {
                //Create an xml doc with correct namespace to analysis project file.
                XmlDocument doc = new XmlDocument();
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                nsMgr.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fullPath);

                //fill up all the extra information required for parent project reference
                projectReference.ProjectName = xmlDoc.SelectSingleNode(@"/msb:Project/msb:PropertyGroup/msb:AssemblyName", nsMgr).InnerText;
                projectReference.TargetFrameworkVersion = xmlDoc.SelectSingleNode(@"/msb:Project/msb:PropertyGroup/msb:TargetFrameworkVersion", nsMgr).InnerText;
                projectReference.Id = Guid.Parse(xmlDoc.SelectSingleNode(@"/msb:Project/msb:PropertyGroup/msb:ProjectGuid", nsMgr).InnerText);

                //get all child project references
                XmlNodeList projectReferences = xmlDoc.SelectNodes(@"/msb:Project/msb:ItemGroup/msb:ProjectReference", nsMgr);

                IList<ProjectReference> projectReferenceObjects = new List<ProjectReference>();

                foreach (XmlElement reference in projectReferences)
                {
                    string subProjectPath = reference.GetAttribute("Include");
                    var name = reference.SelectSingleNode("msb:Name", nsMgr).InnerText;
                    var id = reference.SelectSingleNode("msb:Project", nsMgr).InnerText;

                    var childProjectReference = new ProjectReference(new Guid(id), name, subProjectPath);
                    projectReference.Children.Add(childProjectReference);
                    projectReferenceObjects.Add(childProjectReference);
                }


                foreach (var projReference in projectReferenceObjects)
                {
                    if (totalDepth > currentDepth)
                    {
                        FindAllChildProjects(ProjectFolderPath(fullPath), projReference, excludeProjectsContaining, totalDepth, ++currentDepth);
                    }
                }
            }            
        }

        private string GetProjectFullPath(string path, string parentPath = "")
        {
            string fullPath = Path.GetFullPath(Path.Combine(parentPath, path));

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(fullPath);
            }

            return fullPath;
        }

        private string ProjectFolderPath(string path)
        {
            if (Directory.Exists(path))
            {
                return path;
            }

            if (File.Exists(path))
            {
                return new FileInfo(path).Directory.FullName;
            }

            throw new FileNotFoundException(path);
        }
    }
}