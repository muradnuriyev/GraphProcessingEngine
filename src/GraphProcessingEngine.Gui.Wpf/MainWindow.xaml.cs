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
