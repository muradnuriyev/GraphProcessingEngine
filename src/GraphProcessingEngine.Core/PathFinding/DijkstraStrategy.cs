using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.PathFinding.PriorityQueue;

namespace GraphProcessingEngine.Core.PathFinding;

public sealed class DijkstraStrategy : IPathFindingStrategy
{
    public string Name => "Dijkstra";

    public PathResult FindPath(Graph graph, string startNodeId, string goalNodeId, Func<GraphNode, GraphNode, double>? heuristic = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var start = graph.GetNode(startNodeId) ?? throw new ArgumentException($"Start node '{startNodeId}' is missing.", nameof(startNodeId));
        var goal = graph.GetNode(goalNodeId) ?? throw new ArgumentException($"Goal node '{goalNodeId}' is missing.", nameof(goalNodeId));

        var distances = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        var previous = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        var queue = new BinaryHeapPriorityQueue<GraphNode>();
        var visitedOrder = new List<GraphNode>();
        var settled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var node in graph.Nodes)
        {
            distances[node.Id] = double.PositiveInfinity;
            previous[node.Id] = null;
        }

        distances[start.Id] = 0;
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var current, out var priority) && current is not null)
        {
            if (!settled.Add(current.Id))
            {
                continue;
            }

            visitedOrder.Add(current);

            if (current.Id.Equals(goal.Id, StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            foreach (var edge in graph.GetOutgoingEdges(current))
            {
                if (edge.Weight < 0)
                {
                    throw new InvalidOperationException("Dijkstra cannot process negative edge weights.");
                }

                var neighbor = GetNeighbor(edge, current);
                if (neighbor is null || settled.Contains(neighbor.Id))
                {
                    continue;
                }

                var candidate = distances[current.Id] + edge.Weight;
                if (candidate < distances[neighbor.Id])
                {
                    distances[neighbor.Id] = candidate;
                    previous[neighbor.Id] = current.Id;
                    queue.Enqueue(neighbor, candidate);
                }
            }
        }

        if (double.IsPositiveInfinity(distances[goal.Id]))
        {
            return PathResult.Failure(visitedOrder);
        }

        var path = ReconstructPath(goal.Id, previous, graph);
        return new PathResult(true, distances[goal.Id], path, visitedOrder);
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

    private static IReadOnlyList<GraphNode> ReconstructPath(string goalId, IReadOnlyDictionary<string, string?> previous, Graph graph)
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
            previous.TryGetValue(currentId, out currentId);
        }

        return stack.ToList();
    }
}
