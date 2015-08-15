using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    [TestFixture]
    public class ENodebRepositoryTestExtended : ENodebRepositoryTestConfig
    {
        private ENodebBaseRepository baseRepository;
        private IEnumerable<Town> towns;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            baseRepository = new ENodebBaseRepository(lteRepository.Object);
            towns = townRepository.Object.GetAllList();
        }

        [Test]
        public void TestENodebRepository_ENodebBaseConsidered_SaveENodeb_AddNewOne_TownExists()
        {
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

        [Test]
        public void TestENodebRepository_ENodebBaseConsidered_SaveENodeb_AddNewOne_TownNotExists()
        {
            eNodebInfo.CityName = "Guangzhou";
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

        [Test]
        public void TestENodebRepository_ENodebBaseConsidered_NonUpdate_SameTownAndName_SameId()
        {
            eNodebInfo.Name = "FoshanZhaoming";
            Assert.AreEqual(eNodebInfo.ENodebId, 2);
            eNodebInfo.ENodebId = 1;
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, 1);
        }

        [Test]
        public void TestENodebRepository_ENodebBaseConsidered_NonUpdate_DifferentTownOrName_SameId()
        {
            Assert.AreEqual(eNodebInfo.ENodebId, 2);
            eNodebInfo.ENodebId = 1;
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

    }
}
