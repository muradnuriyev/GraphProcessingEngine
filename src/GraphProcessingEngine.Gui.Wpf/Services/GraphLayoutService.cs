using GraphProcessingEngine.Gui.Wpf.ViewModels;

namespace GraphProcessingEngine.Gui.Wpf.Services;

public sealed class GraphLayoutService
{
    public void ApplyCircularLayout(GraphViewModel viewModel, double radius = 150, double centerX = 260, double centerY = 200)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        if (viewModel.Nodes.Count == 0)
        {
            return;
        }

        var step = (2 * Math.PI) / viewModel.Nodes.Count;
        for (var i = 0; i < viewModel.Nodes.Count; i++)
        {
            var node = viewModel.Nodes[i];
            node.X = centerX + radius * Math.Cos(i * step);
            node.Y = centerY + radius * Math.Sin(i * step);
        }
    }
}
