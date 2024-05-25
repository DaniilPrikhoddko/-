using System.Windows;
using System.Windows.Controls;

namespace CalculatorX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    static class PointExtensions
    {
        public static Point ToMathCoordinates(this Point point, Canvas canvas, float zoom)
        {
            return new Point((point.X - canvas.ActualWidth / 2) / zoom, -(point.Y - canvas.ActualHeight / 2) / zoom);
        }

        public static Point ToUiCoordinates(this Point point, Canvas canvas, float zoom)
        {
            return new Point((point.X * zoom + canvas.ActualWidth / 2), canvas.ActualHeight / 2 - point.Y * zoom);
        }
    }
}