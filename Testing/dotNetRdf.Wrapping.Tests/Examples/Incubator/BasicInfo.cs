using System;
using System.Drawing;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal class BasicInfo : Typed
{
    protected BasicInfo(INode node, IGraph graph) : base(node, graph) { }

    internal static new BasicInfo Wrap(INode node, IGraph graph) => new(node, graph);

    internal static new BasicInfo Wrap(GraphWrapperNode node) => Wrap(node, node.Graph);

    internal static BasicInfo Create(string uri, RDF.Graph g) => Wrap(g.CreateUriNode(g.UriFactory.Create(uri)), g);

    internal Person? Who
    {
        get => this.Singular(Vocabulary.Who, Person.Wrap);

        set => this.OverwriteNullable(Vocabulary.Who, value, Person.Wrap);
    }

    internal GmlPoint? Where
    {
        get => this.Singular(Vocabulary.Where, GmlPoint.Wrap);

        set => this.OverwriteNullable(Vocabulary.Where, value, GmlPoint.Wrap);
    }

    internal DateTimeOffset? When
    {
        get => this.Singular(Vocabulary.When, ValueMappings.DateTimeOffsetFromStringLiteral);

        set => this.OverwriteNullable(Vocabulary.When, value, NodeMappings.StringLiteralFromDateTimeOffset);
    }
}
