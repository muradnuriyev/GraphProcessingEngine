using GraphProcessingEngine.Core.Algorithms;
using GraphProcessingEngine.Core.Builders;

namespace GraphProcessingEngine.Core.Tests.AlgorithmsTests;

public class BfsTests
{
    [Fact]
    public void BreadthFirst_VisitsNeighborsLevelByLevel()
    {
        var graph = new GraphBuilder()
            .AddNode("A")
            .AddNode("B")
            .AddNode("C")
            .AddEdge("A", "B")
            .AddEdge("A", "C")
            .Build();

        var order = BfsDfsAlgorithms.BreadthFirst(graph, "A");

        Assert.Equal(new[] { "A", "B", "C" }, order.Select(n => n.Id));
    }
}
