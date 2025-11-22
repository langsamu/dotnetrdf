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

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VDS.RDF.Parsing;
using VDS.RDF.Writing.Contexts;
using VDS.RDF.Writing.Formatting;

namespace VDS.RDF.Writing;


/// <summary>
/// Class for generating Turtle Concrete RDF Syntax which provides varying levels of Syntax Compression.
/// </summary>
/// <remarks>
/// Similar in speed to the standard <see cref="TurtleWriter">TurtleWriter</see> but capable of using more syntax compressions depending on the Compression level set.
/// </remarks>
/// <threadsafety instance="true">Designed to be Thread Safe - should be able to call the Save() method from multiple threads on different Graphs without issue.</threadsafety>
partial class CompressingTurtleWriter
{
    /// <summary>
    /// Saves a Graph to the given Stream using Turtle Syntax.
    /// </summary>
    /// <param name="g">Graph to save.</param>
    /// <param name="output">Stream to save to.</param>
    protected override async Task SaveInternalAsync(IGraph g, TextWriter output, CancellationToken ct = default)
    {
        // Create the Writing Context
        g.NamespaceMap.Import(DefaultNamespaces);
        var context = new CompressingTurtleWriterContext(g, output, CompressionLevel, PrettyPrintMode, HighSpeedModePermitted, _syntax);
        await GenerateOutputAsync(context, ct);
    }

    /// <summary>
    /// Generates the Turtle Syntax for the Graph.
    /// </summary>
    private async Task GenerateOutputAsync(CompressingTurtleWriterContext context, CancellationToken ct)
    {
        // Create the Header
        // Base Directive
        if (context.Graph.BaseUri is not null)
        {
            await context.Output.WriteLineAsync("@base <" + context.UriFormatter.FormatUri(context.Graph.BaseUri) + ">.");
            await context.Output.WriteLineAsync();
        }
        // Prefix Directives
        foreach (var prefix in context.Graph.NamespaceMap.Prefixes)
        {
            if (TurtleSpecsHelper.IsValidQName(prefix + ":"))
            {
                if (!prefix.Equals(string.Empty))
                {
                    await context.Output.WriteLineAsync("@prefix " + prefix + ": <" + context.UriFormatter.FormatUri(context.Graph.NamespaceMap.GetNamespaceUri(prefix)) + ">.");
                }
                else
                {
                    await context.Output.WriteLineAsync("@prefix : <" + context.UriFormatter.FormatUri(context.Graph.NamespaceMap.GetNamespaceUri(string.Empty)) + ">.");
                }
            }
        }
        await context.Output.WriteLineAsync();

        // Decide on the Write Mode to use
        var hiSpeed = false;
        double subjNodes = context.Graph.Triples.SubjectNodes.Count();
        double triples = context.Graph.Triples.Count;
        if ((subjNodes / triples) > 0.75) hiSpeed = true;

        if (context.CompressionLevel == WriterCompressionLevel.None || (hiSpeed && context.HighSpeedModePermitted))
        {
            RaiseWarning("High Speed Write Mode in use - minimal syntax compression will be used");
            context.CompressionLevel = WriterCompressionLevel.Minimal;
            context.NodeFormatter = _syntax == TurtleSyntax.Rdf11Star ? new UncompressedTurtleStarFormatter() : new UncompressedTurtleFormatter();

            foreach (Triple t in context.Graph.Triples)
            {
                await context.Output.WriteLineAsync(GenerateTripleOutput(context, t));
            }
        }
        else
        {
            if (context.CompressionLevel >= WriterCompressionLevel.More)
            {
                WriterHelper.FindCollections(context);
                if (_syntax == TurtleSyntax.Rdf11Star) WriterHelper.FindAnnotations(context);
            }

            // Get the Triples as a Sorted List
            var ts = context.Graph.Triples.Where(t => !context.TriplesDone.Contains(t)).ToList();
            WriterHelper.SortTriplesBySubjectPredicate(ts);

            // Variables we need to track our writing
            INode lastSubj, lastPred;
            lastSubj = lastPred = null;
            int subjIndent = 0, predIndent = 0;
            string temp;

            foreach (Triple t in ts)
            {
                if (ct.IsCancellationRequested)
                {
                    RaiseWarning("Cancellation requested");
                    break;
                }

                if (lastSubj == null || !t.Subject.Equals(lastSubj))
                {
                    // Terminate previous Triples
                    if (lastSubj != null) await context.Output.WriteLineAsync(".");

                    // Start a new set of Triples
                    temp = GenerateNodeOutput(context, t.Subject, TripleSegment.Subject, 0);
                    context.Output.Write(temp);
                    context.Output.Write(" ");
                    if (temp.Contains('\n'))
                    {
                        subjIndent = temp.Split('\n').Last().Length + 1;
                    }
                    else
                    {
                        subjIndent = temp.Length + 1;
                    }
                    lastSubj = t.Subject;

                    // Write the first Predicate
                    temp = GenerateNodeOutput(context, t.Predicate, TripleSegment.Predicate, subjIndent);
                    context.Output.Write(temp);
                    context.Output.Write(" ");
                    predIndent = temp.Length + 1;
                    lastPred = t.Predicate;
                }
                else if (lastPred == null || !t.Predicate.Equals(lastPred))
                {
                    // Terminate previous Predicate Object list
                    await context.Output.WriteLineAsync(";");

                    if (context.PrettyPrint) context.Output.Write(new string(' ', subjIndent));

                    // Write the next Predicate
                    temp = GenerateNodeOutput(context, t.Predicate, TripleSegment.Predicate, subjIndent);
                    context.Output.Write(temp);
                    context.Output.Write(" ");
                    predIndent = temp.Length + 1;
                    lastPred = t.Predicate;
                }
                else
                {
                    // Continue Object List
                    await context.Output.WriteLineAsync(",");

                    if (context.PrettyPrint) context.Output.Write(new string(' ', subjIndent + predIndent));
                }

                // Write the Object
                temp = GenerateNodeOutput(context, t.Object, TripleSegment.Object, subjIndent + predIndent);
                await context.Output.WriteAsync(temp);

                // Write any annotations on the object
                if (context.Annotations.ContainsKey(t))
                {
                    await context.Output.WriteAsync(GenerateAnnotationOutput(context, context.Annotations[t], subjIndent + predIndent + temp.Length + 1));
                }
            }

            // Terminate Triples
            if (ts.Count > 0) await context.Output.WriteLineAsync(".");
        }
    }
}
