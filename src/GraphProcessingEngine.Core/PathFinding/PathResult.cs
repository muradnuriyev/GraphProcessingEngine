using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.PathFinding;

public sealed class PathResult
{
    public PathResult(bool success, double distance, IReadOnlyList<GraphNode> path, IReadOnlyList<GraphNode> visitedOrder)
    {
        Success = success;
        Distance = distance;
        Path = path;
        VisitedOrder = visitedOrder;
    }

    public bool Success { get; }

    public double Distance { get; }

    public IReadOnlyList<GraphNode> Path { get; }

    public IReadOnlyList<GraphNode> VisitedOrder { get; }

    public static PathResult Failure(IEnumerable<GraphNode> visited)
        => new(false, double.PositiveInfinity, Array.Empty<GraphNode>(), visited.ToList());
}
