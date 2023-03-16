using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace InterviewApp.ViewModels
{
    public class StationViewModel: ViewModelBase
    {
        private Color stationColor;
        
        public Color StationColor {
            get => stationColor;
            set {
                stationColor = value;
                OnPropertyChanged(nameof(StationColor));
            }
        }

        public string Name { get; }
        public List<Point> Polygon { get; }

        public StationViewModel(string name, List<Point> polygon, Color color) {
            StationColor = color;
            Name = name;
            Polygon = polygon;
        }

        public void Draw(DrawingContext context) {
            if (Polygon.Count == 0)
                return;
            var polygon = ConvexHull(Polygon.ToList());
            context.PushOpacity(0.5d);
            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = geometry.Open()) {
                geometryContext.BeginFigure(polygon[0], true, true);
                PointCollection points = new PointCollection(polygon.Skip(1));
                geometryContext.PolyLineTo(points, true, true);
            }
            var brush = new SolidColorBrush(StationColor);
            context.DrawGeometry(brush, new Pen(brush, 20), geometry);
            context.Pop();
        }

        private static int Orientation(Point p, Point q, Point r) {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
                return 0; // colinear
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        private static List<Point> ConvexHull(List<Point> points) {
            int n = points.Count;
            if (n < 3)
                return null;

            List<Point> hull = new List<Point>();

            int l = 0;
            for (int i = 1; i < n; i++)
                if (points[i].X < points[l].X)
                    l = i;

            int p = l, q;
            do {
                hull.Add(points[p]);
                q = (p + 1) % n;
                for (int i = 0; i < n; i++) {
                    if (Orientation(points[p], points[i], points[q]) == 2)
                        q = i;
                }
                p = q;

            } while (p != l);

            return hull;
        }
    }
}
