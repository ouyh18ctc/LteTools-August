using System.Linq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Process
{
    [TestFixture]
    public class ENodebProcessRepositoryTest
    {
        private StubENodebProcessRepository repository;

        [SetUp]
        public void SetUp()
        {
            repository = new StubENodebProcessRepository();
        }

        [Test]
        public void TestENodebProcessRepository_BasicParameters()
        {
            Assert.AreEqual(repository.ENodebs.Count(), 1);
            Assert.AreEqual(repository.ENodebs.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(repository.ENodebs.ElementAt(0).Name, "aaa");
        }

        [Test]
        public void TestENodebProcessRepository_CurrentProgress_0()
        {
            Assert.AreEqual(repository.CurrentProgress, 0);
            Assert.AreEqual(repository.SaveENodebs(null, null), 0);
            Assert.AreEqual(repository.CurrentProgress, 1);
            Assert.AreEqual(repository.SaveENodebs(null, null), 1);
            Assert.AreEqual(repository.CurrentProgress, 2);
        }

        [Test]
        public void TestENodebProcessRepository_CurrentProgress_10()
        {
            Assert.AreEqual(repository.CurrentProgress, 0);
            repository.AddOneENodeb(null);
            Assert.AreEqual(repository.SaveENodebs(null, null), 10);
            Assert.AreEqual(repository.CurrentProgress, 11);
            Assert.AreEqual(repository.SaveENodebs(null, null), 0);
            Assert.AreEqual(repository.CurrentProgress, 1);
        }
    }
}
