namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Graph : WrapperGraph
{
    protected Graph(IGraph graph) : base(graph) { }

    internal static Graph Wrap(IGraph graph) => new(graph);

    internal Decisions MyDecisions(string uri) => Decisions.Wrap(GetUriNode(UriFactory.Create(uri)), this);
}
