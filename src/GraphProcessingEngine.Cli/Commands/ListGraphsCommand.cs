using GraphProcessingEngine.Cli.Services;

namespace GraphProcessingEngine.Cli.Commands;

public sealed class ListGraphsCommand
{
    private readonly CliGraphService _service;

    public ListGraphsCommand(CliGraphService service)
    {
        _service = service;
    }

    public async Task<int> ExecuteAsync()
    {
        var graphs = await _service.ListGraphsAsync();
        if (graphs.Count == 0)
        {
            Console.WriteLine("No graphs found.");
            return 0;
        }

        Console.WriteLine("Available graphs:");
        foreach (var graph in graphs)
        {
            Console.WriteLine($" - {graph}");
        }

        return 0;
    }
}
