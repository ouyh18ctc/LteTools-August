using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Lte.Parameters.Test.Region
{
    internal class TownRepositoryQueryTestHelp
    {
        private readonly IEnumerable<Town> _towns;

        public TownRepositoryQueryTestHelp(ITownRepository townRepository)
        {
            _towns = townRepository.GetAllList();
        }

        public void TestOneMatchedQueries(string cityName, string districtName, string townName, 
            bool townExisted = true)
        {
            IEnumerable<Town> towns = _towns.QueryTowns(cityName, districtName, townName);
            if (townExisted)
            {
                Assert.AreEqual(towns.Count(), 1);
                Assert.AreEqual(towns.ElementAt(0).CityName, cityName);
                Assert.AreEqual(towns.ElementAt(0).DistrictName, districtName);
                Assert.AreEqual(towns.ElementAt(0).TownName, townName);
            }
            else
            {
                Assert.AreEqual(towns.Count(), 0);
            }
        }

        public void TestMatchedDistrictAndTownQueries(string districtName, string townName, int expectedItems)
        {
            IEnumerable<Town> towns = _towns.QueryTowns(districtName, townName);
            Assert.AreEqual(towns.Count(), expectedItems);
            for (int i = 0; i < expectedItems; i++)
            {
                Assert.AreEqual(towns.ElementAt(i).DistrictName, districtName);
                Assert.AreEqual(towns.ElementAt(i).TownName, townName);
            }
        }

        public void TestMatchedCityAndDitrictQueries(string cityName, string districtName, int expectedItems)
        {
            IEnumerable<Town> towns = _towns.QueryTowns(cityName, districtName, null);
            Assert.AreEqual(towns.Count(), expectedItems);
            for (int i = 0; i < expectedItems; i++)
            {
                Assert.AreEqual(towns.ElementAt(i).CityName, cityName);
                Assert.AreEqual(towns.ElementAt(i).DistrictName, districtName);
            }
        }

        public void TestMatchedCityQueries(string cityName, int expectedItems)
        {
            IEnumerable<Town> towns = _towns.QueryTowns(cityName, null, null);
            Assert.AreEqual(towns.Count(), expectedItems);
            for (int i = 0; i < expectedItems; i++)
            {
                Assert.AreEqual(towns.ElementAt(i).CityName, cityName);
            }
        }
    }

    [TestFixture]
    public class TownRepositoryQueryTest : RegionTestConfig
    {
        private TownRepositoryQueryTestHelp helper;

        [SetUp]
        public void TestInitialize()
        {
            Initialize();
            helper = new TownRepositoryQueryTestHelp(townRepository.Object);
        }

        [TestCase(1,1,1,true)]
        [TestCase(2,4,5,true)]
        [TestCase(3,5,7,true)]
        [TestCase(1,1,7,false)]
        [TestCase(2,4,8,false)]
        [TestCase(3,5,9,false)]
        public void TestQueryTowns_AllAssigned(int cityId, int districtId, int townId, bool existed)
        {
            helper.TestOneMatchedQueries("C-"+cityId, "D-"+districtId, "T-"+townId, existed);
        }

        [TestCase(1, 2, 1)]
        [TestCase(1, 3, 0)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 5, 0)]
        [TestCase(3, 5, 2)]
        [TestCase(3, 1, 0)]
        [TestCase(2, 2, 0)]
        [TestCase(3, 6, 1)]
        public void TestQueryTowns_CityAndDistrictAssigned(int cityId, int districtId, int expectedItems)
        {
            helper.TestMatchedCityAndDitrictQueries("C-" + cityId, "D-" + districtId, expectedItems);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 3)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestQueryTowns_CityAssigned(int cityId, int expectedItems)
        {
            helper.TestMatchedCityQueries("C-" + cityId, expectedItems);
        }
    }
}
