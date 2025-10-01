namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Person : GraphWrapperNode
{
    protected Person(INode node, IGraph graph) : base(node, graph) { }

    internal static Person Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Person Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);
}
