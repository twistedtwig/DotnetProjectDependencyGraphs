using System;
using System.Collections.Generic;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectReference
    {
        public ProjectReference(string projectPath)
        {
            ProjectPath = projectPath;
            Children = new List<ProjectReference>();       
        }

        public ProjectReference(Guid id, string projectName, string projectPath) : this(projectPath)
        {
            Id = id;
            ProjectName = projectName;
        }

        public ProjectReference(Guid id, string projectName, string projectPath, string targetFrameworkVersion) : this(id, projectName, projectPath)
        {            
            TargetFrameworkVersion = targetFrameworkVersion;
        }

        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string TargetFrameworkVersion { get; set; }

        public IList<ProjectReference> Children { get; set; }
    }
}