using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterviewApp.DataModels
{
    internal class RailwaySegment: INode 
    {
        public int Id { get; }
        public Point Start { get; set; }
        public Point End { get; set; }
        public INode LeftNeighbour { get; set; }
        public INode RightNeighbour { get; set; }
        public double Length { get; }

        public Station Station { get; set; }

        public List<INode> GetNeighbours(INode from) {
            var result = new List<INode>();
            if (from == LeftNeighbour && RightNeighbour != null) {
                result.Add(RightNeighbour);
            }
            else if (from == RightNeighbour && LeftNeighbour != null) {
                result.Add(LeftNeighbour);
            }

            return result;
        }

        public RailwaySegment(int id, Point start, Point end, INode leftNeighbour, INode rightNeighbour, Station station) {
            Id = id;
            Start = start;
            End = end;
            LeftNeighbour = leftNeighbour;
            RightNeighbour = rightNeighbour;
            Length = Math.Sqrt(Math.Pow(Start.X - End.X, 2) + Math.Pow(Start.Y - End.Y, 2));
            Station = station;
        }
    }
}
