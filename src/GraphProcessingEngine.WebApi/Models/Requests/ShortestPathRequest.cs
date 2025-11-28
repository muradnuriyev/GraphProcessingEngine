namespace GraphProcessingEngine.WebApi.Models.Requests;

public sealed class ShortestPathRequest
{
    public string StartId { get; init; } = string.Empty;

    public string GoalId { get; init; } = string.Empty;

    public string Algorithm { get; init; } = "dijkstra";
}
