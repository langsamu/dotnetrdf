namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Pro : MetricsContainer
{
    protected Pro(INode node, IGraph graph) : base(node, graph) { }

    internal static new Pro Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Pro Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Pro Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
}
