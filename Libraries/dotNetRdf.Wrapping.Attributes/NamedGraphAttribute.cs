
using System;

namespace VDS.RDF.Wrapping.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NamedGraphAttribute(string graphName) : Attribute { }
