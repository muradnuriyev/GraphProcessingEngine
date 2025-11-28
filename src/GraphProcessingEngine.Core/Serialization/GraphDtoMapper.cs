using GraphProcessingEngine.Core.Builders;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.Serialization.Dto;

namespace GraphProcessingEngine.Core.Serialization;

public static class GraphDtoMapper
{
    public static GraphDto ToDto(Graph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);

        var nodes = graph.Nodes
            .Select(n => new GraphNodeDto(n.Id, n.X, n.Y))
            .ToList();

        var edges = graph.Edges
            .Select(e => new GraphEdgeDto(e.SourceId, e.TargetId, e.Weight, e.IsDirected))
            .ToList();

        return new GraphDto(graph.IsDirected, nodes, edges);
    }

    public static Graph FromDto(GraphDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var builder = new GraphBuilder(dto.IsDirected);
        foreach (var nodeDto in dto.Nodes)
        {
            builder.AddNode(nodeDto.Id, nodeDto.X, nodeDto.Y);
        }

        foreach (var edgeDto in dto.Edges)
        {
            builder.AddEdge(edgeDto.SourceId, edgeDto.TargetId, edgeDto.Weight, edgeDto.IsDirected);
        }

        return builder.Build();
    }
}
