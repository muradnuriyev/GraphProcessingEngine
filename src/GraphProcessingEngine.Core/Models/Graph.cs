using System.Collections.ObjectModel;

namespace GraphProcessingEngine.Core.Models;

/// <summary>
/// Mutable in-memory graph representation with lightweight adjacency tracking.
/// </summary>
public sealed class Graph
{
    private readonly Dictionary<string, GraphNode> _nodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<GraphEdge> _edges = new();
    private readonly Dictionary<string, List<GraphEdge>> _adjacency = new(StringComparer.OrdinalIgnoreCase);

    public Graph(bool isDirected = false)
    {
        IsDirected = isDirected;
    }

    /// <summary>
    /// Default direction for newly added edges.
    /// </summary>
    public bool IsDirected { get; }

    /// <summary>
    /// Increments whenever the graph topology changes. Used by caches.
    /// </summary>
    public int Version { get; private set; }

    public IReadOnlyCollection<GraphNode> Nodes => new ReadOnlyCollection<GraphNode>(_nodes.Values.ToList());

    public IReadOnlyCollection<GraphEdge> Edges => new ReadOnlyCollection<GraphEdge>(_edges);

    public GraphNode AddNode(string id, double x = 0, double y = 0)
    {
        if (_nodes.ContainsKey(id))
        {
            throw new InvalidOperationException($"Node '{id}' already exists.");
        }

        var node = new GraphNode(id, x, y);
        _nodes.Add(id, node);
        _adjacency.TryAdd(id, []);
        Version++;
        return node;
    }

    public GraphEdge AddEdge(string sourceId, string targetId, double weight = 1, bool? isDirected = null)
    {
        var source = GetNode(sourceId) ?? throw new KeyNotFoundException($"Node '{sourceId}' is missing.");
        var target = GetNode(targetId) ?? throw new KeyNotFoundException($"Node '{targetId}' is missing.");
        var directed = isDirected ?? IsDirected;
        var edge = new GraphEdge(source, target, weight, directed);

        _edges.Add(edge);
        AddToAdjacency(sourceId, edge);
        if (!edge.IsDirected)
        {
            AddToAdjacency(targetId, edge);
        }

        Version++;
        return edge;
    }

    public GraphNode? GetNode(string id) =>
        id is null ? null : (_nodes.TryGetValue(id, out var node) ? node : null);

    public IEnumerable<GraphEdge> GetOutgoingEdges(GraphNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return _adjacency.TryGetValue(node.Id, out var edges)
            ? edges
            : Enumerable.Empty<GraphEdge>();
    }

    public bool ContainsNode(string id) => _nodes.ContainsKey(id);

    private void AddToAdjacency(string nodeId, GraphEdge edge)
    {
        if (!_adjacency.TryGetValue(nodeId, out var edges))
        {
            edges = new List<GraphEdge>();
            _adjacency[nodeId] = edges;
        }

        edges.Add(edge);
    }
}
