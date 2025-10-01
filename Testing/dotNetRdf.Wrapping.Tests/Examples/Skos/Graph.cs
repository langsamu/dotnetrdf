namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal class Graph : WrapperGraph
{
    protected Graph(IGraph graph) : base(graph) { }

    internal static Graph Wrap(IGraph graph) => new(graph);

    internal Concept MyConcept(string uri) => Concept.Wrap(GetUriNode(UriFactory.Create(uri)), this);
}
