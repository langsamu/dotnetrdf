using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Parsing;

namespace VDS.RDF.Wrapping;

public class RdfCollectionListTest
{
    private readonly static NodeFactory factory = new();
    private readonly static INode s = factory.CreateUriNode(UriFactory.Create("http://example.com/s"));
    private readonly static INode p = factory.CreateUriNode(UriFactory.Create("http://example.com/p"));
    private readonly static string v = Guid.NewGuid().ToString();
    private readonly static INode o = factory.CreateLiteralNode(v);

    private readonly Graph g;
    private readonly SampleWrapper wrapper;

    public RdfCollectionListTest()
    {
        g = new();
        wrapper = new SampleWrapper(s, g);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void IndexerGetBounds(int index) => wrapper.List.Invoking(l => l[index]).Should().Throw<ArgumentOutOfRangeException>();

    [Fact]
    public void IndexerGet()
    {
        g.Assert(s, p, g.AssertList([o]));

        wrapper.List[0].Should().Be(v);
    }

    [Fact]
    public void IndexerSetNull() => wrapper.List.Invoking(l => l[0] = null!).Should().Throw<ArgumentNullException>();

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void IndexerSetBounds(int index) => wrapper.List.Invoking(l => l[index] = "").Should().Throw<ArgumentOutOfRangeException>();

    [Theory]
    [MemberData(nameof(Counts))]
    public void Count(IEnumerable<INode> objects)
    {
        g.Assert(s, p, g.AssertList(objects));

        wrapper.List.Should().HaveCount(objects.Count());
    }

    public static IEnumerable<object[]> Counts => [
        [ Array.Empty<INode>() ],
        [ new INode[] { s } ],
        [ new INode[] { s, p } ],
        [ new INode[] { s, p, o } ],
    ];

    [Fact]
    public void IsReadOnly() => wrapper.List.IsReadOnly.Should().BeFalse();

    [Fact]
    public void AddNull() => wrapper.List.Invoking(l => l.Add(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void AddToNothing()
    {
        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        wrapper.List.Add(v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void AddToEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        wrapper.List.Add(v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void AddToList()
    {
        g.Assert(s, p, g.AssertList([o]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o, o,s]));

        wrapper.List.Add(v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ClearNothing()
    {
        g.LoadFromString("[ a 0 ] .", new TurtleParser());
    
        var expected = new Graph();
        expected.LoadFromString("[ a 0 ] .", new TurtleParser());

        wrapper.List.Clear();

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ClearEmpty()
    {
        g.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> () .", new TurtleParser());

        var expected = new Graph();
        expected.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> () .", new TurtleParser());

        wrapper.List.Clear();

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ClearList()
    {
        g.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> (0) .", new TurtleParser());

        var expected = new Graph();
        expected.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> () .", new TurtleParser());

        wrapper.List.Clear();

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ContainsNull() => wrapper.List.Invoking(l => l.Contains(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void ContainsNothing() => wrapper.List.Should().NotContain(v);

    [Fact]
    public void ContainsEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        wrapper.List.Should().NotContain(v);
    }

    [Fact]
    public void ContainsMismatch()
    {
        g.Assert(s, p, g.AssertList([s]));

        wrapper.List.Should().NotContain(v);
    }

    [Fact]
    public void Contains()
    {
        g.Assert(s, p, g.AssertList([o]));

        wrapper.List.Should().Contain(v);
    }

    [Fact]
    public void IndexOfNull() => wrapper.List.Invoking(l => l.IndexOf(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void InsertNull() => wrapper.List.Invoking(l => l.Insert(0, null!)).Should().Throw<ArgumentNullException>();

    [Theory]
    [InlineData(-1, true)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void InsertBounds(int index, bool throws)
    {
        var inserting = wrapper.List.Invoking(l => l.Insert(index, ""));

        if (throws)
        {
            inserting.Should().Throw<ArgumentOutOfRangeException>();
        }
        else
        {
            inserting.Should().NotThrow<ArgumentOutOfRangeException>();
        }
    }

    [Fact]
    public void RemoveNull() => wrapper.List.Invoking(l => l.Remove(null!)).Should().Throw<ArgumentNullException>();

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void RemoveAtBounds(int index) => wrapper.List.Invoking(l => l.RemoveAt(index)).Should().Throw<ArgumentOutOfRangeException>();

    class SampleWrapper(INode node, IGraph graph) : GraphWrapperNode(node, graph)
    {
        internal IList<string> List => this.List(p, NodeMappings.From, ValueMappings.As<string>);
    }
}
