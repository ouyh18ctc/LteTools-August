using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.CellRepository
{
    [TestFixture]
    public class CellRepositoryTest : CellRepositoryTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
            eNodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb> 
            {
                new ENodeb()
                {
                    ENodebId = 1,
                    Name = "FoshanHuafo",
                    Longtitute = 112.3344,
                    Lattitute = 22.7788
                }
            }.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());
        }

        [Test]
        public void TestCellRepository_QueryCell()
        {
            Assert.IsNull(QueryCell(1, 1));
            Assert.IsNotNull(QueryCell(1, 0));
        }

        [Test]
        public void TestCellRepository_SaveCell_ENodebExist_CellNotExist()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsTrue(SaveOneCell());
            Assert.AreEqual(repository.Object.Count(), 2);
            Assert.IsTrue(repository.Object.GetAll().ElementAt(1).IsOutdoor);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).AntennaPorts, AntennaPortsConfigure.Antenna2T4R);
        }

        [Test]
        public void TestCellRepository_SaveCell_ENodebNotExist()
        {
            cellInfo.ENodebId = 2;
            Assert.IsFalse(SaveOneCell());
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestCellRepository_SaveCell_ENodebExist_CellExist()
        {
            cellInfo.SectorId = 0;
            Assert.IsTrue(SaveOneCell());
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestCellRepository_DeleteCell_CellExist()
        {
            Assert.IsTrue(DeleteOneCell(1, 0));
            Assert.AreEqual(repository.Object.Count(), 0);
        }

        [Test]
        public void TestCellRepository_DeleteCell_CellNotExist()
        {
            Assert.IsFalse(DeleteOneCell(1, 1));
            Assert.AreEqual(repository.Object.Count(), 1);
        }
    }
}
