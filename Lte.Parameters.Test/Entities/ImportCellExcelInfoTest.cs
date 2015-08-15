using Lte.Parameters.Entities;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class ImportCellExcelInfoTest
    {
        private readonly Cell cell = new Cell();
        private readonly CellExcel cellExcel = new CellExcel
        {
            ENodebId = 2,
            AntennaGain = 17.5,
            Azimuth = 23,
            BandClass = 1,
            Pci = 223,
            IsIndoor = "否",
            Prach = 122,
            SectorId = 1,
            Tac = 65535,
            TransmitReceive = "2t4r",
            Longtitute = 112.123,
            Lattitute = 23.456,
        };

        [Test]
        public void TestImportCellExcelInfo_Original()
        {
            cellExcel.IsIndoor = "否";
            cell.Import(cellExcel);
            Assert.AreEqual(cell.ENodebId, 2);
            Assert.AreEqual(cell.AntennaGain, 17.5);
            Assert.AreEqual(cell.Azimuth, 23);
            Assert.AreEqual(cell.BandClass, 1);
            Assert.AreEqual(cell.Pci, 223);
            Assert.IsTrue(cell.IsOutdoor);
            Assert.AreEqual(cell.Prach, 122);
            Assert.AreEqual(cell.SectorId, 1);
            Assert.AreEqual(cell.Tac, 65535);
            Assert.AreEqual(cell.AntennaPorts, AntennaPortsConfigure.Antenna2T4R);
            Assert.AreEqual(cell.Longtitute, 112.123);
            Assert.AreEqual(cell.Lattitute, 23.456);
        }

        [Test]
        public void TestImportCellExcelInfo_IndoorCell()
        {
            cellExcel.IsIndoor = "是";
            cell.Import(cellExcel);
            Assert.AreEqual(cell.IsOutdoor, false);
            Assert.AreEqual(cell.Longtitute, 112.123);
            Assert.AreEqual(cell.Lattitute, 23.456);
        }
    }
}
