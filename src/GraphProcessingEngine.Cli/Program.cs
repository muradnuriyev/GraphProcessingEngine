using GraphProcessingEngine.Cli.Commands;
using GraphProcessingEngine.Cli.Services;

var service = new CliGraphService();
var exitCode = await HandleAsync(args, service);
return exitCode;

static async Task<int> HandleAsync(string[] args, CliGraphService service)
{
    if (args.Length == 0)
    {
        PrintHelp();
        return 1;
    }

    var command = args[0].ToLowerInvariant();
    try
    {
        return command switch
        {
            "create" => await RunCreateAsync(args, service),
            "add-edge" => await RunAddEdgeAsync(args, service),
            "shortest-path" => await RunShortestPathAsync(args, service),
            "list" => await new ListGraphsCommand(service).ExecuteAsync(),
            _ => UnknownCommand(command)
        };
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

static async Task<int> RunCreateAsync(string[] args, CliGraphService service)
{
    if (args.Length < 2)
    {
        Console.WriteLine("Usage: create <graphName> [--directed]");
        return 1;
    }

    var graphName = args[1];
    var directed = args.Contains("--directed", StringComparer.OrdinalIgnoreCase);
    var command = new CreateGraphCommand(service);
    return await command.ExecuteAsync(graphName, directed);
}

static async Task<int> RunAddEdgeAsync(string[] args, CliGraphService service)
{
    if (args.Length < 4)
    {
        Console.WriteLine("Usage: add-edge <graphName> <sourceId> <targetId> [weight] [--directed|--undirected]");
        return 1;
    }

    var graphName = args[1];
    var sourceId = args[2];
    var targetId = args[3];
    var weight = args.Length >= 5 && double.TryParse(args[4], out var parsedWeight) ? parsedWeight : 1d;

    bool? isDirected = args switch
    {
        var a when a.Contains("--directed", StringComparer.OrdinalIgnoreCase) => true,
        var a when a.Contains("--undirected", StringComparer.OrdinalIgnoreCase) => false,
        _ => null
    };

    var command = new AddEdgeCommand(service);
    return await command.ExecuteAsync(graphName, sourceId, targetId, weight, isDirected);
}

static async Task<int> RunShortestPathAsync(string[] args, CliGraphService service)
{
    if (args.Length < 4)
    {
        Console.WriteLine("Usage: shortest-path <graphName> <startId> <goalId> [--algorithm dijkstra|astar]");
        return 1;
    }

    var graphName = args[1];
    var startId = args[2];
    var goalId = args[3];
    var algorithm = "dijkstra";

    var algorithmFlag = args.FirstOrDefault(a => a.StartsWith("--algorithm", StringComparison.OrdinalIgnoreCase));
    if (algorithmFlag is not null)
    {
        var parts = algorithmFlag.Split('=', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
            algorithm = parts[1];
        }
    }

    var command = new ShortestPathCommand(service);
    return await command.ExecuteAsync(graphName, startId, goalId, algorithm);
}

static void PrintHelp()
{
    Console.WriteLine("GraphProcessingEngine CLI");
    Console.WriteLine("Commands:");
    Console.WriteLine("  create <graphName> [--directed]");
    Console.WriteLine("  add-edge <graphName> <sourceId> <targetId> [weight] [--directed|--undirected]");
    Console.WriteLine("  shortest-path <graphName> <startId> <goalId> [--algorithm dijkstra|astar]");
    Console.WriteLine("  list");
}

static int UnknownCommand(string command)
{
    Console.WriteLine($"Unknown command '{command}'.");
    PrintHelp();
    return 1;
}
