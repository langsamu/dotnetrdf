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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VDS.RDF.Parsing;
using VDS.RDF.Writing.Contexts;
using VDS.RDF.Writing.Formatting;

namespace VDS.RDF.Writing;

/// <summary>
/// Class for generating Notation 3 Concrete RDF Syntax which provides varying levels of Syntax Compression.
/// </summary>
/// <threadsafety instance="true">Designed to be Thread Safe - should be able to call the Save() method from multiple threads on different Graphs without issue.</threadsafety>
partial class Notation3Writer
{
    /// <summary>
    /// Saves a Graph to the given Stream using Notation 3 Syntax.
    /// </summary>
    /// <param name="g">Graph to save.</param>
    /// <param name="output">Stream to save to.</param>
    protected override async Task SaveInternalAsync(IGraph g, TextWriter output, CancellationToken ct = default)
    {
        g.NamespaceMap.Import(_defaultNamespaces);
        var context = new CompressingTurtleWriterContext(g, output, CompressionLevel, _prettyprint, _allowHiSpeed);
        context.NodeFormatter = new Notation3Formatter(g);
        await GenerateOutputAsync(context, ct);
    }

    /// <summary>
    /// Generates the Notation 3 Syntax for the Graph.
    /// </summary>
    private async Task GenerateOutputAsync(CompressingTurtleWriterContext context, CancellationToken ct)
    {
        // Create the Header
        // Base Directive
        if (context.Graph.BaseUri != null)
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
        var contextWritten = false;
        double subjNodes = context.Graph.Triples.SubjectNodes.Count();
        double triples = context.Graph.Triples.Count;
        if ((subjNodes / triples) > 0.75) hiSpeed = true;

        if (context.CompressionLevel == WriterCompressionLevel.None || (hiSpeed && context.HighSpeedModePermitted))
        {
            RaiseWarning("High Speed Write Mode in use - minimal syntax compression will be used");
            context.CompressionLevel = WriterCompressionLevel.Minimal;
            context.NodeFormatter = new UncompressedNotation3Formatter();

            foreach (Triple t in context.Graph.Triples)
            {
                if (!contextWritten && t.Context != null && t.Context is VariableContext)
                {
                    var varContext = (VariableContext)t.Context;
                    contextWritten = GenerateVariableQuantificationOutput(context, varContext);
                }
                await context.Output.WriteLineAsync(GenerateTripleOutput(context, t));
            }
        }
        else
        {
            if (context.CompressionLevel >= WriterCompressionLevel.More)
            {
                WriterHelper.FindCollections(context);
            }

            // Get the Triples as a Sorted List
            var ts = context.Graph.Triples.Where(t => !context.TriplesDone.Contains(t)).ToList();
            ts.Sort(new FullTripleComparer(new FastNodeComparer()));

            // Variables we need to track our writing
            INode lastSubj, lastPred;
            lastSubj = lastPred = null;
            int subjIndent = 0, predIndent = 0;
            string temp;

            for (var i = 0; i < ts.Count; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    RaiseWarning("Cancellation requested. ");
                    break;
                }

                Triple t = ts[i];

                if (lastSubj == null || !t.Subject.Equals(lastSubj) || (t.Context != null && t.Context is VariableContext))
                {
                    // Terminate previous Triples
                    if (lastSubj != null) await context.Output.WriteLineAsync(".");

                    // If there's a Variable Context insert the @forAll and @forSome
                    if (!contextWritten && t.Context != null && t.Context is VariableContext)
                    {
                        var varContext = (VariableContext)t.Context;
                        contextWritten = GenerateVariableQuantificationOutput(context, varContext);
                    }

                    // Start a new set of Triples
                    temp = GenerateNodeOutput(context, t.Subject, TripleSegment.Subject, 0);
                    await context.Output.WriteAsync(temp);
                    await context.Output.WriteAsync(" ");
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
                    await context.Output.WriteAsync(temp);
                    await context.Output.WriteAsync(" ");
                    predIndent = temp.Length + 1;
                    lastPred = t.Predicate;
                }
                else if (lastPred == null || !t.Predicate.Equals(lastPred))
                {
                    // Terminate previous Predicate Object list
                    await context.Output.WriteLineAsync(";");

                    if (context.PrettyPrint) await context.Output.WriteAsync(new string(' ', subjIndent));

                    // Write the next Predicate
                    temp = GenerateNodeOutput(context, t.Predicate, TripleSegment.Predicate, subjIndent);
                    await context.Output.WriteAsync(temp);
                    await context.Output.WriteAsync(" ");
                    predIndent = temp.Length + 1;
                    lastPred = t.Predicate;
                }
                else
                {
                    // Continue Object List
                    await context.Output.WriteLineAsync(",");

                    if (context.PrettyPrint) await context.Output.WriteAsync(new string(' ', subjIndent + predIndent));
                }

                // Write the Object
                await context.Output.WriteAsync(GenerateNodeOutput(context, t.Object, TripleSegment.Object, subjIndent + predIndent));
            }

            // Terminate Triples
            if (ts.Count > 0) await context.Output.WriteLineAsync(".");

            return;
        }

    }
}
