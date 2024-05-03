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
    class CanvasDrawer
    {
        private Canvas _canvas;
        private double _axisThickness = 1;
        private Brush _defaultStroke = Brushes.Black;
        private int _scaleLength = 5;

        Point xAxisStart;
        Point xAxisEnd;
        Point yAxisStart;
        Point yAxisEnd;

        private readonly float _xStart;
        private readonly float _xEnd;
        private readonly float _step;
        private readonly float _zoom;
        public CanvasDrawer(Canvas canvas, float xStart, float xEnd, float step, float zoom)
        {
            _canvas = canvas;
            xAxisStart = new Point((int)_canvas.ActualWidth / 2, 0);
            xAxisEnd = new Point((int)_canvas.ActualWidth / 2, (int)_canvas.ActualHeight);
            yAxisStart = new Point(0, (int)_canvas.ActualHeight / 2);
            yAxisEnd = new Point((int)_canvas.ActualWidth, (int)_canvas.ActualHeight / 2);


            _xStart = xStart;
            _xEnd = xEnd;
            _step = step;
            _zoom = zoom;
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
            for (float i=0; i<= _xEnd; i+=_step) //ось иксов
            {
                Point p1 = new Point((float) _canvas.ActualWidth/2 + i*_zoom, (float) _canvas.ActualHeight/2 - 6);
                Point p2 = new Point((float)_canvas.ActualWidth / 2 + i*_zoom, (float)_canvas.ActualHeight / 2 + 6);
                Point p3 = new Point((float)_canvas.ActualWidth / 2 - i * _zoom, (float)_canvas.ActualHeight / 2 - 6);
                Point p4 = new Point((float)_canvas.ActualWidth / 2 - i * _zoom, (float)_canvas.ActualHeight / 2 + 6);

                DrawLine(p1, p2, Colors.Magenta, 1);
                DrawLine(p3, p4, Colors.Magenta, 1);
            }
            
            for (float i = 0; i <= _canvas.ActualHeight / 2; i += _step)//ось игреков
            {
                Point p1 = new Point((float)_canvas.ActualWidth / 2 - 6, (float)_canvas.ActualHeight / 2 + i * _zoom); 
                Point p2 = new Point((float)_canvas.ActualWidth / 2 + 6, (float)_canvas.ActualHeight / 2 + i * _zoom);

                Point p3 = new Point((float)_canvas.ActualWidth / 2 - 6, (float)_canvas.ActualHeight / 2 - i * _zoom);
                Point p4 = new Point((float)_canvas.ActualWidth / 2 + 6, (float)_canvas.ActualHeight / 2 - i * _zoom);

                DrawLine(p1, p2, Colors.Purple, 1);
                DrawLine(p3, p4, Colors.Purple, 1);
            }
        }
        public void DrawTriangle()
        {
            DrawPolygon(new PointCollection
            {
                new Point(_canvas.ActualWidth - 10, _canvas.ActualHeight / 2 + 5),
                new Point(_canvas.ActualWidth, _canvas.ActualHeight / 2),
                new Point(_canvas.ActualWidth - 10, _canvas.ActualHeight / 2 - 5)
            });

            DrawPolygon(new PointCollection
            {
                new Point(_canvas.ActualWidth / 2 - 5, 10),
                new Point(_canvas.ActualWidth / 2, 0),
                new Point(_canvas.ActualWidth / 2 + 5, 10)
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
            var canvasDrawer = new CanvasDrawer(cForGraphic, -35, 33, 1, 10);
            canvasDrawer.DrawAxis();
            canvasDrawer.DrawMarks();
            canvasDrawer.DrawTriangle();
        }
        private void btnStart(object sender, RoutedEventArgs e)
        {
            cForGraphic.Children.Clear();
            RedrawCanvas();
            //float valueOfVariable = float.Parse(tbVariableValue.Text);
            //lblResult.Content = new RpnCalculator(tbExpression.Text).Calculate(valueOfVariable);
        }
        private void cForGraphic_MouseMove(object sender, MouseEventArgs e)
        {
            Point uiPoint = Mouse.GetPosition(cForGraphic);
            float zoom = 1;
            if (!(String.IsNullOrEmpty(tbZoom.Text)))
            {
                zoom = float.Parse(tbZoom.Text);
            }
            var mathPoint = Mouse.GetPosition(cForGraphic).ToMathCoordinates(cForGraphic, zoom);
            lblUiCoordinates.Content = $"{uiPoint.X:0.#},{uiPoint.Y:0.#}";
            lblMathCoordinates.Content = $"{mathPoint.X:0.#},{mathPoint.Y:0.#}";
        }
    
        private void RedrawCanvas()
        {
            float start = float.Parse(tbStartPosition.Text);
            float end = float.Parse(tbFinishPosition.Text);
            float zoom = float.Parse(tbZoom.Text);
            float step = float.Parse(tbStep.Text);

            CanvasDrawer canvasDrawer = new CanvasDrawer(cForGraphic, start, end, step, zoom);
            canvasDrawer.DrawAxis();
            canvasDrawer.DrawMarks();
            canvasDrawer.DrawTriangle();

            RpnCalculator calculator = new RpnCalculator(tbExpression.Text);
            var points = new List<Point>();
            for (float i = start; i <= end; i += step)
            {
                float result = calculator.Calculate(i);
                points.Add(new Point(i, result));
            }

            canvasDrawer.DrawGraphic(points);
        }

    }
}