using System;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class State : Typed
{
    protected State(INode node, IGraph graph) : base(node, graph) { }

    internal static new State Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new State Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static State Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal DateTimeOffset? Date
    {
        get => this.Singular(Vocabulary.Date, ValueMappings.DateTimeOffsetFromStringLiteral);

        set => this.OverwriteNullable(Vocabulary.Date, value, NodeMappings.StringLiteralFromDateTimeOffset);
    }

    internal StateValue? Value
    {
        get => this.Singular(Vocabulary.Value, ValueMappings.EnumFromName<StateValue>);

        set => this.OverwriteNullable(Vocabulary.Value, value, NodeMappings.NameFromEnum);
    }
}
