namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Person : Typed
{
    protected Person(INode node, IGraph graph) : base(node, graph) { }

    internal static new Person Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Person Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Person Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
}
