using System;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class State : GraphWrapperNode
{
    protected State(INode node, IGraph graph) : base(node, graph) { }

    internal static State Wrap(INode node, IGraph graph) => new(node, graph);

    internal static State Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal DateTimeOffset Date
    {
        get => this.Singular(Vocabulary.Date, ValueMappings.DateTimeOffsetFromStringLiteral);

        set => this.Overwrite(Vocabulary.Date, value, NodeMappings.StringLiteralFromDateTimeOffset);
    }

    internal StateValue Value
    {
        get => this.Singular(Vocabulary.Value, ValueMappings.EnumFromName<StateValue>);

        set => this.Overwrite(Vocabulary.Value, value, NodeMappings.NameFromEnum);
    }
}
