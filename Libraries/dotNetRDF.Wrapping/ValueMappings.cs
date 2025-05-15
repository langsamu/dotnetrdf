using System;
using System.Collections.Generic;
using System.Globalization;

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

    public static ValueMapping<IList<T>> AsList<T>(GraphWrapperNode subject, INode predicate, NodeMapping<T> nmap, ValueMapping<T> vmap) =>
        node => new RdfCollectionList<T>(node, subject, predicate, nmap, vmap);
}
