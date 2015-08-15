using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Region
{
    internal class QueryDistinctNamesServiceTestHelper
    {
        private QueryNamesService service;
        private readonly ITownRepository repository;

        public QueryDistinctNamesServiceTestHelper(ITownRepository townRepository)
        {
            repository = townRepository;
        }

        public int ConstructTestId(int cityId, int districtId)
        {
            service = new QueryDistinctTownNamesService(repository.GetAll(), "C-" + cityId, "D-" + districtId);
            return service.QueryCount();
        }

        public int ConstructTestId(int cityId)
        {
            service = new QueryDistinctDistrictNamesService(repository.GetAll(), "C-" + cityId);
            return service.QueryCount();
        }

        public int ConstructTestId()
        {
            service = new QueryDistinctCityNamesService(repository.GetAll());
            return service.QueryCount();
        }
    }

    [TestFixture]
    public class QueryDistinctNamesServiceTest : RegionTestConfig
    {
        private QueryDistinctNamesServiceTestHelper helper;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            helper = new QueryDistinctNamesServiceTestHelper(townRepository.Object);
        }

        [Test]
        public void TestCityCount_Expected3()
        {
            int count = helper.ConstructTestId();
            Assert.AreEqual(count, 3);
        }

        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        public void TestDistrictCount(int cityId, int expectedCount)
        {
            int count = helper.ConstructTestId(cityId);
            Assert.AreEqual(count, expectedCount);
        }

        [TestCase(1, 2, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 3, 1)]
        [TestCase(2, 4, 2)]
        [TestCase(3, 5, 2)]
        [TestCase(3, 6, 1)]
        public void TestTownCount(int cityId, int districtId, int expectedCount)
        {
            int count = helper.ConstructTestId(cityId, districtId);
            Assert.AreEqual(count, expectedCount);
        }
    }
}
