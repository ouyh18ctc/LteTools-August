using System.Linq;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    [TestFixture]
    public class ENodebRepositoryQueryTest : ENodebRepositoryTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestENodebRepository_QueryENodebById()
        {
            ENodeb eNodeb = lteRepository.Object.GetAll().FirstOrDefault(x => x.ENodebId == 1);
            Assert.IsNotNull(eNodeb);
            Assert.AreEqual(eNodeb.Name, "FoshanZhaoming");
            Assert.AreEqual(eNodeb.TownId, 122);
            Assert.AreEqual(eNodeb.GatewayIp.AddressString, "10.17.165.100");
            Assert.AreEqual(eNodeb.Ip.AddressString, "10.17.165.23");
            Assert.IsTrue(eNodeb.IsFdd);
            Assert.AreEqual(eNodeb.Longtitute, 112.9987);
            Assert.AreEqual(eNodeb.Lattitute, 23.1233);
        }

        [Test]
        public void TestENodebRepository_QueryENodebByTownIdAndName()
        {
            ENodeb eNodeb =
                lteRepository.Object.GetAll().FirstOrDefault(x => x.TownId == 122 && x.Name == "FoshanZhaoming");
            Assert.IsNotNull(eNodeb);
            Assert.AreEqual(eNodeb.ENodebId, 1);
            Assert.AreEqual(eNodeb.GatewayIp.AddressString, "10.17.165.100");
            Assert.AreEqual(eNodeb.Ip.AddressString, "10.17.165.23");
            Assert.IsTrue(eNodeb.IsFdd);
            Assert.AreEqual(eNodeb.Longtitute, 112.9987);
            Assert.AreEqual(eNodeb.Lattitute, 23.1233);
        }

    }
}
