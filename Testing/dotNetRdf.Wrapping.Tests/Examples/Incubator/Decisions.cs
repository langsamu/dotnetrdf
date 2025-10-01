using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Decisions : GraphWrapperNode
{
    protected Decisions(INode node, IGraph graph) : base(node, graph) { }

    internal static Decisions Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Decisions Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal ISet<Decision> Items => this.Objects(Vocabulary.hasDecision, Decision.Wrap, Decision.Wrap);
}
