using Microsoft.CodeAnalysis;

namespace VDS.RDF.Wrapping.Generator.Analyzers;

internal static class Rules
{
    internal static DiagnosticDescriptor NodeClassesMustBePartial { get; } = x(1, "t1", "Classes annotated with [Node] must be partial");
    internal static DiagnosticDescriptor Rule2 { get; } = x(2, "t2", "Classes annotated with [Node] must not have ctor(INode, IGraph)");

    private static DiagnosticDescriptor x(int id, string title,string messageFormat) =>
        new($"RdfWrap{id:0000}", title, messageFormat, "", DiagnosticSeverity.Error, isEnabledByDefault: true, helpLinkUri: $"http://example.com/{id:0000}");
}
