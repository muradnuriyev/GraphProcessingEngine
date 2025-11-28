using GraphProcessingEngine.Core.Builders;
using GraphProcessingEngine.Core.PathFinding;

namespace GraphProcessingEngine.Core.Tests.AlgorithmsTests;

public class AStarTests
{
    [Fact]
    public void AStar_UsesHeuristicToFindShortestPath()
    {
        var graph = new GraphBuilder()
            .AddNode("A", 0, 0)
            .AddNode("B", 1, 0)
            .AddNode("C", 2, 0)
            .AddEdge("A", "B", 1)
            .AddEdge("B", "C", 1)
            .AddEdge("A", "C", 5)
            .Build();

        var finder = new PathFinder(new AStarStrategy());
        var result = finder.FindPath(graph, "A", "C", (a, b) =>
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        });

        Assert.True(result.Success);
        Assert.Equal(2, result.Distance);
        Assert.Equal(new[] { "A", "B", "C" }, result.Path.Select(n => n.Id));
    }
}
