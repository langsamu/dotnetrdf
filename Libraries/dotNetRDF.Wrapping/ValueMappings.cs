using System;
using System.Collections.Generic;

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

    public static ValueMapping<IList<T>> AsList<T>(GraphWrapperNode subject, INode predicate, NodeMapping<T> nmap, ValueMapping<T> vmap) =>
        node => new RdfList<T>(node, subject, predicate, nmap, vmap);
}
