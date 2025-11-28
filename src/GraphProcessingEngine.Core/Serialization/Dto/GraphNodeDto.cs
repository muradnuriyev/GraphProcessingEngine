using System.Text.Json.Serialization;

namespace GraphProcessingEngine.Core.Serialization.Dto;

public sealed record GraphNodeDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("x")] double X,
    [property: JsonPropertyName("y")] double Y);
