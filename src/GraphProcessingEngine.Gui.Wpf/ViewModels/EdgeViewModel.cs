using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphProcessingEngine.Gui.Wpf.ViewModels;

public sealed class EdgeViewModel : INotifyPropertyChanged
{
    public EdgeViewModel(NodeViewModel source, NodeViewModel target, double weight = 1, bool isDirected = false)
    {
        Source = source;
        Target = target;
        Weight = weight;
        IsDirected = isDirected;

        Source.PropertyChanged += OnNodeChanged;
        Target.PropertyChanged += OnNodeChanged;
    }

    public NodeViewModel Source { get; }

    public NodeViewModel Target { get; }

    public double Weight { get; }

    public bool IsDirected { get; }

    public double X1 => Source.X;

    public double Y1 => Source.Y;

    public double X2 => Target.X;

    public double Y2 => Target.Y;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnNodeChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(NodeViewModel.X) or nameof(NodeViewModel.Y))
        {
            NotifyPositionChanged();
        }
    }

    private void NotifyPositionChanged()
    {
        OnPropertyChanged(nameof(X1));
        OnPropertyChanged(nameof(Y1));
        OnPropertyChanged(nameof(X2));
        OnPropertyChanged(nameof(Y2));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
