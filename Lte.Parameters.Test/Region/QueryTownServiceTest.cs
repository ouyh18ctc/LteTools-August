using System.Collections.Generic;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Region
{
    internal class QueryTownServiceTestHelper
    {
        private readonly IEnumerable<Town> towns;

        public QueryTownServiceTestHelper(ITownRepository townRepository)
        {
            towns = townRepository.GetAllList();
        }

        public int ConstructTestId(int cityId, int districtId, int townId)
        {
            return towns.QueryId("C-" + cityId, "D-" + districtId, "T-" + townId);
        }

        public int ConstructTestId(int districtId, int townId)
        {
            return towns.QueryId("D-" + districtId, "T-" + townId);
        }
    }

    [TestFixture]
    public class QueryTownServiceTest : RegionTestConfig
    {
        private QueryTownServiceTestHelper helper;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            helper = new QueryTownServiceTestHelper(townRepository.Object);
        }

        [TestCase(1, 1, 1, 1)]
        [TestCase(1, 2, 2, 2)]
        [TestCase(2, 3, 3, 3)]
        [TestCase(2, 3, 4, -1)]
        [TestCase(2, 4, 4, 4)]
        [TestCase(2, 4, 5, 5)]
        [TestCase(2, 4, 6, -1)]
        [TestCase(2, 5, 2, -1)]
        [TestCase(3, 5, 6, 6)]
        [TestCase(3, 5, 7, 7)]
        [TestCase(3, 5, 8, -1)]
        [TestCase(3, 6, 8, 8)]
        public void TestByCityDistrictTownId(int cityId, int districtId, int townId, int expectedId)
        {
            int actualId = helper.ConstructTestId(cityId, districtId, townId);
            Assert.AreEqual(actualId, expectedId);
        }

        [TestCase(1, 1, 1)]
        [TestCase(2, 2, 2)]
        [TestCase(3, 3, 3)]
        [TestCase(4, 4, 4)]
        [TestCase(3, 4, -1)]
        [TestCase(4, 5, 5)]
        [TestCase(5, 6, 6)]
        [TestCase(5, 7, 7)]
        [TestCase(6, 8, 8)]
        public void TestByDistrictTownId(int districtId, int townId, int expectedId)
        {
            int actualId = helper.ConstructTestId(districtId, townId);
            Assert.AreEqual(actualId, expectedId);
        }
    }
}
