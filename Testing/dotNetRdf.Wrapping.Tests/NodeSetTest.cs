using System.Collections.Generic;
using VDS.RDF.Writing;

namespace VDS.RDF.Wrapping;

public class NodeSetTest
{
    private readonly static NodeFactory factory = new();
    private readonly static INode predicate = factory.CreateUriNode(UriFactory.Create("http://example.com/p"));
    private readonly static TripleSegment segment = TripleSegment.Subject;

    private readonly Graph g = new();
    private readonly GraphWrapperNode anchor;

    public NodeSetTest()
    {
        anchor = factory.CreateUriNode(UriFactory.Create("http://example.com/s")).In(g);
    }

    public static IEnumerable<TheoryDataRow<IEnumerable<string>, IEnumerable<string>, bool>> SubsetData => [
        new([], [], true),
        new([], ["b"], true),
        new([], ["a"], true),
        new([], ["a", "b"], true),
        new([], ["b", "a"], true),

        new(["b"], [], false),
        new(["b"], ["b"], true),
        new(["b"], ["a"], false),
        new(["b"], ["a", "b"], true),
        new(["b"], ["b", "a"], true),

        new(["a"], [], false),
        new(["a"], ["b"], false),
        new(["a"], ["a"], true),
        new(["a"], ["a", "b"], true),
        new(["a"], ["b", "a"], true),

        new(["a", "b"], [], false),
        new(["a", "b"], ["b"], false),
        new(["a", "b"], ["a"], false),
        new(["a", "b"], ["a", "b"], true),
        new(["a", "b"], ["b", "a"], true),

        new(["b", "a"], [], false),
        new(["b", "a"], ["b"], false),
        new(["b", "a"], ["a"], false),
        new(["b", "a"], ["a", "b"], true),
        new(["b", "a"], ["b", "a"], true),
    ];

    [Theory]
    [MemberData(nameof(SubsetData))]
    public void Subset(IEnumerable<string> items, IEnumerable<string> other, bool result) => NodeSetFrom(items).IsSubsetOf(other).Should().Be(result);

    public static IEnumerable<TheoryDataRow<IEnumerable<string>, IEnumerable<string>, bool>> ProperSubsetData => [
        new([], [], false),
        new([], ["b"], true),
        new([], ["a"], true),
        new([], ["a", "b"], true),
        new([], ["b", "a"], true),

        new(["b"], [], false),
        new(["b"], ["b"], false),
        new(["b"], ["a"], false),
        new(["b"], ["a", "b"], true),
        new(["b"], ["b", "a"], true),

        new(["a"], [], false),
        new(["a"], ["b"], false),
        new(["a"], ["a"], false),
        new(["a"], ["a", "b"], true),
        new(["a"], ["b", "a"], true),

        new(["a", "b"], [], false),
        new(["a", "b"], ["b"], false),
        new(["a", "b"], ["a"], false),
        new(["a", "b"], ["a", "b"], false),
        new(["a", "b"], ["b", "a"], false),

        new(["b", "a"], [], false),
        new(["b", "a"], ["b"], false),
        new(["b", "a"], ["a"], false),
        new(["b", "a"], ["a", "b"], false),
        new(["b", "a"], ["b", "a"], false),
    ];

    [Theory]
    [MemberData(nameof(ProperSubsetData))]
    public void ProperSubset(IEnumerable<string> items, IEnumerable<string> other, bool result) => NodeSetFrom(items).IsProperSubsetOf(other).Should().Be(result);

    public static IEnumerable<TheoryDataRow<IEnumerable<string>, IEnumerable<string>, bool>> SupersetData => [
        new([], [], true),
        new([], ["b"], false),
        new([], ["a"], false),
        new([], ["a", "b"], false),
        new([], ["b", "a"], false),

        new(["b"], [], true),
        new(["b"], ["b"], true),
        new(["b"], ["a"], false),
        new(["b"], ["a", "b"], false),
        new(["b"], ["b", "a"], false),

        new(["a"], [], true),
        new(["a"], ["b"], false),
        new(["a"], ["a"], true),
        new(["a"], ["a", "b"], false),
        new(["a"], ["b", "a"], false),

        new(["a", "b"], [], true),
        new(["a", "b"], ["b"], true),
        new(["a", "b"], ["a"], true),
        new(["a", "b"], ["a", "b"], true),
        new(["a", "b"], ["b", "a"], true),

        new(["b", "a"], [], true),
        new(["b", "a"], ["b"], true),
        new(["b", "a"], ["a"], true),
        new(["b", "a"], ["a", "b"], true),
        new(["b", "a"], ["b", "a"], true),
    ];

    [Theory]
    [MemberData(nameof(SupersetData))]
    public void Superset(IEnumerable<string> items, IEnumerable<string> other, bool result) => NodeSetFrom(items).IsSupersetOf(other).Should().Be(result);

    public static IEnumerable<TheoryDataRow<IEnumerable<string>, IEnumerable<string>, bool>> ProperSupersetData => [
        new([], [], false),
        new([], ["b"], false),
        new([], ["a"], false),
        new([], ["a", "b"], false),
        new([], ["b", "a"], false),

        new(["b"], [], true),
        new(["b"], ["b"], false),
        new(["b"], ["a"], false),
        new(["b"], ["a", "b"], false),
        new(["b"], ["b", "a"], false),

        new(["a"], [], true),
        new(["a"], ["b"], false),
        new(["a"], ["a"], false),
        new(["a"], ["a", "b"], false),
        new(["a"], ["b", "a"], false),

        new(["a", "b"], [], true),
        new(["a", "b"], ["b"], true),
        new(["a", "b"], ["a"], true),
        new(["a", "b"], ["a", "b"], false),
        new(["a", "b"], ["b", "a"], false),

        new(["b", "a"], [], true),
        new(["b", "a"], ["b"], true),
        new(["b", "a"], ["a"], true),
        new(["b", "a"], ["a", "b"], false),
        new(["b", "a"], ["b", "a"], false),
    ];

    [Theory]
    [MemberData(nameof(ProperSupersetData))]
    public void ProperSuperset(IEnumerable<string> items, IEnumerable<string> other, bool result) => NodeSetFrom(items).IsProperSupersetOf(other).Should().Be(result);

    public static IEnumerable<TheoryDataRow<IEnumerable<string>, IEnumerable<string>, bool>> OverlapsData => [
        new([], [], false),
        new([], ["b"], false),
        new([], ["a"], false),
        new([], ["a", "b"], false),
        new([], ["b", "a"], false),

        new(["b"], [], false),
        new(["b"], ["b"], true),
        new(["b"], ["a"], false),
        new(["b"], ["a", "b"], true),
        new(["b"], ["b", "a"], true),

        new(["a"], [], false),
        new(["a"], ["b"], false),
        new(["a"], ["a"], true),
        new(["a"], ["a", "b"], true),
        new(["a"], ["b", "a"], true),

        new(["a", "b"], [], false),
        new(["a", "b"], ["b"], true),
        new(["a", "b"], ["a"], true),
        new(["a", "b"], ["a", "b"], true),
        new(["a", "b"], ["b", "a"], true),

        new(["b", "a"], [], false),
        new(["b", "a"], ["b"], true),
        new(["b", "a"], ["a"], true),
        new(["b", "a"], ["a", "b"], true),
        new(["b", "a"], ["b", "a"], true),
    ];

    [Theory]
    [MemberData(nameof(OverlapsData))]
    public void Overlaps(IEnumerable<string> items, IEnumerable<string> other, bool result) => NodeSetFrom(items).Overlaps(other).Should().Be(result);

    public static IEnumerable<TheoryDataRow<IEnumerable<string>, IEnumerable<string>, bool>> SetEqualsData => [
        new([], [], true),
        new([], ["b"], false),
        new([], ["a"], false),
        new([], ["a", "b"], false),
        new([], ["b", "a"], false),

        new(["b"], [], false),
        new(["b"], ["b"], true),
        new(["b"], ["a"], false),
        new(["b"], ["a", "b"], false),
        new(["b"], ["b", "a"], false),

        new(["a"], [], false),
        new(["a"], ["b"], false),
        new(["a"], ["a"], true),
        new(["a"], ["a", "b"], false),
        new(["a"], ["b", "a"], false),

        new(["a", "b"], [], false),
        new(["a", "b"], ["b"], false),
        new(["a", "b"], ["a"], false),
        new(["a", "b"], ["a", "b"], true),
        new(["a", "b"], ["b", "a"], true),

        new(["b", "a"], [], false),
        new(["b", "a"], ["b"], false),
        new(["b", "a"], ["a"], false),
        new(["b", "a"], ["a", "b"], true),
        new(["b", "a"], ["b", "a"], true),
    ];

    [Theory]
    [MemberData(nameof(SetEqualsData))]
    public void SetEquals(IEnumerable<string> items, IEnumerable<string> other, bool result) => NodeSetFrom(items).SetEquals(other).Should().Be(result);

    private ISet<string> NodeSetFrom(IEnumerable<string> items)
    {
        var nodes = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);

        foreach (var item in items)
        {
            nodes.Add(item);
        }

        return nodes;
    }
}
