using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.Repositories;

namespace GraphProcessingEngine.WebApi.Endpoints;

internal sealed class GraphStore
{
    private readonly IGraphRepository _repository;
    private readonly string _storageDirectory;

    public GraphStore(IGraphRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _storageDirectory = Path.Combine(environment.ContentRootPath, "data");
        Directory.CreateDirectory(_storageDirectory);
    }

    public Graph CreateNew(bool isDirected) => new(isDirected);

    public Task SaveAsync(Graph graph, string name, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(graph, PathFor(name), cancellationToken);

    public Task<Graph> LoadAsync(string name, CancellationToken cancellationToken = default) =>
        _repository.LoadAsync(PathFor(name), cancellationToken);

    public bool Exists(string name) => File.Exists(PathFor(name));

    public IReadOnlyList<string> ListGraphs() =>
        Directory.GetFiles(_storageDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(n => n is not null)
            .ToList()!;

    private string PathFor(string name) => Path.Combine(_storageDirectory, $"{name}.json");
}
