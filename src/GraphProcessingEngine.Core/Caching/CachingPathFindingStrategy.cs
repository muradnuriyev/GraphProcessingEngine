using System.Runtime.CompilerServices;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.PathFinding;

namespace GraphProcessingEngine.Core.Caching;

/// <summary>
/// Decorator that reuses computed paths until the graph version changes.
/// </summary>
public sealed class CachingPathFindingStrategy : IPathFindingStrategy
{
    private readonly IPathFindingStrategy _inner;
    private readonly Dictionary<CacheKey, PathResult> _cache = new(new CacheKeyComparer());

    public CachingPathFindingStrategy(IPathFindingStrategy inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public string Name => $"{_inner.Name} (cached)";

    public PathResult FindPath(Graph graph, string startNodeId, string goalNodeId, Func<GraphNode, GraphNode, double>? heuristic = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var key = new CacheKey(graph, graph.Version, startNodeId, goalNodeId);

        if (_cache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var result = _inner.FindPath(graph, startNodeId, goalNodeId, heuristic);
        _cache[key] = result;
        return result;
    }

    private readonly record struct CacheKey(Graph Graph, int Version, string Start, string Goal);

    private sealed class CacheKeyComparer : IEqualityComparer<CacheKey>
    {
        public bool Equals(CacheKey x, CacheKey y)
        {
            return ReferenceEquals(x.Graph, y.Graph)
                   && x.Version == y.Version
                   && string.Equals(x.Start, y.Start, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(x.Goal, y.Goal, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(CacheKey obj)
        {
            var hash = new HashCode();
            hash.Add(RuntimeHelpers.GetHashCode(obj.Graph));
            hash.Add(obj.Version);
            hash.Add(obj.Start, StringComparer.OrdinalIgnoreCase);
            hash.Add(obj.Goal, StringComparer.OrdinalIgnoreCase);
            return hash.ToHashCode();
        }
    }
}
