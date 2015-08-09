using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectReferences.Models
{
    public class ProjectDetail
    {
        public ProjectDetail()
        {
            ChildProjects = new List<ProjectLinkObject>();
            ParentProjects = new List<ProjectLinkObject>();
            References = new List<DllReference>();
        }

        public Guid Id { get; set; }
        public string FullPath { get; set; }
        public string DotNetVersion { get; set; }
        public string Name { get; set; }
        public List<ProjectLinkObject> ChildProjects { get; protected set; }
        public List<ProjectLinkObject> ParentProjects { get; protected set; }
        public List<DllReference> References { get; protected set; }


        public void AddParentLinks(List<ProjectLinkObject> parentLinks)
        {
            AddParentLinks(parentLinks.ToArray());    
        }

        public void AddParentLinks(params ProjectLinkObject[] parentLinks)
        {
            foreach (var link in parentLinks)
            {
                if (!ParentProjects.Any(p => p.Id.Equals(link.Id)))
                {
                    ParentProjects.Add(link);
                }
            }
        }
    }
}
