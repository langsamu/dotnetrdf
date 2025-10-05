using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Options : Typed
{
    protected Options(INode node, IGraph graph) : base(node, graph) { }

    internal static new Options Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Options Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Options Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal ISet<Option> Items => this.Objects(Vocabulary.hasOption, Option.Wrap, Option.Wrap);
}
