// unset

using System.IO;
using VDS.RDF.Query;

namespace VDS.RDF.Parsing
{
    /// <summary>
    /// Abstract base class for SPARQL result readers.
    /// </summary>
    /// <remarks>This implementation just implements the defaulting of the old interface methods to the new extended methods that take an <see cref="IUriFactory"/> parameter.</remarks>
    public abstract class BaseSparqlResultsReader : ISparqlResultsReader
    {
        /// <inheritdoc />
        public void Load(SparqlResultSet results, StreamReader input)
        {
            Load(results, input, UriFactory.Root);
        }

        /// <inheritdoc />
        public void Load(SparqlResultSet results, string filename)
        {
            Load(results, filename, UriFactory.Root);
        }

        /// <inheritdoc />
        public void Load(SparqlResultSet results, TextReader input)
        {
            Load(results, input, UriFactory.Root);
        }

        /// <inheritdoc />
        public void Load(ISparqlResultsHandler handler, StreamReader input)
        {
            Load(handler, input, UriFactory.Root);
        }

        /// <inheritdoc />
        public void Load(ISparqlResultsHandler handler, string filename)
        {
            Load(handler, filename, UriFactory.Root);
        }

        /// <inheritdoc />
        public void Load(ISparqlResultsHandler handler, TextReader input)
        {
            Load(handler, input, UriFactory.Root);
        }

        /// <inheritdoc />
        public abstract void Load(SparqlResultSet results, StreamReader input, IUriFactory uriFactory);

        /// <inheritdoc />
        public abstract void Load(SparqlResultSet results, string filename, IUriFactory uriFactory);

        /// <inheritdoc />
        public abstract void Load(SparqlResultSet results, TextReader input, IUriFactory uriFactory);

        /// <inheritdoc />
        public abstract void Load(ISparqlResultsHandler handler, StreamReader input, IUriFactory uriFactory);

        /// <inheritdoc />
        public abstract void Load(ISparqlResultsHandler handler, string filename, IUriFactory uriFactory);

        /// <inheritdoc />
        public abstract void Load(ISparqlResultsHandler handler, TextReader input, IUriFactory uriFactory);

        /// <inheritdoc />
        public abstract event SparqlWarning Warning;
    }
}