using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Nodes;
using VDS.RDF.Parsing;

namespace VDS.RDF.Wrapping;

public class RdfCollectionListTest
{
    private readonly static NodeFactory factory = new();
    private readonly static INode s = factory.CreateUriNode(UriFactory.Create("http://example.com/s"));
    private readonly static INode p = factory.CreateUriNode(UriFactory.Create("http://example.com/p"));
    private readonly static string v = Guid.NewGuid().ToString();
    private readonly static INode o = factory.CreateLiteralNode(v);

    private readonly Graph g = new();

    private RdfCollectionList<string> Collection => new(
        g.GetTriplesWithSubjectPredicate(s, p).Select(t => t.Object).SingleOrDefault(),
        new GraphWrapperNode(s, g),
        p,
        (v, g) => g.CreateLiteralNode(v).In(g),
        n => n.AsValuedNode().AsString());

    private static Func<RdfCollectionList<object>> ConstructorWithoutSubject => () => new(
        default,
        null!,
        default!,
        default!,
        default!);

    private static Func<RdfCollectionList<object>> ConstructorWithoutPredicate => () => new(
        default,
        new GraphWrapperNode(s, default),
        null!,
        default!,
        default!);

    private static Func<RdfCollectionList<object>> ConstructorWithoutNodeMapping => () => new(
        default,
        new GraphWrapperNode(s, default),
        p,
        default!,
        default!);

    private static Func<RdfCollectionList<object>> ConstructorWithoutValueMapping => () => new(
        default,
        new GraphWrapperNode(s, default),
        p,
        (_, _) => default!,
        default!);

    private static Func<RdfCollectionList<object>> ConstructorWithoutGraph => () => new(
        default,
        new GraphWrapperNode(s, null),
        p,
        (_, _) => default!,
        _ => null!);

    private static Func<RdfCollectionList<object>> ConstructorWithoutList => () => new(
        factory.CreateBlankNode(),
        new GraphWrapperNode(s, new Graph()),
        p,
        (_, _) => default!,
        _ => null!);

    [Fact]
    public void ConstructorNoSubject() => ConstructorWithoutSubject
        .Should().ThrowExactly<ArgumentNullException>()
        .WithParameterName("subject");

    [Fact]
    public void ConstructorNoPredicate() => ConstructorWithoutPredicate
        .Should().ThrowExactly<ArgumentNullException>()
        .WithParameterName("predicate");

    [Fact]
    public void ConstructorNoNodeMapping() => ConstructorWithoutNodeMapping
        .Should().ThrowExactly<ArgumentNullException>()
        .WithParameterName("toNode");

    [Fact]
    public void ConstructorNoValueMapping() => ConstructorWithoutValueMapping
        .Should().ThrowExactly<ArgumentNullException>()
        .WithParameterName("toValue");

    [Fact]
    public void ConstructorNoGraph() => ConstructorWithoutGraph
        .Should().ThrowExactly<ArgumentException>()
        .WithParameterName("subject");

    [Fact]
    public void ConstructorNotList() => ConstructorWithoutList
        .Should().ThrowExactly<ArgumentException>()
        .WithParameterName("root");

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void IndexerGetBounds(int index) => Collection
        .Invoking(l => l[index])
        .Should().ThrowExactly<ArgumentOutOfRangeException>()
        .WithParameterName("index");

    [Fact]
    public void IndexerGet()
    {
        g.Assert(s, p, g.AssertList([o]));

        Collection[0].Should().Be(v);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void IndexerSetBounds(int index) => Collection
        .Invoking(l => l[index] = "")
        .Should().ThrowExactly<ArgumentOutOfRangeException>()
        .WithParameterName("index");

    [Fact]
    public void IndexerSet()
    {
        g.Assert(s, p, g.AssertList([s]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        Collection[0] = v;

        g.Should().BeIsomorphicWith(expected);
    }

    [Theory]
    [MemberData(nameof(NodeCollections))]
    public void Count(IEnumerable<INode> objects)
    {
        g.Assert(s, p, g.AssertList(objects));

        Collection.Should().HaveCount(objects.Count());
    }

    public static IEnumerable<object[]> NodeCollections => [
        [ Array.Empty<INode>() ],
        [ new INode[] { s } ],
        [ new INode[] { s, p } ],
        [ new INode[] { s, p, o } ],
    ];

    [Fact]
    public void IsReadOnly() => Collection
        .IsReadOnly.Should().BeFalse();

    [Fact]
    public void AddNothing()
    {
        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        Collection.Add(v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void AddEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        Collection.Add(v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void Add()
    {
        g.Assert(s, p, g.AssertList([o]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o, o]));

        Collection.Add(v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ClearNothing()
    {
        g.LoadFromString("[ a 0 ] .", new TurtleParser());

        var expected = new Graph();
        expected.LoadFromString("[ a 0 ] .", new TurtleParser());

        Collection.Clear();

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ClearEmpty()
    {
        g.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> () .", new TurtleParser());

        var expected = new Graph();
        expected.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> () .", new TurtleParser());

        Collection.Clear();

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void Clear()
    {
        g.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> (0) .", new TurtleParser());

        var expected = new Graph();
        expected.LoadFromString("[ a 0 ] . <http://example.com/s> <http://example.com/p> () .", new TurtleParser());

        Collection.Clear();

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void ContainsNothing() => Collection
        .Should().NotContain(v);

    [Fact]
    public void ContainsEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        Collection.Should().NotContain(v);
    }

    [Fact]
    public void ContainsMismatch()
    {
        g.Assert(s, p, g.AssertList([s]));

        Collection.Should().NotContain(v);
    }

    [Fact]
    public void Contains()
    {
        g.Assert(s, p, g.AssertList([o]));

        Collection.Should().Contain(v);
    }

    [Fact]
    public void CopyTo()
    {
        g.Assert(s, p, g.AssertList([o]));

        var array = new string[5]; // +2 for padding on each side

        Collection.CopyTo(array, 1);

        array.Should().ContainInOrder([null, v, null]);
    }

    [Fact]
    public void GetEnumerator1()
    {
        g.Assert(s, p, g.AssertList([o, o, o]));

        var expected = (new string[] { v, v, v }).GetEnumerator();
        var actual = (Collection as IEnumerable).GetEnumerator();

        while (expected.MoveNext() | actual.MoveNext())
        {
            actual.Current.Should().Be(expected.Current);
        }
    }

    [Fact]
    public void IndexOfNull() => Collection
        .IndexOf(null!).Should().Be(-1);

    [Fact]
    public void IndexOfEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        Collection.IndexOf(null!).Should().Be(-1);
    }

    [Fact]
    public void IndexOf()
    {
        g.Assert(s, p, g.AssertList([g.CreateLiteralNode(""), o]));

        Collection.IndexOf(v).Should().Be(1);
    }

    [Theory]
    [InlineData(-1, true)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void InsertBounds(int index, bool throws)
    {
        var inserting = Collection.Invoking(l => l.Insert(index, ""));

        if (throws)
        {
            inserting.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        else
        {
            inserting.Should().NotThrow<ArgumentOutOfRangeException>();
        }
    }

    [Fact]
    public void InsertNone()
    {
        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        Collection.Insert(0, v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void InsertEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o]));

        Collection.Insert(0, v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void Insert()
    {
        g.Assert(s, p, g.AssertList([o]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([o, o]));

        Collection.Insert(0, v);

        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void RemoveNone() => Collection
        .Remove(v).Should().BeFalse();

    [Fact]
    public void RemoveEmpty()
    {
        g.Assert(s, p, g.AssertList([]));

        Collection.Remove(v).Should().BeFalse();
    }

    [Fact]
    public void RemoveMismatch()
    {
        g.Assert(s, p, g.AssertList([s]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([s]));

        Collection.Remove(v).Should().BeFalse();
        g.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void Remove()
    {
        g.Assert(s, p, g.AssertList([o]));

        var expected = new Graph();
        expected.Assert(s, p, expected.AssertList([]));

        Collection.Remove(v).Should().BeTrue();
        g.Should().BeIsomorphicWith(expected);
    }

    //[Theory]
    //[InlineData(-1)]
    //[InlineData(0)]
    //public void RemoveAtBounds(int index) => Collection
    //    .Invoking(l => l.RemoveAt(index))
    //    .Should().ThrowExactly<ArgumentOutOfRangeException>()
    //    .WithParameterName("index");

    //[Fact]
    //public void RemoveAtEmpty()
    //{
    //    g.Assert(s, p, g.AssertList([]));

    //    Collection.Invoking(l => l.RemoveAt(0))
    //        .Should().ThrowExactly<ArgumentOutOfRangeException>()
    //        .WithParameterName("index");
    //}

    [Fact]
    public void RemoveAt()
    {
        g.Assert(s, p, g.AssertList([o]));

        Collection.RemoveAt(0);
    }
}
