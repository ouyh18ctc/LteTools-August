using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Dingli;
using Lte.Domain.LinqToCsv.Context;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class CoverageStatListTest : TabCsvReader
    {
        private List<CoverageStat> coverageStatList;

        [Test]
        public void TestCoverageStatList_Dingli()
        {
            DescriptionInitialize();
            testInput = DingliRecordExample;
            coverageStatList = CsvContext.ReadString<LogRecord>(testInput, fileDescription_namesUs).Select(x =>
                {
                     CoverageStat stat = new CoverageStat(); stat.Import(x); return stat;
                }).ToList();
            Assert.AreEqual(coverageStatList.Count, 74);
            Assert.AreEqual(coverageStatList[0].Longtitute, -9999);
            Assert.AreEqual(coverageStatList[0].Rsrp, -97.31);
            Assert.AreEqual(coverageStatList[0].Sinr, 14.3);
            CoverageStatChart chart = new CoverageStatChart();
            chart.Import(coverageStatList);
            Assert.AreEqual(chart.StatList.Count, 7);
            Assert.AreEqual(chart.StatList[0].Longtitute, 113.0001);
            Assert.AreEqual(chart.StatList[0].Lattitute, 23.0002);
            Assert.AreEqual(chart.StatList[0].Rsrp, -97.31);
            Assert.AreEqual(chart.StatList[1].Rsrp, -97.25);
            Assert.AreEqual(chart.StatList[2].Rsrp, -97.25);
            Assert.AreEqual(chart.StatList[3].Rsrp, -97.25);
            Assert.AreEqual(chart.StatList[4].Rsrp, -97.25);
            Assert.AreEqual(chart.StatList[5].Rsrp, -97.25);
            Assert.AreEqual(chart.StatList[6].Rsrp, -97.25);
            Assert.AreEqual(chart.StatList[0].Sinr, 14.3);
            Assert.AreEqual(chart.StatList[1].Sinr, 13.4);
            Assert.AreEqual(chart.StatList[2].Sinr, 13.4);
            Assert.AreEqual(chart.StatList[3].Sinr, 13.4);
            Assert.AreEqual(chart.StatList[4].Sinr, 13.4);
            Assert.AreEqual(chart.StatList[5].Sinr, 13.4);
            Assert.AreEqual(chart.StatList[6].Sinr, 13.4);
        }

        [Test]
        public void TestCoverageStatList_Hugeland()
        {
            HugelandDescriptionInitialize();
            testInput = HugelandRecordExample;
            coverageStatList = CsvContext.ReadString<HugelandRecord>(testInput, fileDescription_namesUs).Select(x =>
            {
                CoverageStat stat = new CoverageStat(); stat.Import(x); return stat;
            }).ToList();
            Assert.AreEqual(coverageStatList.Count, 19);
            Assert.AreEqual(coverageStatList[0].Longtitute, 113.13548);
            Assert.AreEqual(coverageStatList[0].Lattitute, 23.07062);
            Assert.AreEqual(coverageStatList[0].Rsrp, -93);
            Assert.AreEqual(coverageStatList[0].Sinr, 3.4);
            CoverageStatChart chart = new CoverageStatChart();
            chart.Import(coverageStatList);
            Assert.AreEqual(chart.StatList.Count, 9);
            Assert.AreEqual(chart.StatList[0].Longtitute, 113.13548);
            Assert.AreEqual(chart.StatList[0].Lattitute, 23.07062);
            Assert.AreEqual(chart.StatList[0].Rsrp, -93);
            Assert.AreEqual(chart.StatList[1].Rsrp, -93.2, 1E-6);
            Assert.AreEqual(chart.StatList[2].Rsrp, -93.15);
            Assert.AreEqual(chart.StatList[3].Rsrp, -92.6);
            Assert.AreEqual(chart.StatList[4].Rsrp, -94.1);
            Assert.AreEqual(chart.StatList[5].Rsrp, -96.5);
            Assert.AreEqual(chart.StatList[6].Rsrp, -98.5);
            Assert.AreEqual(chart.StatList[7].Rsrp, -98.5);
            Assert.AreEqual(chart.StatList[8].Rsrp, -98.5);
            Assert.AreEqual(chart.StatList[0].Sinr, 3.4);
            Assert.AreEqual(chart.StatList[1].Sinr, 2.8);
            Assert.AreEqual(chart.StatList[2].Sinr, 2.55);
        }
    }
}
