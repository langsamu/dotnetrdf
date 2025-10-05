using System.Linq;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Graph : WrapperGraph
{
    protected Graph(IGraph graph) : base(graph) { }

    internal static Graph Wrap(IGraph graph) => new(graph);

    internal Decisions? MyDecisions() =>
        GetTriplesWithPredicateObject(
            Vocabulary.RdfType,
            CreateUriNode(UriFactory.Create("http://www.freshdecisions.org/decisions/Decisions")))
        .Select(static t => t.Subject)
        .In(this)
        .Select(Decisions.Wrap)
        .SingleOrDefault();
}
