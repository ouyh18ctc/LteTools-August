using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Dingli;
using Lte.Domain.Regular;
using Lte.Domain.LinqToCsv.Context;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class RateStatListTest : TabCsvReader
    {
        private List<RateStat> rateStatList;

        [SetUp]
        public void TestInitialize()
        {
            DescriptionInitialize();

        }

        [Test]
        public void TestBasicRateStat_Initialize()
        {
            BasicRateStat stat = new BasicRateStat();
            Assert.AreEqual(stat.PuschRbRate, 0);
        }

        [Test]
        public void TestRateStatList()
        {
            testInput = DingliRecordExample;
            rateStatList = CsvContext.ReadString<LogRecord>(testInput, fileDescription_namesUs).ToList().MergeStat();
            Assert.AreEqual(rateStatList.Count, 28);
            Assert.AreEqual(rateStatList[0].DlThroughput, 18759024);
            Assert.AreEqual(rateStatList[25].DlThroughput, 68256);
            Assert.AreEqual(rateStatList[27].DlThroughput, 113760);
            Assert.AreEqual(rateStatList[2].DlMcs, 17);
            Assert.AreEqual(rateStatList[4].UlMcs, 0);
            Assert.AreEqual(rateStatList[6].Earfcn, 100);
            Assert.AreEqual(rateStatList[8].Pci, -1);
            Assert.AreEqual(rateStatList[10].PdschRbRate, 16160);
            Assert.AreEqual(rateStatList[14].PdschTbCode0, 14257);
            Assert.AreEqual(rateStatList[14].PdschTbCode1, 504);
            Assert.AreEqual(rateStatList[16].PhyThroughputCode0, 114056);
            Assert.AreEqual(rateStatList[16].PhyThroughputCode1, 4032);
            Assert.AreEqual(rateStatList[16].DlMcs, 20);
            Assert.AreEqual(rateStatList[16].PdschRbRate, 16160);
            Assert.AreEqual(rateStatList[16].DlThroughput, 68256);
            Assert.AreEqual(rateStatList[16].PhyRatePerRb, 7.057921,1E-6);
            Assert.AreEqual(rateStatList[16].DlFrequencyEfficiency, 0.070579, 1E-6);
            Assert.AreEqual(rateStatList[16].DlRbsPerSlot, 8.08);
            Assert.AreEqual(rateStatList[16].Time.ToString("HH:mm:ss.fff"), "15:54:49.296");

            rateStatList[16].DividedBy<BasicRateStat>(10);
            Assert.AreEqual(rateStatList[16].PhyThroughputCode0, 11405);
            Assert.AreEqual(rateStatList[16].DlMcs, 2);
            Assert.AreEqual(rateStatList[16].PdschRbRate, 1616);
            Assert.AreEqual(rateStatList[16].DlThroughput, 6825);
            Assert.AreEqual(rateStatList[16].PhyRatePerRb, 7.057550, 1E-6);
            Assert.AreEqual(rateStatList[16].DlFrequencyEfficiency, 0.070575, 1E-6);
            Assert.AreEqual(rateStatList[16].DlRbsPerSlot, 0.808);
            Assert.AreEqual(rateStatList[16].Time.ToString("HH:mm:ss.fff"), "15:54:49.296");

            LogsOperations.RateEvaluationInterval = 1;
            List<BasicRateStat> basicList = rateStatList.Merge();
            Assert.AreEqual(basicList.Count, 2);
            Assert.AreEqual(basicList[0].DlFrequencyEfficiency, 5, 1E-6);
            Assert.AreEqual(basicList[0].DlThroughput, 17512972);

            LogsOperations.RateEvaluationInterval = 0.5;
            basicList = rateStatList.Merge();
            Assert.AreEqual(basicList.Count, 4);
            Assert.AreEqual(basicList[0].DlFrequencyEfficiency, 5, 1E-6);
            Assert.AreEqual(basicList[0].DlThroughput, 18759024);
            Assert.AreEqual(basicList[1].DlFrequencyEfficiency, 5, 1E-6);
            Assert.AreEqual(basicList[1].DlThroughput, 16088914);
            Assert.AreEqual(basicList[2].DlFrequencyEfficiency, 0.070579, 1E-6);
            Assert.AreEqual(basicList[2].DlThroughput, 58017);
            Assert.AreEqual(basicList[3].DlFrequencyEfficiency, 0.155195, 1E-6);
            Assert.AreEqual(basicList[3].DlThroughput, 74756);
        }

        [Test]
        public void TestRateStatList_HugelandRecord()
        {
            testInput = HugelandRecordExample;
            HugelandDescriptionInitialize();
            rateStatList
                = CsvContext.ReadString<HugelandRecord>(testInput, fileDescription_namesUs).ToList().MergeStat();
            Assert.AreEqual(rateStatList.Count, 19);
            Assert.AreEqual(rateStatList[0].DlThroughput, 6556774);
            Assert.AreEqual(rateStatList[2].DlMcs, 8);
            Assert.AreEqual(rateStatList[4].UlMcs, 22);
            Assert.AreEqual(rateStatList[6].Earfcn, 100);
            Assert.AreEqual(rateStatList[8].Pci, 99);
            Assert.AreEqual(rateStatList[10].PdschRbRate, 157120);
        }
    }
}
