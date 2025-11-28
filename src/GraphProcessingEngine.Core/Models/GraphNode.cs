namespace GraphProcessingEngine.Core.Models;

/// <summary>
/// Represents a graph vertex with an identifier and optional layout position.
/// </summary>
public sealed class GraphNode
{
    public GraphNode(string id, double x = 0, double y = 0)
    {
        Id = string.IsNullOrWhiteSpace(id)
            ? throw new ArgumentException("Node id cannot be empty.", nameof(id))
            : id;
        X = x;
        Y = y;
    }

    public string Id { get; }

    /// <summary>
    /// X coordinate used by UI layers for placement.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Y coordinate used by UI layers for placement.
    /// </summary>
    public double Y { get; }

    public override string ToString() => Id;
}
