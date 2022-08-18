using System.Collections.Generic;
using Rubjerg.Graphviz;
using Rubjerg.Graphviz.Test.Linux;

namespace SE.Geospatial.Functions.Dynamic.Workflows.GraphHelpers
{
    public class EdgeHelper : IEdge
    {
        private readonly Edge _edge;

        public EdgeHelper(Edge edge)
        {
            _edge = edge;
        }

        public IDictionary<string, string> GetAttributes()
        {
            return _edge.GetAttributes();
        }

        public string GetName()
        {
            return _edge.GetName();
        }

        public INode Head()
        {
            return new NodeHelper(_edge.Head());
        }
    }
}
