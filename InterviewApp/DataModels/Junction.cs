using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterviewApp.DataModels
{
    internal class Junction: INode
    {
        public int Id { get; }
        public RailwaySegment EntryPoint { get; set; }
        public RailwaySegment ExitLeft { get; set; }
        public RailwaySegment ExitRight { get; set; }
        public Point Position { get; set; }
        public double Length => 0;
        public List<INode> GetNeighbours(INode from)
        {
            var result = new List<INode>();
            if (from == EntryPoint) {
                if (ExitLeft != null) {
                    result.Add(ExitLeft);
                }
                if (ExitRight != null) {
                    result.Add(ExitRight);
                }
            }
            if (from == ExitLeft || from == ExitRight) {
                if (EntryPoint != null) {
                    result.Add(EntryPoint);
                }
            }
            return result;
        }

        public Junction(int id, RailwaySegment entry, RailwaySegment exitLeft, RailwaySegment exitRight, Point position) {
            Id = id;
            EntryPoint = entry;
            ExitLeft = exitLeft;
            ExitRight = exitRight;
            Position = position;
        }
    }
}
