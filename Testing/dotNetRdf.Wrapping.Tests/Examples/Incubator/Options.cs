using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Options : GraphWrapperNode
{
    protected Options(INode node, IGraph graph) : base(node, graph) { }

    internal static Options Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Options Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal ISet<Option> Items => this.Objects(Vocabulary.hasOption, Option.Wrap, Option.Wrap);
}
