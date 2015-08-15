using System.Linq;
using Lte.Parameters.Abstract;
using Lte.WebApp.Controllers.Parameters;
using Lte.WebApp.Tests.ControllerParameters;
using Moq;
using NUnit.Framework;

namespace Lte.WebApp.Tests.ControllerRegion
{
    [TestFixture]
    public class OptimizeRegionControllerTest : ParametersConfig
    {
        private RegionNameController _nameController;

        [SetUp]
        public void Setup()
        {
            Mock<IRegionRepository> repository=new Mock<IRegionRepository>();
            repository.Setup(x => x.GetAll()).Returns(regions.AsQueryable());
            repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
            repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
            _nameController = new RegionNameController(repository.Object);
        }

        [TestCase("City1", "District1", "Region1")]
        [TestCase("City1", "District2", "Region2")]
        [TestCase("City2", "District1", "Region3")]
        [TestCase("City2", "District3", "Region4")]
        public void Test_GetName(string cityName, string districtName, string regionName)
        {
            Assert.AreEqual(_nameController.GetName(cityName,districtName),regionName);
        }
    }
}
