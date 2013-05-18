using System.IO;

namespace ProjectReferences.Models
{
    public class RootNode
    {
        public RootNode()
        {
            ProjectDetails = new ProjectDetailsCollection();
        }

        public RootNodeType NodeType { get; set; }
        public FileInfo File { get; set; }
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public int SearchDepth { get; set; }

        public ProjectDetailsCollection ProjectDetails { get; protected set; }
    }

    public enum RootNodeType
    {
        SLN = 1,
        CSPROJ = 2,
    }
}
