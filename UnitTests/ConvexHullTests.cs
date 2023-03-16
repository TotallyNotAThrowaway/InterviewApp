using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using InterviewApp.Helpers;
using System.Windows;

namespace UnitTests
{
    class ConvexHullTests
    {
        private List<Point> points = new List<Point>() { new Point(0, 0),                                   new Point(2, 0),
                                                                          new Point(1, 1), new Point(2, 1),                  new Point(3, 1),

                                                                          new Point(1, 3),                                   new Point(3, 3)};

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, true)]
        public void TestConvexHull(int pointId, bool isPartOfTheHull)
        {
            var hull = ConvexHullHelper.GetPolygon(points);
            Assert.AreEqual(hull.Contains(points[pointId]), isPartOfTheHull);
        }
    }
}
