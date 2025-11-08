namespace VDS.RDF.Wrapping.Tests.Examples.Skos;

internal static class Vocabulary
{
    internal const string BaseUri = "http://www.w3.org/2004/02/skos/core#";

    private static readonly NodeFactory Factory = new();

    internal static IUriNode prefLabel { get; } = ShaclNode("prefLabel");
    internal static IUriNode altLabel { get; } = ShaclNode("altLabel");
    internal static IUriNode scopeNote { get; } = ShaclNode("scopeNote");
    internal static IUriNode related { get; } = ShaclNode("related");
    internal static IUriNode inScheme { get; } = ShaclNode("inScheme");
    internal static IUriNode narrower { get; } = ShaclNode("narrower");
    internal static IUriNode hasTopConcept { get; } = ShaclNode("hasTopConcept");

    private static IUriNode ShaclNode(string name) => AnyNode($"{BaseUri}{name}");

    private static IUriNode AnyNode(string uri) => Factory.CreateUriNode(Factory.UriFactory.Create(uri));
}
