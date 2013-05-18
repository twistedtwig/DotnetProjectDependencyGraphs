using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectReferenceCollection
    {
        public ProjectReferenceCollection()
        {
            Projects = new List<ProjectReference>();    
        }

        public string RootFile { get; set; }
        public IList<ProjectReference> Projects { get; set; }


        public IEnumerable<ProjectReferenceCollection> CreateChildCollections()
        {
            IList<ProjectReferenceCollection> collections = new List<ProjectReferenceCollection>();

            foreach (var child in Projects)
            {
//                collections.AddRange(CreateChildCollections(child).ToList(), collections);

                CreateChildCollections(child, collections);
            }

            return collections;
        }
//
//        private IEnumerable<ProjectReferenceCollection> CreateChildCollections(ProjectReference item)
//        {
//            IList<ProjectReferenceCollection> collections = new List<ProjectReferenceCollection>();
//
//            foreach (var child in item.Children)
//            {
//                var kid = new ProjectReferenceCollection();
//                kid.Projects.Add(child);
//                kid.RootFile = child.ProjectPath;
//
//                CreateChildCollections(child);
//
//                collections.Add(kid);
//            }
//
//            return collections;
//        }

        private void CreateChildCollections(ProjectReference item, IList<ProjectReferenceCollection> masterCollections)
        {
            var referenceCollection = new ProjectReferenceCollection {RootFile = item.ProjectPath};
            //add self back in so that it is the root
            referenceCollection.Projects.Add(item);

            foreach (var child in item.Children)
            {
                referenceCollection.Projects.Add(child);
                CreateChildCollections(child, masterCollections);
            }
            masterCollections.Add(referenceCollection);

//
//            foreach (var child in item.Children)
//            {
//                var kid = new ProjectReferenceCollection();
//                kid.Projects.Add(child);
//                kid.RootFile = child.ProjectPath;
//
//                CreateChildCollections(child, masterCollections);
//
//                collections.Add(kid);
//            }

        }

        public ProjectReferenceCollection FlattenStructure()
        {
            var newMe = new ProjectReferenceCollection();
            newMe.RootFile = RootFile;
            FlattenList(Projects, newMe.Projects);

            return newMe;
        }

        private void FlattenList(IList<ProjectReference> originalProjects, IList<ProjectReference> flattenProjects)
        {
            foreach (var originalProject in originalProjects)
            {
                flattenProjects.Add(originalProject);
                FlattenList(originalProject.Children, flattenProjects);
            }
        }


        public ProjectReferenceMapCollection RemoveDuplicateRecords()
        {
            IList<ProjectMapping> mappings = new List<ProjectMapping>();

            if (!Projects.Any())
            {
                return null;
            }
            
            foreach (var projectReference in Projects)
            {
                SearchAllChildren(projectReference, mappings);
            }
            Guid projectId = FindProjectId(RootFile, Projects);
            if (Guid.Empty.Equals(projectId))
            {
                throw new ArgumentNullException("could not find project id for: " + RootFile);
            }
            
            return new ProjectReferenceMapCollection(RootFile, projectId, mappings);
        }

        private Guid FindProjectId(string rootFile, IList<ProjectReference> projects)
        {
            foreach (var project in projects)
            {
                if (rootFile.Equals(project.ProjectPath))
                {
                    return project.Id;
                }

                if (project.Children != null && project.Children.Any())
                {
                    return FindProjectId(rootFile, project.Children);
                }
            }

            return Guid.Empty;
        }


        private void SearchAllChildren(ProjectReference parent, IList<ProjectMapping> mappings)
        {
            var parentMapping = Convert(parent);

            if (parent.Children == null || !parent.Children.Any())
            {
                return;
            }

            foreach (var child in parent.Children)
            {
                var childMapping = Convert(child);

                var map = new ProjectMapping(parentMapping, childMapping);
                if (!mappings.Contains(map))
                {
                    mappings.Add(map);
                }

                SearchAllChildren(child, mappings);
            }
        }

        private ProjectMapItem Convert(ProjectReference project)
        {
            return new ProjectMapItem(project.Id, project.ProjectName, project.ProjectPath, project.TargetFrameworkVersion);
        }
    }
}
