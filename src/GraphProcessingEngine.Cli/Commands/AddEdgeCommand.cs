using GraphProcessingEngine.Cli.Services;

namespace GraphProcessingEngine.Cli.Commands;

public sealed class AddEdgeCommand
{
    private readonly CliGraphService _service;

    public AddEdgeCommand(CliGraphService service)
    {
        _service = service;
    }

    public async Task<int> ExecuteAsync(string graphName, string sourceId, string targetId, double weight, bool? isDirected)
    {
        await _service.AddEdgeAsync(graphName, sourceId, targetId, weight, isDirected);
        var directionLabel = isDirected switch
        {
            true => "directed",
            false => "undirected",
            _ => "default"
        };
        Console.WriteLine($"Edge {sourceId} -> {targetId} (w={weight}, {directionLabel}) saved to '{graphName}'.");
        return 0;
    }
}
