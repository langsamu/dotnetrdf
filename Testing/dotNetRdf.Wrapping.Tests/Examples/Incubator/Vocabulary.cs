namespace VDS.RDF.Wrapping.Tests.Examples.Incubator;

internal static class Vocabulary
{
    internal const string BaseUri = "http://www.freshdecisions.org/decisions/";

    private static readonly NodeFactory Factory = new();

    internal static IUriNode hasDecision { get; } = DecisionNode("hasDecision");
    internal static IUriNode hasQuestion { get; } = DecisionNode("hasQuestion");
    internal static IUriNode hasOptions { get; } = DecisionNode("hasOptions");
    internal static IUriNode hasOption { get; } = DecisionNode("hasOption");
    internal static IUriNode hasStates { get; } = DecisionNode("hasStates");
    internal static IUriNode hasBasicInfo { get; } = DecisionNode("hasBasicInfo");
    internal static IUriNode hasMetric { get; } = DecisionNode("hasMetric");
    internal static IUriNode hasIdea { get; } = DecisionNode("hasIdea");
    internal static IUriNode hasPro { get; } = DecisionNode("hasPro");
    internal static IUriNode hasCon { get; } = DecisionNode("hasCon");
    internal static IUriNode hasState { get; } = DecisionNode("hasState");
    internal static IUriNode Title { get; } = DecisionNode("Title");
    internal static IUriNode Description { get; } = DecisionNode("Description");
    internal static IUriNode Value { get; } = DecisionNode("Value");
    internal static IUriNode MetricValueList { get; } = DecisionNode("MetricValueList");
    internal static IUriNode StatesValueList { get; } = DecisionNode("StatesValueList");
    internal static IUriNode Date { get; } = DecisionNode("Date");
    internal static IUriNode Who { get; } = DecisionNode("Who");
    internal static IUriNode Where { get; } = DecisionNode("Where");
    internal static IUriNode When { get; } = DecisionNode("When");

    internal static IUriNode Decision { get; } = DecisionNode("Decision");
    internal static IUriNode Person { get; } = DecisionNode("Person");

    internal static IUriNode gml_pos { get; } = AnyNode("http://www.opengis.net/gml#pos");

    private static IUriNode DecisionNode(string name) => AnyNode($"{BaseUri}{name}");

    private static IUriNode AnyNode(string uri) => Factory.CreateUriNode(Factory.UriFactory.Create(uri));
}
