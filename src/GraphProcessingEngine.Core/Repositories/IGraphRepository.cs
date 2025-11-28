using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.Repositories;

public interface IGraphRepository
{
    Task SaveAsync(Graph graph, string filePath, CancellationToken cancellationToken = default);

    Task<bool> GraphExists(string filePath, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(File.Exists(filePath));
    }

    Task<Graph> LoadAsync(string filePath, CancellationToken cancellationToken = default);
}
