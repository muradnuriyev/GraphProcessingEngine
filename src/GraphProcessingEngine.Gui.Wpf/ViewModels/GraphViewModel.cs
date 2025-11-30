using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GraphProcessingEngine.Core.Algorithms;
using GraphProcessingEngine.Core.Builders;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Core.PathFinding;

namespace GraphProcessingEngine.Gui.Wpf.ViewModels;

public sealed class GraphViewModel : INotifyPropertyChanged
{
    private int _nodeCounter = 1;
    private double _edgeWeight = 1;
    private bool _edgeDirected;
    private string _selectedPathAlgorithm = "Dijkstra";
    private string _selectedTraversalAlgorithm = "BFS";
    private string _statusMessage = "Click to add nodes, select two to connect or run shortest path.";

    public ObservableCollection<NodeViewModel> Nodes { get; } = new();

    public ObservableCollection<EdgeViewModel> Edges { get; } = new();

    public string Name { get; init; } = "Sample";

    public IEnumerable<NodeViewModel> SelectedNodes => Nodes.Where(n => n.IsSelected);

    public double EdgeWeight
    {
        get => _edgeWeight;
        set => SetField(ref _edgeWeight, value);
    }

    public bool EdgeDirected
    {
        get => _edgeDirected;
        set => SetField(ref _edgeDirected, value);
    }

    public string SelectedPathAlgorithm
    {
        get => _selectedPathAlgorithm;
        set => SetField(ref _selectedPathAlgorithm, value);
    }

    public string SelectedTraversalAlgorithm
    {
        get => _selectedTraversalAlgorithm;
        set => SetField(ref _selectedTraversalAlgorithm, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public void AddNodeAt(double x, double y, string? id = null)
    {
        var nodeId = id ?? $"N{_nodeCounter++}";
        Nodes.Add(new NodeViewModel(nodeId, x, y));
    }

    public void ConnectSelected()
    {
        var selected = SelectedNodes.Take(2).ToList();
        if (selected.Count < 2)
        {
            StatusMessage = "Select two nodes first, then click Connect.";
            return;
        }

        var source = selected[0];
        var target = selected[1];
        Edges.Add(new EdgeViewModel(source, target, EdgeWeight, EdgeDirected));
        StatusMessage = $"Connected {source.Id} -> {target.Id} (w={EdgeWeight}, directed={EdgeDirected}).";
    }

    public void ClearHighlights()
    {
        foreach (var node in Nodes)
        {
            node.IsHighlighted = false;
        }

        foreach (var edge in Edges)
        {
            edge.IsHighlighted = false;
        }

        StatusMessage = "Highlights cleared.";
    }

    public void RunShortestPath()
    {
        var selected = SelectedNodes.Take(2).ToList();
        if (selected.Count < 2)
        {
            StatusMessage = "Select start and goal nodes, then run shortest path.";
            return;
        }

        var graph = BuildGraph();
        var start = selected[0].Id;
        var goal = selected[1].Id;

        IPathFindingStrategy strategy = SelectedPathAlgorithm.ToLowerInvariant() switch
        {
            "astar" or "a*" => new AStarStrategy(),
            _ => new DijkstraStrategy()
        };

        var finder = new PathFinder(strategy);
        var result = finder.FindPath(graph, start, goal);
        HighlightPath(result.Path.Select(n => n.Id).ToList());
        StatusMessage = result.Success
            ? $"Path ({strategy.Name}): {string.Join(" -> ", result.Path.Select(n => n.Id))} | distance: {result.Distance}"
            : "No path found.";
    }

    public void Traverse()
    {
        var start = SelectedNodes.FirstOrDefault();
        if (start is null)
        {
            StatusMessage = "Select a start node, then run traversal.";
            return;
        }

        var graph = BuildGraph();
        IReadOnlyList<GraphNode> visited = SelectedTraversalAlgorithm.ToLowerInvariant() switch
        {
            "dfs" => BfsDfsAlgorithms.DepthFirst(graph, start.Id),
            _ => BfsDfsAlgorithms.BreadthFirst(graph, start.Id)
        };

        foreach (var node in Nodes)
        {
            node.IsHighlighted = false;
        }

        foreach (var node in visited)
        {
            var vm = Nodes.FirstOrDefault(n => n.Id.Equals(node.Id, StringComparison.OrdinalIgnoreCase));
            if (vm is not null)
            {
                vm.IsHighlighted = true;
            }
        }

        StatusMessage = $"{SelectedTraversalAlgorithm} order: {string.Join(" -> ", visited.Select(n => n.Id))}";
    }

    public void DeleteSelectedNodes()
    {
        var toRemove = SelectedNodes.ToList();
        if (toRemove.Count == 0)
        {
            StatusMessage = "Select nodes to delete.";
            return;
        }

        foreach (var node in toRemove)
        {
            Nodes.Remove(node);
        }

        var remaining = Nodes.Select(n => n.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var edgesToRemove = Edges.Where(e => !remaining.Contains(e.Source.Id) || !remaining.Contains(e.Target.Id)).ToList();
        foreach (var edge in edgesToRemove)
        {
            Edges.Remove(edge);
        }

        StatusMessage = $"Deleted {toRemove.Count} node(s) and {edgesToRemove.Count} edge(s).";
    }

    public void ClearGraph()
    {
        Nodes.Clear();
        Edges.Clear();
        _nodeCounter = 1;
        StatusMessage = "Graph cleared.";
    }

    public Graph BuildGraph()
    {
        var builder = new GraphBuilder(false);
        foreach (var node in Nodes)
        {
            builder.AddNode(node.Id, node.X, node.Y);
        }

        var graph = builder.Build();
        foreach (var edge in Edges)
        {
            graph.AddEdge(edge.Source.Id, edge.Target.Id, edge.Weight, edge.IsDirected);
        }

        return graph;
    }

    private void HighlightPath(IReadOnlyList<string> pathNodeIds)
    {
        ClearHighlights();
        if (pathNodeIds.Count < 2)
        {
            return;
        }

        foreach (var nodeId in pathNodeIds)
        {
            var vm = Nodes.FirstOrDefault(n => n.Id.Equals(nodeId, StringComparison.OrdinalIgnoreCase));
            if (vm is not null)
            {
                vm.IsHighlighted = true;
            }
        }

        for (var i = 0; i < pathNodeIds.Count - 1; i++)
        {
            var a = pathNodeIds[i];
            var b = pathNodeIds[i + 1];
            foreach (var edge in Edges)
            {
                var matches = (edge.Source.Id.Equals(a, StringComparison.OrdinalIgnoreCase) &&
                               edge.Target.Id.Equals(b, StringComparison.OrdinalIgnoreCase))
                              || (!edge.IsDirected &&
                                  edge.Source.Id.Equals(b, StringComparison.OrdinalIgnoreCase) &&
                                  edge.Target.Id.Equals(a, StringComparison.OrdinalIgnoreCase));
                if (matches)
                {
                    edge.IsHighlighted = true;
                }
            }
        }
    }

    public static GraphViewModel FromGraph(Graph graph, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var vm = new GraphViewModel { Name = name ?? "Graph" };

        var nodeMap = graph.Nodes.ToDictionary(n => n.Id, n => new NodeViewModel(n.Id, n.X, n.Y));
        foreach (var node in nodeMap.Values)
        {
            vm.Nodes.Add(node);
            vm._nodeCounter = Math.Max(vm._nodeCounter, ParseCounter(node.Id) + 1);
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

    private static int ParseCounter(string id)
    {
        if (id.StartsWith("N", StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(id.AsSpan(1), out var number))
        {
            return number;
        }

        return 1;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
