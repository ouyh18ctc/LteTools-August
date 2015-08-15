using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using Lte.WebApp.Controllers.Parameters;
using Lte.WebApp.Tests.ControllerParameters;
using NUnit.Framework;
using Moq;

namespace Lte.WebApp.Tests.ControllerRegion
{
    internal class TownRepositoryWithOptimizeRegionTestHelper
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private readonly Mock<IRegionRepository> regionRepository = new Mock<IRegionRepository>();
        private readonly RegionController controller;
        private IEnumerable<Town> towns;
        private readonly IEnumerable<OptimizeRegion> regions;

        public TownRepositoryWithOptimizeRegionTestHelper(
            IEnumerable<Town> towns, IEnumerable<OptimizeRegion> regions)
        {
            this.towns = towns;
            this.regions = regions;
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            controller = new RegionController(townRepository.Object, null, null, regionRepository.Object);
        }

        public void AssertEmptyRegions()
        {
            regionRepository.Setup(x => x.GetAll()).Returns(
                new List<OptimizeRegion>().AsQueryable());
            ViewResult result = controller.Region();
            Assert.IsNotNull(result, "The view of Region() is null!");
            RegionViewModel viewModel = result.Model as RegionViewModel;
            Assert.IsNotNull(viewModel, "The model of the view is null!");
            Assert.IsNull(viewModel.RegionName);
        }

        public void AssertInitialRegion(string cityName, string districtName, string regionName)
        {
            controller.TempData["RegionViewModel"] =
                new RegionViewModel("")
                {
                    CityName = cityName,
                    DistrictName = districtName,
                    RegionName = regionName
                };
            regionRepository.Setup(x => x.GetAll()).Returns(regions.AsQueryable());
            regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
            regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
            ViewResult result = controller.Region();
            RegionViewModel viewModel = result.Model as RegionViewModel;
            QueryRegionService service = new ByDistrictQueryRegionService(regions,
                cityName, districtName);
            OptimizeRegion region = service.Query();
            if (region != null)
            {
                if (viewModel != null) Assert.AreEqual(viewModel.RegionName, region.Region);
            }
            
        }
    }

    [TestFixture]
    public class TownRepositoryWithOptimizeRegionTest : ParametersConfig
    {
        private TownRepositoryWithOptimizeRegionTestHelper helper;

        [SetUp]
        public void TestInitialize()
        {
            helper = new TownRepositoryWithOptimizeRegionTestHelper(towns, regions);
        }

        [Test]
        public void TestTownRegion_EmptyRegionList()
        {
            helper.AssertEmptyRegions();
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 3, 7)]
        [TestCase(1, 4, 3)]
        [TestCase(1, 5, 23)]
        [TestCase(2, 2, 1)]
        [TestCase(2, 3, 6)]
        [TestCase(2, 4, 5)]
        [TestCase(2, 6, 7)]
        [TestCase(3, 9, 3)]
        [TestCase(4, 21, 1)]
        public void TestTownRegion_InitialRegionName(int cityId, int districtId, int regionId)
        {
            helper.AssertInitialRegion("City" + cityId, "District" + districtId, "Region" + regionId);
        }
    }
}
