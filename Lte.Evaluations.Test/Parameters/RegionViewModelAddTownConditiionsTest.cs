using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Parameters
{
    [TestFixture]
    public class RegionViewModelAddTownConditionsTest
    {
        [Test]
        public void TestRegionViewModelAddTownConditions_EmptyNewNames()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "",
                DistrictName = "Chancheng",
                NewDistrictName = "",
                TownName = "Nanzhuang",
                NewTownName = ""
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Foshan");
            Assert.AreEqual(town.DistrictName, "Chancheng");
            Assert.AreEqual(town.TownName, "");
        }

        [Test]
        public void TestRegionViewModelAddTownConditions_NewTown()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "",
                DistrictName = "Chancheng",
                NewDistrictName = "",
                TownName = "Nanzhuang",
                NewTownName = "Chengqu"
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Foshan");
            Assert.AreEqual(town.DistrictName, "Chancheng");
            Assert.AreEqual(town.TownName, "Chengqu");
        }

        [Test]
        public void TestRegionViewModelAddTownConditions_NewDistrict()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "",
                DistrictName = "Chancheng",
                NewDistrictName = "Nanhai",
                TownName = "Nanzhuang",
                NewTownName = "Chengqu"
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Foshan");
            Assert.AreEqual(town.DistrictName, "Nanhai");
            Assert.AreEqual(town.TownName, "Chengqu");
        }

        [Test]
        public void TestRegionViewModelAddTownConditions_NewCity()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "Shenzhen",
                DistrictName = "Chancheng",
                NewDistrictName = "Nanhai",
                TownName = "Nanzhuang",
                NewTownName = "Chengqu"
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Shenzhen");
            Assert.AreEqual(town.DistrictName, "Nanhai");
            Assert.AreEqual(town.TownName, "Chengqu");
        }

    }
}
