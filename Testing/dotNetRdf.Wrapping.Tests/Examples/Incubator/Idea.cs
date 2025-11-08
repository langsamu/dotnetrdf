namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Idea : TitleAndDescriptionContainer
{
    protected Idea(INode node, IGraph graph) : base(node, graph) { }

    internal static new Idea Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Idea Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);
   
    internal static Idea Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
}
