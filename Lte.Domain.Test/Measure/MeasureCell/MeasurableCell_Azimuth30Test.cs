using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.MeasureCell
{
    [TestFixture]
    public class MeasurableCellAzimuth30Test
    {
        private readonly IGeoPoint<double> point = new GeoPoint(112, 23);
        private IGeoPoint<double> _point2;
        private readonly ILinkBudget<double> budget = new LinkBudget(new BroadcastModel());
        private IOutdoorCell _ocell;
        private ComparableCell _ccell;
        private MeasurableCell _cell;
        const double Eps = 1E-6;

        private void TestInitialize(double distance)
        {
            _point2 = new StubGeoPoint(point, distance, 45);
            _ocell = new StubOutdoorCell(_point2, 195) {Height = 40, ETilt = 4, MTilt = 1};
            _ccell = new ComparableCell(point, _ocell);
            _cell = new MeasurableCell(_ccell, point, budget);
        }

        [Test]
        public void Test_Distance10m()
        {
            TestInitialize(0.00008993);
            Assert.AreEqual(_cell.Cell.Cell.Azimuth, 195);
            Assert.AreEqual(_cell.Cell.Cell.Height, 40);
            Assert.AreEqual(_cell.Cell.AzimuthAngle, 30, Eps);
            Assert.AreEqual(_cell.Cell.Distance, 0.01, Eps);
            _cell.CalculateRsrp();

            Assert.AreEqual(_cell.ReceivedRsrp, -67.783189, Eps);
        }

        [Test]
        public void Test_Distance20m()
        {
            TestInitialize(0.00017986);
            Assert.AreEqual(_cell.Cell.Distance, 0.02, Eps);
            _cell.CalculateRsrp();

            Assert.AreEqual(_cell.ReceivedRsrp, -73.184365, Eps);
        }

        [Test]
        public void Test_Distance50m()
        {
            TestInitialize(0.00044966);
            Assert.AreEqual(_cell.Cell.Distance, 0.05, Eps);
            _cell.CalculateRsrp();

            Assert.AreEqual(_cell.ReceivedRsrp, -76.258291, Eps);
        }

        [Test]
        public void Test_Distance100m()
        {
            TestInitialize(0.00089932);
            Assert.AreEqual(_cell.Cell.Distance, 0.1, Eps);
            _cell.CalculateRsrp();

            Assert.AreEqual(_cell.ReceivedRsrp, -79.390648, Eps);
        }

        [Test]
        public void Test_Distance200m()
        {
            TestInitialize(0.00179865);
            Assert.AreEqual(_cell.Cell.Distance, 0.2, Eps);
            _cell.CalculateRsrp();

            Assert.AreEqual(_cell.ReceivedRsrp, -85.251741, Eps);
        }

        [Test]
        public void Test_Distance500m()
        {
            TestInitialize(0.0044966);
            Assert.AreEqual(_cell.Cell.Distance, 0.5, Eps);
            _cell.CalculateRsrp();

            Assert.AreEqual(_cell.ReceivedRsrp, -96.421747, Eps);
        }
    }
}
