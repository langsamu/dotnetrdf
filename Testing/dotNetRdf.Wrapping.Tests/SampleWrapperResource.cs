using System.Collections.Generic;
using static VDS.RDF.Wrapping.NodeMappings;
using static VDS.RDF.Wrapping.SampleVocabulary;
using static VDS.RDF.Wrapping.ValueMappings;

namespace VDS.RDF.Wrapping;

internal partial class SampleWrapperResource
{
    internal partial string? MyProperty1 { get; set; }

    internal partial SampleWrapperResource? MyProperty2 { get; set; }

    internal partial long? MyProperty3 { get; set; }

    internal partial ISet<string> Objects { get; }

    internal partial ISet<SampleWrapperResource> ResourceObjects { get; }

    internal partial IList<string> List { get; }

    internal partial ISet<IList<string>> ListObjects { get; }

    internal partial IList<IList<string>> ListList { get; }

    internal partial IList<IList<IList<string>>> ListListList { get; }

    internal partial IList<SampleWrapperResource> WrapperList { get; }
}

#nullable enable
internal partial class SampleWrapperResource : GraphWrapperNode
{
    protected SampleWrapperResource(INode node, IGraph graph) : base(node, graph) { }

    internal static SampleWrapperResource Wrap(INode node, IGraph graph) => node switch
    {
        SampleWrapperResource same when ReferenceEquals(same.Graph, graph) => same,
        _ => new(node, graph),
    };

    internal static SampleWrapperResource? Wrap(GraphWrapperNode? node) => node switch
    {
        null => null,
        _ => Wrap(node, node.Graph)
    };

    internal partial string? MyProperty1
    {
        get => this.Singular(P1, As<string>);
        set => this.OverwriteNullable(P1, value, From);
    }

    internal partial SampleWrapperResource? MyProperty2
    {
        get => this.Singular(Graph.CreateUriNode(new System.Uri("P1")), Wrap);
        set => this.OverwriteNullable(P1, value, Wrap);
    }

    internal partial long? MyProperty3
    {
        get => this.Singular(P1, As<long>);
        set => this.OverwriteNullable(P1, value, From);
    }

    internal partial ISet<string> Objects => this.Objects(P1, From, As<string>);

    internal partial ISet<SampleWrapperResource> ResourceObjects => this.Objects(P1, Wrap, Wrap);

    internal partial ISet<IList<string>> ListObjects => this.Objects(P1, AsList<string>(From), AsList(this, P1, From, As<string>));

    internal partial IList<string> List => this.List(P1, From, As<string>);

    internal partial IList<SampleWrapperResource> WrapperList => this.List(P1, Wrap, Wrap);

    internal partial IList<IList<string>> ListList => this.List(P1, AsList<string>(From), AsList(this, P1, From, As<string>));

    internal partial IList<IList<IList<string>>> ListListList => this.List(P1, AsList(AsList<string>(From)), AsList(this, P1, AsList<string>(From), AsList(this, P1, From, As<string>)));
}
#nullable restore
