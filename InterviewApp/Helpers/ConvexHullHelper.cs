using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterviewApp.Helpers
{
    internal class ConvexHullHelper
    {
        private static int Orientation(Point p, Point q, Point r) {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
                return 0; // colinear
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        private static List<Point> ConvexHull(List<Point> points) {
            int n = points.Count;
            if (n < 3)
                return null;

            List<Point> hull = new List<Point>();

            int leftmostId = 0;
            for (int i = 1; i < n; i++)
                if (points[i].X < points[leftmostId].X)
                    leftmostId = i;

            int currentId = leftmostId, nextId;
            do {
                hull.Add(points[currentId]);
                nextId = (currentId + 1) % n;
                for (int i = 0; i < n; i++) {
                    if (Orientation(points[currentId], points[i], points[nextId]) == 2)
                        nextId = i;
                }
                currentId = nextId;

            } while (currentId != leftmostId);

            return hull;
        }
        public static List<Point> GetPolygon(IEnumerable<Point> points) {
            return ConvexHull(points.ToList());
        }
    }
}
