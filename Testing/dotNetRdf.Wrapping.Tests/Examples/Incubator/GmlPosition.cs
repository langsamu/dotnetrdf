using System.Linq;
using VDS.RDF.Nodes;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class GmlPosition : GraphWrapperNode
{
    protected GmlPosition(INode node, IGraph graph) : base(node, graph) { }

    internal static GmlPosition Wrap(INode node, IGraph graph) => new(node, graph);

    internal static GmlPosition Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal double X => double.Parse(this.AsValuedNode().AsString().Split(' ').First());

    internal double Y => double.Parse(this.AsValuedNode().AsString().Split(' ').Last());
}
