namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Contra : MetricsContainer
{
    protected Contra(INode node, IGraph graph) : base(node, graph) { }

    internal static new Contra Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Contra Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Contra Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
}
