using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using NUnit.Framework;
using Lte.Domain.Geo;
using System;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class AngleFromCellAzimuthTest
    {
        private StubOutdoorCell cell1;
        private StubOutdoorCell cell2;
        const double e = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            cell1 = new StubOutdoorCell(113, 23, 45);
            cell2 = new StubOutdoorCell(112, 22, 270);
        }

        [TestCase(new[] { 113, 22.9 }, new[] { 112.1, 21.9 }, 135, -135)]
        [TestCase(new[] { 113.1, 22.9 }, new[] { 112, 22.1 }, 90, 90)]
        [TestCase(new[] { 113.1, 23.1 }, new[] { 111.9, 22 }, 0, 0)]
        public void TestAngleFromCell_135(double[] p1Coor, double[] p2Coor,
            double expectedAngleFromCell1, double expectedAngleFromCell2)
        {
            StubGeoPoint p1 = new StubGeoPoint(p1Coor[0], p1Coor[1]);
            StubGeoPoint p2 = new StubGeoPoint(p2Coor[0], p2Coor[1]);
            double a1 = cell1.AngleFromCellAzimuth(p1);
            double a2 = cell2.AngleFromCellAzimuth(p2);
            Assert.AreEqual(a1, expectedAngleFromCell1, e);
            Assert.AreEqual(a2, expectedAngleFromCell2, e);
        }

        [Test]
        public void TestAngleFromCell_30()
        {
            StubGeoPoint p1 = new StubGeoPoint(113 + 0.1 * Math.Sqrt(3), 22.9);
            StubGeoPoint p2 = new StubGeoPoint(112.1, 22 - 0.1 * Math.Sqrt(3));
            double a1 = cell1.AngleFromCellAzimuth(p1);
            double a2 = cell2.AngleFromCellAzimuth(p2);
            Assert.AreEqual(a1, 75, e);
            Assert.AreEqual(a2, -120, e);
        }
    }
}
