using System.Collections.Generic;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using NUnit.Framework;
using System.Linq;
using Moq;

namespace Lte.Parameters.Test.Region
{
    internal class TownOperationServiceTestHelper
    {
        private TownOperationService service;
        private readonly ITownRepository repository;

        public TownOperationServiceTestHelper(ITownRepository townRepository)
        {
            repository = townRepository;
        }

        public void TestAddTown(int cityId, int districtId, int townId)
        {
            service = new TownOperationService(repository,
                "C-" + cityId, "D-" + districtId, "T-" + townId);
            service.SaveOneTown();
        }

        public bool TestDeleteTown(int cityId, int districtId, int townId)
        {
            service = new TownOperationService(repository,
                "C-" + cityId, "D-" + districtId, "T-" + townId);
            return service.DeleteOneTown();
        }

        public bool TestDeleteTown(int cityId, int districtId, int townId, 
            IENodebRepository eNodebRepository)
        {
            service = new TownOperationService(repository,
                "C-" + cityId, "D-" + districtId, "T-" + townId);
            return service.DeleteOneTown(eNodebRepository, null);
        }

        public bool TestDeleteTown(int cityId, int districtId, int townId,
            IBtsRepository btsRepository)
        {
            service = new TownOperationService(repository,
                "C-" + cityId, "D-" + districtId, "T-" + townId);
            return service.DeleteOneTown(null, btsRepository);
        }

        public bool TestDeleteTown(int cityId, int districtId, int townId,
            IENodebRepository eNodebRepository, IBtsRepository btsRepository)
        {
            service = new TownOperationService(repository,
                "C-" + cityId, "D-" + districtId, "T-" + townId);
            return service.DeleteOneTown(eNodebRepository, btsRepository);
        }
    }

    [TestFixture]
    public class TownOperationServiceTest : RegionTestConfig
    {
        private TownOperationServiceTestHelper helper;
        private readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        private readonly Mock<IBtsRepository> btsRepository = new Mock<IBtsRepository>();

        [SetUp]
        public void SetUp()
        {
            eNodebRepository.Setup(x => x.GetAll()).Returns(
                new List<ENodeb>
                {
                    new ENodeb {Id = 7, TownId = 7}
                }.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());
            btsRepository.Setup(x => x.GetAll()).Returns(
                new List<CdmaBts>
                {
                    new CdmaBts {Id = 6, TownId = 6}
                }.AsQueryable());
            btsRepository.Setup(x => x.GetAllList()).Returns(btsRepository.Object.GetAll().ToList());
            Initialize();
            helper = new TownOperationServiceTestHelper(townRepository.Object);
        }

        [TestCase(1, 1, 1, false)]
        [TestCase(1, 2, 2, false)]
        [TestCase(1, 3, 9, true)]
        [TestCase(2, 3, 3, false)]
        [TestCase(2, 4, 5, false)]
        [TestCase(2, 4, 6, true)]
        [TestCase(2, 5, 8, true)]
        [TestCase(3, 5, 7, false)]
        [TestCase(3, 6, 9, true)]
        [TestCase(4, 2, 3, true)]
        public void TestAdd(int cityId, int districtId, int townId, bool add)
        {
            Assert.AreEqual(townRepository.Object.Count(), 8);
            helper.TestAddTown(cityId, districtId, townId);
            Assert.AreEqual(townRepository.Object.Count(), 8 + (add ? 1 : 0));
        }

        [TestCase(1, 2, 2, true)]
        [TestCase(2, 4, 5, true)]
        [TestCase(2, 4, 6, false)]
        [TestCase(3, 5, 7, true)]
        [TestCase(3, 8, 8, false)]
        [TestCase(4, 1, 2, false)]
        public void TestDelete(int cityId, int districtId, int townId, bool delete)
        {
            Assert.AreEqual(townRepository.Object.Count(), 8);
            if (delete)
            {
                Assert.IsTrue(helper.TestDeleteTown(cityId, districtId, townId));
            }
            else
            {
                Assert.IsFalse(helper.TestDeleteTown(cityId, districtId, townId));
            }
            Assert.AreEqual(townRepository.Object.Count(), 8 - (delete ? 1 : 0));
        }

        [TestCase(1, 2, 2, true)]
        [TestCase(2, 4, 5, true)]
        [TestCase(2, 4, 6, false)]
        [TestCase(3, 5, 7, false)]
        [TestCase(3, 8, 8, false)]
        [TestCase(4, 1, 2, false)]
        public void TestDeleteENodebMatched(int cityId, int districtId, int townId, bool delete)
        {
            Assert.AreEqual(townRepository.Object.Count(), 8);
            if (delete)
            {
                Assert.IsTrue(helper.TestDeleteTown(cityId, districtId, townId, eNodebRepository.Object));
            }
            else
            {
                Assert.IsFalse(helper.TestDeleteTown(cityId, districtId, townId, eNodebRepository.Object));
            }
            Assert.AreEqual(townRepository.Object.Count(), 8 - (delete ? 1 : 0));
        }

        [TestCase(1, 2, 2, true)]
        [TestCase(2, 4, 5, true)]
        [TestCase(2, 4, 6, false)]
        [TestCase(3, 5, 7, true)]
        [TestCase(3, 5, 6, false)]
        [TestCase(3, 8, 8, false)]
        [TestCase(4, 1, 2, false)]
        public void TestDeleteBtsMatched(int cityId, int districtId, int townId, bool delete)
        {
            Assert.AreEqual(townRepository.Object.Count(), 8);
            if (delete)
            {
                Assert.IsTrue(helper.TestDeleteTown(cityId, districtId, townId, btsRepository.Object));
            }
            else
            {
                Assert.IsFalse(helper.TestDeleteTown(cityId, districtId, townId, btsRepository.Object));
            }
            Assert.AreEqual(townRepository.Object.Count(), 8 - (delete ? 1 : 0));
        }

        [TestCase(1, 2, 2, true)]
        [TestCase(2, 4, 5, true)]
        [TestCase(2, 4, 6, false)]
        [TestCase(3, 5, 7, false)]
        [TestCase(3, 5, 6, false)]
        [TestCase(3, 8, 8, false)]
        [TestCase(4, 1, 2, false)]
        public void TestDeleteENodebAndBtsMatched(int cityId, int districtId, int townId, bool delete)
        {
            Assert.AreEqual(townRepository.Object.Count(), 8);
            if (delete)
            {
                Assert.IsTrue(helper.TestDeleteTown(cityId, districtId, townId, 
                    eNodebRepository.Object, btsRepository.Object));
            }
            else
            {
                Assert.IsFalse(helper.TestDeleteTown(cityId, districtId, townId,
                    eNodebRepository.Object, btsRepository.Object));
            }
            Assert.AreEqual(townRepository.Object.Count(), 8 - (delete ? 1 : 0));
        }
    }
}
