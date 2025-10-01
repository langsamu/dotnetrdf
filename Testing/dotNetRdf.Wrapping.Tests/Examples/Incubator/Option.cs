using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Option : GraphWrapperNode
{
    protected Option(INode node, IGraph graph) : base(node, graph) { }

    internal static Option Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Option Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal Idea Idea => this.Singular(Vocabulary.hasIdea, Idea.Wrap);

    internal ISet<Pro> Pros => this.Objects(Vocabulary.hasPro, Pro.Wrap, Pro.Wrap);

    internal ISet<Contra> Contras => this.Objects(Vocabulary.hasCon, Contra.Wrap, Contra.Wrap);
}
