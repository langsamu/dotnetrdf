using VDS.RDF.Parsing;
using VDS.RDF.Query.Patterns;
using VDS.RDF.Writing;

namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

public class Tests(ITestOutputHelper output)
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

        output.WriteLine(command.ToString());

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

        var patch = InsertDeletePatch.Create(actual);
        patch.Type = Vocabulary.InsertDeletePatch.Uri;


        var deletions = new GraphPattern();
        deletions.AddTriplePattern(
            new TriplePattern(
                new VariablePattern("person"),
                new NodeMatchPattern(
                    actual.CreateUriNode(
                        actual.UriFactory.Create("http://www.example.org/terms#givenName"))),
                new NodeMatchPattern(
                    actual.CreateLiteralNode("Claudia"))));

        var insertions = new GraphPattern();
        insertions.AddTriplePattern(
            new TriplePattern(
                new VariablePattern("person"),
                new NodeMatchPattern(
                    actual.CreateUriNode(
                        actual.UriFactory.Create("http://www.example.org/terms#givenName"))),
                new NodeMatchPattern(
                    actual.CreateLiteralNode("Alex"))));

        var where = new GraphPattern();
        where.AddTriplePattern(
            new TriplePattern(
                new VariablePattern("person"),
                new NodeMatchPattern(
                    actual.CreateUriNode(
                        actual.UriFactory.Create("http://www.example.org/terms#familyName"))),
                new NodeMatchPattern(
                    actual.CreateLiteralNode("Garcia"))));

        patch.Deletions = deletions;
        patch.Inserts = insertions;
        patch.Where = where;

        output.WriteLine(StringWriter.Write(actual, new Notation3Writer()));
    }
}
