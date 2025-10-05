using System;
using System.Linq;

namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

public class Tests
{
    private const string originalRdf = """
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
        """;

    private const string modifiedRdf = """
        <?xml version="1.0"?>
        <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:dec="http://www.freshdecisions.org/decisions/" xmlns:georss="http://www.georss.org/georss/11" xmlns:gml="http://www.opengis.net/gml#" xmlns:dc="http://purl.org/dc/elements/1.1/" xml:base="http://www.freshdecisions.org/decisions/">
         <dec:Decisions rdf:about="myDecisions">
          <dec:hasDecision>
           <dec:Decision rdf:about="decision_01.xml">
            <dec:hasQuestion>
             <dec:Question rdf:about="decision_01/question.xml">
              <dec:Title>4747f4c6309d4ebfaa19cd700a073644</dec:Title>
             </dec:Question>
            </dec:hasQuestion>
            <dec:hasOptions>
             <dec:Options rdf:about="decision_01/options.xml">
              <dec:hasOption>
               <dec:Option rdf:about="decision_01/options/1">
                <dec:hasIdea>
                 <dec:Idea rdf:about="research/ideas/17">
                  <dec:Title>3d05772757dc43d7bca7b19502ae9428</dec:Title>
                 </dec:Idea>
                </dec:hasIdea>
                <dec:hasPro>
                 <dec:Pro rdf:about="decision_01/options/1/pros/1.xml">
                  <dec:Title>f270bfb7d5da4494a5c46e8ad781d1d6</dec:Title>
                  <dec:Description>304a73d2e4c448519794182a8007a13b</dec:Description>
                  <dec:hasMetric>
                   <dec:Metric rdf:about="metrics/duration.xml">
                    <dec:MetricValueList>urn:d4747cf35ce647cc8e0d77d025349dee</dec:MetricValueList>
                    <dec:Value>NotYetIdentified</dec:Value>
                   </dec:Metric>
                  </dec:hasMetric>
                 </dec:Pro>
                </dec:hasPro>
                <dec:hasPro>
                 <dec:Pro rdf:about="decision_01/options/1/pros/2.xml">
                  <dec:Title>623453aaa2264057a43b55d34ae92f47</dec:Title>
                  <dec:Description>ab1489ca738446448214b0216c4efa32</dec:Description>
                  <dec:hasMetric>
                   <dec:Metric rdf:about="metrics/focusarea.xml">
                    <dec:MetricValueList>urn:b14f15ed9cc442b0a5f46fc35719ae6c</dec:MetricValueList>
                    <dec:Value>OneYearLong</dec:Value>
                   </dec:Metric>
                  </dec:hasMetric>
                 </dec:Pro>
                </dec:hasPro>
                <dec:hasCon>
                 <dec:Con rdf:about="decision_01/options/1/cons/1.xml">
                  <dec:Title>964791c2212d4d159f2bdf6330084e33</dec:Title>
                  <dec:Description>0989e2f35f3746cc8e1a6315580d48c8</dec:Description>
                  <dec:hasMetric>
                   <dec:Metric rdf:about="metrics/currentapplications.xml">
                    <dec:MetricValueList>urn:16ec026f4fe74188a05696412ef53ed4</dec:MetricValueList>
                    <dec:Value>Technique_A</dec:Value>
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
                  <dec:Title>66167f9861c24d3bb40878ed2a1374ff</dec:Title>
                 </dec:Idea>
                </dec:hasIdea>
                <dec:hasCon>
                 <dec:Con rdf:about="decision_01/options/2/cons/1.xml">
                  <dec:Title>d6009390144441f994e750503b72b570</dec:Title>
                  <dec:Description>57a25573fc2c497db5787b08a2a87c79</dec:Description>
                 </dec:Con>
                </dec:hasCon>
               </dec:Option>
              </dec:hasOption>
             </dec:Options>
            </dec:hasOptions>
            <dec:hasStates>
             <dec:States rdf:about="decision_01/states.xml">
              <dec:StatesValueList>urn:109548165b77496b9424dc460186d317</dec:StatesValueList>
              <dec:hasState>
               <dec:State rdf:about="decision_01/states/1.xml">
                <dec:Value>GatheringInfo</dec:Value>
                <dec:Date>2000-01-01T00:00:00+00:01</dec:Date>
               </dec:State>
              </dec:hasState>
              <dec:hasState>
               <dec:State rdf:about="decision_01/states/2.xml">
                <dec:Value>NotYetStarted</dec:Value>
                <dec:Date>2000-01-01T00:00:00+00:02</dec:Date>
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
              <dec:When>2000-01-01T00:00:00+00:03</dec:When>
             </dec:BasicInfo>
            </dec:hasBasicInfo>
           </dec:Decision>
          </dec:hasDecision>
         </dec:Decisions>
        </rdf:RDF>
        """;

    [Fact]
    public void Read()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf);

        var decisions = Graph.Wrap(actual).MyDecisions();
        var decision1 = decisions.Items.Single();
        var question = decision1.Question;
        var options = decision1.Options;
        var option1 = options.Items.First();
        var option2 = options.Items.Last();
        var idea = option1.Idea;
        var idea2 = option2.Idea;
        var pro1 = option1.Pros.First();
        var pro2 = option1.Pros.Last();
        var con1 = option1.Contras.First();
        var con2 = option2.Contras.First();
        var metric1 = pro1.Metrics.First();
        var metric2 = pro2.Metrics.First();
        var metric3 = con1.Metrics.First();
        var states = decision1.States;
        var state1 = states.Items.First();
        var state2 = states.Items.Last();
        var basicInfo = decision1.BasicInfo;
        var person = basicInfo.Who;
        var point = basicInfo.Where;
        var position = point.Position;

        decisions.Items.Should().HaveCount(1);
        question.Title.Should().Be("What should be the topic of ProfessorX's research proposal?");
        options.Items.Should().HaveCount(2);
        idea.Title.Should().Be("A Novel Approach for Solving Prolems Using Technique A");
        option1.Pros.Should().HaveCount(2);
        pro1.Title.Should().Be("One Year in Duration");
        pro1.Description.Should().Be("One year research efforts are favored.");
        pro1.Metrics.Should().HaveCount(1);
        metric1.MetricValueList.Should().Be(new Uri("http://www.freshdecisions.org/myorganization/research/metrics/durationValues"));
        metric1.Value.Should().Be(MetricValue.OneYearLong);
        pro2.Title.Should().Be("Good Research Area");
        pro2.Description.Should().Be("Research with Technique A is a priority of this organization");
        pro2.Metrics.Should().HaveCount(1);
        metric2.MetricValueList.Should().Be(new Uri("http://www.freshdecisions.org/myorganization/research/metrics/focusareaValues"));
        metric2.Value.Should().Be(MetricValue.Technique_A);
        option1.Contras.Should().HaveCount(1);
        con1.Title.Should().Be("No current applications");
        con1.Description.Should().Be("Research funding favors proposals with current applications");
        con1.Metrics.Should().HaveCount(1);
        metric3.MetricValueList.Should().Be(new Uri("http://www.freshdecisions.org/myorganization/research/metrics/currentapplicationsValues"));
        metric3.Value.Should().Be(MetricValue.NotYetIdentified);
        idea2.Title.Should().Be("Faster Processing Algorithms Using Technique B");
        option2.Contras.Should().HaveCount(1);
        con2.Title.Should().Be("Not major interest of this researcher");
        con2.Description.Should().Be("ProfessorX is primarily interested in other topics");
        states.StatesValueList.Should().Be(new Uri("http://www.freshdecisions.org/decision/states"));
        states.Items.Should().HaveCount(2);
        state1.Value.Should().Be(StateValue.NotYetStarted);
        state1.Date.Should().Be(new DateTimeOffset(2010, 11, 9, 13, 0, 0, TimeSpan.FromHours(2)));
        state2.Value.Should().Be(StateValue.GatheringInfo);
        state2.Date.Should().Be(new DateTimeOffset(2010, 11, 9, 13, 0, 0, TimeSpan.FromHours(2)));
        person.As<IUriNode>().Uri.Should().Be("http://www.myorganization.org/people/ProfessorX.xml");
        position.X.Should().Be(32.7);
        position.Y.Should().Be(-117.211);
        basicInfo.When.Should().Be(DateTimeOffset.Parse("2010-11-09T21:32:52+05:00"));
        basicInfo.When.Should().Be(new DateTimeOffset(2010, 11, 9, 21, 32, 52, TimeSpan.FromHours(5)));
    }

    [Fact]
    public void Write()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf);

        var decisions = Graph.Wrap(actual).MyDecisions();
        var decision1 = decisions.Items.Single();
        var question = decision1.Question;
        var options = decision1.Options;
        var option1 = options.Items.First();
        var option2 = options.Items.Last();
        var idea = option1.Idea;
        var idea2 = option2.Idea;
        var pro1 = option1.Pros.First();
        var pro2 = option1.Pros.Last();
        var con1 = option1.Contras.First();
        var con2 = option2.Contras.First();
        var metric1 = pro1.Metrics.First();
        var metric2 = pro2.Metrics.First();
        var metric3 = con1.Metrics.First();
        var states = decision1.States;
        var state1 = states.Items.First();
        var state2 = states.Items.Last();
        var basicInfo = decision1.BasicInfo;

        decisions.Items.Single().Question.Title = "4747f4c6309d4ebfaa19cd700a073644";
        option1.Idea.Title = "3d05772757dc43d7bca7b19502ae9428";
        pro1.Title = "f270bfb7d5da4494a5c46e8ad781d1d6";
        pro1.Description = "304a73d2e4c448519794182a8007a13b";
        metric1.MetricValueList = new Uri("urn:d4747cf35ce647cc8e0d77d025349dee");
        metric1.Value = MetricValue.NotYetIdentified;
        pro2.Title = "623453aaa2264057a43b55d34ae92f47";
        pro2.Description = "ab1489ca738446448214b0216c4efa32";
        metric2.MetricValueList = new Uri("urn:b14f15ed9cc442b0a5f46fc35719ae6c");
        metric2.Value = MetricValue.OneYearLong;
        con1.Title = "964791c2212d4d159f2bdf6330084e33";
        con1.Description = "0989e2f35f3746cc8e1a6315580d48c8";
        metric3.MetricValueList = new Uri("urn:16ec026f4fe74188a05696412ef53ed4");
        metric3.Value = MetricValue.Technique_A;
        option2.Idea.Title = "66167f9861c24d3bb40878ed2a1374ff";
        con2.Title = "d6009390144441f994e750503b72b570";
        con2.Description = "57a25573fc2c497db5787b08a2a87c79";
        states.StatesValueList = new Uri("urn:109548165b77496b9424dc460186d317");
        state1.Value = StateValue.GatheringInfo;
        state1.Date = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.FromMinutes(1));
        state2.Value = StateValue.NotYetStarted;
        state2.Date = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.FromMinutes(2));
        basicInfo.When = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.FromMinutes(3));

        var expected = new RDF.Graph();
        expected.LoadFromString(modifiedRdf);

        actual.Should().BeIsomorphicWith(expected);
    }

    [Fact]
    public void Clear()
    {
        var actual = new RDF.Graph();
        actual.LoadFromString(originalRdf);

        var decisions = Graph.Wrap(actual).MyDecisions();
        var decision1 = decisions.Items.Single();
        var question = decision1.Question;
        var options = decision1.Options;
        var option1 = options.Items.First();
        var option2 = options.Items.Last();
        var idea1 = option1.Idea;
        var idea2 = option2.Idea;
        var pro1 = option1.Pros.First();
        var pro2 = option1.Pros.Last();
        var con1 = option1.Contras.First();
        var con2 = option2.Contras.First();
        var metric1 = pro1.Metrics.First();
        var metric2 = pro2.Metrics.First();
        var metric3 = con1.Metrics.First();
        var states = decision1.States;
        var state1 = states.Items.First();
        var state2 = states.Items.Last();
        var basicInfo = decision1.BasicInfo;
        var person = basicInfo.Who;
        var point = basicInfo.Where;
        var position = point.Position;

        decision1.Question.Title = null;
        decision1.Question.Type = null;
        decision1.Question = null;
        idea1.Title = null;
        idea1.Type = null;
        option1.Idea = null;
        pro1.Title = null;
        pro1.Description = null;
        metric1.MetricValueList = null;
        metric1.Value = null;
        metric1.Type = null;
        pro1.Metrics.Clear();
        pro1.Type = null;
        pro2.Title = null;
        pro2.Description = null;
        metric2.MetricValueList = null;
        metric2.Value = null;
        metric2.Type = null;
        pro2.Metrics.Clear();
        pro2.Type = null;
        option1.Pros.Clear();
        con1.Title = null;
        con1.Description = null;
        metric3.MetricValueList = null;
        metric3.Value = null;
        metric3.Type = null;
        con1.Metrics.Clear();
        con1.Type = null;
        option1.Contras.Clear();
        option1.Type = null;
        idea2.Title = null;
        idea2.Type = null;
        option2.Idea = null;
        con2.Title = null;
        con2.Description = null;
        con2.Type = null;
        option2.Contras.Clear();
        option2.Type = null;
        options.Items.Clear();
        options.Type = null;
        decision1.Options = null;
        states.StatesValueList = null;
        state1.Value = null;
        state1.Date = null;
        state1.Type = null;
        state2.Value = null;
        state2.Date = null;
        state2.Type = null;
        states.Items.Clear();
        states.Type = null;
        decision1.States = null;
        person.Type = null;
        basicInfo.Who = null;
        point.Position = null;
        point.Type = null;
        basicInfo.Where = null;
        basicInfo.When = null;
        basicInfo.Type = null;
        decision1.BasicInfo = null;
        decision1.Type = null;
        decisions.Items.Clear();
        decisions.Type = null;

        actual.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Create()
    {
        var actual = new RDF.Graph();

        var decisions = Decisions.Create("http://www.freshdecisions.org/decisions/myDecisions", actual);
        var decision1 = Decision.Create("http://www.freshdecisions.org/decisions/decision_01.xml", actual);
        var question = Question.Create("http://www.freshdecisions.org/decisions/decision_01/question.xml", actual);
        var options = Options.Create("http://www.freshdecisions.org/decisions/decision_01/options.xml", actual);
        var option1 = Option.Create("http://www.freshdecisions.org/decisions/decision_01/options/1", actual);
        var option2 = Option.Create("http://www.freshdecisions.org/decisions/decision_01/options/2.xml", actual);
        var idea1 = Idea.Create("http://www.freshdecisions.org/decisions/research/ideas/17", actual);
        var idea2 = Idea.Create("http://www.freshdecisions.org/decisions/research/ideas/49.xml", actual);
        var pro1 = Pro.Create("http://www.freshdecisions.org/decisions/decision_01/options/1/pros/1.xml", actual);
        var pro2 = Pro.Create("http://www.freshdecisions.org/decisions/decision_01/options/1/pros/2.xml", actual);
        var metric1 = Metric.Create("http://www.freshdecisions.org/decisions/metrics/duration.xml", actual);
        var metric2 = Metric.Create("http://www.freshdecisions.org/decisions/metrics/focusarea.xml", actual);
        var con1 = Contra.Create("http://www.freshdecisions.org/decisions/decision_01/options/1/cons/1.xml", actual);
        var con2 = Contra.Create("http://www.freshdecisions.org/decisions/decision_01/options/2/cons/1.xml", actual);
        var metric3 = Metric.Create("http://www.freshdecisions.org/decisions/metrics/currentapplications.xml", actual);
        var states = States.Create("http://www.freshdecisions.org/decisions/decision_01/states.xml", actual);
        var state1 = State.Create("http://www.freshdecisions.org/decisions/decision_01/states/1.xml", actual);
        var state2 = State.Create("http://www.freshdecisions.org/decisions/decision_01/states/2.xml", actual);
        var basicInfo = BasicInfo.Create("http://www.freshdecisions.org/decisions/decision_01/basicInfo.xml", actual);
        var person = Person.Create("http://www.myorganization.org/people/ProfessorX.xml", actual);
        var point = GmlPoint.Create("http://www.freshdecisions.org/decisions/decision_01/location.xml", actual);
        var position = GmlPosition.Create(32.7, -117.211, actual);

        decisions.Type = Vocabulary.Decisions.Uri;
        decisions.Items.Add(decision1);
        decision1.Type = Vocabulary.Decision.Uri;
        decision1.Question = question;
        question.Type = Vocabulary.Question.Uri;
        question.Title = "What should be the topic of ProfessorX's research proposal?";
        decision1.Options = options;
        options.Type = Vocabulary.Options.Uri;
        options.Items.Add(option1);
        option1.Type = Vocabulary.Option.Uri;
        option1.Idea = idea1;
        idea1.Type = Vocabulary.Idea.Uri;
        idea1.Title = "A Novel Approach for Solving Prolems Using Technique A";
        option1.Pros.Add(pro1);
        pro1.Type = Vocabulary.Pro.Uri;
        pro1.Title = "One Year in Duration";
        pro1.Description = "One year research efforts are favored.";
        pro1.Metrics.Add(metric1);
        metric1.Type = Vocabulary.Metric.Uri;
        metric1.MetricValueList = new Uri("http://www.freshdecisions.org/myorganization/research/metrics/durationValues");
        metric1.Value = MetricValue.OneYearLong;
        option1.Pros.Add(pro2);
        pro2.Type = Vocabulary.Pro.Uri;
        pro2.Title = "Good Research Area";
        pro2.Description = "Research with Technique A is a priority of this organization";
        pro2.Metrics.Add(metric2);
        metric2.Type = Vocabulary.Metric.Uri;
        metric2.MetricValueList = new Uri("http://www.freshdecisions.org/myorganization/research/metrics/focusareaValues");
        metric2.Value = MetricValue.Technique_A;
        option1.Contras.Add(con1);
        con1.Type = Vocabulary.Con.Uri;
        con1.Title = "No current applications";
        con1.Description = "Research funding favors proposals with current applications";
        con1.Metrics.Add(metric3);
        metric3.Type = Vocabulary.Metric.Uri;
        metric3.MetricValueList = new Uri("http://www.freshdecisions.org/myorganization/research/metrics/currentapplicationsValues");
        metric3.Value = MetricValue.NotYetIdentified;
        options.Items.Add(option2);
        option2.Type = Vocabulary.Option.Uri;
        option2.Idea = idea2;
        idea2.Type = Vocabulary.Idea.Uri;
        idea2.Title = "Faster Processing Algorithms Using Technique B";
        option2.Contras.Add(con2);
        con2.Type = Vocabulary.Con.Uri;
        con2.Title = "Not major interest of this researcher";
        con2.Description = "ProfessorX is primarily interested in other topics";
        decision1.States = states;
        states.Type = Vocabulary.States.Uri;
        states.StatesValueList = new Uri("http://www.freshdecisions.org/decision/states");
        states.Items.Add(state1);
        state1.Type = Vocabulary.State.Uri;
        state1.Value = StateValue.NotYetStarted;
        state1.Date = new DateTimeOffset(2010, 11, 9, 13, 0, 0, TimeSpan.FromHours(2));
        states.Items.Add(state2);
        state2.Type = Vocabulary.State.Uri;
        state2.Value = StateValue.GatheringInfo;
        state2.Date = new DateTimeOffset(2010, 11, 9, 13, 0, 0, TimeSpan.FromHours(2));
        decision1.BasicInfo = basicInfo;
        basicInfo.Type = Vocabulary.BasicInfo.Uri;
        basicInfo.Who = person;
        person.Type = Vocabulary.Person.Uri;
        basicInfo.Where = point;
        point.Type = Vocabulary.GmlPoint.Uri;
        point.Position = position;
        basicInfo.When = new DateTimeOffset(2010, 11, 9, 21, 32, 52, TimeSpan.FromHours(5));

        var expected = new RDF.Graph();
        expected.LoadFromString(originalRdf);

        actual.Should().BeIsomorphicWith(expected);
    }
}
