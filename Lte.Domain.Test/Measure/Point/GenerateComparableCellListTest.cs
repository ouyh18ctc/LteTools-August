using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Point
{
    [TestFixture]
    public class GenerateComparableCellListTest
    {
        private readonly IList<ILinkBudget<double>> budgetList = new List<ILinkBudget<double>>();
        private readonly IBroadcastModel model = new BroadcastModel();
        private readonly IList<IOutdoorCell> outdoorCellList = new List<IOutdoorCell>();
        const double eps = 1E-6;
        private readonly MeasurePoint measurablePoint = new MeasurePoint();

        [SetUp]
        public void TestInitialize()
        {
            StubGeoPoint point0 = new StubGeoPoint(112, 23);
            StubGeoPoint point = new StubGeoPoint(point0, 0.01);
            measurablePoint.Longtitute = point.Longtitute;
            measurablePoint.Lattitute = point.Lattitute;

        }

        [Test]
        public void TestGenerateComparableCellList_OneCell()
        {
            Mock<IOutdoorCell> outdoorCell = new Mock<IOutdoorCell>();
            outdoorCell.MockOutdoorCell(112, 23, 0, 15.2, 18);
            outdoorCellList.Add(outdoorCell.Object);

            FakeComparableCell[] compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model).Select(
                FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(compCells.Count(), 1);
            Assert.AreEqual(compCells[0].Cell, outdoorCell.Object);
            Assert.AreEqual(compCells[0].Distance, 1.111949, eps);
            Assert.AreEqual(compCells[0].AzimuthAngle, 90);
            Assert.AreEqual(compCells[0].Budget.AntennaGain, 18);
            Assert.AreEqual(compCells[0].Budget.TransmitPower, 15.2);
            Assert.AreEqual(compCells[0].MetricCalculate(), 31.612974, eps);
            Assert.AreEqual(compCells[0].Budget.Model, model);
        }

        [Test]
        public void TestGenerateComparableCellList_TwoCells_InOneStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);

            FakeComparableCell[] compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model).Select(
                FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(compCells.Length, 2);
            Assert.AreEqual(compCells[0].Cell, outdoorCell2.Object);
            Assert.AreEqual(compCells[1].Cell, outdoorCell1.Object);
            Assert.AreEqual(compCells[1].Distance, 1.111949, eps);
            Assert.AreEqual(compCells[0].AzimuthAngle, 45);
            Assert.AreEqual(compCells[1].AzimuthAngle, 90);
            Assert.AreEqual(compCells[0].MetricCalculate(), 8.248211, eps);
            Assert.AreEqual(compCells[1].MetricCalculate(), 31.612974, eps);
            Assert.AreEqual(compCells[1].Budget.Model, model);
        }

        [Test]
        public void TestGenerateComparableCellList_TwoCells_InOneStation_WithDifferentMod()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18, 1);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);

            FakeComparableCell[] compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model).Select(
                FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(compCells.Length, 2);
            Assert.AreEqual(compCells[0].PciModx, 1);
            Assert.AreEqual(compCells[1].PciModx, 0);
            Assert.AreEqual(compCells[0].MetricCalculate(), 8.248211, eps);
            Assert.AreEqual(compCells[1].MetricCalculate(), 31.612974, eps);
            Assert.AreEqual(compCells[1].Budget.Model, model);
        }

        [Test]
        public void TestGenerateComparableCellList_ThreeCells_InOneStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(112, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            FakeComparableCell[] compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model).Select(
                FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(compCells.Length, 3);
            Assert.AreEqual(compCells[0].Cell, outdoorCell3.Object);
            Assert.AreEqual(compCells[1].Cell, outdoorCell2.Object);
            Assert.AreEqual(compCells[2].Cell, outdoorCell1.Object);
            Assert.AreEqual(compCells[0].AzimuthAngle, 0);
            Assert.AreEqual(compCells[1].AzimuthAngle, 45);
            Assert.AreEqual(compCells[2].AzimuthAngle, 90);
            Assert.AreEqual(compCells[0].MetricCalculate(), 1.612974, eps);
            Assert.AreEqual(compCells[1].MetricCalculate(), 8.248211, eps);
            Assert.AreEqual(compCells[2].MetricCalculate(), 31.612974, eps);
        }

        [Test]
        public void TestGenerateComparableCellList_ThreeCells_InOneStation_WithDifferentMods()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18, 1);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(112, 23, 90, 15.2, 18, 4);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            FakeComparableCell[] compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model).Select(
                FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(compCells.Length, 3);
            Assert.AreEqual(compCells[0].PciModx, 1);
            Assert.AreEqual(compCells[1].PciModx, 1);
            Assert.AreEqual(compCells[2].PciModx, 0);
            Assert.AreEqual(compCells[0].MetricCalculate(), 1.612974, eps);
            Assert.AreEqual(compCells[1].MetricCalculate(), 8.248211, eps);
            Assert.AreEqual(compCells[2].MetricCalculate(), 31.612974, eps);

            compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model, 4).Select(
                FakeComparableCell.Parse).ToArray();
            Assert.AreEqual(compCells[0].PciModx, 0);
            Assert.AreEqual(compCells[1].PciModx, 1);
            Assert.AreEqual(compCells[2].PciModx, 0);
        }

        [Test]
        public void TestGenerateComparableCellList_ThreeCells_TwoInOneStation_OtherInOtherStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(111.99, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            FakeComparableCell[] compCells
                = measurablePoint.GenerateComaparbleCellList(outdoorCellList, budgetList, model).Select(
                FakeComparableCell.Parse).ToArray();

            Assert.AreEqual(compCells.Length, 3);
            Assert.AreEqual(compCells[0].Cell, outdoorCell2.Object);
            Assert.AreEqual(compCells[1].Cell, outdoorCell3.Object);
            Assert.AreEqual(compCells[2].Cell, outdoorCell1.Object);
            Assert.AreEqual(compCells[0].AzimuthAngle, 45);
            Assert.AreEqual(compCells[1].AzimuthAngle, 0);
            Assert.AreEqual(compCells[2].AzimuthAngle, 90);
            Assert.AreEqual(compCells[0].MetricCalculate(), 8.248211, eps);
            Assert.AreEqual(compCells[1].MetricCalculate(), 12.149024, eps);
            Assert.AreEqual(compCells[2].MetricCalculate(), 31.612974, eps);
        }
    }
}
