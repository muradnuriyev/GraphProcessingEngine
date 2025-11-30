using System.Windows;
using GraphProcessingEngine.Core.Builders;
using GraphProcessingEngine.Gui.Wpf.Services;
using GraphProcessingEngine.Gui.Wpf.ViewModels;

namespace GraphProcessingEngine.Gui.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = BuildDemoViewModel();
    }

    private MainViewModel ViewModel => (MainViewModel)DataContext;

    private void ConnectSelected_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ConnectSelected();
    }

    private void ShortestPath_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.RunShortestPath();
    }

    private void Traverse_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Traverse();
    }

    private void ClearHighlights_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ClearHighlights();
    }

    private void DeleteSelected_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.DeleteSelected();
    }

    private void ClearGraph_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ClearGraph();
    }

    private static MainViewModel BuildDemoViewModel()
    {
        var graph = new GraphBuilder()
            .AddNode("A")
            .AddNode("B")
            .AddNode("C")
            .AddNode("D")
            .AddEdge("A", "B", 1)
            .AddEdge("B", "C", 2)
            .AddEdge("C", "D", 2)
            .AddEdge("A", "D", 4)
            .Build();

        var layoutService = new GraphLayoutService();
        return new MainViewModel(graph, layoutService);
    }
}
