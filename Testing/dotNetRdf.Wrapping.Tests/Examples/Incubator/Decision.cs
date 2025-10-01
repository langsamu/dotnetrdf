namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class Decision : GraphWrapperNode
{
    protected Decision(INode node, IGraph graph) : base(node, graph) { }

    internal static Decision Wrap(INode node, IGraph graph) => new(node, graph);

    internal static Decision Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal Question Question => this.Singular(Vocabulary.hasQuestion, Question.Wrap);

    internal Options Options => this.Singular(Vocabulary.hasOptions, Options.Wrap);

    internal States States => this.Singular(Vocabulary.hasStates, States.Wrap);

    internal BasicInfo BasicInfo => this.Singular(Vocabulary.hasBasicInfo, BasicInfo.Wrap);
}
