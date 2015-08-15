using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using Lte.Parameters.Test.Repository.ENodebRepository;
using NUnit.Framework;
using Moq;
using Lte.Parameters.MockOperations;

namespace Lte.Parameters.Test.Repository
{
    [TestFixture]
    public class TownRepositoryTest : ENodebRepositoryTestConfig
    {
        private readonly Mock<ITownRepository> repository = new Mock<ITownRepository>();

        [SetUp]
        public void TestInitialize()
        {
            IEnumerable<Town> towns = new List<Town> 
            {
                new Town
                {
                    CityName = "Foshan",
                    DistrictName = "Chancheng",
                    TownName = "Zhangcha",
                    Id = 122
                }
            };
            base.Initialize();
            repository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
            repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
            repository.MockAddOneTownOperation();
            repository.MockRemoveOneTownOperation();
        }

        [Test]
        public void TestSaveTown_Success()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Nanhai", "Guicheng");
            service.SaveOneTown();
            Assert.AreEqual(repository.Object.Count(), 2);
        }

        [Test]
        public void TestSaveTown_Fail()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Chancheng", "Zhangcha");
            service.SaveOneTown();
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestDeleteTown_Success()
        {
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Chancheng", "Zhangcha");
            Assert.IsTrue(service.DeleteOneTown());
            Assert.AreEqual(repository.Object.Count(), 0);
        }

        [Test]
        public void TestDeleteTown_WithWhiteSpace_Success()
        {
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan ", "Chancheng ", "Zhangcha");
            Assert.IsTrue(service.DeleteOneTown());
            Assert.AreEqual(repository.Object.Count(), 0);
        }

        [Test]
        public void TestDeleteTown_WithENodebInfos_Fail()
        {
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan ", "Chancheng ", "Zhangcha");
            Assert.IsFalse(service.DeleteOneTown(lteRepository.Object, null));
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestDeleteTown_Fail()
        {
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Chancheng", "Zumiao");
            Assert.IsFalse(service.DeleteOneTown());
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestSaveAndDeleteTown_AddSuccess_DeleteSucess()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Nanhai ", "Guicheng ");
            service.SaveOneTown();
            Assert.AreEqual(repository.Object.Count(), 2);
            repository.MockRemoveOneTownOperation();
            service = new TownOperationService(repository.Object,
                "Foshan", "Nanhai", "Guicheng");
            Assert.IsTrue(service.DeleteOneTown());
            Assert.AreEqual(repository.Object.Count(), 1);
        }

        [Test]
        public void TestSaveAndDeleteTown_AddSuccess_DeleteFail()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Nanhai ", "Guicheng ");
            service.SaveOneTown();
            Assert.AreEqual(repository.Object.Count(), 2);
            repository.MockRemoveOneTownOperation();
            service = new TownOperationService(repository.Object,
                "Foshan", "Nanhai", "Dali");
            Assert.IsFalse(service.DeleteOneTown());
            Assert.AreEqual(repository.Object.Count(), 2);
        }

        [Test]
        public void TestSaveAndDeleteTown_AddFail_DeleteSuccess()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            TownOperationService service = new TownOperationService(repository.Object,
                "Foshan", "Chancheng ", "Zhangcha ");
            service.SaveOneTown();
            Assert.AreEqual(repository.Object.Count(), 1, "Add town success! But it's expected to be failed!");
            repository.MockRemoveOneTownOperation();
            service = new TownOperationService(repository.Object,
                "Foshan", "Chancheng ", "Zhangcha ");
            Assert.IsTrue(service.DeleteOneTown());
            Assert.AreEqual(repository.Object.Count(), 0);
        }
    }
}
