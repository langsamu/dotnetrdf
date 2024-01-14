﻿/*
dotNetRDF is free and open source software licensed under the MIT License

-----------------------------------------------------------------------------

Copyright (c) 2009-2012 dotNetRDF Project (dotnetrdf-developer@lists.sf.net)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections.Generic;
using VDS.RDF.Query;
using Xunit;
using Xunit.Abstractions;

namespace VDS.RDF.LDF.Client;

public class E2EQpf(ITestOutputHelper output)
{
    private static readonly NodeFactory factory = new();

    protected QpfLiveTripleStore TripleStore = new(new("https://fragments.dbpedia.org/2016-04/en"));

    [Fact]
    public void X1()
    {
        using var quads = TripleStore.Quads.GetEnumerator();

        for (var i = 0; i < 150; i++)
        {
            quads.MoveNext();
            output.WriteLine("{0}", quads.Current);
        }
    }

    [Fact]
    public void X2()
    {
        var r = new LeviathanQueryProcessor(TripleStore).ProcessQuery(Queries.SelectQpf) as SparqlResultSet;
       
        using var ts = this.TripleStore;
        using var quads = ts.Quads.GetEnumerator();

        for (var i = 0; i < 150; i++)
        {
            quads.MoveNext();
            output.WriteLine("{0}", quads.Current);
        }
    }
}
