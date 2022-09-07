using System.Collections.Generic;
using System.Linq;
using Rubjerg.Graphviz;
using Rubjerg.Graphviz.Test.Linux;

namespace SE.Geospatial.Functions.Dynamic.Workflows.GraphHelpers
{
    public class NodeHelper : INode
    {
        private readonly Node _node;

        public NodeHelper(Node node)
        {
            _node = node;
        }

        public IDictionary<string, string> GetAttributes()
        {
            return _node.GetAttributes();
        }

        public string GetName()
        {
            return _node.GetName();
        }

        public IEnumerable<IEdge> EdgesOut()
        {
            return _node.EdgesOut().Select(p => new EdgeHelper(p));
        }
    }
}
