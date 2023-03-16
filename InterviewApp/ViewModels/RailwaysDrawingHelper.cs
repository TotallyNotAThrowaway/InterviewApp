using InterviewApp.DataModels;
using InterviewApp.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace InterviewApp.ViewModels
{
    internal class RailwaysDrawingHelper
    {
        private static readonly Pen RailwayPen = new(Brushes.Black, 2);
        private static readonly Pen RailwayPointPen = new(Brushes.Gray, 2);
        private static readonly Pen JunctionPen = new(Brushes.Red, 3);
        private static readonly Pen PathPen = new(Brushes.White, 1);
        private static readonly Pen Highlight = new(Brushes.DarkCyan, 3);

        public static INode PathStartNode { get; set; } = (INode) MapRepository.Instance.Segments[14];
        public static INode PathEndNode { get; set; } = (INode) MapRepository.Instance.Segments[29];

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
                if (allSegmentPoints.Count == 0) {
                    return;
                }

                var polygon = ConvexHull(allStationPoints.ToList());
                context.PushOpacity(0.5d);

                var geometry = new StreamGeometry();
                using (StreamGeometryContext geometryContext = geometry.Open()) {
                    geometryContext.BeginFigure(polygon[0], true, true);
                    var points = new PointCollection(polygon.Skip(1));
                    geometryContext.PolyLineTo(points, true, true);
                }
                var brush = new SolidColorBrush(station.Color);
                context.DrawGeometry(brush, new Pen(brush, 20), geometry);

                context.Pop();

                var formattedText = new FormattedText(station.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 20, brush);
                context.DrawText(formattedText, new Point(polygon.Average(p => p.X), polygon.Average(p => p.Y)));
            }

            var path = AStar.FindPath(PathStartNode, PathEndNode, out var scores);

            var minScore = scores.Values.Min();
            var maxScore = scores.Values.Max() - minScore;
            foreach (var score in scores) {
                if (score.Key is not RailwaySegment) {
                    continue;
                }

                var line = (RailwaySegment) score.Key;
                int colorValue = (int) ((score.Value - minScore)/ maxScore * 255);
                var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte) (255 - colorValue), (byte) colorValue, 0)), 5);
                context.DrawLine(pen, line.Start, line.End);
            }


            context.DrawLine(Highlight, ((RailwaySegment) PathStartNode).Start, ((RailwaySegment) PathStartNode).End);
            context.DrawLine(Highlight, ((RailwaySegment) PathEndNode).Start, ((RailwaySegment) PathEndNode).End);

            if (path != null) {
                foreach (var node in path) {
                    if (node is Junction) {
                        continue;
                    }

                    var segment = (RailwaySegment) node;
                    context.DrawLine(PathPen, segment.Start, segment.End);
                }
            }


        }

        private static int Orientation(Point p, Point q, Point r) {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) {
                return 0; // colinear
            }

            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        private static List<Point> ConvexHull(List<Point> points) {
            int n = points.Count;
            if (n < 3) {
                return null;
            }

            var hull = new List<Point>();

            int leftmostId = 0;
            for (int i = 1; i < n; i++) {
                if (points[i].X < points[leftmostId].X) {
                    leftmostId = i;
                }
            }

            int currentId = leftmostId, nextId;
            do {
                hull.Add(points[currentId]);
                nextId = (currentId + 1) % n;
                for (int i = 0; i < n; i++) {
                    if (Orientation(points[currentId], points[i], points[nextId]) == 2) {
                        nextId = i;
                    }
                }
                currentId = nextId;

            } while (currentId != leftmostId);

            return hull;
        }
    }
}
