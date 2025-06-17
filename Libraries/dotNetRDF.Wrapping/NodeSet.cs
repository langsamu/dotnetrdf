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
        { Graph: null } => throw new ArgumentException("must have graph", nameof(anchor)),
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

    public int Count => AssertedStatements.Count();

    public bool IsReadOnly => false;

    private IEnumerable<Triple> AssertedStatements => segment switch
    {
        TripleSegment.Subject => Graph.GetTriplesWithSubjectPredicate(anchor, predicate),
        TripleSegment.Object or _ => Graph.GetTriplesWithPredicateObject(predicate, anchor),
    };

    private IEnumerable<GraphWrapperNode> AssertedNodes => segment switch
    {
        TripleSegment.Subject => AssertedStatements.Select(statement => statement.Object).In(Graph),
        TripleSegment.Object or _ => AssertedStatements.Select(statement => statement.Subject).In(Graph),
    };

    private IGraph Graph => anchor.Graph;

    private IEnumerable<T?> AssertedValues => AssertedNodes.Select(toValue.Invoke);

    public bool Add(T item) => Graph.Assert(StatementFrom(item));

    void ICollection<T>.Add(T item) => Add(item);

    public void Clear() => Graph.Retract(AssertedStatements);

    public bool Contains(T item) => Graph.ContainsTriple(StatementFrom(item));

    public void CopyTo(T[] array, int arrayIndex) => AssertedValues.ToArray().CopyTo(array, arrayIndex); // TODO: validate inputs

    public bool Remove(T item) => Graph.Retract(StatementFrom(item));

    public void ExceptWith(IEnumerable<T> other) => Graph.Retract(StatementsFrom(other));

    public IEnumerator<T> GetEnumerator() => AssertedValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void IntersectWith(IEnumerable<T> other) => Graph.Retract(AssertedStatements.Except(StatementsFrom(other)));

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        var otherStatements = StatementsFrom(other);
        var intersecting = AssertedStatements.Intersect(otherStatements);

        Graph.Assert(otherStatements.Except(intersecting));
        Graph.Retract(intersecting);
    }

    public void UnionWith(IEnumerable<T> other) => Graph.Assert(StatementsFrom(other).Except(AssertedStatements));

    public bool IsProperSubsetOf(IEnumerable<T> other) => IsSubsetOf(other) && !IsSupersetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => IsSupersetOf(other) && !IsSubsetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => AssertedValues.All(other.Contains);

    public bool IsSupersetOf(IEnumerable<T> other) => other.All(Contains);

    public bool Overlaps(IEnumerable<T> other) => other.Any(Contains) || AssertedValues.Any(other.Contains);

    public bool SetEquals(IEnumerable<T> other) => IsSupersetOf(other) && IsSubsetOf(other);

    private Triple StatementFrom(T item) => segment switch
    {
        TripleSegment.Subject => new(anchor, predicate, NodeFrom(item)),
        TripleSegment.Object or _ => new(NodeFrom(item), predicate, anchor),
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
