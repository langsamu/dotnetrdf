using System;
using System.Linq;
using VDS.RDF.Query.Patterns;
using VDS.RDF.Update;
using VDS.RDF.Update.Commands;

namespace VDS.RDF.Wrapping.Tests.Examples.SolidPatch;

internal class Graph : WrapperGraph
{
    protected Graph(IGraph graph) : base(graph) { }

    internal static Graph Wrap(IGraph graph) => new(graph);

    internal BaseModificationCommand? Command
    {
        get => Patch switch
        {
            null => null,
            { Inserts: GraphPattern insert, Deletions: GraphPattern delete } => new ModifyCommand(delete, insert, Patch.Where),
            { Deletions: GraphPattern delete } => new DeleteCommand(delete, Patch.Where),
            { Inserts: GraphPattern insert } => new InsertCommand(insert, Patch.Where),
            _ => throw new Exception("No inserts or deletions in patch"),
        };

        set
        {
            if (value is null)
            {
                return;
            }

            if (Patch is null)
            {
                throw new Exception("?");
            }

            if (value is ModifyCommand modify)
            {
                Patch.Inserts = modify.InsertPattern;
                Patch.Deletions = modify.DeletePattern;
                Patch.Where = modify.WherePattern;
            }
            else if (value is InsertCommand insert)
            {
                Patch.Inserts = insert.InsertPattern;
                Patch.Where = insert.WherePattern;
            }
            else if (value is DeleteCommand delete)
            {
                Patch.Deletions = delete.DeletePattern;
                Patch.Where = delete.WherePattern;
            }
            else
            {
                throw new Exception("Unsupported command type");
            }
        }
    }

    internal InsertDeletePatch? Patch =>
        GetTriplesWithPredicateObject(
            Vocabulary.RdfType,
            Vocabulary.InsertDeletePatch)
        .Select(static t => t.Subject)
        .In(this)
        .Select(InsertDeletePatch.Wrap)
        .SingleOrDefault();
}
