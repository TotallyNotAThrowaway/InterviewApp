using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace InterviewApp.DataModels
{
    internal class Station
    {
        public int ID { get; }
        public string Name { get; }

        public Color Color { get; set; }

        public List<RailwaySegment> Segments { get; set; }

        public IEnumerable<Point> AllPoints => Segments.SelectMany(s => new[] { s.Start, s.End }).ToHashSet();

        public Station(int iD, string name, List<RailwaySegment> segments, Color color) {
            ID = iD;
            Name = name;
            Segments = segments;
            Color = color;
        }
    }
}
