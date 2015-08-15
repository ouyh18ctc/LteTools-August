using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using Lte.WebApp.Tests.ControllerParameters;
using Moq;
using System.Collections.Generic;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.WebApp.Tests.Parameters
{
    internal class IRegionDefViewModelTestHelper
    {
        private readonly ITownDefViewModel viewModel;
        private readonly IEnumerable<Town> towns;
        private readonly QueryNamesService service;

        public IRegionDefViewModelTestHelper(ITownDefViewModel viewModel, IEnumerable<Town> towns)
        {
            this.viewModel = viewModel;
            this.towns = towns;
            service = new QueryDistinctCityNamesService(towns);
        }

        public void AssertTest()
        {
            viewModel.Initialize(towns);
            Assert.IsNotNull(viewModel.CityList);
            Assert.IsNotNull(viewModel.DistrictList);
            Assert.IsNotNull(viewModel.TownList);
            Assert.AreEqual(viewModel.CityList.Count, service.QueryCount());
            Assert.AreEqual(viewModel.DistrictList.Count, 0);
            Assert.AreEqual(viewModel.TownList.Count, 0);
        }

        public void AssertTest(ITown town)
        {
            viewModel.Initialize(towns, town);
            Assert.IsNotNull(viewModel.CityList);
            Assert.IsNotNull(viewModel.DistrictList);
            Assert.IsNotNull(viewModel.TownList);

            if (town == null)
            {
                Assert.AreEqual(viewModel.CityList.Count, service.QueryCount());
                Assert.AreEqual(viewModel.DistrictList.Count, 0);
                Assert.AreEqual(viewModel.TownList.Count, 0);
            }
            else
            {
                Assert.IsNotNull(viewModel.CityList);
                Assert.IsNotNull(viewModel.DistrictList);
                Assert.IsNotNull(viewModel.TownList);
                viewModel.AssertRegionList(towns, town);
            }
        }

        public void AssertTest(string cityName, string districtName, string townName)
        {
            Mock<ITown> mockTown = new Mock<ITown>();
            mockTown.SetupGet(x => x.CityName).Returns(cityName);
            mockTown.SetupGet(x => x.DistrictName).Returns(districtName);
            mockTown.SetupGet(x => x.TownName).Returns(townName);
            AssertTest(mockTown.Object);
        }
    }

    [TestFixture]
    public class IRegionDefViewModelTest : ParametersConfig
    {
        private readonly Mock<ITownDefViewModel> mockViewModel = new Mock<ITownDefViewModel>();
        private IRegionDefViewModelTestHelper helper;

        [SetUp]
        public void TestInitialize()
        {
            mockViewModel.BindGetAndSetAttributes(x => x.CityList, (x, v) => x.CityList = v);
            mockViewModel.BindGetAndSetAttributes(x => x.DistrictList, (x, v) => x.DistrictList = v);
            mockViewModel.BindGetAndSetAttributes(x => x.TownList, (x, v) => x.TownList = v);
            helper = new IRegionDefViewModelTestHelper(mockViewModel.Object, towns);
        }

        [Test]
        public void IRegionDefViewModel_Initialize_WithoutAssigningTown()
        {
            helper.AssertTest();
        }

        [Test]
        public void IRegionDefViewModel_Initialize_WithAssigningTown_1_2_1()
        {
            helper.AssertTest("City1", "District2", "Town1");
        }

        [Test]
        public void IRegionDefViewModel_Initialize_WithAssigningTown_1_3_5()
        {
            helper.AssertTest("City1", "District3", "Town5");
        }

        [Test]
        public void IRegionDefViewModel_Initialize_WithAssigningTown_2_4_6()
        {
            helper.AssertTest("City2", "District4", "Town6");
        }

        [Test]
        public void IRegionDefViewModel_Initialize_WithAssigningTown_1_2_4()
        {
            helper.AssertTest("City1", "District2", "Town4");
        }
    }
}
