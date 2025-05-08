using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Writing;

namespace VDS.RDF.Wrapping;

internal class NodeSet<T>(GraphWrapperNode anchor, INode predicate, TripleSegment segment, NodeMapping<T> toNode, ValueMapping<T> toValue) : ISet<T>
{
    private readonly GraphWrapperNode anchor = anchor switch
    {
        null => throw new ArgumentNullException(nameof(anchor)),
        { Graph: null } => throw new ArgumentException("nust have graph", nameof(anchor)),
        _ => anchor,
    };
    private readonly INode predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    private readonly TripleSegment segment = segment switch
    {
        TripleSegment.Subject
        or TripleSegment.Object => segment,

        _ => throw new ArgumentException("must be s or o", nameof(segment)),
    };
    //private readonly NodeMapping<T> toNode = toNode ?? throw new ArgumentNullException(nameof(toNode));
    private readonly ValueMapping<T> toValue = toValue ?? throw new ArgumentNullException(nameof(toValue));

    int ICollection<T>.Count => AssertedStatements.Count();

    bool ICollection<T>.IsReadOnly => false;

    private IEnumerable<Triple> AssertedStatements => segment switch
    {
        TripleSegment.Subject => Graph.GetTriplesWithSubjectPredicate(anchor, predicate),
        TripleSegment.Object => Graph.GetTriplesWithPredicateObject(predicate, anchor),
        _ => throw new InvalidOperationException(),
    };

    private IEnumerable<GraphWrapperNode> AssertedNodes => segment switch
    {
        TripleSegment.Subject => AssertedStatements.Select(statement => statement.Object).In(Graph),
        TripleSegment.Object => AssertedStatements.Select(statement => statement.Subject).In(Graph),
        _ => throw new InvalidOperationException(),
    };

    private IGraph Graph => anchor.Graph;

    private IEnumerable<T?> AssertedValues => AssertedNodes.Select(toValue.Invoke);

    bool ISet<T>.Add(T item) => Graph.Assert(StatementFrom(item));

    void ICollection<T>.Add(T item) => ((ISet<T>)this).Add(item);

    void ICollection<T>.Clear() => Graph.Retract(AssertedStatements);

    bool ICollection<T>.Contains(T item) => Graph.ContainsTriple(StatementFrom(item));

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => AssertedValues.ToArray().CopyTo(array, arrayIndex); // TODO: validate inputs

    bool ICollection<T>.Remove(T item) => Graph.Retract(StatementFrom(item));

    void ISet<T>.ExceptWith(IEnumerable<T> other) => Graph.Retract(StatementsFrom(other));

    public IEnumerator<T> GetEnumerator() => AssertedValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void ISet<T>.IntersectWith(IEnumerable<T> other) => Graph.Retract(AssertedStatements.Except(StatementsFrom(other)));

    void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
    {
        var otherStatements = StatementsFrom(other);
        var intersecting = AssertedStatements.Intersect(otherStatements);

        Graph.Assert(otherStatements.Except(intersecting));
        Graph.Retract(intersecting);
    }

    void ISet<T>.UnionWith(IEnumerable<T> other) => Graph.Assert(StatementsFrom(other).Except(AssertedStatements));

    bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other) => throw new NotImplementedException();

    bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other) => throw new NotImplementedException();

    bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
    {
        // The empty set is a subset of any set, and a set is a subset of itself.
        // Set is always a subset of itself.
        if (((ICollection<T>)this).Count == 0 || other == this)
        {
            return true;
        }

        return !AssertedStatements.Except(StatementsFrom(other)).Any();
    }

    bool ISet<T>.IsSupersetOf(IEnumerable<T> other) => !StatementsFrom(other).Except(AssertedStatements).Any();

    bool ISet<T>.Overlaps(IEnumerable<T> other) => throw new NotImplementedException();

    bool ISet<T>.SetEquals(IEnumerable<T> other) => throw new NotImplementedException();

    private Triple StatementFrom(T item) => segment switch
    {
        TripleSegment.Subject => new(anchor, predicate, NodeFrom(item)),
        TripleSegment.Object => new(NodeFrom(item), predicate, anchor),
        _ => throw new InvalidOperationException(),
    };

    private INode NodeFrom(T item) => item switch
    {
        null => throw new ArgumentNullException(nameof(item)),
        _ => toNode(item, Graph)
    };

    private IEnumerable<Triple> StatementsFrom(IEnumerable<T> other) => other switch
    {
        null => throw new ArgumentNullException(nameof(other)),
        NodeSet<T> otherSet => otherSet.AssertedStatements,
        IGraph otherGraph => otherGraph.Triples,
        IEnumerable<Triple> otherTriples => otherTriples,
        _ => other.Select(StatementFrom)
    };
}
