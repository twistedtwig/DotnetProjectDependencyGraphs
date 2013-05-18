using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class SolutionAnalysis
    {
        public ProjectReferenceCollection GetProjectReferencesFromSln(string filePath, string[] excludeProjectsContainingArray, int totalDepth)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            var solution = new Solution(filePath);
            var collection = new ProjectReferenceCollection();
            collection.RootFile = filePath;

            List<SolutionProject> projects = solution.Projects.Where(p => p.RelativePath.EndsWith(".csproj")).ToList();
            var projectReferenceAnalysis = new ProjectReferenceAnalysis();

            foreach (var project in projects)
            {
                var proj = new ProjectReference(Guid.Parse(project.ProjectGuid), project.ProjectName, project.RelativePath);
                collection.Projects.Add(proj);
                projectReferenceAnalysis.FindAllChildProjects(filePath, proj, excludeProjectsContainingArray, totalDepth, 0);                
            }

            return collection;
        } 
    }
}