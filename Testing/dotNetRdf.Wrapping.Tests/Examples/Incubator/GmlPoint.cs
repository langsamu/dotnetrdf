using System.Drawing;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class GmlPoint : Typed
{
    protected GmlPoint(INode node, IGraph graph) : base(node, graph) { }

    internal static new GmlPoint Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new GmlPoint Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static GmlPoint Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);
 
    internal GmlPosition? Position
    {
        get => this.Singular(Vocabulary.gml_pos, GmlPosition.Wrap);

        set => this.OverwriteNullable(Vocabulary.gml_pos, value, GmlPosition.Wrap);
    }
}
