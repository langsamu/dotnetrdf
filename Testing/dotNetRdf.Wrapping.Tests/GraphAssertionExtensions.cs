using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Writing.Formatting;
using Formatter = FluentAssertions.Formatting.Formatter;

namespace VDS.RDF.Wrapping;

internal static class GraphAssertionExtensions
{
    internal static GraphAssertions Should(this IGraph instance)
    {
        Formatter.AddFormatter(new tf(instance.NamespaceMap));
        Formatter.AddFormatter(new tfx());
        return new(instance, AssertionChain.GetOrCreate());
    }

    internal class GraphAssertions(IGraph instance, AssertionChain chain) : ReferenceTypeAssertions<IGraph, GraphAssertions>(instance, chain)
    {
        private const string Message = """
            Expected {context:subject graph} to be isomorphic with target graph{reason}, but it was not.

            Added ({0}):
            {1}

            Removed ({2}):
            {3}
            """;

        protected override string Identifier => "graph";

        [CustomAssertion]
        internal AndConstraint<GraphAssertions> BeIsomorphicWith(IGraph other, string because = "", params object[] becauseArgs)
        {
            chain
                .BecauseOf(because, becauseArgs)
                .Given(() => new GraphDiff().Difference(other, Subject))
                .ForCondition(diff => diff.AreEqual)
                .FailWith(
                    Message,
                    static diff => diff.AddedTriples.Count(),
                    static diff => diff.AddedTriples,
                    static diff => diff.RemovedTriples.Count(),
                    static diff => diff.RemovedTriples
                );

            return new AndConstraint<GraphAssertions>(this);
        }

    }
    class tf(INamespaceMapper map) : IValueFormatter
    {
        private readonly ITripleFormatter ff = new TurtleW3CFormatter(map);

        bool IValueFormatter.CanHandle(object value) => value is Triple;

        void IValueFormatter.Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var t = value as Triple;
            formattedGraph.AddLine(ff.Format(t));
        }
    }
    class tfx : IValueFormatter
    {
        bool IValueFormatter.CanHandle(object value) => value is IEnumerable<Triple>;

        void IValueFormatter.Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var triples = value as IEnumerable<Triple>;
            formattedGraph.AddLine("{");
            foreach (var item in triples.Order())
            {
                formatChild("x", item, formattedGraph);
            }
            formattedGraph.AddLine("}");
        }
    }
}
