namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Question : TitleAndDescriptionContainer
{
    protected Question(INode node, IGraph graph) : base(node, graph) { }

    internal static Question Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Question Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);
}
