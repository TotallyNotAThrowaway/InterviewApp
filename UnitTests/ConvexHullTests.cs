using InterviewApp.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows;

namespace UnitTests
{
    class ConvexHullTests
    {
        private List<Point> points = new List<Point>() { new Point(0, 0),                                   new Point(2, 0),
                                                                          new Point(1, 1), new Point(2, 1),                  new Point(3, 1),

                                                                          new Point(1, 3),                                   new Point(3, 3)};

        [TestCase(0, true,  TestName = "Point 0 is a part of the hull")]
        [TestCase(1, true,  TestName = "Point 1 is a part of the hull")]
        [TestCase(2, false, TestName = "Point 2 is NOT a part of the hull")]
        [TestCase(3, false, TestName = "Point 3 is NOT a part of the hull")]
        [TestCase(4, true,  TestName = "Point 4 is a part of the hull")]
        [TestCase(5, true,  TestName = "Point 5 is a part of the hull")]
        [TestCase(6, true,  TestName = "Point 6 is a part of the hull")]
        public void OnlyOutsidePointsMustBeAPartOfTheHull(int pointId, bool isPartOfTheHull)
        {
            var hull = ConvexHullHelper.GetPolygon(points);
            Assert.AreEqual(hull.Contains(points[pointId]), isPartOfTheHull);
        }
    }
}
