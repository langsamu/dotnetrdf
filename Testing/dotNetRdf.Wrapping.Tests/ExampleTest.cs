using VDS.RDF.Parsing;

namespace VDS.RDF.Wrapping;

public class MyTestClass
{
    [Fact]
    public void shacl1()
    {
        var g = new Graph();
        g.LoadFromString("""
            @prefix ex: <http://example.com/> .
            @prefix sh: <http://www.w3.org/ns/shacl#> .
            @prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
            @prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .

            ex:PersonShape
                a sh:NodeShape ;
                sh:targetClass ex:Person ;
                sh:property [
            	    sh:path ex:ssn ;
            	    sh:maxCount 1 ;
            	    sh:datatype xsd:string ;
            	    sh:pattern "^\\d{3}-\\d{2}-\\d{4}$" ;
                ] ;
                sh:property [
            	    sh:path ex:worksFor ;
            	    sh:class ex:Company ;
            	    sh:nodeKind sh:IRI ;
                ] ;
                sh:closed true ;
                sh:ignoredProperties ( rdf:type ) .
            """);
    }

    [Fact]
    public void shacl2()
    {
        var g = new Graph();
        g.LoadFromString("""
            @prefix ex: <http://example.com/> .
            @prefix sh: <http://www.w3.org/ns/shacl#> .
            @prefix xsd: <http://www.w3.org/2001/XMLSchema#> .

            [	a sh:ValidationReport ;
            	sh:conforms false ;
            	sh:result
            	[	a sh:ValidationResult ;
            	    sh:resultSeverity sh:Violation ;
            		sh:focusNode ex:Alice ;
            		sh:resultPath ex:ssn ;
            		sh:value "987-65-432A" ;
            		sh:sourceConstraintComponent sh:RegexConstraintComponent ;
            		sh:sourceShape [] ;
            	] ,
            	[	a sh:ValidationResult ;
            		sh:resultSeverity sh:Violation ;
            		sh:focusNode ex:Bob ;
            		sh:resultPath ex:ssn ;
            		sh:sourceConstraintComponent sh:MaxCountConstraintComponent ;
            		sh:sourceShape [] ;
            	] ,
            	[	a sh:ValidationResult ;
            		sh:resultSeverity sh:Violation ;
            		sh:focusNode ex:Calvin ;
            		sh:resultPath ex:worksFor ;
            		sh:value ex:UntypedCompany ;
            		sh:sourceConstraintComponent sh:ClassConstraintComponent ;
            		sh:sourceShape [] ;
            	] ,
            	[	a sh:ValidationResult ;
            		sh:resultSeverity sh:Violation ;
            		sh:focusNode ex:Calvin ;
            		sh:resultPath ex:birthDate ;
            		sh:value "1971-07-07"^^xsd:date ;
            		sh:sourceConstraintComponent sh:ClosedConstraintComponent ;
            		sh:sourceShape sh:PersonShape ;
            	] 
            ] .
            """);
    }

    [Fact]
    public void solidn3patch()
    {
        var g = new Graph();
        g.LoadFromString("""
            @prefix solid: <http://www.w3.org/ns/solid/terms#>.
            @prefix ex: <http://www.example.org/terms#>.

            _:rename a solid:InsertDeletePatch;
              solid:where   { ?person ex:familyName "Garcia". };
              solid:inserts { ?person ex:givenName "Alex". };
              solid:deletes { ?person ex:givenName "Claudia". }.
            """, new Notation3Parser());
    }

    [Fact]
    public void webid()
    {
        var g = new Graph();
        g.LoadFromString("""
            @base <https://bob.example.org/> . # Added by test author

            @prefix foaf: <http://xmlns.com/foaf/0.1/> .

            <> a foaf:PersonalProfileDocument ;
               foaf:maker <#me> ;
               foaf:primaryTopic <#me> .

            <#me> a foaf:Person ;
               foaf:name "Bob" ;
               foaf:knows <https://example.edu/p/Alice#MSc> ;
               foaf:img <https://bob.example.org/picture.jpg> .
            """);
    }

    [Fact]
    public void tna1()
    {
        var g = new Graph();
        g.LoadFromString("""
            @prefix premis: <http://www.loc.gov/premis/rdf/v3/> .
            @prefix schema: <https://schema.org/> .
            @prefix dct:    <http://purl.org/dc/terms/> .
            @prefix rdf:    <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
            @prefix cat:    <http://catalogue.nationalarchives.gov.uk/> .
            @prefix odrl:   <http://www.w3.org/ns/odrl/2/> .

            cat:ADM.2021.2NW6VY.P.1
            	a premis:IntellectualEntity ;
            	schema:identifier [
            	    a schema:identifier ;
            		schema:propertyID <http://www.nationalarchives.gov.uk/ont.ccr> ;
            		schema:value "ADM 53/121006" ;
            	] ;
            	dct:abstract "<scopecontent><p>BIRMINGHAM</p></scopecontent>"^^rdf:XMLLiteral ;
            	dct:accessRights [
            	    a dct:RightsStatement ;
            		odrl:hasPolicy
                        cat:policy.Open_Description,
                        cat:policy.Normal_Closure_before_FOI_Act_30_years_from_1945-05-31 ;
            	] .
            """);
    }

    [Fact]
    public void parliament()
    {
        var g = new Graph();
        g.LoadFromString("""
            <?xml version="1.0" encoding="utf-8" ?>
            <rdf:RDF xmlns="https://id.parliament.uk/schema/" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:xsd="http://www.w3.org/2001/XMLSchema#" xml:base="https://id.parliament.uk/">
              <WorkPackagedThing rdf:about="mkmxygej">
                <name>Merchant Shipping (Maritime Labour Convention and Miscellaneous Amendments) Regulations 2025</name>
                <workPackagedThingHasWorkPackage>
                  <rdf:Description rdf:about="TKkRxutd">
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="4xSirrXM">
                        <businessItemDate rdf:datatype="xsd:date">2025-09-16+01:00</businessItemDate>
                        <businessItemHasBusinessItemWebLink rdf:resource="https://publications.parliament.uk/pa/ld5901/ldselect/ldsecleg/178/17803.htm"/>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="JcCYkCt7">
                            <name>Considered by the Secondary Legislation Scrutiny Committee (SLSC)</name>
                            <procedureStepHasHouse rdf:resource="WkUWUBMx"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="nvbZn0Tj"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="t8ZjDv8Y">
                            <name>Secondary Legislation Scrutiny Committee (SLSC) agreed that the instrument should follow the negative procedure</name>
                            <procedureStepHasHouse rdf:resource="WkUWUBMx"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="pVLiOfek"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="AoaVQB27">
                        <businessItemDate rdf:datatype="xsd:date">2025-10-16+01:00</businessItemDate>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="3TPVFlNJ">
                            <name>Committee sifting period ends</name>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="U6Lcx9Q9"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="Gz5YyYNd">
                        <businessItemDate rdf:datatype="xsd:date">2025-09-05+01:00</businessItemDate>
                        <businessItemHasBusinessItemWebLink rdf:resource="https://lordsbusiness.parliament.uk/ItemOfBusiness?itemOfBusinessId=159819&amp;sectionId=54&amp;businessPaperDate=2025-09-08"/>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="puVMaN7t">
                            <name>Laid before the House of Lords</name>
                            <procedureStepHasHouse rdf:resource="WkUWUBMx"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="UleUhY8K"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="RB8f6CgW">
                        <businessItemDate rdf:datatype="xsd:date">2025-09-05+01:00</businessItemDate>
                        <businessItemHasBusinessItemWebLink rdf:resource="https://commonsbusiness.parliament.uk/Document/97428/Html?subType=Standard#anchor-34"/>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="cspzmb6w">
                            <name>Laid before the House of Commons</name>
                            <procedureStepHasHouse rdf:resource="1AFu55Hs"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="ZZafOTw2"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="Zpc9hypV">
                        <businessItemDate rdf:datatype="xsd:date">2025-09-16+01:00</businessItemDate>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="u5AUJb2q">
                            <name>Procedure concluded in the House of Commons and the House of Lords</name>
                            <procedureStepHasHouse rdf:resource="1AFu55Hs"/>
                            <procedureStepHasHouse rdf:resource="WkUWUBMx"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="aZUTmokb"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="ksqBrm9w">
                        <businessItemDate rdf:datatype="xsd:date">2025-09-05+01:00</businessItemDate>
                        <businessItemHasBusinessItemWebLink rdf:resource="https://www.gov.uk/government/publications/the-merchant-shipping-maritime-labour-convention-and-miscellaneous-amendments-regulations-2025#full-publication-update-history"/>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="u7VOBBH0">
                            <name>Proposed negative statutory instrument created</name>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="wVDtzu4T"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasBusinessItem>
                      <rdf:Description rdf:about="vHtfUuse">
                        <businessItemDate rdf:datatype="xsd:date">2025-09-16+01:00</businessItemDate>
                        <businessItemHasBusinessItemWebLink rdf:resource="https://commonsbusiness.parliament.uk/Document/97673/Html?subType=Standard#anchor-32"/>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="Lyv2wQgB">
                            <name>Transport Committee agreed that the instrument should follow the negative procedure</name>
                            <procedureStepHasHouse rdf:resource="1AFu55Hs"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="n5m8ytrn"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                        <businessItemHasProcedureStep>
                          <rdf:Description rdf:about="RnhLvXEg">
                            <name>Considered by the Transport Committee</name>
                            <procedureStepHasHouse rdf:resource="1AFu55Hs"/>
                            <procedureStepHasStepDisplayDepthInProcedure rdf:resource="pzXoKrSL"/>
                          </rdf:Description>
                        </businessItemHasProcedureStep>
                      </rdf:Description>
                    </workPackageHasBusinessItem>
                    <workPackageHasProcedure>
                      <rdf:Description rdf:about="iCdMN1MW">
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="U6Lcx9Q9">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">6</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="UleUhY8K">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">2</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="ZZafOTw2">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">2</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="aZUTmokb">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">7</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="n5m8ytrn">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:decimal">4.1</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="nvbZn0Tj">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">5</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="pVLiOfek">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:decimal">5.1</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="pzXoKrSL">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">4</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                        <procedureHasStepDisplayDepthInProcedure>
            			  <rdf:Description rdf:about="wVDtzu4T">
                            <stepDisplayDepthInProcedureHasDepth rdf:datatype="xsd:integer">1</stepDisplayDepthInProcedureHasDepth>
                          </rdf:Description>
                        </procedureHasStepDisplayDepthInProcedure>
                      </rdf:Description>
                    </workPackageHasProcedure>
                  </rdf:Description>
                </workPackagedThingHasWorkPackage>
              </WorkPackagedThing>
            </rdf:RDF>
            """);
    }
}
