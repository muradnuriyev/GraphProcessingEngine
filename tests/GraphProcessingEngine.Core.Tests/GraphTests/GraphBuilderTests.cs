using GraphProcessingEngine.Core.Builders;

namespace GraphProcessingEngine.Core.Tests.GraphTests;

public class GraphBuilderTests
{
    [Fact]
    public void Build_FluentAddsNodesAndEdges()
    {
        var graph = new GraphBuilder()
            .AddNode("A")
            .AddNode("B")
            .AddEdge("A", "B", 3)
            .Build();

        Assert.Equal(2, graph.Nodes.Count);
        Assert.Single(graph.Edges);
        Assert.Equal(3, graph.Edges.Single().Weight);
    }
}
