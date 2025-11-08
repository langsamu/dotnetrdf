using VDS.RDF.Parsing;

namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

internal static class Vocabulary
{
    internal const string BaseUri = "http://www.w3.org/ns/solid/terms#";

    private static readonly NodeFactory Factory = new();

    internal static IUriNode InsertDeletePatch { get; } = SolidNode("InsertDeletePatch");

    internal static IUriNode where { get; } = SolidNode("where");
    internal static IUriNode inserts { get; } = SolidNode("inserts");
    internal static IUriNode deletes { get; } = SolidNode("deletes");

    internal static IUriNode RdfType { get; } = AnyNode(RdfSpecsHelper.RdfType);

    private static IUriNode SolidNode(string name) => AnyNode($"{BaseUri}{name}");

    private static IUriNode AnyNode(string uri) => Factory.CreateUriNode(Factory.UriFactory.Create(uri));
}
