using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.WinApp.Import;
using Moq;
using NUnit.Framework;

namespace Lte.WinApp.Test.Import
{
    [TestFixture]
    public class MroFilesImporterTest
    {
        private readonly Func<string, MroRecordSet> recordSetGenerator = path =>
        {
            string[] fields = path.GetSplittedFields('-');
            return new MroRecordSet
            {
                RecordDate = DateTime.Today,
                RecordList = new List<MrRecord>
                {
                    new MroRecord
                    {
                        RefCell = new MrReferenceCell
                        {
                            CellId  = fields[0].ConvertToInt(50000),
                            SectorId = fields[1].ConvertToByte(0),
                            Rsrp = fields[2].ConvertToByte(0)
                        },
                        NbCells = new List<MrNeighborCell>
                        {
                            new MrNeighborCell
                            {
                                Pci = fields[3].ConvertToShort(1),
                                Rsrp = fields[4].ConvertToByte(0)
                            }
                        }
                    }
                },
                ENodebId = fields[0].ConvertToInt(50000)
            };
        };

        private readonly Mock<ICellRepository> cellRepository=new Mock<ICellRepository>();
        private readonly Mock<ILteNeighborCellRepository> neighborRepository=new Mock<ILteNeighborCellRepository>();

        private MroFilesImporter importer;

        [SetUp]
        public void SetUp()
        {
            cellRepository.Setup(x => x.GetAll()).Returns(new List<Cell>
            {
                new Cell {ENodebId = 50011, SectorId = 0, Pci = 301},
                new Cell {ENodebId = 50011, SectorId = 1, Pci = 302},
                new Cell {ENodebId = 50011, SectorId = 2, Pci = 303},
                new Cell {ENodebId = 50012, SectorId = 0, Pci = 304},
                new Cell {ENodebId = 50012, SectorId = 1, Pci = 305},
                new Cell {ENodebId = 50012, SectorId = 2, Pci = 306},
            }.AsQueryable());
            cellRepository.Setup(x => x.GetAllList()).Returns(cellRepository.Object.GetAll().ToList());
            neighborRepository.SetupGet(x => x.NearestPciCells).Returns(new List<NearestPciCell>
            {
                new NearestPciCell {CellId = 50001, SectorId = 0, NearestCellId = 50002, NearestSectorId = 0, Pci = 100},
                new NearestPciCell {CellId = 50001, SectorId = 0, NearestCellId = 50002, NearestSectorId = 1, Pci = 101},
                new NearestPciCell {CellId = 50001, SectorId = 0, NearestCellId = 50002, NearestSectorId = 2, Pci = 102},
                new NearestPciCell {CellId = 50001, SectorId = 1, NearestCellId = 50002, NearestSectorId = 0, Pci = 100},
                new NearestPciCell {CellId = 50001, SectorId = 1, NearestCellId = 50002, NearestSectorId = 1, Pci = 101},
                new NearestPciCell {CellId = 50001, SectorId = 1, NearestCellId = 50002, NearestSectorId = 2, Pci = 102},
                new NearestPciCell {CellId = 50001, SectorId = 2, NearestCellId = 50002, NearestSectorId = 0, Pci = 100},
                new NearestPciCell {CellId = 50001, SectorId = 2, NearestCellId = 50002, NearestSectorId = 1, Pci = 101},
                new NearestPciCell {CellId = 50001, SectorId = 2, NearestCellId = 50002, NearestSectorId = 2, Pci = 102}
            }.AsQueryable());
            importer=new MroFilesImporter(cellRepository.Object,neighborRepository.Object);
        }

        [TestCase("50001-0-4-101-8")]
        [TestCase("50001-0-4-102-8")]
        [TestCase("50001-2-4-102-8")]
        public void Test_Once_OnePath(string path)
        {
            importer.Import(new[] {path}, recordSetGenerator);
            Assert.AreEqual(importer.InterferenceStats.Count, 1);
            Assert.AreEqual(importer.RsrpTaStatList.Count, 1);
        }
    }
}
