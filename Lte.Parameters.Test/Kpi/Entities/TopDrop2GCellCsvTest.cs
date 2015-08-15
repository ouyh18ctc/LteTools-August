using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Entities
{
    [TestFixture]
    public class TopDrop2GCellCsvTest
    {
        private List<TopDrop2GCellCsv> stats;

        [SetUp]
        public void TestInitialize()
        { 
            const string testInput = @",载波,基站名称,掉话总次数,呼叫总次数,掉话率,RSSI均值,平均掉话ECIO,掉话平均距离,0-200米,200-400米,400-600米,600-800米,800-1000米,1000-1200米,1200-1400米,1400-1600米,1600-1800米,1800-2000米,2000-2200米,2200-2400米,2400-2600米,2600-2800米,2800-3000米,3000-4000米,4000-5000米,5000-6000米,6000-7000米,7000-8000米,8000-9000米,9000米以上,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
CDR掉话次数,1_1377_1377_2_201,莱翔石瓷厂,160,6362,2.51,-85.0,-9.3,713.9,5,13,61,53,15,3,3,0,2,0,0,1,1,1,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,17,23,12,7,15,7,20,14,17,6,7,5,5,4,1
掉话Ecio,1_1377_1377_2_201,,,,,,,,-9.4,-8.8,-8.2,-9.6,-10.8,-10.5,-11.3,0.0,-22.0,0.0,0.0,-19.5,-12.5,-7.0,0.0,-9.0,0.0,0.0,0.0,-6.5,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,-10.2,-9.2,-8.3,-9.1,-8.7,-10.0,-9.2,-9.2,-8.6,-9.9,-13.6,-9.6,-9.3,-8.6,-4.5
ECIO优良比,1_1377_1377_2_201,,,,,,,,0.69,0.77,0.75,0.56,0.45,0.39,0.26,0.26,0.16,0.46,0.44,0.38,0.57,0.33,0.86,0.82,0.50,0.00,0.00,1.00,0.00,0.00,,,,,,,,,,,,,,,,,,,,,,,,
呼叫次数,1_1377_1377_2_201,,,,,,,,235,1478,2615,1299,299,118,62,39,38,13,9,16,7,6,36,88,2,1,0,1,0,0,79,48,42,14,4,2,9,40,160,488,606,578,488,469,476,595,473,506,427,174,215,208,173,88
性能数据呼叫次数,1_1377_1377_2_201,莱翔石瓷厂,160,5672,2.82,,,,,,,,,,,,,,,,,,,,,,,,,,39,67,57,20,8,3,4,20,82,281,552,521,497,368,429,579,469,430,411,219,141,198,167,110
性能数据掉话次数,1_1377_1377_2_201,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,0,0,0,0,0,0,0,0,0,8,18,19,13,7,15,10,19,11,13,6,10,4,4,3
Erasuare掉话次数,1_1377_1377_2_201,,160,,100.00,,,,,,,,,,,,,,,,,,,,,,,,,,0,0,0,0,0,0,0,0,0,8,18,19,13,7,15,10,19,11,13,6,10,4,4,3
告警次数,1_1377_1377_2_201,莱翔石瓷厂,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,RSSI问题:1;,,RSSI问题:3;,RSSI问题:1;,,,,,,,,,,,,
RSSI主集,1_1377_1377_2_201,,,,,-82.0851,,,,,,,,,,,,,,,,,,,,,,,,,-84,-84,-85,-85,-85,-84.5,-85,-84,-83.5,-82,-80.5,-80,-80,-80.5,-80.5,-80,-80,-80,-80.5,-81,-81.5,-81,-81.5,-82
RSSI分集,1_1377_1377_2_201,,,,,-109,,,,,,,,,,,,,,,,,,,,,,,,,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109
掉话原因,1_1377_1377_0_201,5_1975_0_201:2.13677km,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,1_1714_1714_1_201_优先级7_0话务,1_1714_1714_1_201_优先级7_0话务,1_1714_1714_1_201_优先级7_0话务,1_1714_1714_0_201_优先级7_0话务;1_1714_1714_1_201_优先级7_0话务,1_1714_1714_0_201_优先级7_0话务;1_1714_1714_1_201_优先级7_0话务,1_1714_1714_0_201_优先级7_0话务;1_1714_1714_1_201_优先级7_0话务,1_1714_1714_0_201_优先级7_0话务,,1_1714_1714_1_201_优先级7_0话务,,,1_2239_2239_0_201_优先级8_0话务,1_2239_2239_0_201_优先级8_0话务,,1_46_46_1_283邻区问题;1_142_142_2_283邻区问题";

            stats = CsvContext.ReadString<TopDrop2GCellCsv>(testInput, CsvFileDescription.CommaDescription).ToList();
        }

        [Test]
        public void TestTopDrop2GCellCsv_CsvResults()
        {
            Assert.IsNotNull(stats);
            Assert.AreEqual(stats.Count, 11);
            Assert.AreEqual(stats[0].FieldName, "CDR掉话次数");
            Assert.AreEqual(stats[0].Carrier, "1_1377_1377_2_201");
            Assert.AreEqual(stats[0].AverageRssi, "-85.0");
            Assert.AreEqual(stats[0].DistanceTo800Info, "53");
            Assert.AreEqual(stats[0].Hour9Info, "17");

            Assert.AreEqual(stats[3].Hour0Info, "79");
            Assert.AreEqual(stats[3].DistanceTo5000Info, "2");
            Assert.AreEqual(stats[10].DropCause, "1_46_46_1_283邻区问题;1_142_142_2_283邻区问题");
        }

        [Test]
        public void TestTopDrop2GCellCsv_CdrDropsDistanceInfo()
        {
            CdrDropsDistanceInfo info 
                = stats[0].GenerateDistanceInfo<CdrDropsDistanceInfo, int>();
            info.AssertDistanceTest(new[]{
                5,13,61,53,15,3,3,0,2,0,0,1,1,1,0,1,0,0,0,1,0,0});
        }

        [Test]
        public void TestTopDrop2GCellCsv_CdrDropsHourInfo()
        {
            CdrDropsHourInfo info
                = stats[0].GenerateHourInfo<CdrDropsHourInfo, int>();
            info.AssertHourTest(new[]{
                0,0,0,0,0,0,0,0,0,17,23,12,7,15,7,20,14,17,6,7,5,5,4,1});
        }

        [Test]
        public void TestTopDrop2GCellCsv_DropEcioDistanceInfo()
        {
            DropEcioDistanceInfo info
                = stats[1].GenerateDistanceInfo<DropEcioDistanceInfo, double>();
            info.AssertDistanceTest(new[]{
                -9.4,-8.8,-8.2,-9.6,-10.8,-10.5,-11.3,0.0,-22.0,0.0,0.0,-19.5,-12.5,-7.0,0.0,-9.0,0.0,0.0,0.0,-6.5,0.0,0.0});
        }

        [Test]
        public void TestTopDrop2GCellCsv_DropEcioHourInfo()
        {
            DropEcioHourInfo info
                = stats[1].GenerateHourInfo<DropEcioHourInfo, double>();
            info.AssertHourTest(new[]{
                0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,-10.2,-9.2,-8.3,-9.1,-8.7,-10.0,-9.2,-9.2,-8.6,-9.9,-13.6,-9.6,-9.3,-8.6,-4.5});
        }

        [Test]
        public void TestTopDrop2GCellCsv_GoodEcioDistanceInfo()
        {
            GoodEcioDistanceInfo info
                = stats[2].GenerateDistanceInfo<GoodEcioDistanceInfo, double>();
            info.AssertDistanceTest(new[]{
                0.69,0.77,0.75,0.56,0.45,0.39,0.26,0.26,0.16,0.46,0.44,0.38,0.57,0.33,0.86,0.82,0.50,0.00,0.00,1.00,0.00,0.00});
        }

        [Test]
        public void TestTopDrop2GCellCsv_CdrCallsDistanceInfo()
        {
            CdrCallsDistanceInfo info
                = stats[3].GenerateDistanceInfo<CdrCallsDistanceInfo, int>();
            info.AssertDistanceTest(new[]{
                235,1478,2615,1299,299,118,62,39,38,13,9,16,7,6,36,88,2,1,0,1,0,0});
        }

        [Test]
        public void TestTopDrop2GCellCsv_CdrCallsHourInfo()
        {
            CdrCallsHourInfo info
                = stats[3].GenerateHourInfo<CdrCallsHourInfo, int>();
            info.AssertHourTest(new[]{
                79,48,42,14,4,2,9,40,160,488,606,578,488,469,476,595,473,506,427,174,215,208,173,88});
        }

        [Test]
        public void TestTopDrop2GCellCsv_KpiCallsHourInfo()
        {
            KpiCallsHourInfo info
                = stats[4].GenerateHourInfo<KpiCallsHourInfo, int>();
            info.AssertHourTest(new[]{
                39,67,57,20,8,3,4,20,82,281,552,521,497,368,429,579,469,430,411,219,141,198,167,110});
        }

        [Test]
        public void TestTopDrop2GCellCsv_KpiDropsHourInfo()
        {
            KpiDropsHourInfo info
                = stats[5].GenerateHourInfo<KpiDropsHourInfo, int>();
            info.AssertHourTest(new[]{
                0,0,0,0,0,0,0,0,0,8,18,19,13,7,15,10,19,11,13,6,10,4,4,3});
        }

        [Test]
        public void TestTopDrop2GCellCsv_ErasureDropsHourInfo()
        {
            ErasureDropsHourInfo info
                = stats[6].GenerateHourInfo<ErasureDropsHourInfo, int>();
            info.AssertHourTest(new[]{
                0,0,0,0,0,0,0,0,0,8,18,19,13,7,15,10,19,11,13,6,10,4,4,3});
        }

        [Test]
        public void TestTopDrop2GCellCsv_MainRssiHourInfo()
        {
            MainRssiHourInfo info
                = stats[8].GenerateHourInfo<MainRssiHourInfo, double>();
            info.AssertHourTest(new[]{
                -84,-84,-85,-85,-85,-84.5,-85,-84,-83.5,-82,-80.5,-80,-80,-80.5,-80.5,-80,-80,-80,-80.5,-81,-81.5,-81,-81.5,-82});
        }

        [Test]
        public void TestTopDrop2GCellCsv_SubRssiHourInfo()
        {
            SubRssiHourInfo info
                = stats[9].GenerateHourInfo<SubRssiHourInfo, double>();
            info.AssertHourTest(new double[]{
                -109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109});
        }

        [Test]
        public void TestTopDrop2GCellCsv_AlarmHourInfos()
        { 
            List<AlarmHourInfo> infos = stats[7].GenerateAlarmHourInfos();
            Assert.AreEqual(infos.Count, 3);
            Assert.AreEqual(infos[0].Hour, 9);
            Assert.AreEqual(infos[1].Hour, 11);
            Assert.AreEqual(infos[2].Hour, 12);
            Assert.AreEqual(infos[0].Alarms, 1);
            Assert.AreEqual(infos[1].Alarms, 3);
            Assert.AreEqual(infos[2].Alarms, 1);
            Assert.AreEqual(infos[0].AlarmType, AlarmType.RssiProblem);
            Assert.AreEqual(infos[1].AlarmType, AlarmType.RssiProblem);
            Assert.AreEqual(infos[2].AlarmType, AlarmType.RssiProblem);
        }

        [Test]
        public void TestTopDrop2GCellCsv_NeighborHourInfos()
        {
            List<NeighborHourInfo> infos = stats[10].GenerateNeighborHourInfos();
            Assert.AreEqual(infos.Count, 13);
            Assert.AreEqual(infos[0].Hour, 10);
            Assert.AreEqual(infos[1].Hour, 11);
            Assert.AreEqual(infos[2].Hour, 12);
            Assert.AreEqual(infos[3].Hour, 13);
            Assert.AreEqual(infos[4].Hour, 13);
            Assert.AreEqual(infos[5].Hour, 14);
            Assert.AreEqual(infos[6].Hour, 14);
            Assert.AreEqual(infos[7].Hour, 15);
            Assert.AreEqual(infos[8].Hour, 15);
            Assert.AreEqual(infos[9].Hour, 16);
            Assert.AreEqual(infos[10].Hour, 18);
            Assert.AreEqual(infos[11].Hour, 21);
            Assert.AreEqual(infos[12].Hour, 22);
        }

        [Test]
        public void TestTopDrop2GCellCsv_ImportCsvStat()
        {
            string carrierInfo = stats[0].Carrier;
            TopDrop2GCellDaily stat = new TopDrop2GCellDaily();
            stat.ImportCarrierInfo(carrierInfo.GetSplittedFields('_'));
            int beginIndex = 0;
            Assert.AreEqual(stat.Import(stats, ref beginIndex, carrierInfo), "1_1377_1377_0_201");
            Assert.AreEqual(beginIndex, 10);
            Assert.AreEqual(stat.MainRssi, -82.0851);
            Assert.AreEqual(stat.AverageRssi, -85);
            Assert.AreEqual(stat.KpiCalls, 5672);
            stat.SubRssiHourInfo.AssertHourTest(new double[]{
                -109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109});
            stat.ErasureDropsHourInfo.AssertHourTest(new[]{
                0,0,0,0,0,0,0,0,0,8,18,19,13,7,15,10,19,11,13,6,10,4,4,3});
            Assert.IsNull(stat.DropCause);
        }

        [Test]
        public void TestTopDrop2GCellCsv_ImportCsvStat_11Elements()
        {
            stats[10].Carrier = "1_1377_1377_2_201";
            string carrierInfo = stats[0].Carrier;
            TopDrop2GCellDaily stat = new TopDrop2GCellDaily();
            stat.ImportCarrierInfo(carrierInfo.GetSplittedFields('_'));
            int beginIndex = 0;
            Assert.AreEqual(stat.Import(stats, ref beginIndex, carrierInfo), "");
            Assert.AreEqual(beginIndex, 11);
            Assert.AreEqual(stat.MainRssi, -82.0851);
            Assert.AreEqual(stat.AverageRssi, -85);
            Assert.AreEqual(stat.KpiCalls, 5672);
            stat.SubRssiHourInfo.AssertHourTest(new double[]{
                -109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109,-109});
            stat.ErasureDropsHourInfo.AssertHourTest(new[]{
                0,0,0,0,0,0,0,0,0,8,18,19,13,7,15,10,19,11,13,6,10,4,4,3});
            Assert.AreEqual(stat.DropCause, "1_46_46_1_283邻区问题;1_142_142_2_283邻区问题");
        }
    }
}
