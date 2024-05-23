using RPNCalc;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            return new Point((point.X - canvas.ActualWidth / 2) / zoom, -(point.Y - canvas.ActualHeight / 2) / zoom);
        }

        public static Point ToUiCoordinates(this Point point, Canvas canvas, float zoom)
        {
            return new Point((point.X * zoom + canvas.ActualWidth / 2), canvas.ActualHeight / 2 - point.Y * zoom);
        }
    }
    class CanvasDrawer
    {
        private Canvas _canvas;

        Point xAxisStart;
        Point xAxisEnd;
        Point yAxisStart;
        Point yAxisEnd;

        private readonly float _xStart;
        private readonly float _xEnd;
        private readonly float _step;
        private readonly float _zoom;
        private readonly float _xAxisDisplacement;
        private readonly float _yAxisDisplacement;

        public CanvasDrawer(Canvas canvas, float xStart, float xEnd, float step, float zoom, float xAxisDisplacement, float yAxisDisplacement)
        {
            _xStart = xStart;
            _xEnd = xEnd;
            _step = step;
            _zoom = zoom;
            _xAxisDisplacement = xAxisDisplacement;
            _yAxisDisplacement = yAxisDisplacement;
            _canvas = canvas;

            yAxisStart = new Point((int)_canvas.ActualWidth / 2 - _xAxisDisplacement, 0);
            yAxisEnd = new Point((int)_canvas.ActualWidth / 2 - _xAxisDisplacement, (int)_canvas.ActualHeight);
            xAxisStart = new Point(0, (int)_canvas.ActualHeight / 2 - _yAxisDisplacement);
            xAxisEnd = new Point((int)_canvas.ActualWidth, (int)_canvas.ActualHeight / 2 - _yAxisDisplacement);


           
        }

        public void DrawLine(Point start, Point end, Color color, int thickness)
        {
            Line line = new Line();
            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = end.X;
            line.Y2 = end.Y;
            SolidColorBrush brush = new SolidColorBrush(color);
            line.StrokeThickness = thickness;
            line.Stroke = brush;
            _canvas.Children.Add(line);
        }

        public void DrawPoint(Point point)
        {
            Ellipse circle = new Ellipse();
            circle.Height = 4;
            circle.Width = 4;
            circle.Stroke = Brushes.DarkMagenta;
            circle.StrokeThickness = 2;
            Canvas.SetLeft(circle, point.X - 2);
            Canvas.SetTop(circle, point.Y - 2);
            _canvas.Children.Add(circle);
        }

        public void DrawAxis()
        {
            DrawLine(xAxisStart, xAxisEnd, Colors.Magenta, 1);
            DrawLine(yAxisStart, yAxisEnd, Colors.Purple, 1);
        }
        public void DrawMarks()
        {
            for (float i = 0; i <= _canvas.ActualWidth/2 + _xAxisDisplacement; i += _step) //ось иксов
            {
                Point p1 = new Point((float)_canvas.ActualWidth / 2 + i * _zoom - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 - 6 - _yAxisDisplacement);
                Point p2 = new Point((float)_canvas.ActualWidth / 2 + i * _zoom - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 + 6 - _yAxisDisplacement);
                DrawLine(p1, p2, Colors.Magenta, 1);
            }
            for (float i = 0; i <= _canvas.ActualWidth/2 - _xAxisDisplacement; i += _step)
            {
                Point p3 = new Point((float)_canvas.ActualWidth / 2 - i * _zoom - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 - 6 - _yAxisDisplacement);
                Point p4 = new Point((float)_canvas.ActualWidth / 2 - i * _zoom - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 + 6 - _yAxisDisplacement);
                DrawLine(p3, p4, Colors.Magenta, 1);
            }

            for (float i = 0; i <= _canvas.ActualHeight / 2 + _yAxisDisplacement; i += _step)//ось игреков
            {
                Point p1 = new Point((float)_canvas.ActualWidth / 2 - 6 - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 - _yAxisDisplacement + i * _zoom);
                Point p2 = new Point((float)_canvas.ActualWidth / 2 + 6 - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 - _yAxisDisplacement + i * _zoom);
                DrawLine(p1, p2, Colors.Purple, 1);
            }
            for (float i = 0; i <= _canvas.ActualHeight / 2 - _yAxisDisplacement; i += _step)
            {
                Point p3 = new Point((float)_canvas.ActualWidth / 2 - 6 - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 - _yAxisDisplacement - i * _zoom);
                Point p4 = new Point((float)_canvas.ActualWidth / 2 + 6 - _xAxisDisplacement, (float)_canvas.ActualHeight / 2 - _yAxisDisplacement - i * _zoom);
                DrawLine(p3, p4, Colors.Purple, 1);
            }
        }
        public void DrawTriangle()
        {
            DrawPolygon(new PointCollection
            {
                new Point(_canvas.ActualWidth - 10, _canvas.ActualHeight / 2 + 5 - _yAxisDisplacement),
                new Point(_canvas.ActualWidth, _canvas.ActualHeight / 2 - _yAxisDisplacement),
                new Point(_canvas.ActualWidth - 10, _canvas.ActualHeight / 2 - 5 - _yAxisDisplacement)
            });

            DrawPolygon(new PointCollection
            {
                new Point(_canvas.ActualWidth / 2 - 5 - _xAxisDisplacement, 10),
                new Point(_canvas.ActualWidth / 2 - _xAxisDisplacement, 0),
                new Point(_canvas.ActualWidth / 2 + 5 - _xAxisDisplacement, 10)
             });
        }

        private void DrawPolygon(PointCollection points)
        {
            var polygon = new Polygon
            {
                Points = points,
                Stroke = Brushes.Black,
                Fill = Brushes.Black
            };

            _canvas.Children.Add(polygon);
        }

        public void DrawGraphic(List<Point> points)
        {
            for (var i = 0; i < points.Count - 2; i++)
            {
                Point startPoint = points[i].ToUiCoordinates(_canvas, _zoom);
                Point endPoint = points[i + 1].ToUiCoordinates(_canvas, _zoom);
                DrawLine(startPoint, endPoint, Colors.DarkOrchid, 2);
                DrawPoint(startPoint);
            }
        }
    }
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
                lastY = 0;
                result = calculator.CalculateExpression(RPN, i);
                if (float.IsNaN(result) || Math.Abs(lastY-result) >= cForGraphic.ActualHeight)
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