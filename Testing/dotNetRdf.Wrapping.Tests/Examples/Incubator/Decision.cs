namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Decision : Typed
{
    protected Decision(INode node, IGraph graph) : base(node, graph) { }

    internal static new Decision Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new Decision Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static Decision Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal Question? Question
    {
        get => this.Singular(Vocabulary.hasQuestion, Question.Wrap);

        set => this.OverwriteNullable(Vocabulary.hasQuestion, value, Question.Wrap);
    }

    internal Options? Options
    {
        get => this.Singular(Vocabulary.hasOptions, Options.Wrap);

        set => this.OverwriteNullable(Vocabulary.hasOptions, value, Options.Wrap);
    }

    internal States? States
    {
        get => this.Singular(Vocabulary.hasStates, States.Wrap);

        set => this.OverwriteNullable(Vocabulary.hasStates, value, States.Wrap);
    }

    internal BasicInfo? BasicInfo
    {
        get => this.Singular(Vocabulary.hasBasicInfo, BasicInfo.Wrap);

        set => this.OverwriteNullable(Vocabulary.hasBasicInfo, value, BasicInfo.Wrap);
    }
}
