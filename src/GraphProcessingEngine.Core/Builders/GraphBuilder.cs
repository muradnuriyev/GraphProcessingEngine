using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Core.Builders;

/// <summary>
/// Fluent helper for constructing graphs with optional coordinates and weights.
/// </summary>
public sealed class GraphBuilder
{
    private readonly Graph _graph;

    public GraphBuilder(bool isDirected = false)
    {
        _graph = new Graph(isDirected);
    }

    public GraphBuilder AddNode(string id, double x = 0, double y = 0)
    {
        _graph.AddNode(id, x, y);
        return this;
    }

    public GraphBuilder AddEdge(string sourceId, string targetId, double weight = 1, bool? isDirected = null)
    {
        _graph.AddEdge(sourceId, targetId, weight, isDirected);
        return this;
    }

    public Graph Build() => _graph;
}
