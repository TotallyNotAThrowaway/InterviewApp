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
        private static Random rand = new Random();
        private static MapRepository instance;
        public static MapRepository Instance => instance ??= new MapRepository();

        public List<RailwaySegment> Segments { get; set; }
        public List<Junction> Junctions { get; set; }
        public List<Station> Stations { get; set; }

        private int segmentCounter = 0;
        private int junctionCounter = 0;
        private int StationCounter = 0;

        //private MapRepository() {
        //    var segmentCounter = 0;
        //    var junctionCounter = 0;
        //    var StationCounter = 0;

        //    Segments = new List<RailwaySegment>();
        //    Junctions = new List<Junction>();
        //    Stations = new List<Station>();

        //    var segment = new RailwaySegment(segmentCounter++, new Point(50, 50), new Point(100, 50), null, null);
        //    Segments.Add(segment);

        //    var junction = new Junction(junctionCounter++, segment, null, null, segment.End);
        //    segment.RightNeighbour = junction;
        //    Junctions.Add(junction);

        //    segment = new RailwaySegment(segmentCounter++, junction.Position, new Point(130, 30), junction, null);
        //    junction.ExitLeft = segment;
        //    Segments.Add(segment);


        //}


        private MapRepository(int a) {

            Segments = new List<RailwaySegment>();
            Junctions = new List<Junction>();
            Stations = new List<Station>();

            var firstSegment = new RailwaySegment(
                                                    id: 0,
                                                    start: new Point(0, 0),
                                                    end: new Point(10, 0),
                                                    leftNeighbour: null,
                                                    rightNeighbour: null,
                                                    station: null
                                                );
            Segments.Add(firstSegment);

            // create the remaining segments
            for (int i = 1; i < 150; i++) {
                var previousSegment = Segments[i - 1];
                var isJunction = rand.NextDouble() < 0.2; // 20% chance of creating a junction

                if (isJunction) {
                    // create a junction
                    var junction = new Junction(
                        id: Junctions.Count,
                        entry: previousSegment,
                        exitLeft: null,
                        exitRight: null,
                        position: previousSegment.End // same position as the end of the previous segment
                    );
                    Junctions.Add(junction);

                    // connect the previous segment to the junction
                    previousSegment.RightNeighbour = junction;
                    junction.EntryPoint = previousSegment;

                    // create a new segment starting from the junction
                    var newSegment = new RailwaySegment(
                        id: Segments.Count,
                        start: junction.Position,
                        end: new Point(junction.Position.X + 10, junction.Position.Y + rand.Next(50) - 25),
                        leftNeighbour: junction,
                        rightNeighbour: null,
                        station: null
                    );
                    Segments.Add(newSegment);

                    // connect the junction to the new segment
                    junction.ExitRight = newSegment;
                    newSegment.LeftNeighbour = junction;
                }
                else {
                    // create a new segment continuing from the previous segment
                    var newSegment = new RailwaySegment(
                        id: Segments.Count,
                        start: previousSegment.End,
                        end: new Point(previousSegment.End.X + 10, previousSegment.End.Y + rand.Next(50) - 25),
                        leftNeighbour: previousSegment,
                        rightNeighbour: null,
                        station: null
                    );
                    Segments.Add(newSegment);

                    // connect the previous segment to the new segment
                    previousSegment.RightNeighbour = newSegment;
                    newSegment.LeftNeighbour = previousSegment;
                }
            }

            // create stations with random segments
            for (int i = 0; i < 10; i++) {
                var stationSegments = new List<RailwaySegment>();
                for (int j = 0; j < 5; j++) {
                    var randomSegmentIndex = rand.Next(Segments.Count);
                    var randomSegment = Segments[randomSegmentIndex];
                    stationSegments.Add(randomSegment);
                }
                var station = new Station(
                    iD: Stations.Count,
                    name: "Station " + i,
                    segments: stationSegments,
                    Color.FromRgb(0, 0, 0)
                );
                Stations.Add(station);
            }
        }

        private MapRepository() {
            Segments = new List<RailwaySegment>();
            Junctions = new List<Junction>();
            Stations = new List<Station>();

            Stations.Add(new Station(StationCounter++, $"Station {StationCounter}", new List<RailwaySegment>(), Color.FromRgb(255, 120, 120)));
            CreateStation(new Point(0, 0), Stations[0]);
            Stations.Add(new Station(StationCounter++, $"Station {StationCounter}", new List<RailwaySegment>(), Color.FromRgb(120, 255, 120)));
            CreateStation(new Point(300, 0), Stations[1]);
            Stations.Add(new Station(StationCounter++, $"Station {StationCounter}", new List<RailwaySegment>(), Color.FromRgb(120, 120, 255)));
            CreateStation(new Point(0, 200), Stations[2]);
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
                    p.RightNeighbour = result;
                    Segments.Add(result);
                    return result as T;
                }
                else if (typeof(T) == typeof(Junction)) {
                    var p = parent as RailwaySegment;
                    var result = new Junction(junctionCounter++, p, null, null, from);
                    p.RightNeighbour = result;
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
            seg = AddNode<RailwaySegment>(seg, new Point(250, 50), target, offset);
            j = AddNode<Junction>(seg, seg.End, target);
            seg = AddNode<RailwaySegment>(j, new Point(210, 50), target, offset);
            seg = AddNode<RailwaySegment>(j, new Point(270, 50), target, offset);
            seg = AddNode<RailwaySegment>(segAcc, new Point(100, 80), target, offset);
            j = InsertJunction(segAcc, seg, seg.Start);
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
