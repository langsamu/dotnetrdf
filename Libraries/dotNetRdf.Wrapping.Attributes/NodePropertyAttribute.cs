using System;

namespace VDS.RDF.Wrapping.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NodePropertyAttribute(string predicate) : Attribute { }
