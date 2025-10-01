namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Pro : MetricsContainer
{
    protected Pro(INode node, IGraph graph) : base(node, graph) { }

    internal static Pro Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Pro Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);
}
