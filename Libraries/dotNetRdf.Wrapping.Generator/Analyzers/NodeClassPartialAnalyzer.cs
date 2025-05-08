using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace VDS.RDF.Wrapping.Generator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NodeClassPartialAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rules.NodeClassesMustBePartial);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(EnsurePartial, SyntaxKind.ClassDeclaration);
    }

    private static void EnsurePartial(SyntaxNodeAnalysisContext context)
    {
        var classNode = (ClassDeclarationSyntax)context.Node;
        if (!IsClassAnnotatedWith(context, "VDS.RDF.Wrapping.Attributes.NodeAttribute"))
        {
            return;
        }

        if (IsClassPartial(context))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rules.NodeClassesMustBePartial, classNode.Identifier.GetLocation()));
    }

    private static bool IsClassPartial(SyntaxNodeAnalysisContext context)
    {
        var classNode = (ClassDeclarationSyntax)context.Node;
        return classNode.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static bool IsClassAnnotatedWith(SyntaxNodeAnalysisContext context, string annotation)
    {
        var classNode = (ClassDeclarationSyntax)context.Node;
        return classNode.AttributeLists.Any(list =>
                    list.Attributes.Any(attribute =>
                        context.SemanticModel.GetTypeInfo(attribute).Type.ToDisplayString() == annotation));
    }
}
