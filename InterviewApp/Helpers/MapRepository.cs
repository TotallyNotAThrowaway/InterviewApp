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
        private static MapRepository instance;
        public static MapRepository Instance => instance ??= new MapRepository();

        public List<RailwaySegment> Segments { get; set; }
        public List<Junction> Junctions { get; set; }
        public List<Station> Stations { get; set; }
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
        //}
    }
}
