using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CalculatorX
{
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
}