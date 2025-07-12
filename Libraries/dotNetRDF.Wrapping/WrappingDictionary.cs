using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VDS.RDF.Wrapping;

public class WrappingDictionary<T, K>(GraphWrapperNode subject, INode predicate, NodeMapping<KeyValuePair<T, K>> nodeMapping, ValueMapping<KeyValuePair<T, K>> valueMapping) : IDictionary<T, K>
{
    public K this[T key]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public ICollection<T> Keys => subject.Graph.GetTriplesWithSubjectPredicate(subject, predicate)
        .Select(t => t.Object)
        .Select(obj => valueMapping(obj.In(subject.Graph)))
        .Select(x => x.Key)
        .Distinct()
        .ToList();

    public ICollection<K> Values => subject.Graph.GetTriplesWithSubjectPredicate(subject, predicate)
        .Select(t => t.Object)
        .Select(obj => valueMapping(obj.In(subject.Graph)))
        .Select(l => l.Value)
        .ToList();

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(T key, K value) => Add(new KeyValuePair<T, K>(key, value));

    public void Add(KeyValuePair<T, K> item) => subject.Graph.Assert(subject, predicate, nodeMapping(item, subject.Graph));

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(KeyValuePair<T, K> item)
    {
        throw new NotImplementedException();
    }

    public bool ContainsKey(T key)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<T, K>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool Remove(T key)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<T, K> item)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(T key, out K value)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
