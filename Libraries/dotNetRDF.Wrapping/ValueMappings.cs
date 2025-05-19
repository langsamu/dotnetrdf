using System;
using System.Collections.Generic;
using System.Globalization;
using VDS.RDF.Nodes;

namespace VDS.RDF.Wrapping;

public static class ValueMappings
{
    public static T? As<T>(GraphWrapperNode? node) =>
        node!.AsObject() switch
        {
            null => default,
            T typed => typed,
            _ => throw new InvalidCastException(), // TODO: describe
        };

    public static GraphWrapperNode? AsIs(GraphWrapperNode? node) => node;

    public static KeyValuePair<CultureInfo, string> AsLangStringPair(GraphWrapperNode? node) => new KeyValuePair<CultureInfo, string>(
        CultureInfo.GetCultureInfo((node as ILiteralNode).Language), (node as ILiteralNode).Value);

    public static T ToEnum<T>(GraphWrapperNode? node) where T : Enum => (T)Enum.ToObject(typeof(T), node.AsValuedNode().AsInteger());
    
    public static T EnumFromName<T>(GraphWrapperNode? node) where T : Enum => (T)Enum.Parse(typeof(T), (node as ILiteralNode).Value);
    
    public static ValueMapping<T> EnumFromUri<T>(string prefix) where T : Enum => node => (T)Enum.Parse(typeof(T), new Uri(prefix).MakeRelativeUri((node as IUriNode).Uri).ToString());

    public static ValueMapping<IList<T>> AsList<T>(GraphWrapperNode subject, INode predicate, NodeMapping<T> nmap, ValueMapping<T> vmap) =>
        node => new RdfCollectionList<T>(node, subject, predicate, nmap, vmap);
}
