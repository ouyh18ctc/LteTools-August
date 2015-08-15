using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.WebApp.Controllers.Parameters;
using Lte.WebApp.Tests.ControllerParameters;
using NUnit.Framework;
using Moq;
using Lte.Parameters.MockOperations;

namespace Lte.WebApp.Tests.ControllerRegion
{
    [TestFixture]
    public class AddRegionToEmptySetTest : ParametersConfig
    {
        private RegionController controller;
        private readonly Mock<IRegionRepository> regionRepository = new Mock<IRegionRepository>();
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private readonly RegionViewModel viewModel = new RegionViewModel("");

        [SetUp]
        public void TestInitialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            regionRepository.Setup(x => x.GetAll()).Returns(new List<OptimizeRegion>().AsQueryable());
            regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
            regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
            regionRepository.MockAddOneRegionOperation();
            controller = new RegionController(townRepository.Object, null, null, regionRepository.Object);
        }

        [TestCase("City1", "District1")]
        [TestCase("Cy1", "Disict1")]
        [TestCase("City1", "District2")]
        [TestCase("City2", "District1")]
        [TestCase("Ciy1", "District3")]
        public void TestModifyRegion_EmptyInput(string city, string district)
        {
            viewModel.CityName = city;
            viewModel.DistrictName = district;
            viewModel.RegionName = "";
            controller.ModifyRegion(viewModel);
            Assert.AreEqual(controller.TempData["error"],
                "保存区域:" + city + "-" + district + "-<Empty>失败。"
                + "输入条件部分为空，或者该片区已存在，或该片区与已存在的片区有冲突，且设置不允许修改！");
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 2, 3)]
        [TestCase(1, 3, 1)]
        [TestCase(1, 4, 2)]
        [TestCase(1, 5, 3)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(2, 1, 3)]
        [TestCase(2, 2, 1)]
        [TestCase(2, 2, 2)]
        [TestCase(2, 2, 3)]
        [TestCase(2, 3, 1)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 5, 3)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 1, 2)]
        [TestCase(3, 1, 3)]
        [TestCase(3, 2, 1)]
        [TestCase(4, 2, 2)]
        [TestCase(4, 2, 3)]
        [TestCase(4, 3, 1)]
        [TestCase(4, 4, 2)]
        [TestCase(4, 5, 3)]
        public void TestSaveRegion_NormalInput(int cityId, int districtId, int regionId)
        {
            viewModel.CityName = "City" + cityId;
            viewModel.DistrictName = "District" + districtId;
            viewModel.RegionName = "Region" + regionId;
            viewModel.ForceSwapRegionDistricts = false;
            Assert.AreEqual(regionRepository.Object.Count(), 0);
            controller.ModifyRegion(viewModel);
            Assert.AreEqual(regionRepository.Object.Count(), 1);
            Assert.AreEqual(controller.TempData["success"], 
                "保存区域:City" + cityId + "-District" + districtId + "-Region" + regionId + "成功");
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 2, 3)]
        [TestCase(1, 3, 1)]
        [TestCase(1, 4, 2)]
        [TestCase(1, 5, 3)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(2, 1, 3)]
        [TestCase(2, 2, 1)]
        [TestCase(2, 2, 2)]
        [TestCase(2, 2, 3)]
        [TestCase(2, 3, 1)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 5, 3)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 1, 2)]
        [TestCase(3, 1, 3)]
        [TestCase(3, 2, 1)]
        [TestCase(4, 2, 2)]
        [TestCase(4, 2, 3)]
        [TestCase(4, 3, 1)]
        [TestCase(4, 4, 2)]
        [TestCase(4, 5, 3)]
        public void TestSaveRegion_NormalInput_ForceSwap(int cityId, int districtId, int regionId)
        {
            viewModel.CityName = "City" + cityId;
            viewModel.DistrictName = "District" + districtId;
            viewModel.RegionName = "Region" + regionId;
            viewModel.ForceSwapRegionDistricts = true;
            Assert.AreEqual(regionRepository.Object.Count(), 0);
            controller.ModifyRegion(viewModel);
            Assert.AreEqual(regionRepository.Object.Count(), 1);
            Assert.AreEqual(controller.TempData["success"],
                "保存区域:City" + cityId + "-District" + districtId + "-Region" + regionId + "成功");
        }
    }
}
