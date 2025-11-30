using System.ComponentModel;
using System.Runtime.CompilerServices;
using GraphProcessingEngine.Core.Models;
using GraphProcessingEngine.Gui.Wpf.Services;

namespace GraphProcessingEngine.Gui.Wpf.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public MainViewModel(Graph graph, GraphLayoutService layoutService)
    {
        var graphVm = GraphViewModel.FromGraph(graph, "Demo Graph");
        layoutService.ApplyCircularLayout(graphVm);
        Graph = graphVm;
        Graph.PropertyChanged += GraphOnPropertyChanged;
    }

    public GraphViewModel Graph { get; }

    public IReadOnlyList<string> PathAlgorithms { get; } = new[] { "Dijkstra", "A*" };

    public IReadOnlyList<string> TraversalAlgorithms { get; } = new[] { "BFS", "DFS" };

    public string Title => "Graph Processing Engine";

    public string SelectedAlgorithm
    {
        get => Graph.SelectedPathAlgorithm;
        set => Graph.SelectedPathAlgorithm = value;
    }

    public string SelectedTraversal
    {
        get => Graph.SelectedTraversalAlgorithm;
        set => Graph.SelectedTraversalAlgorithm = value;
    }

    public double EdgeWeight
    {
        get => Graph.EdgeWeight;
        set => Graph.EdgeWeight = value;
    }

    public bool EdgeDirected
    {
        get => Graph.EdgeDirected;
        set => Graph.EdgeDirected = value;
    }

    public string StatusMessage
    {
        get => Graph.StatusMessage;
        set => Graph.StatusMessage = value;
    }

    public void ConnectSelected() => Graph.ConnectSelected();
    public void RunShortestPath() => Graph.RunShortestPath();
    public void Traverse() => Graph.Traverse();
    public void ClearHighlights() => Graph.ClearHighlights();
    public void DeleteSelected() => Graph.DeleteSelectedNodes();
    public void ClearGraph() => Graph.ClearGraph();

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void GraphOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(GraphViewModel.StatusMessage):
                OnPropertyChanged(nameof(StatusMessage));
                break;
            case nameof(GraphViewModel.EdgeDirected):
                OnPropertyChanged(nameof(EdgeDirected));
                break;
            case nameof(GraphViewModel.EdgeWeight):
                OnPropertyChanged(nameof(EdgeWeight));
                break;
            case nameof(GraphViewModel.SelectedPathAlgorithm):
                OnPropertyChanged(nameof(SelectedAlgorithm));
                break;
            case nameof(GraphViewModel.SelectedTraversalAlgorithm):
                OnPropertyChanged(nameof(SelectedTraversal));
                break;
        }
    }
}
