using System;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectMapping : IComparable<ProjectMapping>
    {
        public ProjectMapping(ProjectMapItem parent, ProjectMapItem child)
        {
            Parent = parent;
            Child = child;
        }

        public ProjectMapItem Parent { get; protected set; }
        public ProjectMapItem Child { get; protected set; }

        public int CompareTo(ProjectMapping other)
        {
            if (Parent.Equals(other.Parent) && Child.Equals(other.Child))
            {
                return 0;
            }

            return 1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectMapping) obj);
        }

        protected bool Equals(ProjectMapping other)
        {
            return Equals(Parent, other.Parent) && Equals(Child, other.Child);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Parent != null ? Parent.GetHashCode() : 0) * 397) ^ (Child != null ? Child.GetHashCode() : 0);
            }
        }
    }
}
