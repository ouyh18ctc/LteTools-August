using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.MeasureCell
{
    [TestFixture]
    public class MeasurableCellAzimuth0Test
    {
        private readonly IGeoPoint<double> point = new GeoPoint(112, 23);
        private IGeoPoint<double> point2;
        private readonly ILinkBudget<double> budget = new LinkBudget(new BroadcastModel());
        private IOutdoorCell ocell;
        private ComparableCell ccell;
        private MeasurableCell cell;
        const double eps = 1E-6;

        private void TestInitialize(double distance)
        {
            point2 = new StubGeoPoint(point, distance, 45);
            ocell = new StubOutdoorCell(point2, 225);
            ocell.Height = 40;
            ocell.ETilt = 4;
            ocell.MTilt = 1;
            ccell = new ComparableCell(point, ocell);
            cell = new MeasurableCell(ccell, point, budget);
        }

        [Test]
        public void Test_Distance10m()
        {
            TestInitialize(0.00008993);
            Assert.AreEqual(cell.Cell.Cell.Azimuth, 225);
            Assert.AreEqual(cell.Cell.Cell.Height, 40);
            Assert.AreEqual(cell.Cell.AzimuthAngle, 0, eps);
            Assert.AreEqual(cell.Cell.Distance, 0.01, eps);
            cell.CalculateRsrp();
            
            Assert.AreEqual(cell.ReceivedRsrp, -65.218534, eps);
        }

        [Test]
        public void Test_Distance20m()
        {
            TestInitialize(0.00017986);
            Assert.AreEqual(cell.Cell.Distance, 0.02, eps);
            cell.CalculateRsrp();

            Assert.AreEqual(cell.ReceivedRsrp, -70.61971, eps);
        }

        [Test]
        public void Test_Distance50m()
        {
            TestInitialize(0.00044966);
            Assert.AreEqual(cell.Cell.Distance, 0.05, eps);
            cell.CalculateRsrp();

            Assert.AreEqual(cell.ReceivedRsrp, -73.693636, eps);
        }

        [Test]
        public void Test_Distance100m()
        {
            TestInitialize(0.00089932);
            Assert.AreEqual(cell.Cell.Cell.Azimuth, 225);
            Assert.AreEqual(cell.Cell.Cell.Height, 40);
            Assert.AreEqual(cell.Cell.AzimuthAngle, 0, eps);
            Assert.AreEqual(cell.Cell.Distance, 0.1, eps);
            cell.CalculateRsrp();

            Assert.AreEqual(cell.ReceivedRsrp, -76.825993, eps);
        }

        [Test]
        public void Test_Distance200m()
        {
            TestInitialize(0.00179865);
            Assert.AreEqual(cell.Cell.Distance, 0.2, eps);
            cell.CalculateRsrp();

            Assert.AreEqual(cell.ReceivedRsrp, -82.687086, eps);
        }

        [Test]
        public void Test_Distance500m()
        {
            TestInitialize(0.0044966);
            Assert.AreEqual(cell.Cell.Distance, 0.5, eps);
            cell.CalculateRsrp();

            Assert.AreEqual(cell.ReceivedRsrp, -93.857092, eps);
        }
    }
}
