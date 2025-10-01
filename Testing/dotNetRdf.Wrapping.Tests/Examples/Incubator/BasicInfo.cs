using System;
using System.Drawing;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class BasicInfo : GraphWrapperNode
{
    protected BasicInfo(INode node, IGraph graph) : base(node, graph) { }

    internal static BasicInfo Wrap(INode node, IGraph graph) => new(node, graph);

    internal static BasicInfo Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal Person Who
    {
        get => this.Singular(Vocabulary.Who, Person.Wrap);

        set => this.Overwrite(Vocabulary.Who, value, Person.Wrap);
    }

    internal GmlPoint Where
    {
        get => this.Singular(Vocabulary.Where, GmlPoint.Wrap);

        set => this.Overwrite(Vocabulary.Where, value, GmlPoint.Wrap);
    }

    internal DateTimeOffset When
    {
        get => this.Singular(Vocabulary.When, ValueMappings.DateTimeOffsetFromStringLiteral);

        set => this.Overwrite(Vocabulary.When, value, NodeMappings.StringLiteralFromDateTimeOffset);
    }
}
