using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Region
{
    internal class QueryRegionNamesServiceTestHelper
    {
        private QueryNamesService service;
        private readonly IRegionRepository repository;

        public QueryRegionNamesServiceTestHelper(IRegionRepository regionRepository)
        {
            repository = regionRepository;
        }

        public int ConstructRegions(int cityId, int districtId)
        {
            service = new ByCityDistrictQueryRegionCityNamesService(
                repository.GetAll(), "C-" + cityId, "D-" + districtId);
            return service.QueryCount();
        }

        public int ConstructTestCities()
        {
            service = new QueryRegionCityNamesService(repository.GetAll());
            return service.QueryCount();
        }

        public int ConstructTestRegions()
        {
            service = new QueryOptimizeRegionNamesService(repository.GetAll());
            return service.QueryCount();
        }
    }

    [TestFixture]
    public class QueryRegionNamesServiceTest : RegionTestConfig
    {
        private QueryRegionNamesServiceTestHelper helper;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            helper = new QueryRegionNamesServiceTestHelper(regionRepository.Object);
        }

        [Test]
        public void TestCityCount_Expected3()
        {
            int count = helper.ConstructTestCities();
            Assert.AreEqual(count, 3);
        }

        [Test]
        public void TestRegionCount_Expected8()
        {
            int count = helper.ConstructTestRegions();
            Assert.AreEqual(count, 8);
        }

        [TestCase(1, 2, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 2, 1)]
        [TestCase(2, 3, 1)]
        [TestCase(3, 5, 1)]
        public void TestTownCount(int cityId, int districtId, int expectedCount)
        {
            int count = helper.ConstructRegions(1, 2);
            Assert.AreEqual(count, 1);
        }
    }
}
