using System;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Metric : Typed
{
    protected Metric(INode node, IGraph graph) : base(node, graph) { }

    internal static new Metric Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Metric Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Metric Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal Uri? MetricValueList
    {
        get => this.Singular(Vocabulary.MetricValueList, ValueMappings.UriFromStringLiteral);

        set => this.OverwriteNullable(Vocabulary.MetricValueList, value, NodeMappings.StringLiteralFromUri);
    }

    internal MetricValue? Value
    {
        get => this.Singular(Vocabulary.Value, ValueMappings.EnumFromName<MetricValue>);

        set => this.OverwriteNullable(Vocabulary.Value, value, NodeMappings.NameFromEnum);
    }
}
