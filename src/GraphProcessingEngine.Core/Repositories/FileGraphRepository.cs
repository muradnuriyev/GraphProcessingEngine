using System.Text.Json;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.Serialization;
using GraphProcessingEngine.Core.Serialization.Dto;

namespace GraphProcessingEngine.Core.Repositories;

public sealed class FileGraphRepository : IGraphRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task SaveAsync(Graph graph, string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(graph);
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));
        }

        var dto = GraphDtoMapper.ToDto(graph);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath))!);
        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, dto, _options, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Graph> LoadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Graph file not found.", filePath);
        }

        await using var stream = File.OpenRead(filePath);
        var dto = await JsonSerializer.DeserializeAsync<GraphDto>(stream, _options, cancellationToken).ConfigureAwait(false)
                  ?? throw new InvalidDataException("Failed to deserialize graph file.");

        return GraphDtoMapper.FromDto(dto);
    }
}
