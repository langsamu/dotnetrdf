using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Option : Typed
{
    protected Option(INode node, IGraph graph) : base(node, graph) { }

    internal static new Option Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Option Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Option Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
 
    internal Idea? Idea
    {
        get => this.Singular(Vocabulary.hasIdea, Idea.Wrap);

        set => this.OverwriteNullable(Vocabulary.hasIdea, value, Idea.Wrap);
    }

    internal ISet<Pro> Pros => this.Objects(Vocabulary.hasPro, Pro.Wrap, Pro.Wrap);

    internal ISet<Contra> Contras => this.Objects(Vocabulary.hasCon, Contra.Wrap, Contra.Wrap);
}
