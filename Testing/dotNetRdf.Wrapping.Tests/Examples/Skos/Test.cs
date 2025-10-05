using System.Globalization;
using System.Linq;

namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

public class Tests
{
    private const string originalRdf = """
        @base <https://elsst.cessda.eu/id/5/> .
        @prefix : <http://www.w3.org/2004/02/skos/core#> .
        
        <> :hasTopConcept <c5310e73-4a7f-4ef6-b8d4-6fe62cef9281> .
        
        <f580f88b-a7f4-4d86-aeea-d337c0cff161>
          :inScheme <> ;
          :narrower
        	<0f2d5c7b-3928-4399-9e93-885d40aabafa>,
        	<93bcf817-ee8f-4ba3-b7d2-6494002cdc0f> ;
          :related
        	<822064c0-ec6c-4a79-9975-f466303a4a00>,
        	<af18c478-a424-4e7d-8e00-3efdd649f7d1> ;
          :prefLabel
        	"TEACHING"@en,
        	"UNTERRICHTEN"@de ;
          :altLabel
        	"INSTRUCTION"@en,
        	"LEHREN"@de ;
          :scopeNote
        	"SEE ALSO THE TERM 'TEACHING PROFESSION'."@en,
        	"SIEHE AUCH 'LEHRBERUF'."@de ;
        .
        
        <c5310e73-4a7f-4ef6-b8d4-6fe62cef9281> :prefLabel "EDUCATION"@en .
        <0f2d5c7b-3928-4399-9e93-885d40aabafa> :prefLabel "TRAINING"@en .
        <822064c0-ec6c-4a79-9975-f466303a4a00> :prefLabel "LEHR- UND LERNMETHODEN"@de .
        <93bcf817-ee8f-4ba3-b7d2-6494002cdc0f> :prefLabel "TEACHING SKILLS"@en .
        <af18c478-a424-4e7d-8e00-3efdd649f7d1> :prefLabel "TEACHING ASSISTANTS"@en .
        """;

    private const string modifiedRdf = """
        @base <https://elsst.cessda.eu/id/5/> .
        @prefix : <http://www.w3.org/2004/02/skos/core#> .
        
        <> :hasTopConcept <c5310e73-4a7f-4ef6-b8d4-6fe62cef9281> .
        
        <f580f88b-a7f4-4d86-aeea-d337c0cff161>
          :inScheme <> ;
          :narrower
        	<0f2d5c7b-3928-4399-9e93-885d40aabafa>,
        	<93bcf817-ee8f-4ba3-b7d2-6494002cdc0f> ;
          :related
        	<822064c0-ec6c-4a79-9975-f466303a4a00>,
        	<af18c478-a424-4e7d-8e00-3efdd649f7d1> ;
          :prefLabel
        	"fc3fe4864c7743309c137b2b30e71365"@en,
        	"ff6c754e4bb94f74ae5eb8872c305192"@de ;
          :altLabel
        	"047b1904f59243179ceb50dd92c85ddb"@en,
        	"6eab7abba3db458c9d90e91397ff92e8"@de ;
          :scopeNote
        	"3e278f44b03040c48dda3bbdde266b47"@en,
        	"520ddaacf1264c2e86b3a6f72f1ede66"@de ;
        .
        
        <c5310e73-4a7f-4ef6-b8d4-6fe62cef9281> :prefLabel "1c93be3560694be2a6ae472a0cfd3805"@en .
        <0f2d5c7b-3928-4399-9e93-885d40aabafa> :prefLabel "5c15375fe4eb4836bb8604fc03a6b510"@en .
        <822064c0-ec6c-4a79-9975-f466303a4a00> :prefLabel "8be808d694f74404ac36db8646d07b6d"@de .
        <93bcf817-ee8f-4ba3-b7d2-6494002cdc0f> :prefLabel "b288ef9e6fac4d6d9687b66b866db090"@en .
        <af18c478-a424-4e7d-8e00-3efdd649f7d1> :prefLabel "f63cd0dd94f844ada9cb813cccc37cd4"@en .
        """;

    [Fact]
    public void Read()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf);

        var english = CultureInfo.GetCultureInfo("en");
        var german = CultureInfo.GetCultureInfo("de");

        var concept = Graph.Wrap(actual).MyConcept("https://elsst.cessda.eu/id/5/f580f88b-a7f4-4d86-aeea-d337c0cff161");

        concept.Related.Should().HaveCount(2);
        concept.Narrower.Should().HaveCount(2);
        concept.PreferredLabels.Should().HaveCount(2);
        concept.PreferredLabels[english].Should().Be("TEACHING");
        concept.PreferredLabels[german].Should().Be("UNTERRICHTEN");
        concept.AlternativeLabels.Should().HaveCount(2);
        concept.AlternativeLabels[english].Should().Be("INSTRUCTION");
        concept.AlternativeLabels[german].Should().Be("LEHREN");
        concept.ScopeNotes.Should().HaveCount(2);
        concept.ScopeNotes[english].Should().Be("SEE ALSO THE TERM 'TEACHING PROFESSION'.");
        concept.ScopeNotes[german].Should().Be("SIEHE AUCH 'LEHRBERUF'.");
        concept.Narrower.First().PreferredLabels[english].Should().Be("TRAINING");
        concept.Narrower.Last().PreferredLabels[english].Should().Be("TEACHING SKILLS");
        concept.Related.First().PreferredLabels[german].Should().Be("LEHR- UND LERNMETHODEN");
        concept.Related.Last().PreferredLabels[english].Should().Be("TEACHING ASSISTANTS");
        concept.Scheme.TopConcepts.Should().HaveCount(1);
        concept.Scheme.TopConcepts.Single().PreferredLabels.Should().HaveCount(1);
        concept.Scheme.TopConcepts.Single().PreferredLabels[english].Should().Be("EDUCATION");
    }

    [Fact]
    public void Write()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf);

        var concept = Graph.Wrap(actual).MyConcept("https://elsst.cessda.eu/id/5/f580f88b-a7f4-4d86-aeea-d337c0cff161");

        var english = CultureInfo.GetCultureInfo("en");
        var german = CultureInfo.GetCultureInfo("de");

        concept.PreferredLabels[english] = "fc3fe4864c7743309c137b2b30e71365";
        concept.PreferredLabels[german] = "ff6c754e4bb94f74ae5eb8872c305192";
        concept.AlternativeLabels[english] = "047b1904f59243179ceb50dd92c85ddb";
        concept.AlternativeLabels[german] = "6eab7abba3db458c9d90e91397ff92e8";
        concept.ScopeNotes[english] = "3e278f44b03040c48dda3bbdde266b47";
        concept.ScopeNotes[german] = "520ddaacf1264c2e86b3a6f72f1ede66";
        concept.Narrower.First().PreferredLabels[english] = "5c15375fe4eb4836bb8604fc03a6b510";
        concept.Narrower.Last().PreferredLabels[english] = "b288ef9e6fac4d6d9687b66b866db090";
        concept.Related.First().PreferredLabels[german] = "8be808d694f74404ac36db8646d07b6d";
        concept.Related.Last().PreferredLabels[english] = "f63cd0dd94f844ada9cb813cccc37cd4";
        concept.Scheme.TopConcepts.Single().PreferredLabels[english] = "1c93be3560694be2a6ae472a0cfd3805";

        var expected = new RDF.Graph();
        expected.LoadFromString(modifiedRdf);

        actual.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void Clear()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf);

        var concept = Graph.Wrap(actual).MyConcept("https://elsst.cessda.eu/id/5/f580f88b-a7f4-4d86-aeea-d337c0cff161");

        var narrower = concept.Narrower;
        var related = concept.Related;
        var topConcepts = concept.Scheme.TopConcepts;
        var preferredLabels = concept.PreferredLabels;
        var alternativeLabels = concept.AlternativeLabels;
        var scopeNotes = concept.ScopeNotes;
        var allConcepts = narrower.Union(related).Union(topConcepts).Union([concept]);
        var allRelations = new[] { narrower, related, topConcepts };

        foreach (var c in allConcepts)
        {
            c.PreferredLabels.Clear();
            c.AlternativeLabels.Clear();
            c.ScopeNotes.Clear();
        }

        foreach (var r in allRelations)
        {
            r.Clear();
        }

        concept.Scheme = null;

        actual.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Create()
    {
        var actual = new RDF.Graph();

        var english = CultureInfo.GetCultureInfo("en");
        var german = CultureInfo.GetCultureInfo("de");

        var concept = Graph.Wrap(actual).MyConcept("https://elsst.cessda.eu/id/5/f580f88b-a7f4-4d86-aeea-d337c0cff161");
        var scheme = ConceptScheme.Create("https://elsst.cessda.eu/id/5/", actual);
        var topConcept = Concept.Create("https://elsst.cessda.eu/id/5/c5310e73-4a7f-4ef6-b8d4-6fe62cef9281", actual);
        var narrower1 = Concept.Create("https://elsst.cessda.eu/id/5/0f2d5c7b-3928-4399-9e93-885d40aabafa", actual);
        var narrower2 = Concept.Create("https://elsst.cessda.eu/id/5/93bcf817-ee8f-4ba3-b7d2-6494002cdc0f", actual);
        var related1 = Concept.Create("https://elsst.cessda.eu/id/5/822064c0-ec6c-4a79-9975-f466303a4a00", actual);
        var related2 = Concept.Create("https://elsst.cessda.eu/id/5/af18c478-a424-4e7d-8e00-3efdd649f7d1", actual);

        concept.PreferredLabels[english] = "TEACHING";
        concept.PreferredLabels[german] = "UNTERRICHTEN";
        concept.AlternativeLabels[english] = "INSTRUCTION";
        concept.AlternativeLabels[german] = "LEHREN";
        concept.ScopeNotes[english] = "SEE ALSO THE TERM 'TEACHING PROFESSION'.";
        concept.ScopeNotes[german] = "SIEHE AUCH 'LEHRBERUF'.";
        concept.Scheme = scheme;
        scheme.TopConcepts.Add(topConcept);
        topConcept.PreferredLabels[english] = "EDUCATION";
        concept.Narrower.Add(narrower1);
        narrower1.PreferredLabels[english] = "TRAINING";
        concept.Narrower.Add(narrower2);
        narrower2.PreferredLabels[english] = "TEACHING SKILLS";
        concept.Related.Add(related1);
        related1.PreferredLabels[german] = "LEHR- UND LERNMETHODEN";
        concept.Related.Add(related2);
        related2.PreferredLabels[english] = "TEACHING ASSISTANTS";

        var expected = new RDF.Graph();
        expected.LoadFromString(originalRdf);

        actual.Should().BeIsomorphicWith(expected);
    }
}
