using System.Text.Json.Serialization;

namespace GraphProcessingEngine.Core.Serialization.Dto;

public sealed record GraphDto(
    [property: JsonPropertyName("isDirected")] bool IsDirected,
    [property: JsonPropertyName("nodes")] IReadOnlyList<GraphNodeDto> Nodes,
    [property: JsonPropertyName("edges")] IReadOnlyList<GraphEdgeDto> Edges);
