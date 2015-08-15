using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Plan
{
    [TestFixture]
    public class MeasurePointCellRelationTest
    {
        private MeasurePlanCellRelation mpcRelation;
        private readonly IOutdoorCell mainCell = new StubOutdoorCell(112, 22, 10);
        private readonly IOutdoorCell otherCell1 = new StubOutdoorCell(112, 22, 70);
        private readonly IOutdoorCell otherCell2 = new StubOutdoorCell(112, 22, 160);
        private readonly IOutdoorCell otherCell3 = new StubOutdoorCell(112.1, 22, 200);

        [SetUp]
        public void TestInitialize()
        {
            MeasurePoint mPoint =
                FakeMeasurePoint.GenerateMeasurePoint(
                new IOutdoorCell[2] { mainCell, otherCell1 },
                new byte[2] { 2, 1 },
                new double[2] { -10, -12 });
            
            mpcRelation = mPoint.GenerateMeasurePlanCellRelation(0.1);
        }

        [Test]
        public void TestGerateMeasurePlanCellRelation()
        {
            Assert.AreEqual(mpcRelation.TrafficLoad, 0.1);
            Assert.AreEqual(mpcRelation.MainCell.PciModx, 2);
            Assert.AreEqual(mpcRelation.MainCell.ReceivePower, 0.1);
            Assert.AreEqual(mpcRelation.MainCell.Cell.Azimuth, 10);
            Assert.AreEqual(mpcRelation.InterferenceCells.Count, 1);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].Cell.Azimuth, 70);
        }

        [Test]
        public void TestImportMeasurePoint_DifferentMainCell()
        {
            MeasurePoint mPoint =
                FakeMeasurePoint.GenerateMeasurePoint(
                new IOutdoorCell[2] { otherCell1, otherCell2 },
                new byte[2] { 1, 3 },
                new double[2] { -11, -9 });
            mpcRelation.ImportMeasurePoint(mPoint);
            Assert.AreEqual(mpcRelation.MainCell.ReceivePower, 0.1);
            Assert.AreEqual(mpcRelation.MainCell.Cell.Azimuth, 10);
            Assert.AreEqual(mpcRelation.InterferenceCells.Count, 1);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].Cell.Azimuth, 70);
        }

        [Test]
        public void TestImportMeasurePoint_MainCellIsNotStrongest()
        {
            MeasurePoint mPoint =
                FakeMeasurePoint.GenerateMeasurePoint(
                new IOutdoorCell[2] { mainCell, otherCell2 },
                new byte[2] { 1, 3 },
                new double[2] { -11, -9 });
            mpcRelation.ImportMeasurePoint(mPoint);
            Assert.AreEqual(mpcRelation.MainCell.ReceivePower, 0.1);
            Assert.AreEqual(mpcRelation.MainCell.Cell.Azimuth, 10);
            Assert.AreEqual(mpcRelation.InterferenceCells.Count, 1);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].Cell.Azimuth, 70);
        }

        [Test]
        public void TestImportMeasurePoint_SameCells()
        {
            MeasurePoint mPoint =
                FakeMeasurePoint.GenerateMeasurePoint(
                new IOutdoorCell[2] { mainCell, otherCell1 },
                new byte[2] { 2, 1 },
                new double[2] { -10, -12 });
            mpcRelation.ImportMeasurePoint(mPoint);
            Assert.AreEqual(mpcRelation.MainCell.ReceivePower, 0.2);
            Assert.AreEqual(mpcRelation.MainCell.Cell.Azimuth, 10);
            Assert.AreEqual(mpcRelation.InterferenceCells.Count, 1);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].Cell.Azimuth, 70);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].ReceivePower, 0.126191, 1E-6);
        }

        [Test]
        public void TestImportMeasurePoint_SameMainCell_DifferentOtherCells()
        {
            MeasurePoint mPoint =
                FakeMeasurePoint.GenerateMeasurePoint(
                new IOutdoorCell[3] { mainCell, otherCell2, otherCell3 },
                new byte[3] { 2, 1, 2 },
                new double[3] { -7, -12, -9 });
            mpcRelation.ImportMeasurePoint(mPoint);
            Assert.AreEqual(mpcRelation.MainCell.ReceivePower, 0.299526, 1E-6);
            Assert.AreEqual(mpcRelation.MainCell.Cell.Azimuth, 10);
            Assert.AreEqual(mpcRelation.InterferenceCells.Count, 3);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].Cell.Azimuth, 70);
            Assert.AreEqual(mpcRelation.InterferenceCells[0].ReceivePower, 0.063096, 1E-6);
            Assert.AreEqual(mpcRelation.InterferenceCells[1].Cell.Azimuth, 160);
            Assert.AreEqual(mpcRelation.InterferenceCells[2].Cell.Azimuth, 200);
        }
    }
}
