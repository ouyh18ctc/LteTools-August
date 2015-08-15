using System;
using Lte.Parameters.Entities;
using System.Data;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class CdmaCellTest : CdmaTestConfig
    {
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();

        [SetUp]
        public void SetUp()
        {
            Initialize();
            ExcelInfoInitialize();
        }

        private void ExcelInfoInitialize()
        {
            Tuple<string, string>[] contents =
            {
                new Tuple<string,string>("基站编号","1111"),
                new Tuple<string,string>("扇区标识","51"),
                new Tuple<string, string>("小区标识", "3333"), 
                new Tuple<string, string>("频点", "12583"),
                new Tuple<string, string>("覆盖类型(室内/室外/地铁)", "室内"), 
                new Tuple<string, string>("载扇类型(1X/DO)", "1X"), 
                new Tuple<string,string>("经度","23.456"),
                new Tuple<string,string>("纬度","112.333"),
                new Tuple<string, string>("挂高", "100"), 
                new Tuple<string,string>("下倾角（机械）","2"),
                new Tuple<string,string>("下倾角（电调）","23"),
                new Tuple<string, string>("方位角","66"), 
                new Tuple<string, string>("天线增益（dBi）","16.8"), 
                new Tuple<string, string>("PN码","27"), 
                new Tuple<string, string>("LAC","0x3233") 
            };
            mockReader.MockReaderContents(contents);
        }

        private void AssertTest(bool importNewInfo, int btsId, int cellId, byte sectorId, short pn, string cellType,
            short frequency1, double longtitute, double lattitute, double height, double mTilt, double eTilt, double azimuth,
            double antennaGain, string lac, CdmaCellExcel cellExcel)
        {
            cell.Import(cellExcel, importNewInfo);
            Assert.IsNotNull(cell);
            Assert.AreEqual(cell.BtsId, btsId);
            Assert.AreEqual(cell.CellId, cellId);
            Assert.AreEqual(cell.SectorId, sectorId);
            Assert.AreEqual(cell.Pn, pn);
            Assert.AreEqual(cell.CellType, cellType);
            Assert.AreEqual(cell.Frequency1, frequency1);
            Assert.AreEqual(cell.Longtitute, longtitute);
            Assert.AreEqual(cell.Lattitute, lattitute);
            Assert.AreEqual(cell.Height, height);
            Assert.AreEqual(cell.MTilt, mTilt);
            Assert.AreEqual(cell.ETilt, eTilt);
            Assert.AreEqual(cell.Azimuth, azimuth);
            Assert.AreEqual(cell.AntennaGain, antennaGain);
            Assert.AreEqual(cell.Lac, lac);
        }

        [TestCase(false, 4, 3964, 3, 198, "DO", 12583, 23.456, 112.333, 100, 2, 23, 66, 16.8, "")]
        [TestCase(true, 1111, 3333, 51, 27, "1X", 12583, 23.456, 112.333, 100, 2, 23, 66, 16.8, "0x3233")]
        public void TestCdmaCell(bool importNewInfo, int btsId, int cellId, byte sectorId, short pn, string cellType,
            short frequency1, double longtitute, double lattitute, double height, double mTilt, double eTilt, double azimuth,
            double antennaGain, string lac)
        {
            CdmaCellExcel cellExcel = new CdmaCellExcel(mockReader.Object);
            cellExcel.Import();
            AssertTest(importNewInfo, btsId, cellId, sectorId, pn, cellType, frequency1, longtitute, lattitute, 
                height, mTilt, eTilt, azimuth, antennaGain, lac, cellExcel);
        }

        [Test]
        public void TestCdmaCell_OneX()
        {
            CdmaCellExcel cellExcel = new CdmaCellExcel(mockReader.Object);
            cellExcel.Import();
            cellLineInfo = new MmlLineInfo(
                "ADD CELL: BTSID=90, FN=2, CN=90, SCTIDLST=\"1\", PNLST=\"232\", SID=13832, NID=65535, PZID=3, TYP=CDMA1X, LAC=\"0x2181\", LCN=90, LSCTID=\"1\", ASSALW1X=YES, IFBORDCELL=NO, REVRSSICARRASSNSW=OFF, AUTODWNFWDEQLCHANTHD=20, AUTODWNCOUNTTHD=600, UNBLKFWDEQLCHANTHD=40, LOCATE=URBAN, MICROCELL=NO, HARDASSIGNTYPE=BOTH_VOICE_DATA, ANASSIST1XDOSW=OFF;"
                );
            cell = cellLineInfo.GenerateCdmaCell();
            cell.Import(cellExcel, false);
            Assert.IsNotNull(cell);
            Assert.AreEqual(cell.BtsId, 90, "bts");
            Assert.AreEqual(cell.CellId, 90, "cell");
            Assert.AreEqual(cell.SectorId, 1);
            Assert.AreEqual(cell.Pn, 232);
            Assert.AreEqual(cell.CellType, "1X");
            Assert.AreEqual(cell.Frequency1, 12583);
            Assert.AreEqual(cell.Longtitute, 23.456);
            Assert.AreEqual(cell.Lattitute, 112.333);
            Assert.AreEqual(cell.Height, 100);
            Assert.AreEqual(cell.MTilt, 2);
            Assert.AreEqual(cell.ETilt, 23);
            Assert.AreEqual(cell.Azimuth, 66);
            Assert.AreEqual(cell.AntennaGain, 16.8);
            Assert.AreEqual(cell.Lac, "0x2181");
        }
    }
}
