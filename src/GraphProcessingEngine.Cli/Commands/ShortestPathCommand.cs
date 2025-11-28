using GraphProcessingEngine.Cli.Services;
using GraphProcessingEngine.Core.PathFinding;

namespace GraphProcessingEngine.Cli.Commands;

public sealed class ShortestPathCommand
{
    private readonly CliGraphService _service;

    public ShortestPathCommand(CliGraphService service)
    {
        _service = service;
    }

    public async Task<int> ExecuteAsync(string graphName, string startId, string goalId, string algorithm)
    {
        PathResult result;
        try
        {
            result = await _service.ComputeShortestPathAsync(graphName, startId, goalId, algorithm);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to compute path: {ex.Message}");
            return 1;
        }

        if (!result.Success)
        {
            Console.WriteLine("No path found.");
            return 0;
        }

        var pathText = string.Join(" -> ", result.Path.Select(n => n.Id));
        Console.WriteLine($"Algorithm: {algorithm}");
        Console.WriteLine($"Distance: {result.Distance}");
        Console.WriteLine($"Path: {pathText}");
        return 0;
    }
}
