using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VDS.RDF.Wrapping;

internal class RdfCollectionList<T>(GraphWrapperNode? root, GraphWrapperNode subject, INode predicate, NodeMapping<T> toNode, ValueMapping<T> toValue) : IList<T>
{
    private GraphWrapperNode? root = root switch
    {
        null => null,
        var root when root.Equals(Vocabulary.Nil) => root,
        var root when !root.IsListRoot(subject.Graph) => throw new ArgumentException("must be list", nameof(root)),
        _ => root,
    };

    private readonly IGraph graph = subject.Graph ?? throw new ArgumentException("must have graph", nameof(subject));

    T IList<T>.this[int index]
    {
        get => default!; // TODO: adapt to this framework version: toValue(Items.GetItemByIndex(index));
        set => throw new NotImplementedException();
    }

    int ICollection<T>.Count => Items.Count();

    bool ICollection<T>.IsReadOnly => false;

    void ICollection<T>.Add(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var isNil = Vocabulary.Nil.Equals(root as INode);

        if (isNil)
        {
            graph.Retract(subject, predicate, root);
        }

        if (root is null || isNil)
        {
            root = graph.AssertList([item], NodeFrom).In(graph);
            graph.Assert(subject, predicate, root);
            return;
        }

        graph.AddToList(root, [item], NodeFrom);
    }

    void ICollection<T>.Clear() => graph.RetractList(root);

    bool ICollection<T>.Contains(T item) => Items.Contains(NodeFrom(item));

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => Values.ToArray().CopyTo(array, arrayIndex);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();

    int IList<T>.IndexOf(T item)
    {
        var itemNode = NodeFrom(item);
        var index = 0;

        foreach (var node in Items)
        {
            if (itemNode.Equals((INode)node))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    void IList<T>.Insert(int index, T item) => throw new NotImplementedException();

    /// <remarks>This implementation removes from the underlying RDF collection all occurences of nodes that correspond to the <paramref name="item"/>. This is different from the definition of <see cref="ICollection{T}.Remove(T)"/>, which is to remove only the first occurence. The justification for this behaviour is to align with idioms of the underlying core library.</remarks>
    bool ICollection<T>.Remove(T item)
    {
        if (!((ICollection<T>)this).Contains(item))
        {
            return false;
        }

        graph.RemoveFromList(root, [item], NodeFrom);
        return true;
    }

    void IList<T>.RemoveAt(int index) => throw new NotImplementedException();

    private IEnumerable<GraphWrapperNode> Items => root switch
    {
        null => [],
        var root when root.Equals(Vocabulary.Nil) => [],
        _ => graph.GetListItems(root).In(graph),
    };

    private IEnumerable<T> Values => Items.Select(item => toValue(item) ?? throw new Exception()); // TODO: Specific exception: No nulls in RDF collections

    private GraphWrapperNode NodeFrom(T item) => item switch
    {
        null => throw new ArgumentNullException("Cannot add null to an RDF collection", nameof(item)),
        _ => toNode(item, graph)
    };
}
