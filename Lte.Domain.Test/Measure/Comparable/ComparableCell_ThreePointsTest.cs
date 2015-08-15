using System;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_ThreePointsTest
    {
        IGeoPoint<double> p;
        FakeComparableCell[] cellList;
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            p = new StubGeoPoint(112, 23);
        }

        [Test]
        public void TestComparableCell_ThreePoints_SameAzimuth()
        {
            IOutdoorCell[] cl =
            {
                new StubOutdoorCell(new StubGeoPoint(p, 0.02, 30), 225),
                new StubOutdoorCell(new StubGeoPoint(p, 0.01), 225),
                new StubOutdoorCell(new StubGeoPoint(p, 0.03, 35), 225)
            };
            cellList = p.GenerateComparableCellList(cl).Select(FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(cellList[0].AzimuthAngle, 15, eps);
            Assert.AreEqual(cellList[1].AzimuthAngle, 45, eps);
            Assert.AreEqual(cellList[2].AzimuthAngle, 10, eps);

            Array.Sort(cellList);
            Assert.AreEqual(cellList[0].AzimuthAngle, 45, eps);
            Assert.AreEqual(cellList[0].MetricCalculate(), 8.248211, eps, "_cellList[0]'s metric: " 
                + cellList[0].MetricCalculate());
            Assert.AreEqual(cellList[1].AzimuthAngle, 15, eps);
            Assert.AreEqual(cellList[1].MetricCalculate(), 12.501933, eps, "_cellList[1]'s metric: " 
                + cellList[1].MetricCalculate());
            Assert.AreEqual(cellList[2].AzimuthAngle, 10, eps);
            Assert.AreEqual(cellList[2].MetricCalculate(), 18.340954, eps, "_cellList[2]'s metric: " 
                + cellList[2].MetricCalculate());
        }
    }
}
