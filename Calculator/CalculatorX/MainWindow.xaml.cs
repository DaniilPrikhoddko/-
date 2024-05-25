using RPNCalc;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CalculatorX
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cForGraphic_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasDrawer canvasDrawer = new CanvasDrawer(cForGraphic, -35, 33, 1, 10, 0, 0);
            canvasDrawer.DrawAxis();
            canvasDrawer.DrawMarks();
            canvasDrawer.DrawTriangle();
        }
        private void btnStart(object sender, RoutedEventArgs e)
        {
            cForGraphic.Children.Clear();
            RedrawCanvas();
            lblStep.Content = (float)sStep.Value;
        }
        private void cForGraphic_MouseMove(object sender, MouseEventArgs e)
        {
            Point uiPoint = Mouse.GetPosition(cForGraphic);
            float zoom = (float)sZoom.Value;           
            var mathPoint = Mouse.GetPosition(cForGraphic).ToMathCoordinates(cForGraphic, zoom);
            lblUiCoordinates.Content = $"{uiPoint.X:0.#},{uiPoint.Y:0.#}";
            lblMathCoordinates.Content = $"{mathPoint.X:0.#},{mathPoint.Y:0.#}";
        }

        private void sStep_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).Value = e.NewValue;
            cForGraphic.Children.Clear();
            if (!(string.IsNullOrEmpty(tbExpression.Text)))
            {
                RedrawCanvas();
                lblStep.Content = (float) sStep.Value;
            }
        }
        private void sZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).Value = e.NewValue;
            if (sZoom.Value != 0 && sStep != null)
            {
                sStep.Value = 1 / sZoom.Value * 10;
            }
            cForGraphic.Children.Clear();
            if (! (string.IsNullOrEmpty(tbExpression.Text)))
            {
                RedrawCanvas();
                lblZoom.Content = (int) sZoom.Value;
            }

        }

        private void sbAxisX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((ScrollBar)sender).Value = e.NewValue;
            cForGraphic.Children.Clear();
            RedrawCanvas();

        }
        private void sbAxisY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((ScrollBar)sender).Value = e.NewValue;
            cForGraphic.Children.Clear();
            RedrawCanvas();

        }
        private void RedrawCanvas()
        {
            float start =(float) -(cForGraphic.ActualWidth / (2 * sZoom.Value));
            float end = (float) (cForGraphic.ActualWidth / (2 * sZoom.Value));
            float step = (float) sStep.Value;
            float zoom = (float) sZoom.Value;
            float xAxisDisplacement = (float) sbAxisX.Value;
            float yAxisDisplacement = (float) sbAxisY.Value;

            CanvasDrawer canvasDrawer = new CanvasDrawer(cForGraphic, start, end, step, zoom, xAxisDisplacement, yAxisDisplacement);
            canvasDrawer.DrawAxis();
            canvasDrawer.DrawMarks();
            canvasDrawer.DrawTriangle();

            RpnCalculator calculator = new RpnCalculator(tbExpression.Text);
            var points = new List<Point>();
            List<Tokens> RPN = calculator.GetRPN();
            float lastY = 0;
            float result = 0;

            for (float i = start + xAxisDisplacement/zoom; i <= end + xAxisDisplacement/zoom; i += step)
            {
                result = calculator.CalculateExpression(RPN, i);
                if (float.IsNaN(result) || Math.Abs(lastY - result) >= cForGraphic.ActualHeight)
                {
                    canvasDrawer.DrawGraphic(points);
                    points.Clear();
                }
                else
                {
                    points.Add(new Point(i - xAxisDisplacement / zoom, result + yAxisDisplacement / zoom));
                    lastY = result;
                }

            }

            canvasDrawer.DrawGraphic(points);
        }

    }
}