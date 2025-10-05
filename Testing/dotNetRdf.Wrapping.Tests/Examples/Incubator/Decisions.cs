using System;
using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Decisions : Typed
{
    protected Decisions(INode node, IGraph graph) : base(node, graph) { }

    internal static new Decisions Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Decisions Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Decisions Create(string uri,RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal ISet<Decision> Items => this.Objects(Vocabulary.hasDecision, Decision.Wrap, Decision.Wrap);
}
