using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewApp.DataModels;

namespace InterviewApp.Helpers
{
    public class AStar
    {
        public static List<INode> FindPath(INode start, INode end, out Dictionary<INode, double> scores) {
            var openSet = new List<INode> { start };
            var closedSet = new HashSet<INode>();
            var cameFrom = new Dictionary<INode, INode>();
            var gScore = new Dictionary<INode, double> { [start] = 0 };
            var fScore = new Dictionary<INode, double> { [start] = Heuristic(start, end) };

            while (openSet.Any()) {
                var current = openSet.OrderBy(n => fScore[n]).First();

                if (current == end) {
                    scores = fScore;
                    return ReconstructPath(cameFrom, end);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                cameFrom.TryGetValue(current, out var previous);

                foreach (var neighbour in current.GetNeighbours(previous ?? current)) {
                    if (closedSet.Contains(neighbour)) {
                        continue;
                    }

                    var tentativeGScore = gScore[current] + (current.Length + neighbour.Length) / 2;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                    else if (tentativeGScore >= gScore[neighbour]) {
                        continue;
                    }

                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = gScore[neighbour] + Heuristic(neighbour, end);
                }
            }

            scores = fScore;
            return null;
        }

        public static List<INode> FindPath(INode start, INode end) {
            return FindPath(start, end, out var _);
        }

        private static List<INode> ReconstructPath(Dictionary<INode, INode> cameFrom, INode current) {
            var path = new List<INode> { current };

            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            return path;
        }

        private static double Heuristic(INode a, INode b) {
            return Math.Sqrt(Math.Pow(a.Length - b.Length, 2));
        }
    }
}
