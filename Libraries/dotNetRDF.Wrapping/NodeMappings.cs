using System;
using System.Collections.Generic;
using System.Globalization;
using VDS.RDF.Nodes;

namespace VDS.RDF.Wrapping;

public static class NodeMappings
{
    public static GraphWrapperNode From<T>(T value, IGraph graph) // TODO: Rename
    {
        var node = value switch
        {
            null => throw new InvalidOperationException(), // TODO: describe
            INode nodeValue => nodeValue,
            Uri uriValue => new UriNode(uriValue),
            bool boolValue => new BooleanNode(boolValue),
            byte byteValue => new ByteNode(byteValue),
            DateTime dateTimeValue => new DateTimeNode(dateTimeValue),
            DateTimeOffset dateTimeOffsetValue => new DateTimeNode(dateTimeOffsetValue),
            decimal decimalValue => new DecimalNode(decimalValue),
            double doubleValue => new DoubleNode(doubleValue),
            float floatValue => new FloatNode(floatValue),
            long longValue => new LongNode(longValue),
            int intValue => new LongNode(intValue),
            string stringValue => new StringNode(stringValue),
            char charValue => new StringNode(charValue.ToString()),
            TimeSpan timeSpanValue => new TimeSpanNode(timeSpanValue),
            _ => throw new InvalidCastException($"Can't convert type {value.GetType()}"),
        };

        return node.In(graph);
    }

    public static GraphWrapperNode FromLangStringPair(KeyValuePair<CultureInfo, string> kv, IGraph graph) => graph.CreateLiteralNode(kv.Value, kv.Key.Name).In(graph);

    public static GraphWrapperNode NameFromEnum<T>(T enumeration, IGraph graph) where T : Enum => From(Enum.GetName(typeof(T), enumeration), graph);

    public static NodeMapping<T> UriFromEnum<T>(string prefix) where T : Enum => (enumeration, graph) => From(new Uri(new Uri(prefix), Enum.GetName(typeof(T), enumeration)), graph);

    public static NodeMapping<IList<T>> AsList<T>(NodeMapping<T> map) => (value, graph) => graph.AssertList(value, item => map(item, graph)).In(graph);
}
