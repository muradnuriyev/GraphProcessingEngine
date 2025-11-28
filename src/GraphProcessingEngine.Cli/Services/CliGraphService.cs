using GraphProcessingEngine.Core.Caching;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.PathFinding;
using GraphProcessingEngine.Core.Repositories;

namespace GraphProcessingEngine.Cli.Services;

public sealed class CliGraphService
{
    private readonly IGraphRepository _repository;
    private readonly string _storageDirectory;

    public CliGraphService(string? storageDirectory = null, IGraphRepository? repository = null)
    {
        _storageDirectory = storageDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "graphs");
        _repository = repository ?? new FileGraphRepository();
        Directory.CreateDirectory(_storageDirectory);
    }

    public async Task CreateGraphAsync(string name, bool isDirected, CancellationToken cancellationToken = default)
    {
        var graph = new Graph(isDirected);
        await _repository.SaveAsync(graph, GetPath(name), cancellationToken);
    }

    public async Task AddEdgeAsync(
        string graphName,
        string sourceId,
        string targetId,
        double weight = 1,
        bool? isDirected = null,
        CancellationToken cancellationToken = default)
    {
        var graph = await LoadGraphAsync(graphName, cancellationToken);
        if (!graph.ContainsNode(sourceId))
        {
            graph.AddNode(sourceId);
        }

        if (!graph.ContainsNode(targetId))
        {
            graph.AddNode(targetId);
        }

        graph.AddEdge(sourceId, targetId, weight, isDirected);
        await _repository.SaveAsync(graph, GetPath(graphName), cancellationToken);
    }

    public async Task<PathResult> ComputeShortestPathAsync(
        string graphName,
        string startId,
        string goalId,
        string algorithm = "dijkstra",
        CancellationToken cancellationToken = default)
    {
        var graph = await LoadGraphAsync(graphName, cancellationToken);
        var strategy = CreateStrategy(graph, algorithm);
        var pathFinder = new PathFinder(strategy);
        var heuristic = GetHeuristicFor(graph);
        return pathFinder.FindPath(graph, startId, goalId, heuristic);
    }

    public Task<IReadOnlyList<string>> ListGraphsAsync(CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(_storageDirectory))
        {
            return Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
        }

        var graphs = Directory.GetFiles(_storageDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name!)
            .ToList();

        return Task.FromResult<IReadOnlyList<string>>(graphs);
    }

    private string GetPath(string name) => Path.Combine(_storageDirectory, $"{name}.json");

    private async Task<Graph> LoadGraphAsync(string name, CancellationToken cancellationToken)
    {
        var path = GetPath(name);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Graph '{name}' not found in '{_storageDirectory}'.");
        }

        return await _repository.LoadAsync(path, cancellationToken);
    }

    private static IPathFindingStrategy CreateStrategy(Graph graph, string algorithm)
    {
        var normalized = algorithm.ToLowerInvariant();
        IPathFindingStrategy strategy = normalized switch
        {
            "astar" or "a*" => new AStarStrategy(),
            "dijkstra" => new DijkstraStrategy(),
            _ => throw new ArgumentException($"Unknown algorithm '{algorithm}'.", nameof(algorithm))
        };

        return new CachingPathFindingStrategy(strategy);
    }

    private static Func<GraphNode, GraphNode, double> GetHeuristicFor(Graph graph)
    {
        // Default heuristic: Euclidean distance using stored coordinates.
        return static (a, b) =>
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return double.IsFinite(distance) ? distance : 0;
        };
    }
}
