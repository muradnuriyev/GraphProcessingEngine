using System.Collections.ObjectModel;
using GraphProcessingEngine.Core.Models;

namespace GraphProcessingEngine.Gui.Wpf.ViewModels;

public sealed class GraphViewModel
{
    public ObservableCollection<NodeViewModel> Nodes { get; } = new();

    public ObservableCollection<EdgeViewModel> Edges { get; } = new();

    public string Name { get; init; } = "Sample";

    public static GraphViewModel FromGraph(Graph graph, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var vm = new GraphViewModel { Name = name ?? "Graph" };

        var nodeMap = graph.Nodes.ToDictionary(n => n.Id, n => new NodeViewModel(n.Id, n.X, n.Y));
        foreach (var node in nodeMap.Values)
        {
            vm.Nodes.Add(node);
        }

        foreach (var edge in graph.Edges)
        {
            if (nodeMap.TryGetValue(edge.SourceId, out var source) &&
                nodeMap.TryGetValue(edge.TargetId, out var target))
            {
                vm.Edges.Add(new EdgeViewModel(source, target, edge.Weight, edge.IsDirected));
            }
        }

        return vm;
    }
}
