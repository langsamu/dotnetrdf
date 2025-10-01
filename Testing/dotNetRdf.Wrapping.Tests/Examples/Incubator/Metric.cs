using System;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Metric : GraphWrapperNode
{
    protected Metric(INode node, IGraph graph) : base(node, graph) { }

    internal static Metric Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Metric Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal Uri MetricValueList
    {
        get => this.Singular(Vocabulary.MetricValueList, ValueMappings.UriFromStringLiteral);

        set => this.Overwrite(Vocabulary.MetricValueList, value, NodeMappings.StringLiteralFromUri);
    }

    internal MetricValue Value
    {
        get => this.Singular(Vocabulary.Value, ValueMappings.EnumFromName<MetricValue>);

        set => this.Overwrite(Vocabulary.Value, value, NodeMappings.NameFromEnum);
    }
}
