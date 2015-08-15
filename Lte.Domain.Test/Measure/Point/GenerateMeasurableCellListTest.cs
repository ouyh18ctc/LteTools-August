using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Point
{
    [TestFixture]
    public class GenerateMeasurableCellListTest
    {
        private readonly MeasurePoint measurablePoint = new MeasurePoint();
        private readonly IBroadcastModel model = new BroadcastModel();
        private ComparableCell[] compCells;
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            StubGeoPoint point0 = new StubGeoPoint(112, 23);
            StubGeoPoint point = new StubGeoPoint(point0, 0.01);
            measurablePoint.Longtitute = point.Longtitute;
            measurablePoint.Lattitute = point.Lattitute;

        }

        [Test]
        public void TestGenerateMeasurableCellList_OneCell()
        {
            Mock<IOutdoorCell> outdoorCell = new Mock<IOutdoorCell>();
            outdoorCell.MockOutdoorCell(112, 23, 0, 15.2, 18);

            compCells = new ComparableCell[1];
            compCells[0] = new ComparableCell(1.111949, 90) {Budget = new LinkBudget(model), Cell = outdoorCell.Object};

            measurablePoint.CellRepository.GenerateMeasurableCellList(compCells, measurablePoint);

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 1);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell, compCells[0]);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget, compCells[0].Budget);
            Assert.AreEqual(FakeComparableCell.Parse(measurablePoint.CellRepository.CellList[0].Cell).MetricCalculate(), 
                31.61297, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.AzimuthFactor(), 30, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget.AntennaGain, 18);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget.TransmitPower, 15.2);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell.Cell.Height, 40);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltFactor(), 1.259912, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -136.877439, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
        }

        [Test]
        public void TestGenerateMeasurableCellList_TwoCells_InOneStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);

            compCells = new ComparableCell[2];
            compCells[0] = new ComparableCell(1.111949, 45) {Budget = new LinkBudget(model), Cell = outdoorCell2.Object};
            compCells[1] = new ComparableCell(1.111949, 90) {Budget = new LinkBudget(model), Cell = outdoorCell1.Object};

            measurablePoint.CellRepository.GenerateMeasurableCellList(compCells, measurablePoint);

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 2);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell, compCells[0]);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget, compCells[0].Budget);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -113.512676, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -136.877439, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 2.939795, eps);
        }

        [Test]
        public void TestGenerateMeasurableCellList_ThreeCells_InOneStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(112, 23, 90, 15.2, 18);

            compCells = new ComparableCell[3];
            compCells[0] = new ComparableCell(1.111949, 0) {Budget = new LinkBudget(model), Cell = outdoorCell3.Object};
            compCells[1] = new ComparableCell(1.111949, 45) {Budget = new LinkBudget(model), Cell = outdoorCell2.Object};
            compCells[2] = new ComparableCell(1.111949, 90) {Budget = new LinkBudget(model), Cell = outdoorCell1.Object};

            measurablePoint.CellRepository.GenerateMeasurableCellList(compCells, measurablePoint);

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 3);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell, compCells[0]);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget, compCells[0].Budget);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -106.877439, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -113.512676, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].ReceivedRsrp, -136.877439, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].TiltAngle, 2.939795, eps);
        }

        [Test]
        public void TestGenerateMeasurableCellList_ThreeCells_TwoInOneStation_OtherInOtherStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(111.99, 23, 90, 15.2, 18);

            compCells = new ComparableCell[3];
            compCells[0] = new ComparableCell(1.111949, 45)
            {
                Budget = new LinkBudget(model), 
                Cell = outdoorCell2.Object
            };
            compCells[1] = new ComparableCell(1.111949, 0)
            {
                Budget = new LinkBudget(model), 
                Cell = outdoorCell3.Object
            };
            compCells[2] = new ComparableCell(1.111949, 90)
            {
                Budget = new LinkBudget(model), 
                Cell = outdoorCell1.Object
            };

            measurablePoint.CellRepository.GenerateMeasurableCellList(compCells, measurablePoint);

            Assert.AreEqual(measurablePoint.CellRepository.CellList.Count, 3);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Cell, compCells[0]);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].Budget, compCells[0].Budget);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].ReceivedRsrp, -113.512676, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[0].TiltAngle, 2.939795, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].ReceivedRsrp, -107.318768, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[1].TiltAngle, 3.969564, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].ReceivedRsrp, -136.877439, eps);
            Assert.AreEqual(measurablePoint.CellRepository.CellList[2].TiltAngle, 2.939795, eps);
        }
    }
}
