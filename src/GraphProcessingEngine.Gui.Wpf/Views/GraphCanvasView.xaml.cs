using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GraphProcessingEngine.Gui.Wpf.ViewModels;

namespace GraphProcessingEngine.Gui.Wpf.Views;

public partial class GraphCanvasView : UserControl
{
    private bool _isDragging;
    private Point _dragOffset;
    private NodeViewModel? _draggedNode;
    private bool _isPointerDownOnNode;

    public GraphCanvasView()
    {
        InitializeComponent();
    }

    private void Node_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Border border && border.DataContext is NodeViewModel node)
        {
            ToggleSelection(node, Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
            _isPointerDownOnNode = true;
            _isDragging = true;
            _draggedNode = node;
            var position = e.GetPosition(border);
            _dragOffset = position;
            border.CaptureMouse();
            e.Handled = true;
        }
    }

    private void Node_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || _draggedNode is null || sender is not Border border)
        {
            return;
        }

        var canvasPos = e.GetPosition(this);
        _draggedNode.X = canvasPos.X - _dragOffset.X;
        _draggedNode.Y = canvasPos.Y - _dragOffset.Y;
    }

    private void Node_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        _draggedNode = null;
        _isPointerDownOnNode = false;
        if (sender is Border border)
        {
            border.ReleaseMouseCapture();
        }
    }

    private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Avoid adding a node when releasing after dragging/selecting.
        if (_isPointerDownOnNode)
        {
            _isPointerDownOnNode = false;
            return;
        }

        if (DataContext is GraphViewModel graph)
        {
            var pos = e.GetPosition(this);
            graph.AddNodeAt(pos.X, pos.Y);
        }
    }

    private void ToggleSelection(NodeViewModel node, bool multiSelect)
    {
        if (!multiSelect)
        {
            if (DataContext is GraphViewModel graph)
            {
                foreach (var n in graph.Nodes)
                {
                    n.IsSelected = false;
                }
            }

            node.IsSelected = true;
            return;
        }

        node.IsSelected = !node.IsSelected;
    }
}
