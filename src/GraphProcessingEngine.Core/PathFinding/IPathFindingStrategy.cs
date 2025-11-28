using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.PathFinding;

public interface IPathFindingStrategy
{
    string Name { get; }

    PathResult FindPath(
        Graph graph,
        string startNodeId,
        string goalNodeId,
        Func<GraphNode, GraphNode, double>? heuristic = null);
}
