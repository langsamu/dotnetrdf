using System;

namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

internal class Typed : GraphWrapperNode
{
    protected Typed(INode node, IGraph graph) : base(node, graph) { }

    internal static Typed Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Typed Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal Uri? Type
    {
        get => this.Singular(Vocabulary.RdfType, ValueMappings.As<Uri>);

        set => this.OverwriteNullable(Vocabulary.RdfType, value);
    }
}
