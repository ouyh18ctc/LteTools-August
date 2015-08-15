using System;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Entities
{
    [TestFixture]
    public class TopDrop2GCellTest
    {
        [TestCase(1, 2, 3, "9_411_1_金沙联沙冼村[411](金沙联沙冼村_1)", 4, 2015, 3, 1, 18, 411)]
        [TestCase(2, 3, 5, "7_6322_1_金543he沙冼村[6322](金沙联沙冼村_5)", 35, 2015, 9, 27, 14, 6322)]
        [TestCase(1022, 1, 201, "4_2311_1_金沙联沙冼村[2311](金沙联沙冼村_7)", 53, 2014, 7, 22, 6, 2311)]
        public void TestTopDrop2GCell(int btsId, byte sectorId, short frequency,
            string cellName, int drops, int year, int month, int day, int hour, int cellId)
        {
            TopDrop2GCellExcel cellExcel = new TopDrop2GCellExcel
            {
                BtsId = btsId,
                SectorId = sectorId,
                Frequency = frequency,
                CellName = cellName,
                Drops = drops,
                StatDate = new DateTime(year, month, day),
                StatHour = hour
            };
            TopDrop2GCell cell = new TopDrop2GCell();
            cell.Import(cellExcel);
            Assert.AreEqual(cell.BtsId, btsId);
            Assert.AreEqual(cell.SectorId, sectorId);
            Assert.AreEqual(cell.Frequency, frequency);
            Assert.AreEqual(cell.CellId, cellId);
            Assert.AreEqual(cell.Drops, drops);
            Assert.AreEqual(cell.StatTime.Year, year);
            Assert.AreEqual(cell.StatTime.Month, month);
            Assert.AreEqual(cell.StatTime.Day, day);
            Assert.AreEqual(cell.StatTime.Hour, hour);
        }
    }
}
