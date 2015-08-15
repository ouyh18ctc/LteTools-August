using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using Lte.WebApp.Controllers.Parameters;
using Lte.WebApp.Models;
using Lte.WebApp.Tests.ControllerParameters;
using NUnit.Framework;
using Moq;

namespace Lte.WebApp.Tests.ControllerRegion
{
    internal class TownRepositoryRegionTestHelper
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private readonly RegionController controller;
        private readonly IEnumerable<Town> towns;

        public TownRepositoryRegionTestHelper(IEnumerable<Town> towns)
        {
            controller = new RegionController(townRepository.Object, null, null, null);
            this.towns = towns;
        }

        public void AssertEmptyTowns()
        {
            townRepository.Setup(x => x.GetAll()).Returns((new List<Town>()).AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            ViewResult result = controller.Region();
            Assert.IsNotNull(result, "The view of Region() is null!");
            RegionViewModel viewModel = result.Model as RegionViewModel;
            Assert.IsNotNull(viewModel, "The model of the view is null!");
            Assert.IsNotNull(viewModel.CityList);
            Assert.AreEqual(viewModel.CityList.Count, 0);
            Assert.IsNotNull(viewModel.DistrictList);
            Assert.IsNotNull(viewModel.TownList);
        }

        public void AssertInitialTown(string cityName = null, string districtName = null, string townName = null)
        {
            controller.TempData["RegionViewModel"] =
                (cityName == null && districtName == null && townName == null) ? null :
                new RegionViewModel("")
                {
                    CityName = cityName,
                    DistrictName = districtName,
                    TownName = townName
                };
            RegionViewModel initialViewModel = controller.TempData["RegionViewModel"] as RegionViewModel;
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            ViewResult result = controller.Region();
            RegionViewModel viewModel = result.Model as RegionViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.CityList);
            Assert.IsNotNull(viewModel.DistrictList);
            Assert.IsNotNull(viewModel.TownList);

            AssertNames(initialViewModel, viewModel);
            QueryNamesService namesService = new QueryDistinctCityNamesService(towns);
            Assert.AreEqual(viewModel.CityList.Count, namesService.QueryCount());
            namesService = new QueryDistinctDistrictNamesService(towns, viewModel.CityName);
            Assert.AreEqual(viewModel.DistrictList.Count, namesService.QueryCount());
            namesService = new QueryDistinctTownNamesService(towns, viewModel.CityName, viewModel.DistrictName);
            Assert.AreEqual(viewModel.TownList.Count, namesService.QueryCount());
            AssertSelectedItems(viewModel);
        }

        private void AssertSelectedItems(RegionViewModel viewModel)
        {
            if (towns.FirstOrDefault(x => x.CityName == viewModel.CityName) != null)
            {
                Assert.AreEqual(viewModel.CityList.GetSelectedItemText(), viewModel.CityName);
                Assert.AreEqual(viewModel.CityList.GetSelectedItemValue(), viewModel.CityName);
            }
            else
            {
                Assert.IsNull(viewModel.CityList.GetSelectedItemText());
                Assert.IsNull(viewModel.CityList.GetSelectedItemValue());
            }

            if (towns.FirstOrDefault(x => x.CityName == viewModel.CityName
                && x.DistrictName == viewModel.DistrictName) != null)
            {
                Assert.AreEqual(viewModel.DistrictList.GetSelectedItemText(), viewModel.DistrictName);
                Assert.AreEqual(viewModel.DistrictList.GetSelectedItemValue(), viewModel.DistrictName);
            }
            else
            {
                Assert.IsNull(viewModel.DistrictList.GetSelectedItemText());
                Assert.IsNull(viewModel.DistrictList.GetSelectedItemValue());
            }

            if (towns.Query(viewModel) != null)
            {
                Assert.AreEqual(viewModel.TownList.GetSelectedItemText(), viewModel.TownName);
                Assert.AreEqual(viewModel.TownList.GetSelectedItemValue(), viewModel.TownName);
            }
            else
            {
                Assert.IsNull(viewModel.TownList.GetSelectedItemText());
                Assert.IsNull(viewModel.TownList.GetSelectedItemValue());
            }
        }

        private void AssertNames(RegionViewModel initialViewModel, RegionViewModel viewModel)
        {
            if (initialViewModel == null)
            {
                Assert.AreEqual(viewModel.CityName, towns.ElementAt(0).CityName);
                Assert.AreEqual(viewModel.DistrictName, towns.ElementAt(0).DistrictName);
                Assert.AreEqual(viewModel.TownName, towns.ElementAt(0).TownName);
            }
            else
            {
                Assert.AreEqual(viewModel.CityName, initialViewModel.CityName);
                Assert.AreEqual(viewModel.DistrictName, initialViewModel.DistrictName);
                Assert.AreEqual(viewModel.TownName, initialViewModel.TownName);
            }
        }
    }

    [TestFixture]
    public class TownRepositoryRegionTest : ParametersConfig
    {
        private TownRepositoryRegionTestHelper helper;

        [SetUp]
        public void TestInitialize()
        { 
            helper = new TownRepositoryRegionTestHelper(towns);
        }

        [Test]
        public void TestTownRegion_EmptyTownRepository()
        {
            helper.AssertEmptyTowns();
        }

        [Test]
        public void TestTownRegion_NullRegionViewModel()
        {
            helper.AssertInitialTown();
        }

        [TestCase(1, 2, 4)]
        [TestCase(1, 1, 2)]
        [TestCase(3, 3, 3)]
        [TestCase(1, 4, 2)]
        [TestCase(2, 2, 8)]
        [TestCase(3, 5, 7)]
        [TestCase(4, 2, 3)]
        public void TestTownRegion_NotNullRegionViewModel(int cityId, int districtId, int townId)
        {
            helper.AssertInitialTown("City" + cityId, "District" + districtId, "Town" + townId);
        }
    }
}
