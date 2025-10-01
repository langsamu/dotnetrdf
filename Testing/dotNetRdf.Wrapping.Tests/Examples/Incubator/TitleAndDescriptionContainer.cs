namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class TitleAndDescriptionContainer : GraphWrapperNode
{
    protected TitleAndDescriptionContainer(INode node, IGraph graph) : base(node, graph) { }

    internal static TitleAndDescriptionContainer Wrap(INode node, IGraph graph) => new(node, graph);

    internal static TitleAndDescriptionContainer Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal string Title => this.Singular(Vocabulary.Title, ValueMappings.As<string>);

    internal string Description => this.Singular(Vocabulary.Description, ValueMappings.As<string>);
}
