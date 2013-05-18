using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectReferences.Models;

namespace ProjectReference
{
    public class SolutionFileManager
    {
        /// <summary>
        /// Find projects that are referenced in a solution file and creates a list of references to them.
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public List<InvestigationLink> FindAllProjectLinks(RootNode rootNode)
        {
            var solution = new Solution(rootNode.File.FullName);

            var projects = solution.Projects.Where(p => p.RelativePath.EndsWith(".csproj")).ToList();
            var projectLinks = new List<InvestigationLink>(projects.Count);
            projectLinks.AddRange(
                from project in projects 
                let path = Path.Combine(rootNode.Directory.FullName, project.RelativePath)
                select new InvestigationLink { Parent = null, FullPath = path }
            );

            return projectLinks;
        }
    }
}
