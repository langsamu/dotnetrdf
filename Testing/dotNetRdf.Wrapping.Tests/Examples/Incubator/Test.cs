using System;
using System.Linq;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

public class Tests
{
    [Fact]
    public void Test()
    {
        var g = new RDF.Graph();
        g.LoadFromString("""
            <?xml version="1.0"?>
            <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:dec="http://www.freshdecisions.org/decisions/" xmlns:georss="http://www.georss.org/georss/11" xmlns:gml="http://www.opengis.net/gml#" xmlns:dc="http://purl.org/dc/elements/1.1/" xml:base="http://www.freshdecisions.org/decisions/">
             <dec:Decisions rdf:about="myDecisions">
              <dec:hasDecision>
               <dec:Decision rdf:about="decision_01.xml">
                <dec:hasQuestion>
                 <dec:Question rdf:about="decision_01/question.xml">
                  <dec:Title>What should be the topic of ProfessorX's research proposal?</dec:Title>
                 </dec:Question>
                </dec:hasQuestion>
                <dec:hasOptions>
                 <dec:Options rdf:about="decision_01/options.xml">
                  <dec:hasOption>
                   <dec:Option rdf:about="decision_01/options/1">
                    <dec:hasIdea>
                     <dec:Idea rdf:about="research/ideas/17">
                      <dec:Title>A Novel Approach for Solving Prolems Using Technique A</dec:Title>
                     </dec:Idea>
                    </dec:hasIdea>
                    <dec:hasPro>
                     <dec:Pro rdf:about="decision_01/options/1/pros/1.xml">
                      <dec:Title>One Year in Duration</dec:Title>
                      <dec:Description>One year research efforts are favored.</dec:Description>
                      <dec:hasMetric>
                       <dec:Metric rdf:about="metrics/duration.xml">
                        <dec:MetricValueList>http://www.freshdecisions.org/myorganization/research/metrics/durationValues</dec:MetricValueList>
                        <dec:Value>OneYearLong</dec:Value>
                       </dec:Metric>
                      </dec:hasMetric>
                     </dec:Pro>
                    </dec:hasPro>
                    <dec:hasPro>
                     <dec:Pro rdf:about="decision_01/options/1/pros/2.xml">
                      <dec:Title>Good Research Area</dec:Title>
                      <dec:Description>Research with Technique A is a priority of this organization</dec:Description>
                      <dec:hasMetric>
                       <dec:Metric rdf:about="metrics/focusarea.xml">
                        <dec:MetricValueList>http://www.freshdecisions.org/myorganization/research/metrics/focusareaValues</dec:MetricValueList>
                        <dec:Value>Technique_A</dec:Value>
                       </dec:Metric>
                      </dec:hasMetric>
                     </dec:Pro>
                    </dec:hasPro>
                    <dec:hasCon>
                     <dec:Con rdf:about="decision_01/options/1/cons/1.xml">
                      <dec:Title>No current applications</dec:Title>
                      <dec:Description>Research funding favors proposals with current applications</dec:Description>
                      <dec:hasMetric>
                       <dec:Metric rdf:about="metrics/currentapplications.xml">
                        <dec:MetricValueList>http://www.freshdecisions.org/myorganization/research/metrics/currentapplicationsValues</dec:MetricValueList>
                        <dec:Value>NotYetIdentified</dec:Value>
                       </dec:Metric>
                      </dec:hasMetric>
                     </dec:Con>
                    </dec:hasCon>
                   </dec:Option>
                  </dec:hasOption>
                  <dec:hasOption>
                   <dec:Option rdf:about="decision_01/options/2.xml">
                    <dec:hasIdea>
                     <dec:Idea rdf:about="research/ideas/49.xml">
                      <dec:Title>Faster Processing Algorithms Using Technique B</dec:Title>
                     </dec:Idea>
                    </dec:hasIdea>
                    <dec:hasCon>
                     <dec:Con rdf:about="decision_01/options/2/cons/1.xml">
                      <dec:Title>Not major interest of this researcher</dec:Title>
                      <dec:Description>ProfessorX is primarily interested in other topics</dec:Description>
                     </dec:Con>
                    </dec:hasCon>
                   </dec:Option>
                  </dec:hasOption>
                 </dec:Options>
                </dec:hasOptions>
                <dec:hasStates>
                 <dec:States rdf:about="decision_01/states.xml">
                  <dec:StatesValueList>http://www.freshdecisions.org/decision/states</dec:StatesValueList>
                  <dec:hasState>
                   <dec:State rdf:about="decision_01/states/1.xml">
                    <dec:Value>NotYetStarted</dec:Value>
                    <dec:Date>2010-11-09T13:00:00+02:00</dec:Date>
                   </dec:State>
                  </dec:hasState>
                  <dec:hasState>
                   <dec:State rdf:about="decision_01/states/2.xml">
                    <dec:Value>GatheringInfo</dec:Value>
                    <dec:Date>2010-11-09T13:00:00+02:00</dec:Date>
                   </dec:State>
                  </dec:hasState>
                 </dec:States>
                </dec:hasStates>
                <dec:hasBasicInfo>
                 <dec:BasicInfo rdf:about="decision_01/basicInfo.xml">
                  <dec:Who>
                   <dec:Person rdf:about="http://www.myorganization.org/people/ProfessorX.xml"/>
                  </dec:Who>
                  <dec:Where>
                   <gml:Point rdf:about="decision_01/location.xml">
                    <gml:pos>32.7 -117.211</gml:pos>
                   </gml:Point>
                  </dec:Where>
                  <dec:When>2010-11-09T21:32:52+05:00</dec:When>
                 </dec:BasicInfo>
                </dec:hasBasicInfo>
               </dec:Decision>
              </dec:hasDecision>
             </dec:Decisions>
            </rdf:RDF>
            """);

        var decisions = Graph.Wrap(g).MyDecisions("http://www.freshdecisions.org/decisions/myDecisions");

        decisions.Items.Count.Should().Be(1);
        decisions.Items.Single().Question.Title.Should().Be("What should be the topic of ProfessorX's research proposal?");
        decisions.Items.Single().Options.Items.Count.Should().Be(2);
        decisions.Items.Single().Options.Items.First().Idea.Title.Should().Be("A Novel Approach for Solving Prolems Using Technique A");
        decisions.Items.Single().Options.Items.First().Pros.Count.Should().Be(2);
        decisions.Items.Single().Options.Items.First().Pros.First().Title.Should().Be("One Year in Duration");
        decisions.Items.Single().Options.Items.First().Pros.First().Description.Should().Be("One year research efforts are favored.");
        decisions.Items.Single().Options.Items.First().Pros.First().Metrics.Count.Should().Be(1);
        decisions.Items.Single().Options.Items.First().Pros.First().Metrics.First().MetricValueList.Should().Be(new Uri("http://www.freshdecisions.org/myorganization/research/metrics/durationValues"));
        decisions.Items.Single().Options.Items.First().Pros.First().Metrics.First().Value.Should().Be(MetricValue.OneYearLong);
        decisions.Items.Single().Options.Items.First().Pros.Last().Title.Should().Be("Good Research Area");
        decisions.Items.Single().Options.Items.First().Pros.Last().Description.Should().Be("Research with Technique A is a priority of this organization");
        decisions.Items.Single().Options.Items.First().Pros.Last().Metrics.Count.Should().Be(1);
        decisions.Items.Single().Options.Items.First().Pros.Last().Metrics.First().MetricValueList.Should().Be(new Uri("http://www.freshdecisions.org/myorganization/research/metrics/focusareaValues"));
        decisions.Items.Single().Options.Items.First().Pros.Last().Metrics.First().Value.Should().Be(MetricValue.Technique_A);
        decisions.Items.Single().Options.Items.First().Contras.Count.Should().Be(1);
        decisions.Items.Single().Options.Items.First().Contras.First().Title.Should().Be("No current applications");
        decisions.Items.Single().Options.Items.First().Contras.First().Description.Should().Be("Research funding favors proposals with current applications");
        decisions.Items.Single().Options.Items.First().Contras.First().Metrics.Count.Should().Be(1);
        decisions.Items.Single().Options.Items.First().Contras.First().Metrics.First().MetricValueList.Should().Be(new Uri("http://www.freshdecisions.org/myorganization/research/metrics/currentapplicationsValues"));
        decisions.Items.Single().Options.Items.First().Contras.First().Metrics.First().Value.Should().Be(MetricValue.NotYetIdentified);
        decisions.Items.Single().Options.Items.Last().Idea.Title.Should().Be("Faster Processing Algorithms Using Technique B");
        decisions.Items.Single().Options.Items.Last().Contras.Count.Should().Be(1);
        decisions.Items.Single().Options.Items.Last().Contras.First().Title.Should().Be("Not major interest of this researcher");
        decisions.Items.Single().Options.Items.Last().Contras.First().Description.Should().Be("ProfessorX is primarily interested in other topics");
        decisions.Items.Single().States.StatesValueList.Should().Be(new Uri("http://www.freshdecisions.org/decision/states"));
        decisions.Items.Single().States.Items.Count.Should().Be(2);
        decisions.Items.Single().States.Items.First().Value.Should().Be(StateValue.NotYetStarted);
        decisions.Items.Single().States.Items.First().Date.Should().Be(DateTimeOffset.Parse("2010-11-09T13:00:00+02:00"));
        decisions.Items.Single().States.Items.Last().Value.Should().Be(StateValue.GatheringInfo);
        decisions.Items.Single().States.Items.Last().Date.Should().Be(DateTimeOffset.Parse("2010-11-09T13:00:00+02:00"));
        decisions.Items.Single().BasicInfo.Who.As<IUriNode>().Uri.Should().Be("http://www.myorganization.org/people/ProfessorX.xml");
        decisions.Items.Single().BasicInfo.Where.Position.X.Should().Be(32.7);
        decisions.Items.Single().BasicInfo.Where.Position.Y.Should().Be(-117.211);
        decisions.Items.Single().BasicInfo.When.Should().Be(DateTimeOffset.Parse("2010-11-09T21:32:52+05:00"));
    }
}
