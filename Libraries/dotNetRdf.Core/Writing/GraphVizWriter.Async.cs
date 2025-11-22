/*
// <copyright>
// dotNetRDF is free and open source software licensed under the MIT License
// -------------------------------------------------------------------------
// 
// Copyright (c) 2009-2025 dotNetRDF Project (http://dotnetrdf.org/)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
*/

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VDS.RDF.Writing.Contexts;

namespace VDS.RDF.Writing;

partial class GraphVizWriter
{
    protected override async Task SaveInternalAsync(IGraph g, TextWriter output, CancellationToken ct = default)
    {
        var context = new BaseWriterContext(g, output) { PrettyPrint = PrettyPrintMode };

        await WriteGraphAsync(context, CollapseLiterals, ct);
    }

    private async Task WriteGraphAsync(BaseWriterContext context, bool collapseLiterals, CancellationToken ct)
    {
        await context.Output.WriteAsync(Dot.Digraph);

        if (context.Graph.BaseUri != null)
        {
            var graphId = ReduceToQName(context.Graph.BaseUri, context);

            await PrettifyAsync(Dot.Space, context);
            await WriteQuotedAsync(graphId, context);
        }

        await PrettifyAsync(Dot.Space, context);
        await context.Output.WriteAsync(Dot.OpenCurly);
        await PrettifyAsync(Dot.NewLine, context);

        foreach (Triple t in context.Graph.Triples)
        {
            if (ct.IsCancellationRequested)
            {
                Warning?.Invoke("Cancellation requested");
                break;
            }

            await WriteTripleAsync(t, context, collapseLiterals);
        }

        await context.Output.WriteAsync(Dot.CloseCurly);
    }

    private static async Task WriteTripleAsync(Triple triple, BaseWriterContext context, bool collapseLiterals)
    {
        if (triple.Predicate is IUriNode predicateNode)
        {
            // Output Node lines for Literal Node so we show them as Boxes
            // This is in keeping with Standard Graph representation of RDF
            // Literals are shown in Boxes, Uri Nodes in ellipses (GraphViz's default shape)
            var subjectId = ProcessNode(triple, TripleSegment.Subject, context, collapseLiterals);
            var objectId = ProcessNode(triple, TripleSegment.Object, context, collapseLiterals);

            // Output the actual lines that state the relationship between the Nodes
            // We use the Predicate as the Label on the relationship
            var predicateLabel = ReduceToQName(predicateNode.Uri, context);

            await PrettifyAsync(Dot.Tab, context);
            await WriteQuotedAsync(subjectId, context);
            await PrettifyAsync(Dot.Space, context);
            await context.Output.WriteAsync(Dot.Arrow);
            await PrettifyAsync(Dot.Space, context);
            await WriteQuotedAsync(objectId, context);
            await PrettifyAsync(Dot.Space, context);
            await context.Output.WriteAsync(Dot.OpenSquare);
            await context.Output.WriteAsync(Dot.Label);
            await PrettifyAsync(Dot.Space, context);
            await context.Output.WriteAsync(Dot.Equal);
            await PrettifyAsync(Dot.Space, context);
            await WriteQuotedAsync(predicateLabel, context);
            await context.Output.WriteAsync(Dot.CloseSquare);
            await context.Output.WriteAsync(Dot.Semicolon);
            await PrettifyAsync(Dot.NewLine, context);
        }
    }

    private static async Task WriteQuotedAsync(string value, IWriterContext context)
    {
        await context.Output.WriteAsync(Dot.Quote);

        await context.Output.WriteAsync(value);

        await context.Output.WriteAsync(Dot.Quote);
    }

    private static async Task WriteLiteralNodeLabelAsync(ILiteralNode literalNode, BaseWriterContext context)
    {
        var nodeValue = Escape(literalNode.Value);

        context.Output.Write(Dot.Quote);
        context.Output.Write(nodeValue);

        if (!string.IsNullOrEmpty(literalNode.Language))
        {
            context.Output.Write("@");
            context.Output.Write(literalNode.Language);
        }

        if (literalNode.DataType != null)
        {
            var datatype = ReduceToQName(literalNode.DataType, context);

            context.Output.Write("^^");
            context.Output.Write(datatype);
        }

        context.Output.Write(Dot.Quote);
    }

    private static async Task PrettifyAsync(string value, BaseWriterContext context)
    {
        if (context.PrettyPrint)
        {
            await context.Output.WriteAsync(value);
        }
    }
}
