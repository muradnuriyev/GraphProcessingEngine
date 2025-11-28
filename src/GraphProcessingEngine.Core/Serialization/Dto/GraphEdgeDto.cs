using System.Text.Json.Serialization;

namespace GraphProcessingEngine.Core.Serialization.Dto;

public sealed record GraphEdgeDto(
    [property: JsonPropertyName("sourceId")] string SourceId,
    [property: JsonPropertyName("targetId")] string TargetId,
    [property: JsonPropertyName("weight")] double Weight,
    [property: JsonPropertyName("isDirected")] bool IsDirected);
