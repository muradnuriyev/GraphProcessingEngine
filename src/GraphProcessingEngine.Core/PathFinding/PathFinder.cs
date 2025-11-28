using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.PathFinding;

/// <summary>
/// Context wrapper that delegates to a chosen path-finding strategy.
/// </summary>
public sealed class PathFinder
{
    public PathFinder(IPathFindingStrategy strategy)
    {
        Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public IPathFindingStrategy Strategy { get; private set; }

    public PathResult FindPath(Graph graph, string startNodeId, string goalNodeId, Func<GraphNode, GraphNode, double>? heuristic = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        return Strategy.FindPath(graph, startNodeId, goalNodeId, heuristic);
    }

    public void UseStrategy(IPathFindingStrategy strategy)
    {
        Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }
}
