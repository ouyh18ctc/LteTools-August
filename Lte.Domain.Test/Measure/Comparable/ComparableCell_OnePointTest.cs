using System;
using System.Globalization;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_OnePointTest
    {
        private FakeComparableCell[] cellList;
        const double eps = 1E-6;

        [Test]
        public void TestComparableCell_OnePoint()
        {
            cellList = new FakeComparableCell[1];
            IGeoPoint<double> p = new StubGeoPoint(112, 22);
            IOutdoorCell c = new StubOutdoorCell(112.001, 22.001, 60);
            cellList[0] = FakeComparableCell.Parse(new ComparableCell(p, c));
            double dist = p.SimpleDistance(new StubGeoPoint(c.Longtitute, c.Lattitute));
            Assert.AreEqual(cellList[0].Distance, dist, 1E-6);
            Assert.AreEqual(cellList[0].AzimuthAngle, 165, 1E-6);
        }

        [Test]
        public void TestComparableCell_OneCell_Distance()
        {
            ComparableCell c = new ComparableCell(0.5, 0);
            Assert.AreEqual(c.Distance, 0.5);
        }

        [Test]
        public void TestComparableCell_OneCell_With0azimuth_Metric()
        {
            FakeComparableCell c = FakeComparableCell.Parse(new ComparableCell(0.5, 0));
            Assert.AreEqual(c.Distance, 0.5);
            double metric = 35 * Math.Log10(0.5);
            Assert.AreEqual(c.MetricCalculate(), metric);
        }

        [Test]
        public void TestComparableCell_TwoCells()
        {
            cellList = new FakeComparableCell[2];
            cellList[0] = FakeComparableCell.Parse(new ComparableCell(0.5, 0));
            cellList[1] = FakeComparableCell.Parse(new ComparableCell(0.2, 1));
            Assert.AreEqual(cellList[0].Distance, 0.5);
            Array.Sort(cellList);
            Assert.AreEqual(cellList[0].Distance, 0.2);
            Assert.AreEqual(cellList[1].AzimuthAngle, 0);
        }

        [Test]
        public void TestComparableCell_FourCells()
        {
            cellList = new FakeComparableCell[4];
            cellList[0] = FakeComparableCell.Parse(new ComparableCell(0.2, 60));
            cellList[1] = FakeComparableCell.Parse(new ComparableCell(0.2, 120));
            cellList[2] = FakeComparableCell.Parse(new ComparableCell(0.4, 0));
            cellList[3] = FakeComparableCell.Parse(new ComparableCell(0.2, 0));
            Array.Sort(cellList);
            Assert.AreEqual(cellList[0].AzimuthAngle, 0);
            double metric0 = cellList[0].MetricCalculate();
            Assert.AreEqual(metric0, -24.46395, eps, metric0.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(cellList[1].AzimuthAngle, 0);
            double metric1 = cellList[1].MetricCalculate();
            Assert.AreEqual(metric1, -13.9279, eps, metric1.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(cellList[2].AzimuthAngle, 60);
            double metric2 = cellList[2].MetricCalculate();
            Assert.AreEqual(metric2, -11.899295, eps, metric2.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(cellList[3].AzimuthAngle, 120);
            double metric3 = cellList[3].MetricCalculate();
            Assert.AreEqual(metric3, 5.53605, eps, metric3.ToString(CultureInfo.InvariantCulture));
        }
    }
}
