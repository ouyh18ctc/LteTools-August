using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Record
{
    [TestFixture]
    public class NearestPciCellRepositoryTest
    {
        private readonly IEnumerable<Cell> cells = new List<Cell>
        {
            new Cell {ENodebId = 1, SectorId = 1, Pci = 101, Longtitute = 112, Lattitute = 23},
            new Cell {ENodebId = 3, SectorId = 4, Pci = 112, Longtitute = 112.1, Lattitute = 23.1},
            new Cell {ENodebId = 1, SectorId = 2, Pci = 102, Longtitute = 112, Lattitute = 23},
            new Cell {ENodebId = 1, SectorId = 3, Pci = 103, Longtitute = 112, Lattitute = 23},
            new Cell {ENodebId = 2, SectorId = 1, Pci = 111, Longtitute = 112, Lattitute = 23},
            new Cell {ENodebId = 2, SectorId = 2, Pci = 112, Longtitute = 112, Lattitute = 23},
            new Cell {ENodebId = 2, SectorId = 3, Pci = 113, Longtitute = 112, Lattitute = 23}
        };

        private INearestPciCellRepository repository;

        private readonly Mock<ILteNeighborCellRepository> neiRepository = new Mock<ILteNeighborCellRepository>();

        [SetUp]
        public void SetUp()
        {
            repository = new NearestPciCellRepository(cells);
            neiRepository.SetupGet(x => x.NearestPciCells).Returns(new List<NearestPciCell>
            {
                new NearestPciCell {CellId = 1, SectorId = 1, NearestCellId = 1, NearestSectorId = 2, Pci = 102},
                new NearestPciCell {CellId = 1, SectorId = 1, NearestCellId = 1, NearestSectorId = 3, Pci = 103},
                new NearestPciCell {CellId = 1, SectorId = 1, NearestCellId = 2, NearestSectorId = 1, Pci = 111}
            }.AsQueryable());
            neiRepository.SetupGet(x => x.NeighborCells).Returns(new List<LteNeighborCell>
            {
                new LteNeighborCell {CellId = 1, SectorId = 2, NearestCellId = 1, NearestSectorId = 1},
                new LteNeighborCell {CellId = 1, SectorId = 2, NearestCellId = 1, NearestSectorId = 3},
                new LteNeighborCell {CellId = 1, SectorId = 2, NearestCellId = 2, NearestSectorId = 1}
            }.AsQueryable());
            repository=new NearestPciCellRepository(cells);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Test_AddNeighbors_ENodebInexisted(int eNodebId)
        {
            repository.AddNeighbors(neiRepository.Object, eNodebId);
            Assert.AreEqual(repository.NearestPciCells.Count, 0);
        }

        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 5)]
        [TestCase(6, 6)]
        public void Test_AddNeighbors_ENodebInexisted_Twice(int eNodebId1, int eNodebId2)
        {
            repository.AddNeighbors(neiRepository.Object, eNodebId1);
            repository.AddNeighbors(neiRepository.Object, eNodebId2);
            Assert.AreEqual(repository.NearestPciCells.Count, 0);
        }

        [Test]
        public void Test_AddNeighbors_ENodebExisted()
        {
            repository.AddNeighbors(neiRepository.Object, 1);
            Assert.AreEqual(repository.NearestPciCells.Count, 3);
            Assert.AreEqual(repository.NearestPciCells[0].Pci, 102);
            Assert.AreEqual(repository.NearestPciCells[1].Pci, 103);
            Assert.AreEqual(repository.NearestPciCells[2].Pci, 111);
        }

        [Test]
        public void TestAddNeighbors_Twice_ENodebExisted()
        {
            repository.AddNeighbors(neiRepository.Object, 1);
            repository.AddNeighbors(neiRepository.Object, 1);
            Assert.AreEqual(repository.NearestPciCells.Count, 3);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestAddNeighbors_Twice_FirstExisted(int eNodebId)
        {
            repository.AddNeighbors(neiRepository.Object, 1);
            repository.AddNeighbors(neiRepository.Object, eNodebId);
            Assert.AreEqual(repository.NearestPciCells.Count, 0);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestAddNeighbors_Twice_SecondExisted(int eNodebId)
        {
            repository.AddNeighbors(neiRepository.Object, eNodebId);
            repository.AddNeighbors(neiRepository.Object, 1);
            Assert.AreEqual(repository.NearestPciCells.Count, 3);
        }

        [TestCase(102)]
        [TestCase(103)]
        [TestCase(111)]
        public void TestImportCell_CellExistedInNeighbors(short pci)
        {
            repository.AddNeighbors(neiRepository.Object, 1);
            Mock<ICell> cell=new Mock<ICell>();
            cell.SetupGet(x => x.CellId).Returns(1);
            cell.SetupGet(x => x.SectorId).Returns(1);
            NearestPciCell pciCell = repository.Import(cell.Object, pci);
            Assert.AreEqual(repository.NearestPciCells.Count, 3);
            Assert.AreEqual(pciCell.Pci, pci);
        }

        [TestCase(1, 1, 112, 2, 2)]
        [TestCase(1, 1, 113, 2, 3)]
        [TestCase(2, 2, 102, 1, 2)]
        [TestCase(2, 1, 113, 2, 3)]
        public void TestImportCell_CanCalculateDistance(int eNodebId, byte sectorId, short pci,
            int neiCellId, byte neiSectorId)
        {
            repository.AddNeighbors(neiRepository.Object, 1);
            Mock<ICell> cell = new Mock<ICell>();
            cell.SetupGet(x => x.CellId).Returns(eNodebId);
            cell.SetupGet(x => x.SectorId).Returns(sectorId);
            NearestPciCell pciCell = repository.Import(cell.Object, pci);
            Assert.AreEqual(repository.NearestPciCells.Count, 4);
            Assert.AreEqual(pciCell.Pci, pci);
            Assert.AreEqual(pciCell.NearestCellId, neiCellId);
            Assert.AreEqual(pciCell.NearestSectorId, neiSectorId);
        }
    }
}
