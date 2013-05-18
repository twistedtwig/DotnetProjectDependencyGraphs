using System;
using System.Collections.Generic;
using System.IO;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectReferenceMapCollection
    {
        public ProjectReferenceMapCollection(string rootFilePath, Guid projectId, IList<ProjectMapping> projectMappings)
        {
            RootFilePath = rootFilePath;
            ProjectId = projectId;
            ProjectMappings = projectMappings;

            if (!File.Exists(Path.GetFullPath(rootFilePath)))
            {
                throw new FileNotFoundException(rootFilePath);
            }

            RootFileName = new FileInfo(Path.GetFullPath(RootFilePath)).Name;
        }


        public IList<ProjectMapping> ProjectMappings { get; protected set; }
        public Guid ProjectId { get; protected set; }
        public string RootFilePath { get; set; }
        public string RootFileName { get; protected set; }
        public string PngFileName { get { return RootFileName + ".png"; } }
    }
}
