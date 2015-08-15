using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Service.Lte;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository
{
    [TestFixture]
    public class QueryENodebsTest
    {
        private readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();

        [SetUp]
        public void TestInitialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(new List<Town>{
                new Town{Id=1,CityName="Guangzhou",DistrictName="Tianhe",TownName="Wushan"},
                new Town{Id=2,CityName="Guangzhou",DistrictName="Tianhe",TownName="Shipai"},
                new Town{Id=3,CityName="Guangzhou",DistrictName="YueXiu",TownName="Taojin"},
                new Town{Id=4,CityName="Foshan",DistrictName="Chancheng",TownName="Zhangcha"}
            }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());

            eNodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb>{
                new ENodeb{Name="GuangzhouHengda",Address="Guangzhou 123",TownId=1},
                new ENodeb{Name="GuangzhouHengda",Address="Guangzhou 123",TownId=2},
                new ENodeb{Name="GuangzhouFuli",Address="Guangzhou 456",TownId=1},
                new ENodeb{Name="GuangzhouDianxin",Address="Guangzhou 789",TownId=3},
                new ENodeb{Name="FoshanHengda",Address="Foshan 123",TownId=4}
            }.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());
            eNodebRepository.Setup(
                x => x.GetAllWithNames(It.IsAny<ITownRepository>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns<
                    ITownRepository, string, string, string, string, string>(
                        (ti, c, d, t, e, a) => eNodebRepository.Object.QueryWithNames(ti, c, d, t, e, a).ToList());
        }

        [Test]
        public void TestQueryENodebs_PreciseTownName_FullConditions()
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.Object.GetAllWithNames(townRepository.Object,
                "Guangzhou", "Tianhe", "Wushan", "Hengda", "Guangzhou");
            Assert.AreEqual(eNodebs.Count(), 1);
            Assert.AreEqual(eNodebs.ElementAt(0).Name, "GuangzhouHengda");

        }

        [Test]
        public void TestQueryENodebs_PreciseTownName_EmptyConditions()
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.Object.GetAllWithNames(townRepository.Object,
                "Guangzhou", "Tianhe", "Wushan", "", "");
            Assert.AreEqual(eNodebs.Count(), 2);
            Assert.AreEqual(eNodebs.ElementAt(0).Name, "GuangzhouHengda");
            Assert.AreEqual(eNodebs.ElementAt(1).Name, "GuangzhouFuli");
        }

        [Test]
        public void TestQueryENodebs_PreciseDistrictName_EmptyConditions()
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.Object.GetAllWithNames(townRepository.Object,
                "Guangzhou", "Tianhe", "=不限定镇区=", "", "");
            Assert.AreEqual(eNodebs.Count(), 3);
            Assert.AreEqual(eNodebs.ElementAt(0).Name, "GuangzhouHengda");
            Assert.AreEqual(eNodebs.ElementAt(1).Name, "GuangzhouHengda");
            Assert.AreEqual(eNodebs.ElementAt(2).Name, "GuangzhouFuli");
        }

        [Test]
        public void TestQueryENodebs_PreciseCityName_EmptyConditions()
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.Object.GetAllWithNames(townRepository.Object,
                "Guangzhou", "=不限定区域=", "=不限定镇区=", "", "");
            Assert.AreEqual(eNodebs.Count(), 4);
            Assert.AreEqual(eNodebs.ElementAt(0).Name, "GuangzhouHengda");
            Assert.AreEqual(eNodebs.ElementAt(1).Name, "GuangzhouHengda");
            Assert.AreEqual(eNodebs.ElementAt(2).Name, "GuangzhouFuli");
            Assert.AreEqual(eNodebs.ElementAt(3).Name, "GuangzhouDianxin");
        }

        [Test]
        public void TestQueryENodebs_PreciseCityName_SetName()
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.Object.GetAllWithNames(townRepository.Object,
                "Guangzhou", "=不限定区域=", "=不限定镇区=", "Hengda", "");
            Assert.AreEqual(eNodebs.Count(), 2);
        }
    }
}
