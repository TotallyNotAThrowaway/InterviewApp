using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InterviewApp.DataModels
{
    internal class Station
    {
        public int ID { get; }
        public string Name { get; }

        public Color Color { get; }

        public List<RailwaySegment> Segments { get; set; }

        public Station(int iD, string name, List<RailwaySegment> segments, Color color) {
            ID = iD;
            Name = name;
            Segments = segments;
            Color = color;
        }
    }
}
