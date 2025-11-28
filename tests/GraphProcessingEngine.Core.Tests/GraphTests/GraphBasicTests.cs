using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.Tests.GraphTests;

public class GraphBasicTests
{
    [Fact]
    public void AddNode_IncrementsVersionAndStoresNode()
    {
        var graph = new Graph();

        var node = graph.AddNode("A");

        Assert.True(graph.ContainsNode("A"));
        Assert.Equal("A", node.Id);
        Assert.Equal(1, graph.Version);
    }

    [Fact]
    public void AddEdge_AddsNodesIfPresentAndTracksAdjacency()
    {
        var graph = new Graph();
        graph.AddNode("A");
        graph.AddNode("B");

        graph.AddEdge("A", "B", 2);

        var edges = graph.Edges.ToList();
        Assert.Single(edges);
        Assert.Equal(2, edges[0].Weight);
        Assert.Equal(2, graph.Version);
    }
}
