using Rubjerg.Graphviz;
using Rubjerg.Graphviz.Test.Linux;

namespace SE.Geospatial.Functions.Dynamic.Workflows.GraphHelpers
{
    public class GraphCreator : IGraphCreator
    {
        public IRootGraph FromDotString(string graph)
        {
            return new RootGraphHelper(RootGraph.FromDotString(graph));
        }
    }
}
