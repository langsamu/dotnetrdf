﻿/*
// <copyright>
// dotNetRDF is free and open source software licensed under the MIT License
// -------------------------------------------------------------------------
// 
// Copyright (c) 2009-2017 dotNetRDF Project (http://dotnetrdf.org/)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
*/

namespace VDS.RDF.Shacl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VDS.RDF.Nodes;
    using VDS.RDF.Parsing;
    using VDS.RDF.Query;
    using VDS.RDF.Query.Expressions.Primary;
    using VDS.RDF.Query.Paths;
    using VDS.RDF.Query.Patterns;

    internal class ShaclSparqlAskConstraint : ShaclConstraint
    {
        private readonly IEnumerable<KeyValuePair<string, INode>> parameters;

        public ShaclSparqlAskConstraint(ShaclShape shape, INode value, IEnumerable<KeyValuePair<string, INode>> parameters)
            : base(shape, value)
        {
            this.parameters = parameters;
        }

        internal override INode Component => null;

        private string Ask => Shacl.Ask.ObjectsOf(this).Single().AsValuedNode().AsString();

        private IEnumerable<ShaclPrefixDeclaration> Prefixes => Shacl.Prefixes.ObjectsOf(this).Select(p => new ShaclPrefixes(p)).SingleOrDefault() ?? Enumerable.Empty<ShaclPrefixDeclaration>();

        private INode Message => Shacl.Message.ObjectsOf(this).SingleOrDefault();

        public override bool Validate(INode focusNode, IEnumerable<INode> valueNodes, ShaclValidationReport report)
        {
            var queryString = new SparqlParameterizedString(Ask);

            foreach (var item in Prefixes)
            {
                queryString.Namespaces.AddNamespace(item.Prefix, item.Namespace);
            }

            var query = new SparqlQueryParser().ParseFromString(queryString);

            Validate(query.RootGraphPattern);

            query.RootGraphPattern.TriplePatterns.Insert(0, new BindPattern("this", new ConstantTerm(focusNode)));
            query.RootGraphPattern.TriplePatterns.Insert(0, new BindPattern("currentShape", new ConstantTerm(Shape)));
            query.RootGraphPattern.TriplePatterns.Insert(0, new BindPattern("shapesGraph", new ConstantTerm(Shape.Graph.CreateUriNode(Shape.GraphUri))));

            foreach (var parameter in parameters)
            {
                query.RootGraphPattern.TriplePatterns.Insert(0, new BindPattern(parameter.Key, new ConstantTerm(parameter.Value)));
            }

            var invalidValues =
                from valueNode in valueNodes
                where !((SparqlResultSet)focusNode.Graph.ExecuteQuery(query)).Result
                select valueNode;

            return ReportValueNodes(focusNode, invalidValues, report);
        }

        private void Validate(GraphPattern pattern)
        {
            if (pattern.IsMinus || pattern.InlineData != null || pattern.IsService)
            {
                throw new Exception("illegal clauses");
            }

            foreach (var subPattern in pattern.ChildGraphPatterns)
            {
                Validate(subPattern);
            }

            foreach (var subQueryPattern in pattern.TriplePatterns.OfType<SubQueryPattern>())
            {
                if (!subQueryPattern.Variables.Contains("this"))
                {
                    throw new Exception("missing projection");
                }

                Validate(subQueryPattern.SubQuery.RootGraphPattern);
            }
        }

        private static void BindPath(GraphPattern pattern, ISparqlPath path)
        {
            for (var i = 0; i < pattern.TriplePatterns.Count(); i++)
            {
                if (pattern.TriplePatterns[i] is TriplePattern triplePattern && triplePattern.Predicate.VariableName == "PATH")
                {
                    pattern.TriplePatterns.RemoveAt(i);
                    pattern.TriplePatterns.Insert(i, new PropertyPathPattern(triplePattern.Subject, path, triplePattern.Object));
                }
            }

            foreach (var subPattern in pattern.ChildGraphPatterns)
            {
                BindPath(subPattern, path);
            }

            foreach (var subQueryPattern in pattern.TriplePatterns.OfType<SubQueryPattern>())
            {
                BindPath(subQueryPattern.SubQuery.RootGraphPattern, path);
            }
        }
    }
}