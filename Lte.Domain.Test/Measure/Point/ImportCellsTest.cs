using Lte.Domain.Geo.Entities;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Point
{
    [TestFixture]
    public class ImportCellsTest : ImportCellTestConfig
    {
        [SetUp]
        public void TestInitialize()
        {
            Initialize();
            StubGeoPoint point0 = new StubGeoPoint(112, 23);
            StubGeoPoint point = new StubGeoPoint(point0, 0.01);
            measurablePoint.Longtitute = point.Longtitute;
            measurablePoint.Lattitute = point.Lattitute;

        }

        [Test]
        public void TestImportCells_OneCell()
        {
            ImportOneCell();

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 1);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.Distance, 1.111949, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.AzimuthAngle, 90);
            Assert.AreEqual(FakeComparableCell.Parse(measurablePoint.CellRepository.CellList[0].Cell).MetricCalculate(), 
                31.612974, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.AzimuthFactor(), 30, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.Cell.Height, 40);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget.AntennaGain, 18);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget.TransmitPower, 15.2);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltFactor(), 1.259912, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -136.877442, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
        }

        [Test]
        public void TestImportCells_TwoCells_InOneStation()
        {
            ImportTwoCellsInOneStation();

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 2);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -113.512679, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -136.877442, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 2.939795, eps);
        }

        [Test]
        public void TestImportCells_TwoCells_InOneStation_WithDifferentMods()
        {
            ImportTwoCellsInOneStation_WithDifferentMods();

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 2);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -113.512679, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.PciModx, 1);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -136.877442, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].Cell.PciModx, 0);
        }

        [Test]
        public void TestImportCells_ThreeCells_InOneStation()
        {
            ImportThreeCellsInOneStation();

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 3);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -106.877442, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -113.512679, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].ReceivedRsrp, -136.877442, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].TiltAngle, 2.939795, eps);
        }

        [Test]
        public void TestGenerateMeasurableCellList_ThreeCells_TwoInOneStation_OtherInOtherStation()
        {
            ImportThreeCellsInDifferentStations();

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 3);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -113.512679, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -117.676163, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 3.969564, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].ReceivedRsrp, -136.877442, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].TiltAngle, 2.939795, eps);
        }

    }
}
