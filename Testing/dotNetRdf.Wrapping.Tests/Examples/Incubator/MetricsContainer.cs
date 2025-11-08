using System.Collections.Generic;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class MetricsContainer : TitleAndDescriptionContainer
{
    protected MetricsContainer(INode node, IGraph graph) : base(node, graph) { }

    internal static new MetricsContainer Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new MetricsContainer Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal ISet<Metric> Metrics => this.Objects(Vocabulary.hasMetric, Metric.Wrap, Metric.Wrap);
}
