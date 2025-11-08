namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

internal class Something : GraphWrapperNode
{
    protected Something(INode node, IGraph graph) : base(node, graph) { }

    internal static Something Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Something Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Something Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
}
