using InterviewApp.DataModels;
using InterviewApp.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using static System.Formats.Asn1.AsnWriter;

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
            var allSegmentPoints = MapRepository.Instance.Segments.SelectMany(s => new[] { s.Start, s.End }).ToHashSet();

            DrawSegments(MapRepository.Instance.Segments, context);
            DrawSegmentsPoints(allSegmentPoints, context);
            DrawJunctions(MapRepository.Instance.Junctions, context);

            

            foreach (var station in MapRepository.Instance.Stations) {
                DrawStation(station, context);
            }

            var path = AStar.FindPath(PathStartNode, PathEndNode, out var scores);

            DrawPathfinding(path, scores, context);
        }

        private static void DrawSegments(IEnumerable<RailwaySegment> segments, DrawingContext context) {
            foreach (var segment in segments) {
                context.DrawLine(RailwayPen, segment.Start, segment.End);
            }
        }

        private static void DrawSegmentsPoints(IEnumerable<Point> points, DrawingContext context) {
            foreach (var point in points) {
                context.DrawEllipse(null, RailwayPointPen, point, 2, 2);
            }
        }

        private static void DrawJunctions(IEnumerable<Junction> junctions, DrawingContext context) {
            foreach (var junction in junctions) {
                context.DrawEllipse(null, JunctionPen, junction.Position, 3, 3);
            }
        }

        private static void DrawStation(Station station, DrawingContext context) {

            var allStationPoints = station.Segments.SelectMany(s => new[] { s.Start, s.End }).ToHashSet();
            if (allStationPoints.Count == 0) {
                return;
            }

            var polygon = ConvexHullHelper.GetPolygon(allStationPoints.ToList());
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

            var formattedText = new FormattedText(station.Name,
                                                  CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight,
                                                  new Typeface("Arial"),
                                                  20,
                                                  brush,
                                                  20);
            context.DrawText(formattedText, new Point(polygon.Average(p => p.X), polygon.Average(p => p.Y)));
        }

        private static void DrawPathfinding(IEnumerable<INode> path, Dictionary<INode, double> scores, DrawingContext context) {
            HighlightPathfinding(scores, context);

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

        private static void HighlightPathfinding(Dictionary<INode, double> scores, DrawingContext context) {
            if (scores == null) {
                return;
            }

            var minScore = scores.Values.Min();
            var maxScore = scores.Values.Max() - minScore;
            foreach (var score in scores) {
                if (score.Key is not RailwaySegment) {
                    continue;
                }

                var line = (RailwaySegment) score.Key;
                int colorValue = (int) ((score.Value - minScore) / maxScore * 255);
                var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte) (255 - colorValue), (byte) colorValue, 0)), 5);
                context.DrawLine(pen, line.Start, line.End);
            }
        }
    }
}
