namespace VDS.RDF.Wrapping;

internal class SampleVocabulary
{
    private const string BaseUri = "http://example.com/";
    private static readonly NodeFactory Factory = new();
 
    private static IUriNode Node(string name) => Factory.CreateUriNode(UriFactory.Create(name));
    
    private static IUriNode Node(string baseUri, string name) => Node($"{baseUri}{name}");
 
    internal static INode P1 { get; } = Node(BaseUri, "p1");
}
