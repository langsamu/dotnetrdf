namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal class Graph : WrapperGraph
{
    protected Graph(IGraph graph) : base(graph) { }

    internal static Graph Wrap(IGraph graph) => new(graph);

    internal Concept MyConcept(string uri) => Concept.Wrap(CreateUriNode(UriFactory.Create(uri)), this);
}
