namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Question : TitleAndDescriptionContainer
{
    protected Question(INode node, IGraph graph) : base(node, graph) { }

    internal static new Question Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Question Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Question Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
}
