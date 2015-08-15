using Lte.Parameters.MockOperations;
using Lte.Parameters.Entities;
using System.Collections.Generic;
using NUnit.Framework;

namespace Lte.Parameters.Test.MockOperations
{
    [TestFixture]
    public class MockSaveENodebsTest : MockENodebTestConfig
    {
        [SetUp]
        public void TestInitialize()
        {
            Initialize();
            eNodebRepository.MockENodebRepositorySaveENodeb();
        }

        [Test]
        public void TestMockAddSuccessiveENodebs()
        {
            eNodebRepository.MockENodebRepositorySaveENodeb();
            Assert.AreEqual(eNodebRepository.Object.Count(), 7);
            eNodebRepository.Object.Insert(
                new ENodeb { TownId = 5, ENodebId = 11, Name = "E-11" });
            Assert.AreEqual(eNodebRepository.Object.Count(), 8);
            eNodebRepository.Object.Insert(
                new ENodeb { TownId = 4, ENodebId = 12, Name = "E-12" });
            Assert.AreEqual(eNodebRepository.Object.Count(), 9);
        }

        [Test]
        public void TestMockSaveENodebs_TwoSuccessiveENodebs()
        {
            Assert.AreEqual(eNodebRepository.Object.Count(), 7, "eNodeb Counts");
            Assert.AreEqual(SaveENodebs(new List<ENodebExcel>
            {
                new ENodebExcel { CityName = "C-5", DistrictName = "D-5", TownName = "T-5", Name = "E-1", ENodebId = 100001 },
                new ENodebExcel { CityName = "C-5", DistrictName = "D-5", TownName = "T-5", Name = "E-2", ENodebId = 100002 }
            }), 2);
            Assert.AreEqual(eNodebRepository.Object.Count(), 9, "Counts after");
        }

        [Test]
        public void TestMockSaveENodebs_ThreeSuccessiveENodebs()
        {
            Assert.AreEqual(eNodebRepository.Object.Count(), 7);
            Assert.AreEqual(SaveENodebs(new List<ENodebExcel>
            {
                new ENodebExcel { CityName = "C-5", DistrictName = "D-5", TownName = "T-5", Name = "E-1", ENodebId = 100001 },
                new ENodebExcel { CityName = "C-5", DistrictName = "D-5", TownName = "T-5", Name = "E-2", ENodebId = 100002 },
                new ENodebExcel { CityName = "C-5", DistrictName = "D-5", TownName = "T-5", Name = "E-3", ENodebId = 100003 }
            }), 3);
            Assert.AreEqual(eNodebRepository.Object.Count(), 10);
        }
    }
}
