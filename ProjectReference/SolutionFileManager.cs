using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectReferences.Models;

namespace ProjectReference
{
    public class SolutionFileManager
    {

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
