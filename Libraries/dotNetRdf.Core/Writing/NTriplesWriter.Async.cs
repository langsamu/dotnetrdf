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
using VDS.RDF.Writing.Contexts;

namespace VDS.RDF.Writing;

/// <summary>
/// Class for generating RDF in NTriples Concrete Syntax.
/// </summary>
/// <threadsafety instance="true">Designed to be Thread Safe - should be able to call the Save() method from multiple threads on different Graphs without issue.</threadsafety>
partial class NTriplesWriter
{
    /// <summary>
    /// Saves the Graph in NTriples Syntax to the given stream.
    /// </summary>
    /// <param name="g">Graph to save.</param>
    /// <param name="filename">File to save to.</param>
    public override async Task SaveAsync(IGraph g, string filename, CancellationToken ct = default)
    {
        using var writer = new StreamWriter(File.Open(filename, FileMode.Create), Encoding.ASCII);
        await SaveAsync(g, writer, ct);
    }

    /// <summary>
    /// Saves the Graph in NTriples Syntax to the given stream.
    /// </summary>
    /// <param name="g">Graph to save.</param>
    /// <param name="output">Stream to save to.</param>
    protected override async Task SaveInternalAsync(IGraph g, TextWriter output, CancellationToken ct)
    {
        var context = new NTriplesWriterContext(g, output, Syntax);
        var ts = g.Triples.ToList();
        if (_sort) ts.Sort(new FullTripleComparer(new FastNodeComparer()));

        foreach (Triple t in ts)
        {
            if (ct.IsCancellationRequested)
            {
                RaiseWarning("Cancellation requested");
                break;
            }

            await output.WriteLineAsync(TripleToNTriples(context, t));
        }
    }
}
