using System;
using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class States : GraphWrapperNode
{
    protected States(INode node, IGraph graph) : base(node, graph) { }

    internal static States Wrap(INode node, IGraph graph) => new(node, graph);

    internal static States Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal Uri StatesValueList
    {
        get => this.Singular(Vocabulary.StatesValueList, ValueMappings.UriFromStringLiteral);

        set => this.Overwrite(Vocabulary.StatesValueList, value, NodeMappings.StringLiteralFromUri);
    }


    internal ISet<State> Items => this.Objects(Vocabulary.hasState, State.Wrap, State.Wrap);
}
