/*
dotNetRDF is free and open source software licensed under the MIT License

-----------------------------------------------------------------------------

Copyright (c) 2009-2013 dotNetRDF Project (dotnetrdf-developer@lists.sf.net)

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

namespace VDS.RDF.Shacl
{
    using VDS.RDF.Writing;
    using Xunit;
    using Xunit.Abstractions;

    public class Advanced
    {
        private readonly ITestOutputHelper output;

        public Advanced(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SparqlTarget()
        {
            var dataGraph = new Graph();
            dataGraph.LoadFromString(@"
@prefix : <urn:> .

:ignored a :C0 .
:invalid a :C1 .
:valid a :C1, :C2 .
");

            var shapesGraph = new Graph();
            shapesGraph.LoadFromString(@"
@prefix : <urn:> .
@prefix sh: <http://www.w3.org/ns/shacl#> .
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .

[
    sh:target [
        a sh:SPARQLTarget ;
        sh:prefixes [
            sh:declare [
                sh:prefix ""ex"" ;
                sh:namespace ""urn:""^^xsd:anyUri ;
            ] ;
        ] ;
        sh:select """"""
            SELECT ?this
            WHERE {
                ?this a ex:C1.
            }
            """""" ;
    ] ;
    sh:class :C2
] .
");

            var expected = new Graph();
            expected.LoadFromString(@"
@prefix : <urn:> .
@prefix sh: <http://www.w3.org/ns/shacl#> .

[
    a sh:ValidationReport ;
    sh:conforms false ;
    sh:result [
        a sh:ValidationResult ;
        sh:sourceConstraintComponent sh:ClassConstraintComponent ;
        sh:resultSeverity sh:Violation ;
        sh:sourceShape [] ;
        sh:focusNode :invalid ;
        sh:value :invalid
    ]
] .
");

            var processor = new ShapesGraph(shapesGraph);
            var actual = processor.Validate(dataGraph).Normalised;

            var writer = new CompressingTurtleWriter();
            output.WriteLine(StringWriter.Write(expected, writer));
            output.WriteLine(StringWriter.Write(actual, writer));

            Assert.Equal(expected, actual);
        }
    }
}
