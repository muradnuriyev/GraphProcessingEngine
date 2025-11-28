using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.PathFinding.PriorityQueue;

namespace GraphProcessingEngine.Core.PathFinding;

public sealed class AStarStrategy : IPathFindingStrategy
{
    public string Name => "A*";

    public PathResult FindPath(Graph graph, string startNodeId, string goalNodeId, Func<GraphNode, GraphNode, double>? heuristic = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var start = graph.GetNode(startNodeId) ?? throw new ArgumentException($"Start node '{startNodeId}' is missing.", nameof(startNodeId));
        var goal = graph.GetNode(goalNodeId) ?? throw new ArgumentException($"Goal node '{goalNodeId}' is missing.", nameof(goalNodeId));

        heuristic ??= static (_, _) => 0;

        var openSet = new BinaryHeapPriorityQueue<GraphNode>();
        var cameFrom = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        var gScore = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        var visitedOrder = new List<GraphNode>();

        foreach (var node in graph.Nodes)
        {
            gScore[node.Id] = double.PositiveInfinity;
            cameFrom[node.Id] = null;
        }

        gScore[start.Id] = 0;
        openSet.Enqueue(start, heuristic(start, goal));

        var closed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (openSet.TryDequeue(out var current, out var priority) && current is not null)
        {
            if (!closed.Add(current.Id))
            {
                continue;
            }

            visitedOrder.Add(current);

            if (current.Id.Equals(goal.Id, StringComparison.OrdinalIgnoreCase))
            {
                var path = ReconstructPath(goal.Id, cameFrom, graph);
                return new PathResult(true, gScore[goal.Id], path, visitedOrder);
            }

            foreach (var edge in graph.GetOutgoingEdges(current))
            {
                if (edge.Weight < 0)
                {
                    throw new InvalidOperationException("A* cannot process negative edge weights.");
                }

                var neighbor = GetNeighbor(edge, current);
                if (neighbor is null || closed.Contains(neighbor.Id))
                {
                    continue;
                }

                var tentativeG = gScore[current.Id] + edge.Weight;
                if (tentativeG < gScore[neighbor.Id])
                {
                    cameFrom[neighbor.Id] = current.Id;
                    gScore[neighbor.Id] = tentativeG;
                    var fScore = tentativeG + heuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return PathResult.Failure(visitedOrder);
    }

    private static GraphNode? GetNeighbor(GraphEdge edge, GraphNode current)
    {
        if (edge.SourceId.Equals(current.Id, StringComparison.OrdinalIgnoreCase))
        {
            return edge.Target;
        }

        if (!edge.IsDirected && edge.TargetId.Equals(current.Id, StringComparison.OrdinalIgnoreCase))
        {
            return edge.Source;
        }

        return null;
    }

    private static IReadOnlyList<GraphNode> ReconstructPath(string goalId, IReadOnlyDictionary<string, string?> cameFrom, Graph graph)
    {
        var stack = new Stack<GraphNode>();
        var currentId = goalId;

        while (currentId is not null)
        {
            var node = graph.GetNode(currentId);
            if (node is null)
            {
                break;
            }

            stack.Push(node);
            cameFrom.TryGetValue(currentId, out currentId);
        }

        return stack.ToList();
    }
}
