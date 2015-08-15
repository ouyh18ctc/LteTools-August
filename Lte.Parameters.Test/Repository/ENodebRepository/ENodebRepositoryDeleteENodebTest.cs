using System.Linq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    [TestFixture]
    public class ENodebRepositoryDeleteENodebTest : ENodebRepositoryTestConfig
    {
        [Test]
        public void TestENodebRepository_DeleteENodeb_ByENodebId_IdExisted()
        {
            Initialize();
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.IsTrue(DeleteOneENodeb(1));
            Assert.AreEqual(lteRepository.Object.Count(), 0);
        }

        [Test]
        public void TestENodebRepository_DeleteENodeb_ByENodebId_IdInexisted()
        {
            Initialize();
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.IsFalse(DeleteOneENodeb(2));
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }

        [Test]
        public void TestENodebRepository_DeleteENodeb_ByTownAndName()
        {
            Initialize();
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.IsTrue(DeleteOneENodeb("Foshan", "Chancheng", "Qinren", "FoshanZhaoming"));
            Assert.AreEqual(lteRepository.Object.Count(), 0);
        }

        [Test]
        public void TestENodebRepository_DeleteENodeb_ByTownAndName_InvalidENodebName()
        {
            Initialize();
            Assert.AreEqual(lteRepository.Object.Count(), 1);
            Assert.IsFalse(DeleteOneENodeb("Foshan", "Chancheng", "Qinren", "FoshanHuafo"));
            Assert.AreEqual(lteRepository.Object.Count(), 1);
        }
    }
}
