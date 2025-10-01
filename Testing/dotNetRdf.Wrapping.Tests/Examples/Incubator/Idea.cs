namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Idea : TitleAndDescriptionContainer
{
    protected Idea(INode node, IGraph graph) : base(node, graph) { }

    internal static Idea Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Idea Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);
}
