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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VDS.RDF.Writing;

partial class BaseRdfWriter
{
    /// <inheritdoc/>
    public virtual async Task SaveAsync(IGraph g, string filename, CancellationToken ct = default) =>
        await SaveAsync(g, filename, new UTF8Encoding(), ct);

    /// <inheritdoc/>
    public virtual async Task SaveAsync(IGraph g, string filename, Encoding fileEncoding, CancellationToken ct = default)
    {
        using FileStream stream = File.Open(filename, FileMode.Create);
        await SaveAsync(g, new StreamWriter(stream, fileEncoding), ct);
    }

    /// <inheritdoc/>
    public async Task SaveAsync(IGraph g, TextWriter output, CancellationToken ct = default) =>
        await SaveAsync(g, output, false, ct);

    /// <inheritdoc/>
    public async Task SaveAsync(IGraph g, TextWriter output, bool leaveOpen, CancellationToken ct = default)
    {
        try
        {
            await SaveInternalAsync(g, output, ct);
            if (!leaveOpen)
            {
                output.Close();
            }
        }
        catch (Exception)
        {
            if (!leaveOpen)
            {
                try
                {
                    output.Close();
                }
                catch (Exception)
                {
                    // No handling, just clean up
                }
            }
            throw;
        }
    }

    /// <summary>
    /// Method to be implemented in derived classes to perform the actual writing to a TextWriter.
    /// </summary>
    /// <param name="graph">The graph to be saved.</param>
    /// <param name="output">The <see cref="TextWriter"/> to save the graph to.</param>
    protected abstract Task SaveInternalAsync(IGraph graph, TextWriter output, CancellationToken ct = default);
}
