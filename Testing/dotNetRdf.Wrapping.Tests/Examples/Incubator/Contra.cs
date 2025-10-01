namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Contra : MetricsContainer
{
    protected Contra(INode node, IGraph graph) : base(node, graph) { }

    internal static Contra Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Contra Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);
}
