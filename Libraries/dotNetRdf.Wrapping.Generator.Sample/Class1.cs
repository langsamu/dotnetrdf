using VDS.RDF.Wrapping.Attributes;

namespace VDS.RDF.Wrapping.Generator.Sample;

[Dataset]
public partial class Dataset1
{
    [NamedGraph("http://example.com/g1")]
    public partial Graph1 Graph1 { get; }
}

[Graph]
public partial class Graph1
{
    //public partial Node1 Node { get; }
}


[Node]
public partial class Node1
{
    //[NodeProperty("http://example.com/p1")]
    //public partial Node1 PropertyGetSet { get; set; }

    //[NodeProperty("http://example.com/p1")]
    //public partial Node1 PropertySet { set; }

    //[NodeProperty("http://example.com/p1")]
    //public partial string StringProperty { get; }

    [NodeProperty("http://example.com/p1")]
    public partial Node1 PropertyGet { get; }

    [NodeProperty("http://example.com/p1")]
    public partial Node1 PropertyInternalGet { internal get; set; }

    [NodeProperty("http://example.com/p1")]
    public partial Node1 PropertyInternalSet { get; internal set; }

    [NodeProperty("http://example.com/p1")]
    public partial Node1 PropertyProtectedGet { protected get; set; }

    [NodeProperty("http://example.com/p1")]
    public partial Node1 PropertyProtectedSet { get; protected set; }

    //[NodeProperty("http://example.com/p1")]
    //public partial Node1 PropertyInternalGet { internal get; set; }

    //[NodeProperty("http://example.com/p1")]
    //public partial Node1 PropertyPrivateGet { private get; set; }

    //[NodeProperty("http://example.com/p1")]
    //public partial Node1 PropertyProtectedGet { protected get; set; }
}

[Node]
public partial class Node2 : Node1
{
}

[Node]
public partial class NodeXXX : Node1
{
    public int MyProperty { get; set; }
}
