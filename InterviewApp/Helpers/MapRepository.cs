using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewApp.DataModels;
using System.Windows;
using System.Windows.Media;

namespace InterviewApp.Helpers
{
    internal class MapRepository {
        private static MapRepository instance;
        public static MapRepository Instance => instance ??= new MapRepository();

        public List<RailwaySegment> Segments { get; set; }
        public List<Junction> Junctions { get; set; }
        public List<Station> Stations { get; set; }

        private int segmentCounter = 0;
        private int junctionCounter = 0;
        private int stationCounter = 0;

        private MapRepository() {
            Segments = new List<RailwaySegment>();
            Junctions = new List<Junction>();
            Stations = new List<Station>();

            Stations.Add(new Station(stationCounter++, $"Station {stationCounter}", new List<RailwaySegment>(), Color.FromRgb(255, 120, 120)));
            CreateStation(new Point(0, 0), Stations[0]);
            Stations.Add(new Station(stationCounter++, $"Station {stationCounter}", new List<RailwaySegment>(), Color.FromRgb(120, 255, 120)));
            CreateStation(new Point(330, 0), Stations[1]);
            Stations.Add(new Station(stationCounter++, $"Station {stationCounter}", new List<RailwaySegment>(), Color.FromRgb(120, 120, 255)));
            CreateStation(new Point(0, 200), Stations[2]);

            RailwaySegment nLeft, nRight;
            ConnectSegments(Stations[0].Segments.Last(), Stations[1].Segments.First());
            ConnectSegments(Stations[0].Segments[14], Stations[2].Segments.First());
            nLeft = AddNode<RailwaySegment>(Stations[0].Segments.First(), new Point(30, 50));
            nRight = AddNode<RailwaySegment>(Stations[2].Segments[14], new Point(30, 250));
            ConnectSegments(nRight, nLeft);
            nLeft = AddNode<RailwaySegment>(Stations[0].Segments[10], new Point(270, -10));
            nRight = AddNode<RailwaySegment>(Stations[1].Segments[10], new Point(600, -10));
            ConnectSegments(nLeft, nRight);
            ConnectSegments(Stations[2].Segments[10], Stations[1].Segments[14]);
            nLeft = AddNode<RailwaySegment>(Stations[2].Segments[18], new Point(630, 250));
            ConnectSegments(nLeft, Stations[1].Segments[18]);
        }

        private void ConnectSegments(RailwaySegment left, RailwaySegment right) {
            var connection = new RailwaySegment(segmentCounter++, left.RightNeighbour == null ? left.End : left.Start, right.LeftNeighbour == null ?  right.Start : right.End, left, right, null);

            if (left.RightNeighbour == null)
                left.RightNeighbour = connection;
            else
                left.LeftNeighbour = connection;

            if (right.LeftNeighbour == null)
                right.LeftNeighbour = connection;
            else
                right.RightNeighbour = connection;

            Segments.Add(connection);
        }

        /// <summary>
        /// This creates a new node based on type
        /// It connects to the end on the parent
        /// or one of the exits on the junction
        /// </summary>
        /// <typeparam name="T">Junction or RailwaySegment</typeparam>
        /// <param name="parent"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private T AddNode<T>(INode parent, Point to) where T : class, INode {
            if (parent is Junction) {
                var from = (parent as Junction).Position;
                if (typeof(T) != typeof(RailwaySegment)) {
                    throw new NotSupportedException("Type of Junction only connects to RailwaySegment");
                }
                else {
                    var result = new RailwaySegment(segmentCounter++, from, to, parent, null, null);
                    var p = parent as Junction;
                    if (p.ExitLeft == null) {
                        p.ExitLeft = result;
                    }
                    else {
                        p.ExitRight = result;
                    }
                    p.Position = from;
                    Segments.Add(result);
                    return result as T;
                }
            }
            else {
                var from = (parent as RailwaySegment).End;
                if (typeof(T) == typeof(RailwaySegment)) {
                    var result = new RailwaySegment(segmentCounter++, from, to, parent, null, null);
                    var p = parent as RailwaySegment;
                    p.End = from;
                    if (p.RightNeighbour == null)
                        p.RightNeighbour = result;
                    else
                        p.LeftNeighbour = result;
                    Segments.Add(result);
                    return result as T;
                }
                else if (typeof(T) == typeof(Junction)) {
                    var p = parent as RailwaySegment;
                    var result = new Junction(junctionCounter++, p, null, null, from);
                    if (p.RightNeighbour == null)
                        p.RightNeighbour = result;
                    else
                        p.LeftNeighbour = result;
                    p.End = from;
                    Junctions.Add(result);
                    return result as T;
                }
            }
            throw new NotSupportedException("Wrong generation");
        }

        private T AddNode<T>(INode parent, Point to, Station station, Point? offset = null) where T : class, INode {
            if (offset != null) {
                to.X += offset.Value.X;
                to.Y += offset.Value.Y;
            }
            var node = AddNode<T>(parent, to);
            if (node is RailwaySegment)
                station.Segments.Add(node as RailwaySegment);
            return node;
        }

        private Junction InsertJunction(RailwaySegment entry, RailwaySegment exit, Point position) {
            if ((entry.Start != position &&
                entry.End != position) ||
                (exit.Start != position &&
                exit.End != position))
                throw new ArgumentException("segments don't connect to the position");

            var junction = new Junction(junctionCounter++, entry, exit, null, position);

            if (entry.Start == position)
                entry.LeftNeighbour = junction;
            else
                entry.RightNeighbour = junction;

            if (exit.Start == position)
                exit.LeftNeighbour = junction;
            else
                exit.RightNeighbour = junction;

            Junctions.Add(junction);
            return junction;
        }

        private List<RailwaySegment> CreateStation(Point offset, Station target) {
            var returnIndexOffset = target.Segments.Count;
            var seg = new RailwaySegment(segmentCounter++, new Point(50 + offset.X, 50 + offset.Y), new Point(100 + offset.X, 50 + offset.Y), null, null, null);
            Segments.Add(seg);
            target.Segments.Add(seg);
            var j = AddNode<Junction>(seg, seg.End, target);
            seg = AddNode<RailwaySegment>(j, new Point(120, 30), target, offset);
            var segR = AddNode<RailwaySegment>(j, new Point(150, 50), target, offset);
            var segAcc = segR;
            seg = AddNode<RailwaySegment>(seg, new Point(150, 30), target, offset);
            segR = AddNode<RailwaySegment>(segR, new Point(170, 50), target, offset);
            j = InsertJunction(segR, segAcc, segR.Start);
            segAcc = AddNode<RailwaySegment>(j, new Point(120, 80), target, offset);
            j = AddNode<Junction>(seg, seg.End, target);
            seg = AddNode<RailwaySegment>(j, new Point(170, 10), target, offset);
            segR = AddNode<RailwaySegment>(j, new Point(180, 30), target, offset);
            seg = AddNode<RailwaySegment>(seg, new Point(220, 10), target, offset);
            seg = AddNode<RailwaySegment>(seg, new Point(250, 10), target, offset);
            j = AddNode<Junction>(seg, seg.End, target);
            seg = AddNode<RailwaySegment>(j, new Point(270, 10), target, offset);
            seg = AddNode<RailwaySegment>(j, new Point(270, 30), target, offset);
            seg = AddNode<RailwaySegment>(segAcc, new Point(100, 80), target, offset);
            j = InsertJunction(seg, segAcc, seg.Start);
            segAcc = AddNode<RailwaySegment>(j, new Point(170, 80), target, offset);
            seg = AddNode<RailwaySegment>(seg, new Point(50, 80), target, offset);
            seg = AddNode<RailwaySegment>(segAcc, new Point(220, 80), target, offset);
            seg = AddNode<RailwaySegment>(seg, new Point(250, 80), target, offset);
            seg = AddNode<RailwaySegment>(seg, new Point(280, 50), target, offset);
            seg = AddNode<RailwaySegment>(seg, new Point(300, 50), target, offset);
            return target.Segments.Skip(returnIndexOffset).ToList();
        }

    }
}
