using GraphProcessingEngine.Core.Caching;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.PathFinding;
using GraphProcessingEngine.WebApi.Models.Requests;
using GraphProcessingEngine.WebApi.Models.Responses;

namespace GraphProcessingEngine.WebApi.Endpoints;

public static class PathFindingEndpoints
{
    public static IEndpointRouteBuilder MapPathFindingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/graphs/{name}/paths");

        group.MapPost("/shortest", async (string name, ShortestPathRequest request, GraphStore store, CancellationToken ct) =>
        {
            if (!store.Exists(name))
            {
                return Results.NotFound($"Graph '{name}' not found.");
            }

            var graph = await store.LoadAsync(name, ct);
            var normalized = (request.Algorithm ?? "dijkstra").ToLowerInvariant();
            IPathFindingStrategy strategy = normalized switch
            {
                "astar" or "a*" => new AStarStrategy(),
                _ => new DijkstraStrategy()
            };

            var finder = new PathFinder(new CachingPathFindingStrategy(strategy));
            var heuristic = CreateHeuristic();

            PathResult result;
            try
            {
                result = finder.FindPath(graph, request.StartId, request.GoalId, heuristic);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

            var response = new ShortestPathResponse
            {
                Success = result.Success,
                Distance = result.Distance,
                Algorithm = strategy.Name,
                Path = result.Path.Select(n => n.Id).ToList(),
                VisitedOrder = result.VisitedOrder.Select(n => n.Id).ToList()
            };

            return Results.Ok(response);
        });

        return app;
    }

    private static Func<GraphNode, GraphNode, double> CreateHeuristic()
    {
        return static (a, b) =>
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        };
    }
}
