using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.MeasureCell
{
    [TestFixture]
    public class MeasurableCellConnectionTest
    {
        private IOutdoorCell _cell;
        private IGeoPoint<double> _point;
        private MeasurableCell _mCell;
        const double Eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        { 
            _point = new StubGeoPoint(112, 23);
            _cell = new StubOutdoorCell
            {
                RsPower = 15.2,
                AntennaGain = 17.5,
                Azimuth = 60,
                Longtitute = 112.01,
                Lattitute = 23.01,
                Height = 30,
                MTilt = 5,
                ETilt = 1,
                Pci = 22,
                Frequency = 100,
                CellName = "Cell-1"
            };
        }

        [Test]
        public void TestMeasurableCellConnection_2_1G_Angle165()
        {
            _mCell = new MeasurableCell(_point, _cell);
            Assert.IsNotNull(_mCell);
            Assert.AreEqual(_mCell.CellName, "Cell-1");
            Assert.AreEqual(_mCell.PciModx, 1);
            Assert.AreEqual(_mCell.DistanceInMeter, 1572.533733, Eps);
            Assert.AreEqual(_mCell.Cell.AzimuthAngle, 165, Eps);
            Assert.AreEqual(_mCell.TiltAngle, 4.907073, Eps);
            Assert.AreEqual(_mCell.Budget.AntennaGain, 17.5);
            Assert.AreEqual(_mCell.Budget.TransmitPower, 15.2);
            Assert.AreEqual(_mCell.Budget.Model.Earfcn, 100);
            Assert.AreEqual(_mCell.Budget.Model.UrbanType, UrbanType.Large);
            _mCell.CalculateRsrp();
            Assert.AreEqual(_mCell.ReceivedRsrp, -145.286797, Eps);
        }

        [Test]
        public void TestMeasurableCellConnection_2_1G_Angle15()
        {
            _cell.Longtitute = 111.99;
            _cell.Lattitute = 22.99;
            _mCell = new MeasurableCell(_point, _cell);
            Assert.IsNotNull(_mCell);
            Assert.AreEqual(_mCell.CellName, "Cell-1");
            Assert.AreEqual(_mCell.PciModx, 1);
            Assert.AreEqual(_mCell.DistanceInMeter, 1572.533733, Eps);
            Assert.AreEqual(_mCell.Cell.AzimuthAngle, -15, Eps);
            Assert.AreEqual(_mCell.TiltAngle, 4.907073, Eps);
            Assert.AreEqual(_mCell.Budget.AntennaGain, 17.5);
            Assert.AreEqual(_mCell.Budget.TransmitPower, 15.2);
            Assert.AreEqual(_mCell.Budget.Model.Earfcn, 100);
            Assert.AreEqual(_mCell.Budget.Model.UrbanType, UrbanType.Large);
            _mCell.CalculateRsrp();
            Assert.AreEqual(_mCell.ReceivedRsrp, -115.639707, Eps);
        }
    }
}
