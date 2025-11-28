using GraphProcessingEngine.Core.Builders;
using GraphProcessingEngine.Core.Repositories;

namespace GraphProcessingEngine.Core.Tests.RepositoryTests;

public class FileGraphRepositoryTests
{
    [Fact]
    public async Task SaveAndLoad_RoundTripsGraph()
    {
        var repository = new FileGraphRepository();
        var graph = new GraphBuilder()
            .AddNode("A", 1, 2)
            .AddNode("B", 3, 4)
            .AddEdge("A", "B", 2.5)
            .Build();

        var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

        await repository.SaveAsync(graph, tempFile);
        var loaded = await repository.LoadAsync(tempFile);

        Assert.Equal(graph.Nodes.Count, loaded.Nodes.Count);
        Assert.Equal(graph.Edges.Single().Weight, loaded.Edges.Single().Weight);
    }
}
