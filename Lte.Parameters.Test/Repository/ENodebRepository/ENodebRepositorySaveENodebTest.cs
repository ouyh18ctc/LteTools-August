using System.Linq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    [TestFixture]
    public class ENodebRepositorySaveENodebTest : ENodebRepositoryTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestENodebRepository_SaveENodeb_AddNewOne_TownExists()
        {
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

        [Test]
        public void TestENodebRepository_SaveENodeb_AddNewOne_TownNotExists()
        {
            eNodebInfo.CityName = "Guangzhou";
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

        [Test]
        public void TestENodebRepository_Update_SameTownAndName_SameId()
        {
            eNodebInfo.Name = "FoshanZhaoming";
            Assert.AreEqual(eNodebInfo.ENodebId, 2);
            eNodebInfo.ENodebId = 1;
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.AreEqual(lteRepository.Object.GetAll().ElementAt(0).ENodebId, 1);
        }

        [Test]
        public void TestENodebRepository_Update_DifferentTownOrName_SameId()
        {
            Assert.AreEqual(eNodebInfo.ENodebId, 2);
            eNodebInfo.ENodebId = 1;
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

    }
}
