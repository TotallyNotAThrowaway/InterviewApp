using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using InterviewApp.Helpers;

namespace InterviewApp.ViewModels
{
    class RailwaysDrawingHelper
    {
        private static Pen RailwayPen = new Pen(Brushes.Black, 2);
        private static Pen RailwayPointPen = new Pen(Brushes.Gray, 2);
        private static Pen JunctionPen = new Pen(Brushes.Red, 3);
        //private static Brush StationBrush = new 

        public static void Draw(DrawingContext context) {
            foreach (var segment in MapRepository.Instance.Segments) {
                context.DrawLine(RailwayPen, segment.Start, segment.End);
            }
            var allSegmentPoints = MapRepository.Instance.Segments.SelectMany(s => new[] { s.Start, s.End }).ToHashSet();
            foreach (var point in allSegmentPoints) {
                context.DrawEllipse(null, RailwayPointPen, point, 2, 2);
            }
            foreach (var junction in MapRepository.Instance.Junctions) {
                context.DrawEllipse(null, JunctionPen, junction.Position, 3, 3);
            }

            foreach (var station in MapRepository.Instance.Stations) {
                var allStationPoints = station.Segments.SelectMany(s => new[] { s.Start, s.End }).ToHashSet();
                if (allSegmentPoints.Count == 0)
                    return;
                var polygon = ConvexHull(allStationPoints.ToList());
                context.PushOpacity(0.5d);
                //var allPoints = Railways.SelectMany(r => r.Points).ToList();
                //var Points = ConvexHull(allPoints).Select(p => new Point(p.X, p.Y)).ToList();
                StreamGeometry geometry = new StreamGeometry();
                using (StreamGeometryContext geometryContext = geometry.Open()) {
                    geometryContext.BeginFigure(polygon[0], true, true);
                    PointCollection points = new PointCollection(polygon.Skip(1));
                    geometryContext.PolyLineTo(points, true, true);
                }
                var brush = new SolidColorBrush(station.Color);
                context.DrawGeometry(brush, new Pen(brush, 20), geometry);
                context.Pop();
            }
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
