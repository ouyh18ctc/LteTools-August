using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Concrete;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.CellRepository
{
    [TestFixture]
    public class CellRepositoryTestExtended : CellRepositoryTestConfig
    {
        [Test]
        public void TestCellRepository_CellBaseConsidered_SaveCell_ENodebExist_CellNotExist()
        {
            Initialize();
            CellBaseRepository baseRepository = new CellBaseRepository(repository.Object);
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsTrue(SaveOneCell(baseRepository));
            Assert.AreEqual(repository.Object.Count(), 2);
            Assert.IsTrue(repository.Object.GetAll().ElementAt(1).IsOutdoor);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).AntennaPorts, AntennaPortsConfigure.Antenna2T4R);
        }

        [Test]
        public void TestCellRepository_CellBaseConsidered_SaveCell_ENodebNotExist()
        {
            Initialize();
            cellInfo.ENodebId = 2;
            CellBaseRepository baseRepository = new CellBaseRepository(repository.Object);
            Assert.IsFalse(SaveOneCell(baseRepository));
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestCellRepository_CellBaseConsidered_SaveCell_ENodebExist_CellExist()
        {
            Initialize();
            cellInfo.SectorId = 0;
            CellBaseRepository baseRepository = new CellBaseRepository(repository.Object);
            Assert.IsFalse(SaveOneCell(baseRepository));
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestCellRepository_CellBaseConsidered_SaveCell_ENodebExist_CellExist_UpdateExisted()
        {
            Initialize();
            cellInfo.SectorId = 0;
            CellBaseRepository baseRepository = new CellBaseRepository(repository.Object);
            Assert.IsTrue(SaveOneCell(baseRepository, true));
            Assert.AreEqual(repository.Object.Count(), 1);
        }

    }
}
