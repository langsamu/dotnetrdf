using System;
using System.Collections.Generic;
using VDS.RDF.Writing;

namespace VDS.RDF.Wrapping;

public class NodeSetTest
{
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
    public void Subset(IEnumerable<string> data1, IEnumerable<string> data2, bool result)
    {
        var g = new Graph();
        var anchor = g.CreateUriNode(new Uri("http://example.com/s")).In(g);
        var predicate = g.CreateUriNode(new Uri("http://example.com/p"));
        var segment = TripleSegment.Subject;

        var a = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);
        foreach (var item in data1)
        {
            a.Add(item);
        }

        a.IsSubsetOf(data2).Should().Be(result);
    }

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
    public void ProperSubset(IEnumerable<string> data1, IEnumerable<string> data2, bool result)
    {
        var g = new Graph();
        var anchor = g.CreateUriNode(new Uri("http://example.com/s")).In(g);
        var predicate = g.CreateUriNode(new Uri("http://example.com/p"));
        var segment = TripleSegment.Subject;

        var a = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);
        foreach (var item in data1)
        {
            a.Add(item);
        }

        a.IsProperSubsetOf(data2).Should().Be(result);
    }

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
    public void Superset(IEnumerable<string> data1, IEnumerable<string> data2, bool result)
    {
        var g = new Graph();
        var anchor = g.CreateUriNode(new Uri("http://example.com/s")).In(g);
        var predicate = g.CreateUriNode(new Uri("http://example.com/p"));
        var segment = TripleSegment.Subject;

        var a = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);
        foreach (var item in data1)
        {
            a.Add(item);
        }

        a.IsSupersetOf(data2).Should().Be(result);
    }

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
    public void ProperSuperset(IEnumerable<string> data1, IEnumerable<string> data2, bool result)
    {
        var g = new Graph();
        var anchor = g.CreateUriNode(new Uri("http://example.com/s")).In(g);
        var predicate = g.CreateUriNode(new Uri("http://example.com/p"));
        var segment = TripleSegment.Subject;

        var a = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);
        foreach (var item in data1)
        {
            a.Add(item);
        }

        a.IsProperSupersetOf(data2).Should().Be(result);
    }

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
    public void Overlaps(IEnumerable<string> data1, IEnumerable<string> data2, bool result)
    {
        var g = new Graph();
        var anchor = g.CreateUriNode(new Uri("http://example.com/s")).In(g);
        var predicate = g.CreateUriNode(new Uri("http://example.com/p"));
        var segment = TripleSegment.Subject;

        var a = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);
        foreach (var item in data1)
        {
            a.Add(item);
        }

        a.Overlaps(data2).Should().Be(result);
    }

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
    public void SetEquals(IEnumerable<string> data1, IEnumerable<string> data2, bool result)
    {
        var g = new Graph();
        var anchor = g.CreateUriNode(new Uri("http://example.com/s")).In(g);
        var predicate = g.CreateUriNode(new Uri("http://example.com/p"));
        var segment = TripleSegment.Subject;

        var a = new NodeSet<string>(anchor, predicate, segment, NodeMappings.From, ValueMappings.As<string>);
        foreach (var item in data1)
        {
            a.Add(item);
        }

        a.SetEquals(data2).Should().Be(result);
    }
}
