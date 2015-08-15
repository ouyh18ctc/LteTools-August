using System.Data;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Test.Entities;
using Moq;
using System;
using NUnit.Framework;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class CdmaRegionStatConstructionTest
    {
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();
        private readonly CdmaRegionStat stat = new CdmaRegionStat();

        [SetUp]
        public void TestInitialize()
        {
            Tuple<string, string>[] contents =
            {
                new Tuple<string,string>("地市","Foshan"),
                new Tuple<string,string>("日期","2015-2-10"),
                new Tuple<string,string>("2G全天话务量不含切换","3344"),
                new Tuple<string,string>("掉话分子","112.123"),
                new Tuple<string,string>("EcIo分子","222"),
                new Tuple<string,string>("全天流量MB","12345"),
                new Tuple<string,string>("反向链路繁忙率分母","22334"),
                new Tuple<string,string>("3G利用率分子","1234")
            };
            mockReader.MockReaderContents(contents);
        }

        [Test]
        public void TestCdmaRegionStatConstruction()
        {
            stat.Import(mockReader.Object);
            Assert.AreEqual(stat.Region, "Foshan");
            Assert.AreEqual(stat.StatDate, new DateTime(2015, 2, 10));
            Assert.AreEqual(stat.ErlangIncludingSwitch, 0);
            Assert.AreEqual(stat.ErlangExcludingSwitch, 3344);
            Assert.AreEqual(stat.Drop2GNum, 0);
            Assert.AreEqual(stat.Drop2GDem, 1);
            Assert.AreEqual(stat.EcioNum, 222);
            Assert.AreEqual(stat.EcioDem, 1);
            Assert.AreEqual(stat.Flow, 12345);
            Assert.AreEqual(stat.LinkBusyDem, 22334);
            Assert.AreEqual(stat.Utility3GNum, 1234);
        }
    }
}
