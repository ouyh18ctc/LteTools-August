using System.Linq;
using System.Web.Mvc;
using Lte.Domain.Geo.Abstract;
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
    internal class RegionAndAddTownTestHelper
    {
        private readonly RegionController controller;
        private readonly ITownRepository townRepository;
        private readonly QueryNamesService cityService;
        private QueryNamesService districtService;
        private QueryNamesService townService;

        public RegionAndAddTownTestHelper(RegionController controller, ITownRepository townRepository)
        {
            this.controller = controller;
            this.townRepository = townRepository;
            cityService = new QueryDistinctCityNamesService(townRepository.GetAll());
        }

        public void AssertTest(int cityId = 0, int districtId = 0, int townId = 0,
            int newCityId = 0, int newDistrictId = 0, int newTownId = 0)
        {
            ViewResult result = controller.Region();
            RegionViewModel viewModel = result.Model as RegionViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(viewModel.CityName, townRepository.GetAll().First().CityName);
            Assert.AreEqual(viewModel.DistrictName, townRepository.GetAll().First().DistrictName);
            Assert.AreEqual(viewModel.TownName, townRepository.GetAll().First().TownName);

            UpdateServices(viewModel);
            Assert.AreEqual(viewModel.CityList.Count, cityService.QueryCount());
            Assert.AreEqual(viewModel.DistrictList.Count, districtService.QueryCount());
            Assert.AreEqual(viewModel.TownList.Count, townService.QueryCount());

            Town addConditions 
                = SetAddConditions(cityId, districtId, townId, newCityId, newDistrictId, newTownId, viewModel);

            controller.AddTown(viewModel);

            if (addConditions.IsAddConditionsValid())
            {
                Assert.IsNull(controller.TempData["error"]);
                Assert.AreEqual(controller.TempData["success"].ToString(),
                    "增加镇街:" + addConditions.GetAddConditionsInfo() + "成功");
                result = controller.Region();
                viewModel = result.Model as RegionViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsNotNull(viewModel.CityList);
                Assert.IsNotNull(viewModel.DistrictList);
                Assert.IsNotNull(viewModel.TownList);
                Assert.AreEqual(viewModel.CityName, addConditions.CityName);
                Assert.AreEqual(viewModel.DistrictName, addConditions.DistrictName);
                Assert.AreEqual(viewModel.TownName, addConditions.TownName);

                UpdateServices(viewModel);
                Assert.AreEqual(viewModel.CityList.Count, cityService.QueryCount());
                Assert.AreEqual(viewModel.DistrictList.Count, districtService.QueryCount());
                Assert.AreEqual(viewModel.TownList.Count, townService.QueryCount());
            }
            else
            {
                Assert.AreEqual(controller.TempData["error"].ToString(),
                    "输入有误！城市、区域、镇区都不能为空。");
            }
        }

        private void UpdateServices(RegionViewModel viewModel)
        {
            districtService = new QueryDistinctDistrictNamesService(townRepository.GetAll(), viewModel.CityName);
            townService = new QueryDistinctTownNamesService(townRepository.GetAll(),
                viewModel.CityName, viewModel.DistrictName);
        }

        private static Town SetAddConditions(int cityId, int districtId, int townId, int newCityId, 
            int newDistrictId, int newTownId, RegionViewModel viewModel)
        {
            if (cityId > 0) { viewModel.CityName = "City" + cityId; }
            if (districtId > 0) { viewModel.DistrictName = "District" + districtId; }
            if (townId > 0) { viewModel.TownName = "Town" + townId; }
            if (newCityId > 0) { viewModel.NewCityName = "City" + newCityId; }
            if (newDistrictId > 0) { viewModel.NewDistrictName = "District" + newDistrictId; }
            if (newTownId > 0) { viewModel.NewTownName = "Town" + newTownId; }
            Town addConditions = viewModel.AddTownConditions;
            return addConditions;
        }
    }

    [TestFixture]
    public class RegionAndAddTownTest : ParametersConfig
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private RegionController controller;
        private RegionAndAddTownTestHelper helper;

        [SetUp]
        public void TestInitialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            controller = new RegionController(townRepository.Object, null, null, null);
            helper = new RegionAndAddTownTestHelper(controller, townRepository.Object);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(500)]
        [TestCase(1000)]
        [TestCase(2000)]
        [TestCase(5000)]
        [TestCase(10000)]
        [TestCase(20000)]
        public void TestRegion_AddTown_Region_OriginalNamesUnchanged_AddNewTown(int townId)
        {
            helper.AssertTest(newTownId: townId);
        }

        [TestCase(2, 1, 5, 1)]
        [TestCase(2, 1, 5, 2)]
        [TestCase(2, 1, 5, 3)]
        [TestCase(2, 1, 5, 4)]
        [TestCase(2, 1, 5, 5)]
        [TestCase(2, 1, 5, 6)]
        [TestCase(2, 1, 5, 7)]
        [TestCase(2, 1, 5, 8)]
        [TestCase(2, 1, 5, 9)]
        [TestCase(2, 1, 5, 10)]
        [TestCase(2, 1, 5, 20)]
        [TestCase(2, 1, 5, 50)]
        [TestCase(2, 1, 5, 100)]
        [TestCase(2, 1, 5, 200)]
        [TestCase(2, 1, 5, 500)]
        [TestCase(2, 1, 5, 1000)]
        [TestCase(2, 1, 5, 2000)]
        [TestCase(2, 1, 5, 5000)]
        [TestCase(2, 1, 5, 10000)]
        [TestCase(2, 1, 5, 20000)]
        public void TestRegion_AddTown_Region_OriginalNamesChanged_AddNewTown(
            int cityId, int districtId, int townId, int newTownId)
        {
            helper.AssertTest(cityId, districtId, townId, newTownId: newTownId);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(500)]
        [TestCase(1000)]
        [TestCase(2000)]
        [TestCase(5000)]
        [TestCase(10000)]
        [TestCase(20000)]
        public void TestRegion_AddTown_Region_OriginalNamesUnchanged_AddNewDistrict_EmptyTownName(int districtId)
        {
            helper.AssertTest(newDistrictId: districtId);
        }

        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 2)]
        [TestCase(5, 2)]
        [TestCase(6, 2)]
        [TestCase(7, 2)]
        [TestCase(8, 2)]
        [TestCase(9, 2)]
        [TestCase(10, 2)]
        [TestCase(20, 2)]
        [TestCase(50, 2)]
        [TestCase(64, 202)]
        [TestCase(100, 2)]
        [TestCase(200, 2)]
        [TestCase(500, 2)]
        [TestCase(603, 32)]
        [TestCase(603, 38)]
        [TestCase(603, 47)]
        [TestCase(603, 56)]
        [TestCase(603, 67)]
        [TestCase(603, 79)]
        [TestCase(603, 112)]
        [TestCase(603, 232)]
        [TestCase(603, 321)]
        [TestCase(1000, 2)]
        [TestCase(2000, 2)]
        [TestCase(5000, 2)]
        [TestCase(10000, 2)]
        [TestCase(20000, 2)]
        public void TestRegion_AddTown_Region_OriginalNamesUnchanged_AddNewDistrict_NewTownName(
            int districtId, int townId)
        {
            helper.AssertTest(newDistrictId: districtId, newTownId: townId);
        }
    }
}
