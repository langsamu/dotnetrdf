using System.Collections.Generic;
using System.Globalization;

namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal class Concept : GraphWrapperNode
{
    protected Concept(INode node, IGraph graph) : base(node, graph) { }

    internal static Concept Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Concept Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal ConceptScheme Scheme => this.Singular(Vocabulary.inScheme, ConceptScheme.Wrap);

    internal ISet<Concept> Narrower => this.Objects(Vocabulary.narrower, Wrap, Wrap);

    internal ISet<Concept> Related => this.Objects(Vocabulary.related, Wrap, Wrap);

    internal IDictionary<CultureInfo, string> PreferredLabels => this.Dictionary(Vocabulary.prefLabel, NodeMappings.FromLangStringPair, ValueMappings.AsLangStringPair);

    internal IDictionary<CultureInfo, string> AlternativeLabels => this.Dictionary(Vocabulary.altLabel, NodeMappings.FromLangStringPair, ValueMappings.AsLangStringPair);

    internal IDictionary<CultureInfo, string> ScopeNotes => this.Dictionary(Vocabulary.scopeNote, NodeMappings.FromLangStringPair, ValueMappings.AsLangStringPair);
}
