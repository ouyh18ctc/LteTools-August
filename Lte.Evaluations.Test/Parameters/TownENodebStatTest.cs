using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Parameters
{
    [TestFixture]
    public class TownENodebStatTest
    {
        private readonly Mock<IENodebRepository> repository = new Mock<IENodebRepository>();
        private readonly Mock<IRegionRepository> regionRepository = new Mock<IRegionRepository>();

        [SetUp]
        public void TestInitialize()
        {
            repository.Setup(x => x.GetAll()).Returns(
                new List<ENodeb>{
                    new ENodeb { Id=1, ENodebId=2, Name="aaa", TownId=1 },
                    new ENodeb { Id=2, ENodebId=3, Name="bbb", TownId=1 },
                    new ENodeb { Id=3, ENodebId=4, Name="ccc", TownId=2 }
                }.AsQueryable());
        }

        [Test]
        public void TestTownENodebStat()
        {
            TownENodebStat stat = new TownENodebStat(
                new Town
                {
                    Id = 1,
                    CityName = "Foshan"
                },
                repository.Object.GetAll(), regionRepository.Object);
            Assert.AreEqual(stat.TownId, 1);
            Assert.AreEqual(stat.CityName, "Foshan");
            Assert.AreEqual(stat.TotalENodebs, 2);
        }
    }
}
