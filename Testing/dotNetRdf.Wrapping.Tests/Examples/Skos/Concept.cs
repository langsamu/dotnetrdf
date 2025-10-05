using System.Collections.Generic;
using System.Globalization;

namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal class Concept : GraphWrapperNode
{
    protected Concept(INode node, IGraph graph) : base(node, graph) { }

    internal static Concept Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Concept Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Concept Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal ConceptScheme? Scheme
    {
        get => this.Singular(Vocabulary.inScheme, ConceptScheme.Wrap);

        set => this.OverwriteNullable(Vocabulary.inScheme, value, ConceptScheme.Wrap);
    }

    internal ISet<Concept> Narrower => this.Objects(Vocabulary.narrower, Wrap, Wrap);

    internal ISet<Concept> Related => this.Objects(Vocabulary.related, Wrap, Wrap);

    internal IDictionary<CultureInfo, string> PreferredLabels => this.Dictionary(Vocabulary.prefLabel, NodeMappings.FromLangStringPair, ValueMappings.AsLangStringPair);

    internal IDictionary<CultureInfo, string> AlternativeLabels => this.Dictionary(Vocabulary.altLabel, NodeMappings.FromLangStringPair, ValueMappings.AsLangStringPair);

    internal IDictionary<CultureInfo, string> ScopeNotes => this.Dictionary(Vocabulary.scopeNote, NodeMappings.FromLangStringPair, ValueMappings.AsLangStringPair);
}
