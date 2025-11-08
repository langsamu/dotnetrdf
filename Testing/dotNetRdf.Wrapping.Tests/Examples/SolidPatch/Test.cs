using System;
using VDS.RDF.Parsing;
using VDS.RDF.Query.Patterns;
using VDS.RDF.Update.Commands;

namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

public class Tests
{
    private const string originalRdf = """
        @prefix solid: <http://www.w3.org/ns/solid/terms#>.
        @prefix ex: <http://www.example.org/terms#>.
        
        _:rename a solid:InsertDeletePatch;
          solid:where   { ?person ex:familyName "Garcia". };
          solid:inserts { ?person ex:givenName "Alex". };
          solid:deletes { ?person ex:givenName "Claudia". }.
        """;

    private const string modifiedRdf = """
        x
        """;

    [Fact]
    public void Read()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf, new Notation3Parser());

        var command = Graph.Wrap(actual).Command;

        Console.WriteLine(command);

    }

    [Fact]
    public void Write()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf, new Notation3Parser());

    }

    [Fact]
    public void Clear()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf, new Notation3Parser());

    }

    [Fact]
    public void Create()
    {
        var actual = new RDF.Graph();
        var x = InsertDeletePatch.Create(actual);
        x.Type = Vocabulary.InsertDeletePatch.Uri;

        Graph.Wrap(actual).Command = new ModifyCommand(
            new GraphPattern(),
            new GraphPattern(),
            new GraphPattern());
    }
}
