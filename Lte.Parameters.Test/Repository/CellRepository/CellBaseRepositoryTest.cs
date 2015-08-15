using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.CellRepository
{
    [TestFixture]
    public class CellBaseRepositoryTest : CellRepositoryTestConfig
    {
        private CellBaseRepository baseRepository;

        [SetUp]
        public void TestInitialize()
        {
            Initialize();
            baseRepository = new CellBaseRepository(repository.Object);
        }

        [Test]
        public void TestCellBaseRepository_QueryCell()
        {
            CellBase cell = baseRepository.QueryCell(1, 0);
            Assert.IsNotNull(cell);
            Assert.AreEqual(cell.SectorId, 0);
        }

        [Test]
        public void TestCellBaseRepository_ImportNewCellInfo_ExistedCellInfo()
        {
            Assert.AreEqual(baseRepository.CellBaseList.Count, 1);
            baseRepository.ImportNewCellInfo(new CellExcel
            {
                ENodebId = 1,
                SectorId = 0,
                Azimuth = 33
            });
            Assert.AreEqual(baseRepository.CellBaseList.Count, 1);
        }

        [Test]
        public void TestCellBaseRepository_ImportNewCellInfo_NewCellInfo()
        {
            Assert.AreEqual(baseRepository.CellBaseList.Count, 1);
            baseRepository.ImportNewCellInfo(new CellExcel
            {
                ENodebId = 1,
                SectorId = 1,
                Azimuth = 36
            });
            Assert.AreEqual(baseRepository.CellBaseList.Count, 2);
        }
    }
}
