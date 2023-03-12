using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace InterviewApp.Models
{
    internal class Station
    {
        private const int drawingOffset = 10;
        private SolidColorBrush brush;

        public List<Railway> Railways = new();

        public Station(Color color) {
            brush = new SolidColorBrush(color);
        }

        public Rect CalculateRect() {
            var allPoints = Railways.SelectMany(r => r.Points);
            var minX = allPoints.Min(p => p.X);
            var maxX = allPoints.Max(p => p.X);
            var minY = allPoints.Min(p => p.Y);
            var maxY = allPoints.Max(p => p.Y);
            var width = maxX - minX + drawingOffset * 2;
            var height = maxY - minY + drawingOffset * 2;

            return new Rect(minX - drawingOffset, minY - drawingOffset, width, height);
        }

        public void Draw(DrawingContext context) {
            context.PushOpacity(0.3d);
            context.DrawRoundedRectangle(brush, null, CalculateRect(), drawingOffset, drawingOffset);
            context.Pop();
        }

        public void Draw2(DrawingContext context) {
            context.PushOpacity(0.5d);
            var allPoints = Railways.SelectMany(r => r.Points).ToList();
            var Points = ConvexHull(allPoints).Select(p => new Point(p.X, p.Y)).ToList();
            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = geometry.Open()) {
                geometryContext.BeginFigure(Points[0], true, true);
                PointCollection points = new PointCollection(Points.Skip(1));
                geometryContext.PolyLineTo(points, true, true);
            }

            context.DrawGeometry(brush, new Pen(Brushes.White, 2), geometry);
            context.Pop();
        }

        private int Orientation(Position p, Position q, Position r) {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
                return 0; // colinear
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        private List<Position> ConvexHull(List<Position> points) {
            int n = points.Count;
            if (n < 3)
                return null;

            List<Position> hull = new List<Position>();

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
