using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal class ConceptScheme : GraphWrapperNode
{
    protected ConceptScheme(INode node, IGraph graph) : base(node, graph) { }

    internal static ConceptScheme Wrap(INode node, IGraph graph) => new(node, graph);

    internal static ConceptScheme Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static ConceptScheme Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal ISet<Concept> TopConcepts => this.Objects(Vocabulary.hasTopConcept, Concept.Wrap, Concept.Wrap);
}
