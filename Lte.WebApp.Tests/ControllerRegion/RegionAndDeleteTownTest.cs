using System.Linq;
using System.Web.Mvc;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.MockOperations;
using Lte.Parameters.Service.Region;
using Lte.WebApp.Controllers.Parameters;
using Lte.WebApp.Tests.ControllerParameters;
using NUnit.Framework;
using Moq;

namespace Lte.WebApp.Tests.ControllerRegion
{
    internal class RegionAndDeleteTownTestHelper
    {
        private readonly RegionController controller;
        private readonly ITownRepository townRepository;
        private QueryNamesService service;

        public RegionAndDeleteTownTestHelper(RegionController controller, ITownRepository townRepository)
        {
            this.controller = controller;
            this.townRepository = townRepository;
            service = new QueryDistinctCityNamesService(townRepository.GetAll());
        }

        public void AssertTest(string cityName = "", string districtName = "", string townName = "")
        {
            ViewResult result = controller.Region();
            RegionViewModel viewModel = result.Model as RegionViewModel;
            Assert.IsNotNull(viewModel);
            if (!string.IsNullOrEmpty(cityName)) { viewModel.CityName = cityName; }
            if (!string.IsNullOrEmpty(districtName)) { viewModel.DistrictName = districtName; }
            if (!string.IsNullOrEmpty(townName)) { viewModel.TownName = townName; }
            controller.DeleteTown(viewModel);
            if (controller.TempData["error"] == null)
            {
                Assert.AreEqual(controller.TempData["success"].ToString(), viewModel.DeleteSuccessMessage);
                result = controller.Region();
                viewModel = result.Model as RegionViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsNotNull(viewModel.CityList);
                Assert.IsNotNull(viewModel.DistrictList);
                Assert.IsNotNull(viewModel.TownList);
                Town town = townRepository.GetAll().First();
                Assert.AreEqual(viewModel.CityName, town.CityName);
                Assert.AreEqual(viewModel.DistrictName, town.DistrictName);
                Assert.AreEqual(viewModel.TownName, town.TownName);
                Assert.AreEqual(viewModel.CityList.Count, service.QueryCount());
                service = new QueryDistinctDistrictNamesService(townRepository.GetAll(), viewModel.CityName);
                Assert.AreEqual(viewModel.DistrictList.Count, service.QueryCount());
                service = new QueryDistinctTownNamesService(townRepository.GetAll(), viewModel.CityName,
                    viewModel.DistrictName);
                Assert.AreEqual(viewModel.TownList.Count, service.QueryCount());
            }
        }
    }

    [TestFixture]
    public class RegionAndDeleteTownTest : ParametersConfig
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private RegionController controller;
        private RegionAndDeleteTownTestHelper helper;

        [SetUp]
        public void TestInitialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            townRepository.MockRemoveOneTownOperation();
            controller = new RegionController(townRepository.Object,null, null, null);
            helper = new RegionAndDeleteTownTestHelper(controller, townRepository.Object);
        }

        [TestCase(1, 2, 3, false)]
        [TestCase(1, 1, 1, true)]
        [TestCase(1, 1, 2, true)]
        [TestCase(1, 1, 2, true)]
        [TestCase(1, 1, 7, false)]
        [TestCase(1, 2, 1, true)]
        [TestCase(1, 2, 4, true)]
        [TestCase(1, 2, 6, false)]
        [TestCase(2, 1, 5, true)]
        [TestCase(2, 3, 2, true)]
        [TestCase(3, 2, 4, false)]
        public void TestMockDeleteTownAction(int cityId, int districtId, int townId, bool success)
        {
            TownOperationService service = new TownOperationService(townRepository.Object, 
                "City" + cityId, "District" + districtId, "Town" + townId);
            if (success)
            {
                Assert.IsTrue(service.DeleteOneTown(null, null));
            }
            else
            {
                Assert.IsFalse(service.DeleteOneTown(null, null));
            }
        }

        [Test]
        public void TestRegion_AddTown_Region_OriginalNamesUnchanged_DeleteTown()
        {
            helper.AssertTest();
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
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestRegion_AddTown_Region_AdjustCityName(int cityId)
        {
            helper.AssertTest("City" + cityId);
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
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestRegion_AddTown_Region_AdjustDistrictName(int districtId)
        {
            helper.AssertTest(districtName: "District" + districtId);
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
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestRegion_AddTown_Region_AdjustTownName_Town2(int townId)
        {
            helper.AssertTest(townName: "Town" + townId);
        }

        [TestCase(1, 2, 3)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 1, 7)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 2, 4)]
        [TestCase(1, 2, 6)]
        [TestCase(2, 1, 5)]
        [TestCase(2, 3, 2)]
        [TestCase(3, 2, 4)]
        public void TestRegion_AddTown_Region_OriginalNamesChanged_DeleteTown(
            int cityId, int districtId, int townId)
        {
            helper.AssertTest("City" + cityId, "District" + districtId, "Town" + townId);
        }
    }
}
