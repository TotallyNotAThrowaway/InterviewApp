using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewApp.DataModels;
using System.Windows;

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
                    segments: stationSegments
                );
                Stations.Add(station);
            }
        }

        private MapRepository() {
            Segments = new List<RailwaySegment>();
            Junctions = new List<Junction>();
            Stations = new List<Station>();
            var seg = new RailwaySegment(segmentCounter++, new Point(50, 50), new Point(100, 50), null, null, null);
            Segments.Add(seg);
            var j = AddNode<Junction>(seg, seg.End);
            seg = AddNode<RailwaySegment>(j, new Point(120, 30));
            var segR = AddNode<RailwaySegment>(j, new Point(150, 50));
            seg = AddNode<RailwaySegment>(seg, new Point(150, 30));
            segR = AddNode<RailwaySegment>(segR, new Point(170, 50));
            j = AddNode<Junction>(seg, seg.End);
            seg = AddNode<RailwaySegment>(j, new Point(170, 10));
            segR = AddNode<RailwaySegment>(j, new Point(180, 30));
            seg = AddNode<RailwaySegment>(seg, new Point(220, 10));
            seg = AddNode<RailwaySegment>(seg, new Point(250, 50));
            j = AddNode<Junction>(seg, seg.End);
            seg = AddNode<RailwaySegment>(j, new Point(210, 50));
            seg = AddNode<RailwaySegment>(j, new Point(270, 50));


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

    }
}
