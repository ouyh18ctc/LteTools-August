using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;
using Lte.Parameters.MockOperations;
using Lte.Parameters.Service.Cdma;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.BtsRepository
{
    [TestFixture]
    public class BtsRepositoryTest : BtsRepositoryTestConfig
    {
        private BtsExcel btsInfo;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            townRepository.Setup(x => x.GetAll()).Returns(new List<Town> 
            {
                new Town
                {
                    CityName = "Foshan",
                    DistrictName = "Chancheng",
                    TownName = "Qinren",
                    Id = 122
                }
            }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            btsInfo = new BtsExcel
            {
                BtsId = 2,
                Name = "First eNodeb",
                DistrictName = "Chancheng",
                TownName = "Qinren",
                Longtitute = 112.3344,
                Lattitute = 23.5566
            };
            repository.MockBtsRepositoryDeleteBts();
        }

        [Test]
        public void TestBtsRepository_QueryBtsById()
        {
            CdmaBts bts = repository.Object.GetAll().FirstOrDefault(x => x.BtsId == 1);
            Assert.IsNotNull(bts);
            Assert.AreEqual(bts.Name, "FoshanZhaoming");
            Assert.AreEqual(bts.TownId, 122);
            Assert.AreEqual(bts.Longtitute, 112.9987);
            Assert.AreEqual(bts.Lattitute, 23.1233);
        }

        [Test]
        public void TestBtsRepository_QueryBtsByTownIdAndName()
        {
            CdmaBts bts = repository.Object.QueryBts(122, "FoshanZhaoming");
            Assert.IsNotNull(bts);
            Assert.AreEqual(bts.BtsId, 1);
            Assert.AreEqual(bts.Longtitute, 112.9987);
            Assert.AreEqual(bts.Lattitute, 23.1233);
        }

        [Test]
        public void TestBtsRepository_SaveBts_AddNewOne_TownExists()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsTrue(SaveOneBts(btsInfo));
            Assert.AreEqual(repository.Object.Count(), 2);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).TownId, 122);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).Longtitute, 112.3344);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).Lattitute, 23.5566);
        }

        [Test]
        public void TestBtsRepository_SaveBts_AddNewOne_TownExists_UpdateLteInfo()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsTrue(SaveOneBts(btsInfo));
            Assert.AreEqual(repository.Object.Count(), 2);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).TownId, 122);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).Longtitute, 112.3344);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).Lattitute, 23.5566);
        }

        [Test]
        public void TestBtsRepository_SaveBts_AddNewOne_TownNotExists()
        {
            btsInfo.DistrictName = "Guangzhou";
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsTrue(SaveOneBts(btsInfo));
            Assert.AreEqual(repository.Object.Count(), 2);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).TownId, -1);
        }

        [Test]
        public void TestBtsRepository_Update_SameTownAndName_SameId()
        {
            btsInfo.Name = "FoshanZhaoming";
            Assert.AreEqual(btsInfo.BtsId, 2, "Wrong Id");
            btsInfo.BtsId = 1;
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(0).BtsId, 1);
            Assert.IsFalse(SaveOneBts(btsInfo));
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(0).BtsId, 1);
        }

        [Test]
        public void TestBtsRepository_Update_SameTownAndName_DifferentId()
        {
            btsInfo.Name = "FoshanZhaoming";
            Assert.AreEqual(btsInfo.BtsId, 2);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(0).BtsId, 1);
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsFalse(SaveOneBts(btsInfo));
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(0).BtsId, 1);
        }

        [Test]
        public void TestBtsRepository_Update_DifferentTownOrName_SameId()
        {
            Assert.AreEqual(btsInfo.BtsId, 2);
            btsInfo.BtsId = 1;
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsFalse(SaveOneBts(btsInfo));
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestBtsRepository_DeleteBts_ByBtsId()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            IEnumerable<CdmaBts> btss = repository.Object.GetAll();
            Assert.AreEqual(btss.Count(), 1);
            CdmaBts bts = repository.Object.GetAll().FirstOrDefault(x => x.BtsId == 1);
            Assert.IsNotNull(bts);
            Assert.IsTrue(repository.Object.DeleteOneBts(1), "Delete Failed");
            Assert.AreEqual(repository.Object.Count(), 0);
            bts = repository.Object.GetAll().FirstOrDefault(x => x.BtsId == 1);
            Assert.IsNull(bts);
            Assert.IsFalse(repository.Object.DeleteOneBts(1), "Delete Success");
        }

        [Test]
        public void TestBtsRepository_DeleteBts_ByTownAndName()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.IsTrue(repository.Object.DeleteOneBts(townRepository.Object, "Chancheng", "Qinren", "FoshanZhaoming"));
            Assert.AreEqual(repository.Object.Count(), 0);
            Assert.IsFalse(repository.Object.DeleteOneBts(townRepository.Object, "Chancheng", "Qinren", "FoshanZhaoming"));
        }
    }
}
