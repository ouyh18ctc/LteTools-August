using Lte.Evaluations.ViewHelpers;
using NUnit.Framework;

namespace Lte.WebApp.Tests.Parameters
{
    [TestFixture]
    public class RegionViewModelMessageTest
    {
        [Test]
        public void TestRegionViewModel_DeleteTownSuccessMessage()
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
            Assert.AreEqual(viewModel.DeleteSuccessMessage,
                "删除镇街:Foshan-Chancheng-Nanzhuang成功");
        }

        [Test]
        public void TestRegionViewModel_DeleteTownFailMessage()
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
            Assert.AreEqual(viewModel.DeleteFailMessage,
                "删除镇街:Foshan-Chancheng-Nanzhuang失败。该镇街不存在或镇街下面还带有基站！");
        }
    }
}
