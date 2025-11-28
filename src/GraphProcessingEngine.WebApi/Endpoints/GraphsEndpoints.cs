using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.WebApi.Models.Requests;

namespace GraphProcessingEngine.WebApi.Endpoints;

public static class GraphsEndpoints
{
    public static IEndpointRouteBuilder MapGraphsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/graphs");

        group.MapGet("/", (GraphStore store) =>
        {
            var graphs = store.ListGraphs();
            return Results.Ok(graphs);
        });

        group.MapPost("/", async (CreateGraphRequest request, GraphStore store, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Graph name is required.");
            }

            var graph = store.CreateNew(request.IsDirected);
            await store.SaveAsync(graph, request.Name, ct);
            return Results.Created($"/graphs/{request.Name}", request);
        });

        group.MapPost("/{name}/edges", async (string name, AddEdgeRequest request, GraphStore store, CancellationToken ct) =>
        {
            if (!store.Exists(name))
            {
                return Results.NotFound($"Graph '{name}' not found.");
            }

            if (string.IsNullOrWhiteSpace(request.SourceId) || string.IsNullOrWhiteSpace(request.TargetId))
            {
                return Results.BadRequest("SourceId and TargetId are required.");
            }

            var graph = await store.LoadAsync(name, ct);
            EnsureNode(graph, request.SourceId);
            EnsureNode(graph, request.TargetId);
            graph.AddEdge(request.SourceId, request.TargetId, request.Weight, request.IsDirected);
            await store.SaveAsync(graph, name, ct);

            return Results.Ok();
        });

        return app;
    }

    private static void EnsureNode(Graph graph, string nodeId)
    {
        if (!graph.ContainsNode(nodeId))
        {
            graph.AddNode(nodeId);
        }
    }
}
