using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Parsing;

namespace VDS.RDF.Wrapping;

public class RdfCollectionListTest
{
    private readonly static NodeFactory factory = new();
    private readonly static INode s = factory.CreateUriNode(UriFactory.Create("http://example.com/s"));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IndexerGetBounds(int index)
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l[index]).Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IndexerSetNull()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l[0] = null!).Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IndexerSetBounds(int index)
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l[index] = "").Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddNull()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l.Add(null!)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ContainsNull()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l.Contains(null!)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IndexOfNull()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l.IndexOf(null!)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void InsertNull()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Invoking(l => l.Insert(0, null!)).Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(-1, true)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void InsertBounds(int index, bool throws)
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;
        var inserting = l.Invoking(l => l.Insert(index, ""));

        if (throws)
        {
            inserting.Should().Throw<ArgumentOutOfRangeException>();
        }
        else
        {
            inserting.Should().NotThrow<ArgumentOutOfRangeException>();
        }
    }














    //[Fact]
    public void EmptyWithNoTriple()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Should().BeEmpty();
        g.IsEmpty.Should().BeTrue();
    }

    //[Fact]
    public void AssertsWithNoTriple()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ('o') .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Add("o");

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void List1()
    {
        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> () .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Should().BeEmpty();
    }

    //[Fact]
    public void List2()
    {
        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> ('o') .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Should().BeEquivalentTo(["o"]);
    }

    //[Fact]
    public void Add1()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ('o') .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Add("o");

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void Add2()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ('o') .", new TurtleParser());

        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> () .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Add("o");

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void Add3()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ('o' 'o2') .", new TurtleParser());

        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> ('o') .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g).List;

        l.Add("o2");

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void WrapperList1()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> (<http://example.com/s>) .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.WrapperList.Add(l);

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void WrapperList2()
    {
        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> ( [ <http://example.com/p1> 'o' ] ) .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g);

        l.WrapperList.Select(x => x.MyProperty1).Single().Should().Be("o");
    }

    //[Fact]
    public void ListObjects3()
    {
        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> ( 'o1' 'o2' ), (), ( 'o3' 'o4' ) .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListObjects.SelectMany(x => x).Should().BeEquivalentTo(["o1", "o2", "o3", "o4"]);
    }

    //[Fact]
    public void ListObjects4()
    {
        var g = new Graph();
        g.LoadFromString("<http://example.com/s> <http://example.com/p1> () .", new TurtleParser());
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListObjects.SelectMany(x => x).Should().BeEmpty();
    }

    //[Fact]
    public void ListObjects5()
    {
        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListObjects.SelectMany(x => x).Should().BeEmpty();
    }

    //[Fact]
    public void ListObjects6()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ( 'o' ) .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListObjects.Add(["o"]);

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void ListObjects7()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ( 'o1' 'o2' ) .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListObjects.Add(["o1"]);
        l.ListObjects.First().Add("o2");

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void ListObjects8()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ( 'o1' 'o2' ), ( 'o3' 'o4' ) .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListObjects.Add(["o1"]);
        l.ListObjects.First().Add("o2");
        l.ListObjects.Add(["o3"]);
        l.ListObjects.Last().Add("o4");

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void ListList1()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ( ( 'o1' 'o' ) ( 'o2' 'o' )  ) .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListList.Add(["o1"]);
        l.ListList.Add(["o2"]);

        foreach (var list in l.ListList)
        {
            list.Add("o");
        }

        g.Equals(expected).Should().BeTrue();
    }

    //[Fact]
    public void ListListList1()
    {
        var expected = new Graph();
        expected.LoadFromString("<http://example.com/s> <http://example.com/p1> ( ( ( 'o1' 'o2' 'o' ) ( 'o3' 'o4' 'o' ) ) ( ( 'o5' 'o6' 'o' ) ( 'o7' 'o8' 'o' ) ) ) .", new TurtleParser());

        var g = new Graph();
        var l = SampleWrapperResource.Wrap(s, g);

        l.ListListList.Add([["o1", "o2"], ["o3", "o4"]]);
        l.ListListList.Add([["o5", "o6"], ["o7", "o8"]]);

        foreach (var listlist in l.ListListList)
        {
            foreach (var list in listlist)
            {
                list.Add("o");
            }
        }

        g.Equals(expected).Should().BeTrue();
    }
}
