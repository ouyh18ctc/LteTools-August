using System.Collections.Generic;
using Lte.Parameters.Abstract;
using Lte.WebApp.Controllers.Parameters;
using Lte.WebApp.Tests.ControllerParameters;
using Moq;
using System.Linq;
using NUnit.Framework;

namespace Lte.WebApp.Tests.ControllerParametersQuery
{
    [TestFixture]
    public class GetDistrictListJsonTest : ParametersConfig
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private DistrictListController controller;

        [SetUp]
        public void TestInitialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            controller = new DistrictListController(townRepository.Object);
        }

        [Test]
        public void TestGetDistrictList_City1()
        {
            IEnumerable<string> result = controller.GetDistrictListByCityName("City1");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.ElementAt(0), "District1");
            Assert.AreEqual(result.ElementAt(1), "District2");
        }

        [Test]
        public void TestGetDistrictList_City2()
        {
            IEnumerable<string> result = controller.GetDistrictListByCityName("City2");
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.ElementAt(0), "District1");
            Assert.AreEqual(result.ElementAt(1), "District3");
        }
    }
}
