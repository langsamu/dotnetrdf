using VDS.RDF.Query.Patterns;

namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

internal class InsertDeletePatch : Typed
{
    protected InsertDeletePatch(INode node, IGraph graph) : base(node, graph) { }

    internal static InsertDeletePatch Wrap(INode node, IGraph graph) => new(node, graph);

    internal static InsertDeletePatch Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static InsertDeletePatch Create(RDF.Graph g) => Wrap(g.CreateBlankNode(), g);

    internal GraphPattern? Where
    {
        get => this.Singular(Vocabulary.where, ValueMappings.GraphPatternFromGraphLiteral);

        set => this.OverwriteNullable(Vocabulary.where, value, NodeMappings.GraphLiteraFromGraphPattern);
    }

    internal GraphPattern? Inserts
    {
        get => this.Singular(Vocabulary.inserts, ValueMappings.GraphPatternFromGraphLiteral);

        set => this.OverwriteNullable(Vocabulary.inserts, value, NodeMappings.GraphLiteraFromGraphPattern);
    }

    internal GraphPattern? Deletions
    {
        get => this.Singular(Vocabulary.deletes, ValueMappings.GraphPatternFromGraphLiteral);

        set => this.OverwriteNullable(Vocabulary.deletes, value, NodeMappings.GraphLiteraFromGraphPattern);
    }
}
