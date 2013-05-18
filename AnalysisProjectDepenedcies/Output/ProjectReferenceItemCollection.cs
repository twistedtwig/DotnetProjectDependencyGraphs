using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisProjectDepenedcies.Output
{
    public class ProjectReferenceItemCollection
    {
        public ProjectReferenceItemCollection()
        {
            Items = new List<ProjectReferenceItem>();
        }

        public IList<ProjectReferenceItem> Items { get; protected set; } 

        public bool ContainsReferenceItem(Guid referenceItemId)
        {
            return Items.Any(i => i.Id.Equals(referenceItemId));
        }

        public ProjectReferenceItem GetReferenceItem(Guid referenceItemId)
        {
            return Items.FirstOrDefault(i => i.Id.Equals(referenceItemId));
        }

        public void AddIfNewReferenceItem(ProjectReferenceItem item)
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
            }
        }
    }
}