using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using InterviewApp.Helpers;
using InterviewApp.DataModels;
using System.Windows;
using System.Windows.Shell;

namespace UnitTests
{
    class PathNode : INode
    {
        public double Length { get; }

        public Point Position { get; }

        private List<INode> neighbours { get; } = new ();

        public List<INode> GetNeighbours(INode from) => neighbours;

        public PathNode(Point position, double length)
        {
            Position = position;
            Length = length;
        }
    } 

    class PathFindingTests
    {
        private (PathNode start, PathNode end) GeneratePossiblePath()
        {
            var first = new PathNode(new Point(0, 0), 10);
            var last = first;
            for (int i = 1; i < 10; i++)
            {
                var curr = new PathNode(new Point(i, i), 10);
                last.GetNeighbours(null).Add(curr);
                curr.GetNeighbours(null).Add(last);
                last = curr;
            }
            return (first, last);
        }

        [Test]
        public void WhenPathIsPossiblePathMustBeFound()
        {
            var (start, end) = GeneratePossiblePath();
            var path = AStar.FindPath(start, end);

            Assert.AreEqual(start, path[0]);
            Assert.AreEqual(end, path.Last());
            var prev = path[0];
            foreach (var node in path.Skip(1))
            {
                Assert.That(prev.GetNeighbours(null).Contains(node));
                prev = node;
            }
        }

        private (PathNode start, PathNode end) GenerateImpossiblePath()
        {
            var first = new PathNode(new Point(0, 0), 10);
            var last = first;
            for (int i = 1; i < 10; i++)
            {
                var curr = new PathNode(new Point(i, i), 10);
                last.GetNeighbours(null).Add(curr);
                curr.GetNeighbours(null).Add(last);
                last = curr;
            }
            return (first, new PathNode(new Point(20, 20), 5));
        }

        [Test]
        public void WhenPathIsImpoossibleMustReturnNull()
        {
            var (start, end) = GenerateImpossiblePath();
            var path = AStar.FindPath(start, end);

            Assert.IsNull(path);
        }
    }
}
