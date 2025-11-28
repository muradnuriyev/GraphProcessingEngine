namespace GraphProcessingEngine.Core.Models;

/// <summary>
/// Represents a connection between two nodes. Direction is respected when IsDirected is true.
/// </summary>
public sealed class GraphEdge
{
    public GraphEdge(GraphNode source, GraphNode target, double weight = 1, bool? isDirected = null)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Target = target ?? throw new ArgumentNullException(nameof(target));
        Weight = weight;
        IsDirected = isDirected ?? false;
    }

    public GraphNode Source { get; }

    public GraphNode Target { get; }

    public double Weight { get; }

    public bool IsDirected { get; }

    public string SourceId => Source.Id;

    public string TargetId => Target.Id;

    public override string ToString() =>
        $"{SourceId} {(IsDirected ? "->" : "--")} {TargetId} (w={Weight})";
}
