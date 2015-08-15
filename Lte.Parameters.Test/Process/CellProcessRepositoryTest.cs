using System.Linq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Process
{
    [TestFixture]
    public class CellProcessRepositoryTest
    {
        private StubCellProcessRepository repository;

        [SetUp]
        public void SetUp()
        {
            repository = new StubCellProcessRepository();
        }

        [Test]
        public void TestCellProcessRepository_BasicParameters()
        {
            Assert.AreEqual(repository.Count(), 1);
            Assert.AreEqual(repository.GetAll().ElementAt(0).ENodebId, 1);
            Assert.AreEqual(repository.GetAll().ElementAt(0).SectorId, 2);
        }

        [Test]
        public void TestCellProcessRepository_CurrentProgress_0()
        {
            Assert.AreEqual(repository.CurrentProgress, 0);
            Assert.AreEqual(repository.SaveCells(null, null), 0);
            Assert.AreEqual(repository.CurrentProgress, 1);
            Assert.AreEqual(repository.SaveCells(null, null), 1);
            Assert.AreEqual(repository.CurrentProgress, 2);
        }

        [Test]
        public void TestCellProcessRepository_CurrentProgress_5()
        {
            Assert.AreEqual(repository.CurrentProgress, 0, "current process");
            repository.AddOneCell(null);
            Assert.AreEqual(repository.SaveCells(null, null), 5, "save cells");
            Assert.AreEqual(repository.CurrentProgress, 6, "current process two");
            repository.AddOneCell(null);
            Assert.AreEqual(repository.SaveCells(null, null), 0, "save cells 2");
            Assert.AreEqual(repository.CurrentProgress, 1, "current process 3");
        }
    }
}
