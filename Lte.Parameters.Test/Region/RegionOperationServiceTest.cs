using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Region
{
    internal class RegionOperationServiceTestHelper
    {
        private RegionOperationService service;
        private readonly IRegionRepository repository;

        public RegionOperationServiceTestHelper(IRegionRepository regionRepository)
        {
            repository = regionRepository;
        }

        public bool TestAddRegion(int cityId, int districtId, int regionId)
        {
            service = new RegionOperationService(repository,
                "C-" + cityId, "D-" + districtId, "R-" + regionId);
            return service.SaveOneRegion();
        }

        public bool TestAddRegionForce(int cityId, int districtId, int regionId)
        {
            service = new RegionOperationService(repository,
                "C-" + cityId, "D-" + districtId, "R-" + regionId);
            return service.SaveOneRegion(true);
        }

        public bool TestDeleteRegion(int cityId, int districtId, int regionId)
        {
            service = new RegionOperationService(repository,
                "C-" + cityId, "D-" + districtId, "R-" + regionId);
            return service.DeleteOneRegion();
        }
    }

    [TestFixture]
    public class RegionOperationServiceTest : RegionTestConfig
    {
        private RegionOperationServiceTestHelper helper;

        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [TestCase(1, 1, 1, false, false)]
        [TestCase(2, 3, 7, false, false)]
        [TestCase(3, 5, 6, false, false)]
        [TestCase(2, 6, 7, true, true)]
        [TestCase(1, 3, 9, true, true)]
        [TestCase(3, 6, 9, false, false)]
        [TestCase(4, 2, 1, true, true)]
        public void TestAdd(int cityId, int districtId, int regionId, bool success, bool increase)
        {
            helper = new RegionOperationServiceTestHelper(regionRepository.Object);
            Assert.AreEqual(regionRepository.Object.Count(), 8);
            if (success)
            {
                Assert.IsTrue(helper.TestAddRegion(cityId, districtId, regionId));
            }
            else
            {
                Assert.IsFalse(helper.TestAddRegion(cityId, districtId, regionId));
            }
            Assert.AreEqual(regionRepository.Object.Count(), 8 + (increase ? 1 : 0));
        }

        [TestCase(1, 1, 1, false, false)]
        [TestCase(2, 3, 7, true, false)]
        [TestCase(3, 5, 6, false, false)]
        [TestCase(2, 6, 7, true, true)]
        [TestCase(1, 3, 9, true, true)]
        [TestCase(3, 6, 9, true, false)]
        [TestCase(4, 2, 1, true, true)]
        public void TestAddForce(int cityId, int districtId, int regionId, bool success, bool increase)
        {
            helper = new RegionOperationServiceTestHelper(regionRepository.Object);
            Assert.AreEqual(regionRepository.Object.Count(), 8);
            if (success)
            {
                Assert.IsTrue(helper.TestAddRegionForce(cityId, districtId, regionId));
            }
            else
            {
                Assert.IsFalse(helper.TestAddRegionForce(cityId, districtId, regionId));
            }
            Assert.AreEqual(regionRepository.Object.Count(), 8 + (increase ? 1 : 0));
        }

        [TestCase(1, 2, 2, true)]
        [TestCase(1, 3, 9, false)]
        [TestCase(2, 4, 5, true)]
        [TestCase(2, 3, 4, true)]
        [TestCase(3, 6, 9, false)]
        [TestCase(3, 7, 8, true)]
        public void TestDelete(int cityId, int districtId, int regionId, bool success)
        {
            helper = new RegionOperationServiceTestHelper(regionRepository.Object);
            Assert.AreEqual(regionRepository.Object.Count(), 8);
            if (success)
            {
                Assert.IsTrue(helper.TestDeleteRegion(cityId, districtId, regionId));
                Assert.AreEqual(regionRepository.Object.Count(), 7);
            }
            else
            {
                Assert.IsFalse(helper.TestDeleteRegion(cityId, districtId, regionId));
                Assert.AreEqual(regionRepository.Object.Count(), 8);
            }
        }
    }
}
