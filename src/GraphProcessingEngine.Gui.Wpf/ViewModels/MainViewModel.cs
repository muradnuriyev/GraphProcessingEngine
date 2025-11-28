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
    }

    public GraphViewModel Graph { get; }

    public IReadOnlyList<string> Algorithms { get; } = new[] { "BFS", "DFS", "Dijkstra", "A*" };

    public string Title => "Graph Processing Engine";

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
