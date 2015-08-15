using System;
using System.Globalization;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_TwoPointsTest
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
        public void TestTwoPoints_SamePoint()
        {
            IGeoPoint<double> op = new StubGeoPoint(p, 0.01, 45);
            IOutdoorCell[] cl =
            {
                new StubOutdoorCell(op, 180),
                new StubOutdoorCell(op, 225)
            };
            cellList = p.GenerateComparableCellList(cl).Select(FakeComparableCell.Parse).ToArray();
            Array.Sort(cellList);
            Assert.AreEqual(cellList[0].AzimuthAngle, 0, eps);
            Assert.AreEqual(cellList[1].AzimuthAngle, 45, eps);

            cellList[0].SetAzimuthAngle(p, 135);
            cellList[1].SetAzimuthAngle(p, 90);
            Array.Sort(cellList);
            Assert.AreEqual(cellList[0].AzimuthAngle, 135, eps);
            Assert.AreEqual(cellList[0].MetricCalculate(), 31.612974, eps, cellList[0].MetricCalculate().ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(cellList[1].AzimuthAngle, 90, eps);
            Assert.AreEqual(cellList[1].MetricCalculate(), 31.612974, eps, cellList[1].MetricCalculate().ToString(CultureInfo.InvariantCulture));

        }

        [Test]
        public void TestTwoPoints_SameRing()
        {
            IOutdoorCell[] cl =
            {
                new StubOutdoorCell(new StubGeoPoint(p, 0.01, 45), 180),
                new StubOutdoorCell(new StubGeoPoint(p, 0.01), 240)
            };
            cellList = p.GenerateComparableCellList(cl).Select(FakeComparableCell.Parse).ToArray();

            double[] ml = cellList.Select(x => x.MetricCalculate()).ToArray();
            Assert.IsTrue(ml[0] > ml[1], "First metrical compare failed, metric0:" + ml[0]
                + ", metric1:" + ml[1]);

            Array.Sort(cellList);
            ml = cellList.Select(x => x.MetricCalculate()).ToArray();
            Assert.IsTrue(ml[0] < ml[1], "Second metrical compare failed, metric0:" + ml[0]
                + ", metric1:" + ml[1]);
            Assert.AreEqual(cellList[0].AzimuthAngle, 30, eps);
            Assert.AreEqual(cellList[1].AzimuthAngle, 45, eps);

            cellList[0].SetAzimuthAngle(p, 90);
            cellList[1].SetAzimuthAngle(p, 90);
            Assert.AreEqual(cellList[0].AzimuthAngle, 180, eps, "First unmatch!");
            Assert.AreEqual(cellList[1].AzimuthAngle, 135, eps, "Second unmatch!");

            ml = cellList.Select(x => x.MetricCalculate()).ToArray();
            Assert.AreEqual(ml[0], ml[1], eps, "Third metrical compare failed, metric0:" + ml[0]
                + ", metric1:" + ml[1]);

            Array.Sort(cellList);
            Assert.AreEqual(cellList[0].AzimuthAngle, 135, eps, "Third unmatch!");
            Assert.AreEqual(cellList[1].AzimuthAngle, 180, eps, "Fourth unmatch!");

        }

    }
}
