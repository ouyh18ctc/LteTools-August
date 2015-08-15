using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class CdmaCellConstructionTest
    {
        [Test]
        public void TestCdmaCellConstruction_NewCell()
        {
            CdmaCell cell = new CdmaCell { BtsId = 1, SectorId = 2, CellId = 255, Frequency1 = -1 };
            CdmaCellExcel cellExcelInfo = new CdmaCellExcel
            {
                BtsId = 22,
                SectorId = 2,
                CellId = 255,
                Frequency = 37,
                CellType = "DO"
            };
            cell.Import(cellExcelInfo, false);
            Assert.IsNotNull(cell);
            Assert.AreEqual(cell.BtsId, 1);
            Assert.AreEqual(cell.SectorId, 2);
            Assert.AreEqual(cell.CellId, 255);
            Assert.AreEqual(cell.CellType, null);
            Assert.AreEqual(cell.Frequency1, 37);
            Assert.IsTrue(cell.HasFrequency(37));
            Assert.IsFalse(cell.HasFrequency(78));
            Assert.AreEqual(cell.Frequency, 1);
            Assert.AreEqual(cell.Frequency2, -1);

            cell = new CdmaCell { BtsId = 1, SectorId = 2, CellId = 255, Frequency1 = -1 };
            cell.Import(cellExcelInfo, true);
            Assert.IsNotNull(cell);
            Assert.AreEqual(cell.BtsId, 22);
            Assert.AreEqual(cell.SectorId, 2);
            Assert.AreEqual(cell.CellId, 255);
            Assert.AreEqual(cell.CellType, "DO");
            Assert.AreEqual(cell.Frequency1, 37);
            Assert.IsTrue(cell.HasFrequency(37));
            Assert.IsFalse(cell.HasFrequency(78));
            Assert.AreEqual(cell.Frequency, 1);
            Assert.AreEqual(cell.Frequency2, -1);
        }

        [Test]
        public void TestCdmaCellConstruction_SameFirstFrequency()
        {
            CdmaCellExcel cellExcelInfo = new CdmaCellExcel
            {
                BtsId = 22,
                SectorId = 2,
                CellId = 255,
                Frequency = 37,
                CellType = "DO"
            };
            using (CdmaCell cell = new CdmaCell { BtsId = 1, SectorId = 2, CellId = 255, Frequency1 = 37, Frequency = 1 })
            {
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37, "f1");
                Assert.AreEqual(cell.Frequency, 1);
                Assert.AreEqual(cell.Frequency2, -1, "f1");
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsFalse(cell.HasFrequency(78));
            }
            using (CdmaCell cell = new CdmaCell { BtsId = 1, SectorId = 2, CellId = 255, Frequency1 = 37, Frequency = 1 })
            {
                CdmaCell.UpdateFirstFrequency = true;
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37);
                Assert.AreEqual(cell.Frequency, 1);
                Assert.AreEqual(cell.Frequency2, -1);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsFalse(cell.HasFrequency(78));
            }
        }

        [Test]
        public void TestCdmaCellConstruction_UpdateSecondFrequency()
        {
            CdmaCellExcel cellExcelInfo = new CdmaCellExcel
            {
                BtsId = 22,
                SectorId = 2,
                CellId = 255,
                Frequency = 78,
                CellType = "DO"
            };
            using (CdmaCell cell = new CdmaCell { BtsId = 1, SectorId = 2, CellId = 255, Frequency1 = 37, Frequency = 1 })
            {
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37, "f1");
                Assert.AreEqual(cell.Frequency2, 78, "f1");
                Assert.AreEqual(cell.Frequency3, -1);
                Assert.AreEqual(cell.Frequency, 3);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
            }
            using (CdmaCell cell = new CdmaCell { BtsId = 1, SectorId = 2, CellId = 255, Frequency1 = 37, Frequency = 1 })
            {
                CdmaCell.UpdateFirstFrequency = true;
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37);
                Assert.AreEqual(cell.Frequency2, 78);
                Assert.AreEqual(cell.Frequency3, -1);
                Assert.AreEqual(cell.Frequency, 3);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
            }
        }

        [Test]
        public void TestCdmaCellConstruction_SameSecondFrequency()
        {
            CdmaCellExcel cellExcelInfo = new CdmaCellExcel
            {
                BtsId = 22,
                SectorId = 2,
                CellId = 255,
                Frequency = 78,
                CellType = "DO"
            };
            using (CdmaCell cell = new CdmaCell
            {
                BtsId = 1,
                SectorId = 2,
                CellId = 255,
                Frequency1 = 37,
                Frequency2 = 78,
                Frequency = 3
            })
            {
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37, "f1");
                Assert.AreEqual(cell.Frequency2, 78, "f1");
                Assert.AreEqual(cell.Frequency3, -1);
                Assert.AreEqual(cell.Frequency, 3);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
            }
            using (CdmaCell cell = new CdmaCell
            {
                BtsId = 1,
                SectorId = 2,
                CellId = 255,
                Frequency1 = 37,
                Frequency2 = 78,
                Frequency = 3
            })
            {
                CdmaCell.UpdateFirstFrequency = true;
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37);
                Assert.AreEqual(cell.Frequency2, 78);
                Assert.AreEqual(cell.Frequency3, -1);
                Assert.AreEqual(cell.Frequency, 3);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
            }
        }

        [Test]
        public void TestCdmaCellConstruction_UpdateThirdFrequency()
        {
            CdmaCellExcel cellExcelInfo = new CdmaCellExcel
            {
                BtsId = 22,
                SectorId = 2,
                CellId = 255,
                Frequency = 119,
                CellType = "DO"
            };
            using (CdmaCell cell = new CdmaCell
            {
                BtsId = 1,
                SectorId = 2,
                CellId = 255,
                Frequency1 = 37,
                Frequency2 = 78,
                Frequency = 3
            })
            {
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37, "f1");
                Assert.AreEqual(cell.Frequency2, 78, "f1");
                Assert.AreEqual(cell.Frequency3, 119);
                Assert.AreEqual(cell.Frequency4, -1);
                Assert.AreEqual(cell.Frequency, 7);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
                Assert.IsTrue(cell.HasFrequency(119));
            }
            using (CdmaCell cell = new CdmaCell
            {
                BtsId = 1,
                SectorId = 2,
                CellId = 255,
                Frequency1 = 37,
                Frequency2 = 78,
                Frequency = 3
            })
            {
                CdmaCell.UpdateFirstFrequency = true;
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37);
                Assert.AreEqual(cell.Frequency2, 78);
                Assert.AreEqual(cell.Frequency3, 119);
                Assert.AreEqual(cell.Frequency4, -1);
                Assert.AreEqual(cell.Frequency, 7);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
                Assert.IsTrue(cell.HasFrequency(119));
            }
        }

        [Test]
        public void TestCdmaCellConstruction_UpdateFourthFrequency()
        {
            ;
            CdmaCellExcel cellExcelInfo = new CdmaCellExcel
            {
                BtsId = 22,
                SectorId = 2,
                CellId = 255,
                Frequency = 160,
                CellType = "DO"
            };
            using (CdmaCell cell = new CdmaCell
            {
                BtsId = 1,
                SectorId = 2,
                CellId = 255,
                Frequency1 = 37,
                Frequency2 = 78,
                Frequency3 = 119,
                Frequency = 7
            })
            {
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37, "f1");
                Assert.AreEqual(cell.Frequency2, 78, "f1");
                Assert.AreEqual(cell.Frequency3, 119);
                Assert.AreEqual(cell.Frequency4, 160);
                Assert.AreEqual(cell.Frequency5, -1);
                Assert.AreEqual(cell.Frequency, 15);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
                Assert.IsTrue(cell.HasFrequency(119));
                Assert.IsTrue(cell.HasFrequency(160));
            }
            using (CdmaCell cell = new CdmaCell
            {
                BtsId = 1,
                SectorId = 2,
                CellId = 255,
                Frequency1 = 37,
                Frequency2 = 78,
                Frequency3 = 119,
                Frequency = 7
            })
            {
                CdmaCell.UpdateFirstFrequency = true;
                cell.Import(cellExcelInfo, true);
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.BtsId, 1);
                Assert.AreEqual(cell.SectorId, 2);
                Assert.AreEqual(cell.CellId, 255);
                Assert.AreEqual(cell.CellType, null);
                Assert.AreEqual(cell.Frequency1, 37);
                Assert.AreEqual(cell.Frequency2, 78);
                Assert.AreEqual(cell.Frequency3, 119);
                Assert.AreEqual(cell.Frequency4, 160);
                Assert.AreEqual(cell.Frequency5, -1);
                Assert.AreEqual(cell.Frequency, 15);
                Assert.IsTrue(cell.HasFrequency(37));
                Assert.IsTrue(cell.HasFrequency(78));
                Assert.IsTrue(cell.HasFrequency(119));
                Assert.IsTrue(cell.HasFrequency(160));
            }
        }
    }
}
