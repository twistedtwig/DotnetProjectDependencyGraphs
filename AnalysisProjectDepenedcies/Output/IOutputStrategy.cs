using System.Collections.Generic;
using AnalysisProjectDepenedcies.ProjectAnalysis;

namespace AnalysisProjectDepenedcies.Output
{
    public interface IOutputStrategy
    {
        void Generate(AnalysisRequest request, IList<ProjectReferenceMapCollection> collections);
    }
}