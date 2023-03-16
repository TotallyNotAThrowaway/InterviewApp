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
        [Test]
        public void TestConvexHull()
        {
            var points = new List<Point>() { new Point(0, 0), new Point(1, 0),
                                             new Point(0, 1), new Point(1, 1)};

            var result = ConvexHullHelper.GetPolygon(points);
            Assert.IsTrue(result.All(p => points.Contains(p)));
        }
    }
}
