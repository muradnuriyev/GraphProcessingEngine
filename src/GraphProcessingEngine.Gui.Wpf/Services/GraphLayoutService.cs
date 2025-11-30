using GraphProcessingEngine.Gui.Wpf.ViewModels;

namespace GraphProcessingEngine.Gui.Wpf.Services;

public sealed class GraphLayoutService
{
    public void ApplyCircularLayout(GraphViewModel viewModel, double radius = 220, double centerX = 480, double centerY = 320)
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
