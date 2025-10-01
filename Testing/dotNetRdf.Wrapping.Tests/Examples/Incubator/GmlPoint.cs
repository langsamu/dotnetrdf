using System.Drawing;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class GmlPoint : GraphWrapperNode
{
    protected GmlPoint(INode node, IGraph graph) : base(node, graph) { }

    internal static GmlPoint Wrap(INode node, IGraph graph) => new(node, graph);

    internal static GmlPoint Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal GmlPosition Position
    {
        get => this.Singular(Vocabulary.gml_pos, GmlPosition.Wrap);

        set => this.Overwrite(Vocabulary.gml_pos, value, GmlPosition.Wrap);
    }
}
