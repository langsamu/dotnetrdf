using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal class ConceptScheme : GraphWrapperNode
{
    protected ConceptScheme(INode node, IGraph graph) : base(node, graph) { }

    internal static ConceptScheme Wrap(INode node, IGraph graph) => new(node, graph);

    internal static ConceptScheme Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal ISet<Concept> TopConcepts => this.Objects(Vocabulary.hasTopConcept, Concept.Wrap, Concept.Wrap);
}
