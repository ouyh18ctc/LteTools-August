using Lte.Parameters.MockOperations;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.MockOperations
{
    [TestFixture]
    public class MockSaveENodebTest : MockENodebTestConfig
    {
        [SetUp]
        public void TestInitialize()
        {
            Initialize();
            eNodebRepository.MockENodebRepositorySaveENodeb();
        }

        [Test]
        public void TestMockAddOneENodeb()
        {
            Assert.AreEqual(eNodebRepository.Object.Count(), 7);
            eNodebRepository.Object.Insert(
                new ENodeb { TownId = 5, ENodebId = 11, Name = "E-11" });
            Assert.AreEqual(eNodebRepository.Object.Count(), 8);
        }
    }
}
