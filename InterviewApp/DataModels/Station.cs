using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.DataModels
{
    internal class Station
    {
        public int ID { get; }
        public string Name { get; }

        public List<RailwaySegment> Segments { get; set; }

        public Station(int iD, string name, List<RailwaySegment> segments) {
            ID = iD;
            Name = name;
            Segments = segments;
        }
    }
}
