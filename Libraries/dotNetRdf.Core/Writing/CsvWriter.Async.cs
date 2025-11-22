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

namespace VDS.RDF.Writing;

/// <summary>
/// Class for generating CSV output from RDF Graphs.
/// </summary>
partial class CsvWriter
{
    protected override async Task SaveInternalAsync(IGraph g, TextWriter output, CancellationToken ct = default)
    {
        foreach (Triple t in g.Triples)
        {
            if (ct.IsCancellationRequested)
            {
                Warning?.Invoke("Cancellation requested");
                break;
            }

            await GenerateNodeOutputAsync(output, t.Subject, TripleSegment.Subject);
            await output.WriteAsync(',');
            await GenerateNodeOutputAsync(output, t.Predicate, TripleSegment.Predicate);
            await output.WriteAsync(',');
            await GenerateNodeOutputAsync(output, t.Object, TripleSegment.Object);
            await output.WriteAsync("\r\n");
        }
    }

    /// <summary>
    /// Generates Node Output for the given Node.
    /// </summary>
    /// <param name="output">Text Writer.</param>
    /// <param name="n">Node.</param>
    /// <param name="segment">Triple Segment.</param>
    private async Task GenerateNodeOutputAsync(TextWriter output, INode n, TripleSegment segment)
    {
        switch (n.NodeType)
        {
            case NodeType.Blank:
                if (segment == TripleSegment.Predicate) throw new RdfOutputException(WriterErrorMessages.BlankPredicatesUnserializable("CSV"));

                await output.WriteAsync(_formatter.Format(n));
                break;

            case NodeType.GraphLiteral:
                throw new RdfOutputException(WriterErrorMessages.GraphLiteralsUnserializable("CSV"));

            case NodeType.Literal:
                if (segment == TripleSegment.Subject) throw new RdfOutputException(WriterErrorMessages.LiteralSubjectsUnserializable("CSV"));
                if (segment == TripleSegment.Predicate) throw new RdfOutputException(WriterErrorMessages.LiteralPredicatesUnserializable("CSV"));

                await output.WriteAsync(_formatter.Format(n));
                break;

            case NodeType.Uri:
                await output.WriteAsync(_formatter.Format(n));
                break;

            default:
                throw new RdfOutputException(WriterErrorMessages.UnknownNodeTypeUnserializable("CSV"));
        }
    }
}
