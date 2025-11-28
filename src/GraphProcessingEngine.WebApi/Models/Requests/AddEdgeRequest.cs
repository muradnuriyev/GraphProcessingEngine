namespace GraphProcessingEngine.WebApi.Models.Requests;

public sealed class AddEdgeRequest
{
    public string SourceId { get; init; } = string.Empty;

    public string TargetId { get; init; } = string.Empty;

    public double Weight { get; init; } = 1d;

    public bool? IsDirected { get; init; }
}
