using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using VDS.RDF.Writing;

namespace VDS.RDF.Wrapping;

internal static class GraphAssertionExtensions
{
    internal static GraphAssertions Should(this IGraph instance) => new(instance);

    internal class GraphAssertions(IGraph instance) : ReferenceTypeAssertions<IGraph, GraphAssertions>(instance)
    {
        private const string Message = """
            Expected {context:subject graph} to be isomorphic with target graph{reason}, but it was not.

            Subject graph:
            {0}

            Target graph:
            {1}
            """;

        protected override string Identifier => "graph";

        internal AndConstraint<GraphAssertions> BeIsomorphicWith(IGraph other, string because = "", params object[] becauseArgs)
        {
            var subjectRdf = StringWriter.Write(Subject, new CompressingTurtleWriter());
            var otherRdf = StringWriter.Write(other, new CompressingTurtleWriter());

            Execute.Assertion
                .ForCondition(Subject.Equals(other))
                .BecauseOf(because, becauseArgs)
                .FailWith(Message, subjectRdf, otherRdf, becauseArgs);

            return new AndConstraint<GraphAssertions>(this);
        }
    }
}
