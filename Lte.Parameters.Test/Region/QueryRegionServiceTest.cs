using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Region
{
    internal class QueryRegionServiceTestHelper
    {
        private QueryRegionService service;
        private readonly IRegionRepository repository;

        public QueryRegionServiceTestHelper(IRegionRepository regionRepository)
        {
            repository = regionRepository;
        }

        public OptimizeRegion ConstructTestRegion(int cityId, int districtId)
        {
            service = new ByDistrictQueryRegionService(repository.GetAll(),
                "C-" + cityId, "D-" + districtId);
            return service.Query();
        }

        public OptimizeRegion ConstructTestRegion(int cityId, int districtId, int regionId)
        {
            service = new ByRegionQueryRegionService(repository.GetAll(),
                "C-" + cityId, "D-" + districtId, "R-" + regionId);
            return service.Query();
        }
    }

    [TestFixture]
    public class QueryRegionServiceTest : RegionTestConfig
    {
        private QueryRegionServiceTestHelper helper;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            helper = new QueryRegionServiceTestHelper(regionRepository.Object);
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 3, -1)]
        [TestCase(2, 2, 3)]
        [TestCase(2, 3, 4)]
        [TestCase(2, 4, 5)]
        [TestCase(2, 6, -1)]
        [TestCase(3, 5, 6)]
        [TestCase(3, 6, 7)]
        [TestCase(3, 7, 8)]
        [TestCase(3, 8, -1)]
        [TestCase(4, 1, -1)]
        [TestCase(5, 2, -1)]
        public void TestByCityDistrict(int cityId, int districtId, int regionId)
        {
            OptimizeRegion region = helper.ConstructTestRegion(cityId, districtId);
            if (regionId == -1)
            {
                Assert.IsNull(region);
            }
            else
            {
                Assert.IsNotNull(region);
                Assert.AreEqual(region.Id, regionId);
            }
        }

        [TestCase(1, 1, 1, true)]
        [TestCase(1, 2, 2, true)]
        [TestCase(1, 2, 3, false)]
        [TestCase(2, 2, 3, true)]
        [TestCase(2, 3, 4, true)]
        [TestCase(2, 4, 4, false)]
        [TestCase(2, 4, 5, true)]
        [TestCase(2, 5, 1, false)]
        [TestCase(3, 5, 6, true)]
        [TestCase(3, 5, 7, false)]
        [TestCase(3, 6, 7, true)]
        [TestCase(3, 7, 8, true)]
        [TestCase(3, 9, 2, false)]
        [TestCase(4, 3, 3, false)]
        public void TestByCityDistrictRegion(int cityId, int districtId, int regionId, bool existed)
        {
            OptimizeRegion region = helper.ConstructTestRegion(cityId, districtId, regionId);
            if (existed)
            {
                Assert.IsNotNull(region);
            }
            else
            {
                Assert.IsNull(region);
            }
        }
    }
}
