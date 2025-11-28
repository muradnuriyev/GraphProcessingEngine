namespace GraphProcessingEngine.WebApi.Models.Requests;

public sealed class CreateGraphRequest
{
    public string Name { get; init; } = string.Empty;

    public bool IsDirected { get; init; }
}
