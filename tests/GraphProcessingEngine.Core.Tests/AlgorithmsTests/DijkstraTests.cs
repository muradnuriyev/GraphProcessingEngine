using GraphProcessingEngine.Core.Builders;
using GraphProcessingEngine.Core.PathFinding;

namespace GraphProcessingEngine.Core.Tests.AlgorithmsTests;

public class DijkstraTests
{
    [Fact]
    public void Dijkstra_FindsShortestWeightedPath()
    {
        var graph = new GraphBuilder()
            .AddNode("A")
            .AddNode("B")
            .AddNode("C")
            .AddEdge("A", "B", 1)
            .AddEdge("B", "C", 2)
            .AddEdge("A", "C", 5)
            .Build();

        var finder = new PathFinder(new DijkstraStrategy());
        var result = finder.FindPath(graph, "A", "C");

        Assert.True(result.Success);
        Assert.Equal(3, result.Distance);
        Assert.Equal(new[] { "A", "B", "C" }, result.Path.Select(n => n.Id));
    }
}
