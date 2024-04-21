using RPNCalc;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    static class PointExtensions
    {
        public static Point ToMathCoordinates(this Point point, Canvas canvas, float zoom)
        {
            return new Point((point.X - canvas.ActualWidth / 2) / zoom, (point.Y - canvas.ActualHeight / 2) / zoom);
        }

        public static Point ToUiCoordinates(this Point point, Canvas canvas, float zoom)
        {
            return new Point((point.X * zoom + canvas.ActualWidth / 2), canvas.ActualHeight / 2 - point.Y * zoom);
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnStart(object sender, RoutedEventArgs e)
        {
            float valueOfVariable = float.Parse(tbVariableValue.Text);
            lblResult.Content = new RpnCalculator(tbExpression.Text).Calculate(valueOfVariable);
        }
        private void cForGraphic_MouseMove(object sender, MouseEventArgs e)
        {
            var uiPoint = Mouse.GetPosition(cForGraphic);
            var zoom = float.Parse(tbZoom.Text);
            var mathPoint = Mouse.GetPosition(cForGraphic).ToMathCoordinates(cForGraphic, zoom);
            lblUiCoordinates.Content = $"{uiPoint.X:0.#},{uiPoint.Y:0.#}";
            lblMathCoordinates.Content = $"{mathPoint.X:0.#},{mathPoint.Y:0.#}";
        }
    }
}