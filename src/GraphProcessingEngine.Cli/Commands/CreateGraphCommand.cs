using GraphProcessingEngine.Cli.Services;

namespace GraphProcessingEngine.Cli.Commands;

public sealed class CreateGraphCommand
{
    private readonly CliGraphService _service;

    public CreateGraphCommand(CliGraphService service)
    {
        _service = service;
    }

    public async Task<int> ExecuteAsync(string graphName, bool isDirected)
    {
        await _service.CreateGraphAsync(graphName, isDirected);
        Console.WriteLine($"Graph '{graphName}' created (directed={isDirected}).");
        return 0;
    }
}
