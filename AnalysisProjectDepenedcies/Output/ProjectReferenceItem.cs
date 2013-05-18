using System;
using System.Collections.Generic;

namespace AnalysisProjectDepenedcies.Output
{
    public class ProjectReferenceItem : IComparable<ProjectReferenceItem>
    {
        public ProjectReferenceItem()
        {
            ReferencedBy = new List<Guid>();
            References = new List<Guid>();
        }

        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ImageName { get; set; }

        public IList<Guid> ReferencedBy { get; protected set; }
        public IList<Guid> References { get; protected set; }


        public int CompareTo(ProjectReferenceItem other)
        {
            return Id.CompareTo(other.Id);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProjectReferenceItem;
            if (other == null) { return false; }
            return CompareTo(other) == 0;
        }
    }
}