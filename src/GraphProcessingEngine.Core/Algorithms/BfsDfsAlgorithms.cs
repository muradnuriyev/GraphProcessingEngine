using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.Algorithms;

/// <summary>
/// Basic traversal routines used by UI and tests.
/// </summary>
public static class BfsDfsAlgorithms
{
    public static IReadOnlyList<GraphNode> BreadthFirst(Graph graph, string startNodeId)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var start = graph.GetNode(startNodeId) ?? throw new ArgumentException($"Start node '{startNodeId}' is missing.", nameof(startNodeId));

        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var queue = new Queue<GraphNode>();
        var order = new List<GraphNode>();

        queue.Enqueue(start);
        visited.Add(start.Id);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            order.Add(current);

            foreach (var neighbor in GetNeighbors(graph, current))
            {
                if (visited.Add(neighbor.Id))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return order;
    }

    public static IReadOnlyList<GraphNode> DepthFirst(Graph graph, string startNodeId)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var start = graph.GetNode(startNodeId) ?? throw new ArgumentException($"Start node '{startNodeId}' is missing.", nameof(startNodeId));

        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var stack = new Stack<GraphNode>();
        var order = new List<GraphNode>();

        stack.Push(start);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (!visited.Add(current.Id))
            {
                continue;
            }

            order.Add(current);

            foreach (var neighbor in GetNeighbors(graph, current).Reverse())
            {
                stack.Push(neighbor);
            }
        }

        return order;
    }

    private static IEnumerable<GraphNode> GetNeighbors(Graph graph, GraphNode node)
    {
        return graph.GetOutgoingEdges(node)
            .Select(edge => GetCounterpart(edge, node))
            .OfType<GraphNode>()
            .DistinctBy(n => n.Id);
    }

    private static GraphNode? GetCounterpart(GraphEdge edge, GraphNode current)
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
}
