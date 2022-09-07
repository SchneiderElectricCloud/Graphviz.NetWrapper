using Rubjerg.Graphviz;
using Rubjerg.Graphviz.Test.Linux;

namespace SE.Geospatial.Functions.Dynamic.Workflows.GraphHelpers
{
    public class RootGraphHelper : IRootGraph
    {
        private readonly RootGraph _graph;

        public RootGraphHelper(RootGraph graph)
        {
            _graph = graph;
        }

        public INode GetNode(string name)
        {
            return new NodeHelper(_graph.GetNode(name));

        }
    }
}
