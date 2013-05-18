using System;

namespace AnalysisProjectDepenedcies.ProjectAnalysis
{
    public class ProjectMapItem : IComparable<ProjectMapItem>
    {
        public ProjectMapItem(Guid id, string projectName, string projectPath, string targetFrameworkVersion)
        {
            Id = id;
            ProjectName = projectName;
            ProjectPath = projectPath;
            TargetFrameworkVersion = targetFrameworkVersion;
        }

        public Guid Id { get; protected set; }
        public string ProjectName { get; protected set; }
        public string ProjectPath { get; protected set; }
        public string TargetFrameworkVersion { get; protected set; }

        public int CompareTo(ProjectMapItem other)
        {
            return other == null ? 1 : Id.CompareTo(other.Id);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", ProjectName.StripIllegalCharacters(), TargetFrameworkVersion);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectMapItem) obj);
        }

        protected bool Equals(ProjectMapItem other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
