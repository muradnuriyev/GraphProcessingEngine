namespace GraphProcessingEngine.WebApi.Models.Responses;

public sealed class ShortestPathResponse
{
    public bool Success { get; init; }

    public double Distance { get; init; }

    public IReadOnlyList<string> Path { get; init; } = Array.Empty<string>();

    public IReadOnlyList<string> VisitedOrder { get; init; } = Array.Empty<string>();

    public string Algorithm { get; init; } = string.Empty;
}
