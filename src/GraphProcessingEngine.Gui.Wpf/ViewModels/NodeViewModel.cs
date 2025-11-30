using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphProcessingEngine.Gui.Wpf.ViewModels;

public sealed class NodeViewModel : INotifyPropertyChanged
{
    private double _x;
    private double _y;
    private bool _isSelected;
    private bool _isHighlighted;

    public NodeViewModel(string id, double x = 0, double y = 0)
    {
        Id = id;
        _x = x;
        _y = y;
    }

    public string Id { get; }

    public double X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    public double Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    public bool IsHighlighted
    {
        get => _isHighlighted;
        set => SetField(ref _isHighlighted, value);
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
