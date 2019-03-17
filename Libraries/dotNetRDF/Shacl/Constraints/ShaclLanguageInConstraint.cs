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
    using System.Collections.Generic;
    using System.Linq;
    using VDS.RDF;
    using VDS.RDF.Query;

    internal class ShaclLanguageInConstraint : ShaclConstraint
    {
        public ShaclLanguageInConstraint(ShaclShape shape, INode node)
            : base(shape, node)
        {
        }

        internal override INode Component => Shacl.LanguageInConstraintComponent;

        public override bool Validate(INode focusNode, IEnumerable<INode> valueNodes, ShaclValidationReport report)
        {
            var items = this.Graph.GetListItems(this);
            return valueNodes.All(node => items.Any(item => LanguageIn(node, item)));
        }

        private static bool LanguageIn(INode node, INode item)
        {
            var query = new SparqlParameterizedString(@"
ASK {
    FILTER(LANGMATCHES(LANG(?value), ?language))
}
");
            query.SetVariable("value", node);
            query.SetVariable("language", item);

            var result = (SparqlResultSet)node.Graph.ExecuteQuery(query);

            return result.Result;
        }
    }
}
