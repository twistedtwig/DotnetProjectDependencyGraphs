using ProjectReferences.Models;

namespace ProjectReferences.Interfaces
{
    public interface IOutputProvider
    {
        /// <summary>
        /// Creates the output for the given Node
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        OutputResponse Create(RootNode rootNode, string outputFolder);
    }
}