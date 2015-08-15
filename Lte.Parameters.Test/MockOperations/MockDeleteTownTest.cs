using Lte.Parameters.Service.Region;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.MockOperations;

namespace Lte.Parameters.Test.MockOperations
{
    [TestFixture]
    public class MockDeleteTownTest
    {
        private readonly Mock<ITownRepository> mockTownRepository = new Mock<ITownRepository>();
        private readonly Mock<IENodebRepository> mockENodebRepository = new Mock<IENodebRepository>();

        [SetUp]
        public void TestInitialize()
        {
            IEnumerable<Town> towns = new List<Town> {
                new Town { Id = 22, CityName = "City1", DistrictName = "District1", TownName = "Town1" }
            };
            mockTownRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            mockTownRepository.Setup(x => x.GetAllList()).Returns(mockTownRepository.Object.GetAll().ToList());
            mockTownRepository.Setup(x => x.Count()).Returns(mockTownRepository.Object.GetAll().Count());
            mockTownRepository.MockRemoveOneTownOperation();
        }

        [Test]
        public void TestMockDeleteTown_NullENodebRepository()
        {
            TownOperationService service = new TownOperationService(mockTownRepository.Object,
                "City1", "District1", "Town1");
            Assert.IsTrue(service.DeleteOneTown(null,null));
            Assert.AreEqual(mockTownRepository.Object.Count(), 0);
        }

        [Test]
        public void TestMockDeleteTown_NoENodebRepository()
        {
            TownOperationService service = new TownOperationService(mockTownRepository.Object,
                "City1", "District1", "Town1");
            Assert.IsTrue(service.DeleteOneTown());
            Assert.AreEqual(mockTownRepository.Object.Count(), 0);
        }

        [Test]
        public void TestMockDeleteTown_WrongENodebRepository()
        {
            mockENodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb> {
                new ENodeb { TownId = 21, Name = "E1" }
            }.AsQueryable());
            TownOperationService service = new TownOperationService(mockTownRepository.Object,
                "City1", "District1", "Town1");
            Assert.IsTrue(service.DeleteOneTown(mockENodebRepository.Object,null));
            Assert.AreEqual(mockTownRepository.Object.Count(), 0);
        }

        [Test]
        public void TestMockDeleteTown_RightENodebRepository()
        {
            mockENodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb> {
                new ENodeb { TownId = 22, Name = "E1" }
            }.AsQueryable());
            TownOperationService service = new TownOperationService(mockTownRepository.Object,
                "City1", "District1", "Town1");
            Assert.IsFalse(service.DeleteOneTown(mockENodebRepository.Object,null));
            Assert.AreEqual(mockTownRepository.Object.Count(), 1);
        }
    }
}
