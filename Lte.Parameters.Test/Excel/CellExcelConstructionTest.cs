using System.Data;
using Lte.Parameters.Test.Entities;
using NUnit.Framework;
using Lte.Parameters.Entities;
using Moq;
using System;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class CellExcelConstructionTest
    {
        private CellExcel cellExcel;
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();

        [SetUp]
        public void TestInitialize()
        {
            Tuple<string, string>[] contents =
            {
                new Tuple<string,string>("eNodeB ID","3344"),
                new Tuple<string,string>("CELL_ID","1"),
                new Tuple<string,string>("经度","112.123"),
                new Tuple<string,string>("纬度","23.456"),
                new Tuple<string,string>("PCI","34")
            };
            mockReader.MockReaderContents(contents);
        }

        [Test]
        public void TestCellExcelConstruction_BasicParameters()
        {
            cellExcel = new CellExcel(mockReader.Object);
            cellExcel.Import();
            Assert.AreEqual(cellExcel.ENodebId, 3344);
            Assert.AreEqual(cellExcel.SectorId, 1);
            Assert.AreEqual(cellExcel.Longtitute, 112.123);
            Assert.AreEqual(cellExcel.Lattitute, 23.456);
            Assert.AreEqual(cellExcel.Pci, 34);
            Assert.AreEqual(cellExcel.Frequency, 100);
            Assert.AreEqual(cellExcel.Height, 30);
            Assert.AreEqual(cellExcel.BandClass, 1);
        }
    }

    [TestFixture]
    public class CdmaCellExcelConstructionTest
    {
        private CdmaCellExcel cellExcel;
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();

        [SetUp]
        public void TestInitialize()
        {
            Tuple<string, string>[] contents =
            {
                new Tuple<string,string>("基站编号","3344"),
                new Tuple<string,string>("小区标识","12"),
                new Tuple<string,string>("经度","112.123"),
                new Tuple<string,string>("纬度","23.456"),
                new Tuple<string,string>("LAC","0x34"),
                new Tuple<string,string>("载扇类型(1X/DO)", "DO")
            };
            mockReader.MockReaderContents(contents);
        }

        [Test]
        public void TestCdmaCellExcelConstruction()
        {
            cellExcel = new CdmaCellExcel(mockReader.Object);
            cellExcel.Import();
            Assert.AreEqual(cellExcel.BtsId, 3344);
            Assert.AreEqual(cellExcel.CellId, 12);
            Assert.AreEqual(cellExcel.Longtitute, 112.123);
            Assert.AreEqual(cellExcel.Lattitute, 23.456);
            Assert.AreEqual(cellExcel.Lac, "0x34");
            Assert.AreEqual(cellExcel.CellType, "DO");
        }
    }
}
