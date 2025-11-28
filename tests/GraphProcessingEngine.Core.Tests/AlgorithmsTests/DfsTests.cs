using GraphProcessingEngine.Core.Algorithms;
using GraphProcessingEngine.Core.Builders;

namespace GraphProcessingEngine.Core.Tests.AlgorithmsTests;

public class DfsTests
{
    [Fact]
    public void DepthFirst_VisitsDepthBeforeBreadth()
    {
        var graph = new GraphBuilder()
            .AddNode("A")
            .AddNode("B")
            .AddNode("C")
            .AddEdge("A", "B")
            .AddEdge("B", "C")
            .Build();

        var order = BfsDfsAlgorithms.DepthFirst(graph, "A");

        Assert.Equal(new[] { "A", "B", "C" }, order.Select(n => n.Id));
    }
}
