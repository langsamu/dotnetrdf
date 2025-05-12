using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VDS.RDF.Wrapping;

internal class RdfCollectionList<T>(INode? root, GraphWrapperNode subject, INode predicate, NodeMapping<T> toNode, ValueMapping<T> toValue) : IList<T>
{
    private INode? root = root switch
    {
        null => null,
        var root when root.Equals(Vocabulary.Nil) => root,
        var root when !root.IsListRoot(subject.Graph) => throw new ArgumentException("must be list", nameof(root)),
        _ => root,
    };

    private readonly IGraph graph = subject.Graph ?? throw new ArgumentException("must have graph", nameof(subject));

    T IList<T>.this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return toValue(Items.ElementAt(index))!;
        }

        set
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            RemoveAt(index);
            Insert(index, value);
        }
    }

    public int Count => Items.Count();

    bool ICollection<T>.IsReadOnly => false;

    void ICollection<T>.Add(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var isNil = Vocabulary.Nil.Equals(root);

        if (isNil)
        {
            graph.Retract(subject, predicate, root);
        }

        if (root is null || isNil)
        {
            root = graph.AssertList([item], NodeFrom);
            graph.Assert(subject, predicate, root);
            return;
        }

        graph.AddToList(root, [item], NodeFrom);
    }

    void ICollection<T>.Clear()
    {
        if (root is null)
        {
            return;
        }

        graph.RetractList(root);
        if (!Vocabulary.Nil.Equals(root))
        {
            graph.Retract(subject, predicate, root);
            graph.Assert(subject, predicate, Vocabulary.Nil);
        }
    }

    public bool Contains(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return Items.Contains(NodeFrom(item));
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        if (arrayIndex > array.Length)
        {
            throw new ArgumentException();
        }

        Values.ToArray().CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    int IList<T>.IndexOf(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

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

    public void Insert(int index, T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (index < 0 || index > Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        throw new NotImplementedException();
    }

    // TODO: Surface below remark to extension method and generator attribute documentation
    /// <remarks>This implementation removes from the underlying RDF collection all occurences of nodes that correspond to the <paramref name="item"/>. This is different from the definition of <see cref="ICollection{T}.Remove(T)"/>, which is to remove only the first occurence. The justification for this behaviour is to align with idioms of the underlying core library.</remarks>
    bool ICollection<T>.Remove(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (!Contains(item))
        {
            return false;
        }

        graph.RemoveFromList(root, [item], NodeFrom);
        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        throw new NotImplementedException();
    }

    private IEnumerable<GraphWrapperNode> Items => root switch
    {
        null => [],
        var root when root.Equals(Vocabulary.Nil) => [],
        _ => graph.GetListItems(root).In(graph),
    };

    private IEnumerable<T> Values => Items.Select(item => toValue(item)!);

    private GraphWrapperNode NodeFrom(T item) => toNode(item, graph);
}
