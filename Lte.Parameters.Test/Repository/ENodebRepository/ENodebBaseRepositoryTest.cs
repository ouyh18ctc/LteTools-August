using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    [TestFixture]
    public class ENodebBaseRepositoryTest : ENodebRepositoryTestConfig
    {
        private ENodebBaseRepository baseRepository;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            baseRepository = new ENodebBaseRepository(lteRepository.Object);
        }

        [Test]
        public void TestENodebBaseRepository_QueryENodebById()
        {
            ENodebBase eNodeb = baseRepository.QueryENodeb(1);
            Assert.IsNotNull(eNodeb);
            Assert.AreEqual(eNodeb.Name, "FoshanZhaoming");
            Assert.AreEqual(eNodeb.TownId, 122);
        }

        [Test]
        public void TestENodebBaseRepository_QueryENodebByTownIdAndName()
        {
            ENodebBase eNodeb = baseRepository.QueryENodeb(122, "FoshanZhaoming");
            Assert.IsNotNull(eNodeb);
            Assert.AreEqual(eNodeb.ENodebId, 1);
        }
    }
}
