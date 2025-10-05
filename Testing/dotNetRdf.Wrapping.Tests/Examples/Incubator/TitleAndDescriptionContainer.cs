namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class TitleAndDescriptionContainer : Typed
{
    protected TitleAndDescriptionContainer(INode node, IGraph graph) : base(node, graph) { }

    internal static new TitleAndDescriptionContainer Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new TitleAndDescriptionContainer Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal string? Title
    {
        get => this.Singular(Vocabulary.Title, ValueMappings.As<string>);

        set => this.OverwriteNullable(Vocabulary.Title, value);
    }

    internal string? Description
    {
        get => this.Singular(Vocabulary.Description, ValueMappings.As<string>);

        set => this.OverwriteNullable(Vocabulary.Description, value);
    }
}
