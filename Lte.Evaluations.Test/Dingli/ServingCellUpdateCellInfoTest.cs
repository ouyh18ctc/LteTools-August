using System.Collections.Generic;
using Lte.Evaluations.Dingli;
using Lte.Parameters.Entities;
using Moq;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class ServingCellUpdateCellInfoTest
    {
        private List<Cell> cellList;
        private Mock<IServingCellRecord> mockRecord = new Mock<IServingCellRecord>();

        [SetUp]
        public void TestInitialize()
        {
            cellList = new List<Cell>{
                new Cell {
                    Pci = 1, ENodebId = 1, SectorId = 2, Frequency = 100, Longtitute = 113.01, Lattitute = 22.01 },
                new Cell {
                    Pci = 1, ENodebId = 1, SectorId = 3, Frequency = 100, Longtitute = 113.02, Lattitute = 22.02 },
                new Cell {
                    Pci = 1, ENodebId = 2, SectorId = 1, Frequency = 1825, Longtitute = 113.03, Lattitute = 22.03 },
                new Cell {
                    Pci = 4, ENodebId = 2, SectorId = 2, Frequency = 100, Longtitute = 113.01, Lattitute = 22.01 }
            };
            mockRecord.BindGetAndSetAttributes(x => x.ENodebId, (x, v) => x.ENodebId = v);
            mockRecord.BindGetAndSetAttributes(x => x.SectorId, (x, v) => x.SectorId = v);
            mockRecord.BindGetAndSetAttributes(x => x.Earfcn, (x, v) => x.Earfcn = v);
        }

        [Test]
        public void TestServingCellUpdateCellInfo_FirstMatch()
        {
            mockRecord.SetupGet(x => x.Pci).Returns(1);
            mockRecord.SetupGet(x => x.Longtitute).Returns(113.011);
            mockRecord.SetupGet(x => x.Lattitute).Returns(22.011);
            mockRecord.Object.UpdateCellInfo(cellList);
            Assert.AreEqual(mockRecord.Object.ENodebId, 1);
            Assert.AreEqual(mockRecord.Object.SectorId, 2);
            Assert.AreEqual(mockRecord.Object.Earfcn, 100);
        }

        [Test]
        public void TestServingCellUpdateCellInfo_SecondMatch()
        {
            mockRecord.SetupGet(x => x.Pci).Returns(1);
            mockRecord.SetupGet(x => x.Longtitute).Returns(113.021);
            mockRecord.SetupGet(x => x.Lattitute).Returns(22.021);
            mockRecord.Object.UpdateCellInfo(cellList);
            Assert.AreEqual(mockRecord.Object.ENodebId, 1);
            Assert.AreEqual(mockRecord.Object.SectorId, 3);
            Assert.AreEqual(mockRecord.Object.Earfcn, 100);
        }

        [Test]
        public void TestServingCellUpdateCellInfo_ThirdMatch()
        {
            mockRecord.SetupGet(x => x.Pci).Returns(1);
            mockRecord.SetupGet(x => x.Longtitute).Returns(113.031);
            mockRecord.SetupGet(x => x.Lattitute).Returns(22.031);
            mockRecord.Object.UpdateCellInfo(cellList);
            Assert.AreEqual(mockRecord.Object.ENodebId, 2);
            Assert.AreEqual(mockRecord.Object.SectorId, 1);
            Assert.AreEqual(mockRecord.Object.Earfcn, 1825);
        }

        [Test]
        public void TestServingCellUpdateCellInfo_FourthMatch()
        {
            mockRecord.SetupGet(x => x.Pci).Returns(4);
            mockRecord.SetupGet(x => x.Longtitute).Returns(113.011);
            mockRecord.SetupGet(x => x.Lattitute).Returns(22.011);
            mockRecord.Object.UpdateCellInfo(cellList);
            Assert.AreEqual(mockRecord.Object.ENodebId, 2);
            Assert.AreEqual(mockRecord.Object.SectorId, 2);
            Assert.AreEqual(mockRecord.Object.Earfcn, 100);
        }
    }
}
