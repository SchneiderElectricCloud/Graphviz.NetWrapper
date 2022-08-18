using FluentAssertions;
using System;
using System.Diagnostics;
using System.Linq;
//using NUnit.Framework;

namespace Rubjerg.Graphviz.Test
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<Pending>")]
    public class CGraphBasicOperations
    {
        public void TestReadDotFile()
        {
            RootGraph root = RootGraph.FromDotString(@"
digraph test {
    A;
    B;
    B -> B;
    A -> B[key = edgename];
    A -> B;
    A -> B;
}
");
            var edges = root.Edges().ToList();
            var names = edges.Select(e => e.GetName());
            // The attribute 'key' maps to the edgename
            names.Any(n => n == "edgename").Should().Be(true);
            names.All(n => n == "edgename" || string.IsNullOrEmpty(n)).Should().Be(true);

            // However, it is strange that the other two edges both seem to have the same name, namely ""
            // According to the documentation, the name is used to distinguish between multi-edges
            var A = root.GetNode("A");
            var B = root.GetNode("B");
            A.EdgesOut().Count().Should().Be(3);

            // The documentation seem to be correct for edges that are added through the C interface
            _ = root.GetOrAddEdge(A, B, "");
            A.EdgesOut().Count().Should().Be(4);
            _ = root.GetOrAddEdge(A, B, "");
            A.EdgesOut().Count().Should().Be(4);
            root.Dispose();
        }

        public void TestCopyAttributes()
        {
            RootGraph root = Utils.CreateUniqueTestGraph();
            Node n1 = root.GetOrAddNode("1");
            Node.IntroduceAttribute(root, "test", "foo");
            n1.GetAttribute("test").Should().Be("foo");
            n1.SetAttribute("test", "bar");
            n1.GetAttribute("test").Should().Be("bar");
            root.Dispose();

            RootGraph root2 = Utils.CreateUniqueTestGraph();
            Node n2 = root2.GetOrAddNode("2");

            n2.GetAttribute("test").Should().Be(null);
            n1.CopyAttributesTo(n2).Should().Be(0);
            n2.GetAttribute("test").Should().Be("bar");
            root2.Dispose();
        }

        public void TestDeletions()
        {
            RootGraph root = Utils.CreateUniqueTestGraph();

            Node tail = root.GetOrAddNode("1");
            Node head = root.GetOrAddNode("2");
            Node other = root.GetOrAddNode("3");

            Edge edge = root.GetOrAddEdge(tail, head, "edge");
            Edge tailout = root.GetOrAddEdge(tail, other, "tailout");
            Edge headout = root.GetOrAddEdge(head, other, "headout");
            Edge tailin = root.GetOrAddEdge(other, tail, "tailin");
            Edge headin = root.GetOrAddEdge(other, head, "headin");

            root.Equals(root.MyRootGraph).Should().Be(true);
            root.Equals(tail.MyRootGraph).Should().Be(true);
            root.Equals(edge.MyRootGraph).Should().Be(true);

            tail.TotalDegree().Should().Be(3);
            head.TotalDegree().Should().Be(3);
            root.Nodes().Count().Should().Be(3);

            root.Delete(edge);


            tail.TotalDegree().Should().Be(2);
            head.TotalDegree().Should().Be(2);
            root.Nodes().Count().Should().Be(3);

            root.Delete(tail);

            root.Nodes().Count().Should().Be(2);
            other.TotalDegree().Should().Be(2);
            root.Dispose();
        }

        public void TestNodeMerge()
        {
            RootGraph root = Utils.CreateUniqueTestGraph();
            Node merge = root.GetOrAddNode("merge");
            Node target = root.GetOrAddNode("target");
            Node other = root.GetOrAddNode("other");

            Edge selfloop = root.GetOrAddEdge(merge, merge, "selfloop");
            Edge contracted = root.GetOrAddEdge(merge, target, "contracted");
            Edge counter = root.GetOrAddEdge(target, merge, "counter");
            Edge mergeout = root.GetOrAddEdge(merge, other, "mergeout");
            Edge targetout = root.GetOrAddEdge(target, other, "targetout");
            Edge mergein = root.GetOrAddEdge(other, merge, "mergein");
            Edge targetin = root.GetOrAddEdge(other, target, "targetin");

            merge.TotalDegree().Should().Be(6);
            target.TotalDegree().Should().Be(4);
            root.Nodes().Count().Should().Be(3);

            //root.ComputeDotLayout();
            //root.ToSvgFile("dump1.svg");
            //root.FreeLayout();
            //root.ToDotFile("dump1.dot");

            root.Merge(merge, target);

            //root.ComputeDotLayout();
            //root.ToSvgFile("dump2.svg");
            //root.FreeLayout();
            //root.ToDotFile("dump2.dot");


            root.Nodes().Count().Should().Be(2);
            target.InDegree().Should().Be(3);
            target.OutDegree().Should().Be(3);
            other.InDegree().Should().Be(2);
            other.OutDegree().Should().Be(2);
            root.Dispose();
        }

        public void TestEdgeContraction()
        {
            //NativeMethods.AllocConsole();
            RootGraph root = Utils.CreateUniqueTestGraph();
            Node tail = root.GetOrAddNode("x");
            Node head = root.GetOrAddNode("xx");
            Node other = root.GetOrAddNode("xxx");

            Edge contracted = root.GetOrAddEdge(tail, head, "tocontract");
            Edge parallel = root.GetOrAddEdge(tail, head, "parallel");
            Edge counterparallel = root.GetOrAddEdge(head, tail, "counterparallel");
            Edge tailout = root.GetOrAddEdge(tail, other, "tailout");
            Edge headout = root.GetOrAddEdge(head, other, "headout");
            Edge tailin = root.GetOrAddEdge(other, tail, "tailin");
            Edge headin = root.GetOrAddEdge(other, head, "headin");

            foreach (Node n in root.Nodes())
            {
                n.SafeSetAttribute("label", n.GetName(), "no");
                n.SafeSetAttribute("fontname", "Arial", "Arial");
                foreach (Edge e in n.EdgesOut())
                {
                    e.SafeSetAttribute("label", e.GetName(), "no");
                    e.SafeSetAttribute("fontname", "Arial", "Arial");
                }
            }


            tail.TotalDegree().Should().Be(5);
            head.TotalDegree().Should().Be(5);
            root.Nodes().Count().Should().Be(3);

            Node contraction = root.Contract(contracted, "contraction result");

            foreach (Node n in root.Nodes())
            {
                n.SafeSetAttribute("label", n.GetName(), "no");
                n.SafeSetAttribute("fontname", "Arial", "Arial");
                foreach (Edge e in n.EdgesOut())
                {
                    e.SafeSetAttribute("label", e.GetName(), "no");
                    e.SafeSetAttribute("fontname", "Arial", "Arial");
                }
            }


            root.Nodes().Count().Should().Be(2);
            contraction.InDegree().Should().Be(2);
            contraction.OutDegree().Should().Be(2);
            other.InDegree().Should().Be(2);
            other.OutDegree().Should().Be(2);
            //Console.Read();
            
            root.Dispose();
        }

    }
}
