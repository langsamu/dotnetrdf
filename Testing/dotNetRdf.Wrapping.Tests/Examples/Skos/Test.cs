using System.Globalization;
using System.Linq;

namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

public class Tests
{
    [Fact]
    public void Test()
    {
        var g = new RDF.Graph();
        g.LoadFromString("""
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
            """);

        var english = CultureInfo.GetCultureInfo("en");
        var german = CultureInfo.GetCultureInfo("de");

        var concept = Graph.Wrap(g).MyConcept("https://elsst.cessda.eu/id/5/f580f88b-a7f4-4d86-aeea-d337c0cff161");

        concept.Related.Count.Should().Be(2);
        concept.Narrower.Count.Should().Be(2);
        concept.PreferredLabels.Count.Should().Be(2);
        concept.PreferredLabels[english].Should().Be("TEACHING");
        concept.PreferredLabels[german].Should().Be("UNTERRICHTEN");
        concept.AlternativeLabels.Count.Should().Be(2);
        concept.AlternativeLabels[english].Should().Be("INSTRUCTION");
        concept.AlternativeLabels[german].Should().Be("LEHREN");
        concept.ScopeNotes.Count.Should().Be(2);
        concept.ScopeNotes[english].Should().Be("SEE ALSO THE TERM 'TEACHING PROFESSION'.");
        concept.ScopeNotes[german].Should().Be("SIEHE AUCH 'LEHRBERUF'.");
        concept.Scheme.TopConcepts.Count.Should().Be(1);
        concept.Scheme.TopConcepts.Single().PreferredLabels.Count.Should().Be(1);
        concept.Scheme.TopConcepts.Single().PreferredLabels[english].Should().Be("EDUCATION");
    }
}
