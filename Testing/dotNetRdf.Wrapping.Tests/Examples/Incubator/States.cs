using System;
using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class States : Typed
{
    protected States(INode node, IGraph graph) : base(node, graph) { }

    internal static new States Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new States Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static States Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal Uri? StatesValueList
    {
        get => this.Singular(Vocabulary.StatesValueList, ValueMappings.UriFromStringLiteral);

        set => this.OverwriteNullable(Vocabulary.StatesValueList, value, NodeMappings.StringLiteralFromUri);
    }


    internal ISet<State> Items => this.Objects(Vocabulary.hasState, State.Wrap, State.Wrap);
}
