using System;
using VDS.RDF.Nodes;
using VDS.RDF.Parsing;

namespace VDS.RDF.Wrapping;

public class ValueMappingsTest
{
    [Fact]
    public void KnownNullIsNull()
    {
        ValueMappings.As<object>(null).Should().BeNull();
    }

    [Fact]
    public void UnkownNullIsNull()
    {
        ValueMappings.As<UnknownClass>(null).Should().BeNull();
    }

    [Fact]
    public void UnknownInstanceThrows()
    {
        var g = new Graph();
        var n = new LiteralNode("", new Uri("http://example.com/dt")).In(g);

        n.Invoking(n => ValueMappings.As<UnknownClass>(n)).Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void IncompatibleThrows()
    {
        var g = new Graph();
        var n = new LiteralNode("0",new Uri( XmlSpecsHelper.XmlSchemaDataTypeInt)).In(g);

        n.Invoking(n => ValueMappings.As<string>(n)).Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void CompatibleConverts()
    {
        var g = new Graph();
        var original = new LongNode(0);
        var n = original.In(g);

        ValueMappings.As<long>(n).Should().BeOfType(typeof(long)).And.Be(original.AsInteger());
    }

    class UnknownClass { }
}