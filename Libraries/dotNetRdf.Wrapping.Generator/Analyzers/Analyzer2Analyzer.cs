using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using System;
using System.Diagnostics;

namespace VDS.RDF.Wrapping.Generator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer2Analyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rules.Rule2);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(NewMethod, SyntaxKind.ClassDeclaration);
    }

    private void NewMethod(SyntaxNodeAnalysisContext context)
    {
        var classNode = (ClassDeclarationSyntax)context.Node;
        var a = classNode.Members
            .Where(x => x.Kind() == SyntaxKind.ConstructorDeclaration)
            .Cast<ConstructorDeclarationSyntax>()
            .Select(x => x.ParameterList.Parameters)
            .Where(x => x.Count == 2)
            .Where(x => context.SemanticModel.GetTypeInfo(x.First().Type).Type.ToDisplayString() == "VDS.RDF.INode")
            .Where(x => context.SemanticModel.GetTypeInfo(x.Last().Type).Type.ToDisplayString() == "VDS.RDF.IGraph")
            .Any();

        if (!a)
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rules.Rule2, classNode.Identifier.GetLocation()));
    }
}
