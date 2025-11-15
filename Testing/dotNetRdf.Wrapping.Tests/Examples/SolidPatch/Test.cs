using VDS.RDF.Parsing;
using VDS.RDF.Query.Patterns;

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

    private const string sampleBeforePatch = """
        @prefix : <http://www.example.org/terms#>.
        
        <urn:example:>
            :givenName "Claudia" ;
            :familyName "Garcia" ;
        .
        """;

    private const string sampleAfterPatch = """
        @prefix : <http://www.example.org/terms#>.
        
        <urn:example:>
            :givenName "Alex" ;
            :familyName "Garcia" ;
        .
        """;

    [Fact]
    public void Read()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf, new Notation3Parser());

        var sample = new RDF.Graph();
        sample.LoadFromString(sampleBeforePatch);

        var expected = new RDF.Graph();
        expected.LoadFromString(sampleAfterPatch);

        var command = Graph.Wrap(actual).Command;

        var ts = new TripleStore();
        ts.Add(sample);
        ts.ExecuteUpdate(command);

        sample.Should().BeIsomorphicWith(expected);
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

        var expected = new RDF.Graph();
        expected.LoadFromString(originalRdf, new Notation3Parser());

        actual.Should().BeIsomorphicWith(expected);
    }
}
