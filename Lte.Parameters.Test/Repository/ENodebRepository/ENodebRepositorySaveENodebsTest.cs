using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    [TestFixture]
    public class ENodebRepositorySaveENodebsTest : ENodebRepositoryTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
            townRepository.Setup(x => x.GetAll()).Returns(new List<Town> {
                new Town()
                {
                    CityName = "Foshan",
                    DistrictName = "Chancheng",
                    TownName = "Qinren",
                    Id = 122
                }
            }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
        }

        [TestCase(true, 2, 3, 122)]
        [TestCase(false, 2, 3, 122)]
        public void TestENodebRepositorySaveENodebs_Original(bool update, int saveResults, int resultCounts, int townId)
        {
            Assert.AreEqual(eNodebInfos[0].ENodebId,4);
            Assert.AreEqual(eNodebInfos[1].ENodebId,3);
            Assert.AreEqual(lteRepository.Object.Count(), 1, "lte counts");
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, 1);
            Assert.AreEqual(SaveENodebs(update), saveResults);
            Assert.AreEqual(lteRepository.Object.Count(), resultCounts);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(1).TownId, townId);
        }

        [TestCase(true, 2, 3, -1)]
        [TestCase(false, 2, 3, -1)]
        public void TestENodebRepositorySaveENodebs_ModifyFirstItem_TownNotExists(
            bool update, int saveResults, int resultCounts, int townId)
        {
            eNodebInfos[0].CityName = "Guangzhou";
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.AreEqual(SaveENodebs(update), saveResults);
            Assert.AreEqual(lteRepository.Object.Count(), resultCounts);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(1).TownId, townId);
        }

        [TestCase(true, 1, 2, "10.17.165.121", 1)]
        [TestCase(false, 1, 2, "10.17.165.23", 1)]
        public void TestENodebRepositorySaveENodebs_ModifyFirstItem_SameTownAndName_SameId(
            bool update, int saveResults, int resultCounts, string ipAddress, int eNodebId)
        {
            eNodebInfos[0].Name = "FoshanZhaoming";
            Assert.AreEqual(eNodebInfos[0].ENodebId, 4);
            eNodebInfos[0].ENodebId = 1;
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, 1);
            Assert.AreEqual(SaveENodebs(update), saveResults);
            Assert.AreEqual(lteRepository.Object.Count(), resultCounts);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).Ip.AddressString, ipAddress);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, eNodebId);
        }

        [TestCase(true, 1, 2, "10.17.165.121", 4)]
        [TestCase(false, 1, 2, "10.17.165.23", 1)]
        public void TestENodebRepositorySaveENodebs_ModifyFirstItem_SameTownAndName_DifferentId(
            bool update, int saveResults, int resultCounts, string ipAddress, int eNodebId)
        {
            eNodebInfos[0].Name = "FoshanZhaoming";
            Assert.AreEqual(eNodebInfos[0].ENodebId, 4);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, 1);
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.AreEqual(SaveENodebs(update), saveResults, "save Results");
            Assert.AreEqual(lteRepository.Object.Count(), resultCounts);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).Ip.AddressString, ipAddress);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, eNodebId, "eNodebId");
        }

        [TestCase(true, 1, 2, "10.17.165.121", 1)]
        [TestCase(false, 1, 2, "10.17.165.23", 1)]
        public void TestENodebRepositorySaveENodebs_ModifyFirstItem_DifferentTownOrName_SameId(
            bool update, int saveResults, int resultCounts, string ipAddress, int eNodebId)
        {
            Assert.AreEqual(eNodebInfos[0].ENodebId, 4);
            eNodebInfos[0].ENodebId = 1;
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.AreEqual(SaveENodebs(update), saveResults);
            Assert.AreEqual(lteRepository.Object.Count(), resultCounts);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).Ip.AddressString, ipAddress);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, eNodebId);
        }
    }
}
