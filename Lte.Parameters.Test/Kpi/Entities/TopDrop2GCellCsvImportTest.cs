using System.Globalization;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Entities
{
    [TestFixture]
    public class TopDrop2GCellCsvImportTest
    {
        private class FakeCdrDrops : ICdrDrops
        {
            public int CdrCalls { get; set; }
            public int CdrDrops { get; set; }
            public CdrDropsDistanceInfo CdrDropsDistanceInfo { get; set; }
            public double AverageRssi { get; set; }
            public double AverageDropEcio { get; set; }
            public double AverageDropDistance { get; set; }
            public CdrDropsHourInfo CdrDropsHourInfo { get; set; }
        }

        private class FakeDropEcio : IDropEcio
        {
            public DropEcioDistanceInfo DropEcioDistanceInfo { get; set; }
            public DropEcioHourInfo DropEcioHourInfo { get; set; }
        }

        private class FakeGoodEcio : IGoodEcio
        {
            public GoodEcioDistanceInfo GoodEcioDistanceInfo { get; set; }
        }

        private class FakeCdrCalls : ICdrCalls
        {
            public CdrCallsDistanceInfo CdrCallsDistanceInfo { get; set; }
            public CdrCallsHourInfo CdrCallsHourInfo { get; set; }
        }

        private class FakeKpiCalls : IKpiCalls
        {
            public int KpiCalls { get; set; }
            public int KpiDrops { get; set; }
            public KpiCallsHourInfo KpiCallsHourInfo { get; set; }
        }

        private class FakeMainRssi : IMainRssi
        {
            public double MainRssi { get; set; }
            public MainRssiHourInfo MainRssiHourInfo { get; set; }
        }

        private class FakeSubRssi : ISubRssi
        {
            public double SubRssi { get; set; }
            public SubRssiHourInfo SubRssiHourInfo { get; set; }
        }

        private class FakeKpiDrops : IKpiDrops
        {
            public KpiDropsHourInfo KpiDropsHourInfo { get; set; }
        }

        private static void AssertDistanceInfos<T>(IDrop2GDistanceInfo<T> source, T[] dest)
        {
            Assert.AreEqual(source.DistanceTo200Info, dest[0]);
            Assert.AreEqual(source.DistanceTo400Info, dest[1]);
            Assert.AreEqual(source.DistanceTo600Info, dest[2]);
            Assert.AreEqual(source.DistanceTo800Info, dest[3]);
            Assert.AreEqual(source.DistanceTo1000Info, dest[4]);
            Assert.AreEqual(source.DistanceTo1200Info, dest[5]);
            Assert.AreEqual(source.DistanceTo1400Info, dest[6]);
            Assert.AreEqual(source.DistanceTo1600Info, dest[7]);
            Assert.AreEqual(source.DistanceTo1800Info, dest[8]);
            Assert.AreEqual(source.DistanceTo2000Info, dest[9]);
            Assert.AreEqual(source.DistanceTo2200Info, dest[10]);
            Assert.AreEqual(source.DistanceTo2400Info, dest[11]);
            Assert.AreEqual(source.DistanceTo2600Info, dest[12]);
            Assert.AreEqual(source.DistanceTo2800Info, dest[13]);
            Assert.AreEqual(source.DistanceTo3000Info, dest[14]);
            Assert.AreEqual(source.DistanceTo4000Info, dest[15]);
            Assert.AreEqual(source.DistanceTo5000Info, dest[16]);
            Assert.AreEqual(source.DistanceTo6000Info, dest[17]);
            Assert.AreEqual(source.DistanceTo7000Info, dest[18]);
            Assert.AreEqual(source.DistanceTo8000Info, dest[19]);
            Assert.AreEqual(source.DistanceTo9000Info, dest[20]);
            Assert.AreEqual(source.DistanceToInfInfo, dest[21]);
        }

        private static void AssertHourInfos<T>(IDrop2GHourInfo<T> source, T[] dest)
        {
            Assert.AreEqual(source.Hour0Info, dest[0]);
            Assert.AreEqual(source.Hour1Info, dest[1]);
            Assert.AreEqual(source.Hour2Info, dest[2]);
            Assert.AreEqual(source.Hour3Info, dest[3]);
            Assert.AreEqual(source.Hour4Info, dest[4]);
            Assert.AreEqual(source.Hour5Info, dest[5]);
            Assert.AreEqual(source.Hour6Info, dest[6]);
            Assert.AreEqual(source.Hour7Info, dest[7]);
            Assert.AreEqual(source.Hour8Info, dest[8]);
            Assert.AreEqual(source.Hour9Info, dest[9]);
            Assert.AreEqual(source.Hour10Info, dest[10]);
            Assert.AreEqual(source.Hour11Info, dest[11]);
            Assert.AreEqual(source.Hour12Info, dest[12]);
            Assert.AreEqual(source.Hour13Info, dest[13]);
            Assert.AreEqual(source.Hour14Info, dest[14]);
            Assert.AreEqual(source.Hour15Info, dest[15]);
            Assert.AreEqual(source.Hour16Info, dest[16]);
            Assert.AreEqual(source.Hour17Info, dest[17]);
            Assert.AreEqual(source.Hour18Info, dest[18]);
            Assert.AreEqual(source.Hour19Info, dest[19]);
            Assert.AreEqual(source.Hour20Info, dest[20]);
            Assert.AreEqual(source.Hour21Info, dest[21]);
            Assert.AreEqual(source.Hour22Info, dest[22]);
            Assert.AreEqual(source.Hour23Info, dest[23]);
        }

        [TestCase(232, 223, 1.2, 12.2, 3456.77,
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(232, 2333, 7.2, 19.2, 346.677,
            new[] { 1, 2, 3, 4, 56, 6, 7, 8, 9, 10, 11, 1, 13, 14, 15, 16, 17, 18, 19, 2654, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(562, 8733, 76.2, 49.2, 3766.677,
            new[] { 1, 87, 3, 4, 56, 6, 7, 8, 99, 10, 11, 1, 13, 14, 15, 16, 17, 18, 19, 2654, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        public void Test_ImportCdrDrops(int cdrCalls, int cdrDrops, 
            double averageRssi, double averageDropEcio, double averageDropDistance,
            int[] distanceInfos, int[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                Calls = cdrCalls.ToString(CultureInfo.InvariantCulture),
                Drops = cdrDrops.ToString(CultureInfo.InvariantCulture),
                AverageRssi = averageRssi.ToString(CultureInfo.InvariantCulture),
                AverageDropEcio = averageDropEcio.ToString(CultureInfo.InvariantCulture),
                AverageDropDistance = averageDropDistance.ToString(CultureInfo.InvariantCulture),
                DistanceTo200Info = distanceInfos[0].ToString(CultureInfo.InvariantCulture),
                DistanceTo400Info = distanceInfos[1].ToString(CultureInfo.InvariantCulture),
                DistanceTo600Info = distanceInfos[2].ToString(CultureInfo.InvariantCulture),
                DistanceTo800Info = distanceInfos[3].ToString(CultureInfo.InvariantCulture),
                DistanceTo1000Info = distanceInfos[4].ToString(CultureInfo.InvariantCulture),
                DistanceTo1200Info = distanceInfos[5].ToString(CultureInfo.InvariantCulture),
                DistanceTo1400Info = distanceInfos[6].ToString(CultureInfo.InvariantCulture),
                DistanceTo1600Info = distanceInfos[7].ToString(CultureInfo.InvariantCulture),
                DistanceTo1800Info = distanceInfos[8].ToString(CultureInfo.InvariantCulture),
                DistanceTo2000Info = distanceInfos[9].ToString(CultureInfo.InvariantCulture),
                DistanceTo2200Info = distanceInfos[10].ToString(CultureInfo.InvariantCulture),
                DistanceTo2400Info = distanceInfos[11].ToString(CultureInfo.InvariantCulture),
                DistanceTo2600Info = distanceInfos[12].ToString(CultureInfo.InvariantCulture),
                DistanceTo2800Info = distanceInfos[13].ToString(CultureInfo.InvariantCulture),
                DistanceTo3000Info = distanceInfos[14].ToString(CultureInfo.InvariantCulture),
                DistanceTo4000Info = distanceInfos[15].ToString(CultureInfo.InvariantCulture),
                DistanceTo5000Info = distanceInfos[16].ToString(CultureInfo.InvariantCulture),
                DistanceTo6000Info = distanceInfos[17].ToString(CultureInfo.InvariantCulture),
                DistanceTo7000Info = distanceInfos[18].ToString(CultureInfo.InvariantCulture),
                DistanceTo8000Info = distanceInfos[19].ToString(CultureInfo.InvariantCulture),
                DistanceTo9000Info = distanceInfos[20].ToString(CultureInfo.InvariantCulture),
                DistanceToInfInfo = distanceInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            ICdrDrops stat = new FakeCdrDrops();
            info.ImportCdrDrops(stat);
            Assert.AreEqual(stat.CdrCalls, cdrCalls);
            Assert.AreEqual(stat.CdrDrops, cdrDrops);
            Assert.AreEqual(stat.AverageDropDistance, averageDropDistance);
            Assert.AreEqual(stat.AverageDropEcio, averageDropEcio);
            Assert.AreEqual(stat.AverageRssi, averageRssi);
            AssertDistanceInfos(stat.CdrDropsDistanceInfo, distanceInfos);
            AssertHourInfos(stat.CdrDropsHourInfo, hourInfos);
        }

        [TestCase(
            new[] { 1, 2, 3, 4, 5, 6, 7.9, 8, 9, 10, 11, 12, 13, 14.98, 15, 16, 17, 18, 19, 20, 21.87, 22 },
            new[] { 1, 2, 3, 4.67, 5, 6.8, 7, 8, 9, 10, 11, 12, 13, 14, 15.5, 16, 17.99, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(
            new[] { 1, 2, 3, 4, 5.6, 6, 7, 8.99, 9, 10, 11, 1, 13, 14, 1.95, 16, 17, 18, 19, 2.654, 21, 22 },
            new[] { 1, 2, 3.98, 4, 5, 6.7, 7, 8, 9, 0, 11, 12, 13.43, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(
            new[] { 1, 87, 3.6, 4, 56, 6, 7, 8, 99, 10, 11, 1, 13, 14, 15, 16, 17.988, 18, 19, 26.54, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0.98, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(
            new[] { 1, 8.97, 3.6, 4, 56, 6, 7, 8, 9.9, 10, 11, 1, 13, 14, 15, 16, 17.988, 18, 19, 26.54, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0.98, 11, 12, 13, 14.99, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        public void Test_ImportDropEcio(double[] distanceInfos, double[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                DistanceTo200Info = distanceInfos[0].ToString(CultureInfo.InvariantCulture),
                DistanceTo400Info = distanceInfos[1].ToString(CultureInfo.InvariantCulture),
                DistanceTo600Info = distanceInfos[2].ToString(CultureInfo.InvariantCulture),
                DistanceTo800Info = distanceInfos[3].ToString(CultureInfo.InvariantCulture),
                DistanceTo1000Info = distanceInfos[4].ToString(CultureInfo.InvariantCulture),
                DistanceTo1200Info = distanceInfos[5].ToString(CultureInfo.InvariantCulture),
                DistanceTo1400Info = distanceInfos[6].ToString(CultureInfo.InvariantCulture),
                DistanceTo1600Info = distanceInfos[7].ToString(CultureInfo.InvariantCulture),
                DistanceTo1800Info = distanceInfos[8].ToString(CultureInfo.InvariantCulture),
                DistanceTo2000Info = distanceInfos[9].ToString(CultureInfo.InvariantCulture),
                DistanceTo2200Info = distanceInfos[10].ToString(CultureInfo.InvariantCulture),
                DistanceTo2400Info = distanceInfos[11].ToString(CultureInfo.InvariantCulture),
                DistanceTo2600Info = distanceInfos[12].ToString(CultureInfo.InvariantCulture),
                DistanceTo2800Info = distanceInfos[13].ToString(CultureInfo.InvariantCulture),
                DistanceTo3000Info = distanceInfos[14].ToString(CultureInfo.InvariantCulture),
                DistanceTo4000Info = distanceInfos[15].ToString(CultureInfo.InvariantCulture),
                DistanceTo5000Info = distanceInfos[16].ToString(CultureInfo.InvariantCulture),
                DistanceTo6000Info = distanceInfos[17].ToString(CultureInfo.InvariantCulture),
                DistanceTo7000Info = distanceInfos[18].ToString(CultureInfo.InvariantCulture),
                DistanceTo8000Info = distanceInfos[19].ToString(CultureInfo.InvariantCulture),
                DistanceTo9000Info = distanceInfos[20].ToString(CultureInfo.InvariantCulture),
                DistanceToInfInfo = distanceInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            IDropEcio stat = new FakeDropEcio();
            info.ImportDropEcio(stat);
            AssertDistanceInfos(stat.DropEcioDistanceInfo, distanceInfos);
            AssertHourInfos(stat.DropEcioHourInfo, hourInfos);
        }

        [TestCase(
            new[] { 1, 2, 3, 4, 5, 6, 7.9, 8, 9, 10, 11, 12, 13, 14.98, 15, 16, 17, 18, 19, 20, 21.87, 22 })]
        [TestCase(
            new[] { 1, 2, 3, 4, 5.6, 6, 7, 8.99, 9, 10, 11, 1, 13, 14, 1.95, 16, 17, 18, 19, 2.654, 21, 22 })]
        [TestCase(
            new[] { 1, 87, 3.6, 4, 56, 6, 7, 8, 99, 10, 11, 1, 13, 14, 15, 16, 17.988, 18, 19, 26.54, 21, 22 })]
        [TestCase(
            new[] { 1, 8.97, 3.6, 4, 56, 6, 7, 8, 9.9, 10, 11, 1, 13, 14, 15, 16, 17.988, 18, 19, 26.54, 21, 22 })]
        [TestCase(
            new[] { 1, 8.97, 3.6, 4, 56, 6, 7, 8.33, 9.9, 10, 1.1, 1, 13, 14, 15, 16, 17.988, 18, 19, 26.54, 21, 22 })]
        public void Test_ImportGoodEcio(double[] distanceInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                DistanceTo200Info = distanceInfos[0].ToString(CultureInfo.InvariantCulture),
                DistanceTo400Info = distanceInfos[1].ToString(CultureInfo.InvariantCulture),
                DistanceTo600Info = distanceInfos[2].ToString(CultureInfo.InvariantCulture),
                DistanceTo800Info = distanceInfos[3].ToString(CultureInfo.InvariantCulture),
                DistanceTo1000Info = distanceInfos[4].ToString(CultureInfo.InvariantCulture),
                DistanceTo1200Info = distanceInfos[5].ToString(CultureInfo.InvariantCulture),
                DistanceTo1400Info = distanceInfos[6].ToString(CultureInfo.InvariantCulture),
                DistanceTo1600Info = distanceInfos[7].ToString(CultureInfo.InvariantCulture),
                DistanceTo1800Info = distanceInfos[8].ToString(CultureInfo.InvariantCulture),
                DistanceTo2000Info = distanceInfos[9].ToString(CultureInfo.InvariantCulture),
                DistanceTo2200Info = distanceInfos[10].ToString(CultureInfo.InvariantCulture),
                DistanceTo2400Info = distanceInfos[11].ToString(CultureInfo.InvariantCulture),
                DistanceTo2600Info = distanceInfos[12].ToString(CultureInfo.InvariantCulture),
                DistanceTo2800Info = distanceInfos[13].ToString(CultureInfo.InvariantCulture),
                DistanceTo3000Info = distanceInfos[14].ToString(CultureInfo.InvariantCulture),
                DistanceTo4000Info = distanceInfos[15].ToString(CultureInfo.InvariantCulture),
                DistanceTo5000Info = distanceInfos[16].ToString(CultureInfo.InvariantCulture),
                DistanceTo6000Info = distanceInfos[17].ToString(CultureInfo.InvariantCulture),
                DistanceTo7000Info = distanceInfos[18].ToString(CultureInfo.InvariantCulture),
                DistanceTo8000Info = distanceInfos[19].ToString(CultureInfo.InvariantCulture),
                DistanceTo9000Info = distanceInfos[20].ToString(CultureInfo.InvariantCulture),
                DistanceToInfInfo = distanceInfos[21].ToString(CultureInfo.InvariantCulture)
            };
            IGoodEcio stat = new FakeGoodEcio();
            info.ImportGoodEcio(stat);
            AssertDistanceInfos(stat.GoodEcioDistanceInfo, distanceInfos);
        }

        [TestCase(
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(
            new[] { 1, 2, 3, 4, 56, 6, 7, 8, 9, 10, 11, 1, 13, 14, 15, 16, 17, 18, 19, 2654, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(
            new[] { 1, 87, 3, 4, 56, 6, 7, 8, 99, 10, 11, 1, 13, 14, 15, 16, 17, 18, 19, 2654, 21, 22 },
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        public void Test_ImportCdrCalls(int[] distanceInfos, int[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                DistanceTo200Info = distanceInfos[0].ToString(CultureInfo.InvariantCulture),
                DistanceTo400Info = distanceInfos[1].ToString(CultureInfo.InvariantCulture),
                DistanceTo600Info = distanceInfos[2].ToString(CultureInfo.InvariantCulture),
                DistanceTo800Info = distanceInfos[3].ToString(CultureInfo.InvariantCulture),
                DistanceTo1000Info = distanceInfos[4].ToString(CultureInfo.InvariantCulture),
                DistanceTo1200Info = distanceInfos[5].ToString(CultureInfo.InvariantCulture),
                DistanceTo1400Info = distanceInfos[6].ToString(CultureInfo.InvariantCulture),
                DistanceTo1600Info = distanceInfos[7].ToString(CultureInfo.InvariantCulture),
                DistanceTo1800Info = distanceInfos[8].ToString(CultureInfo.InvariantCulture),
                DistanceTo2000Info = distanceInfos[9].ToString(CultureInfo.InvariantCulture),
                DistanceTo2200Info = distanceInfos[10].ToString(CultureInfo.InvariantCulture),
                DistanceTo2400Info = distanceInfos[11].ToString(CultureInfo.InvariantCulture),
                DistanceTo2600Info = distanceInfos[12].ToString(CultureInfo.InvariantCulture),
                DistanceTo2800Info = distanceInfos[13].ToString(CultureInfo.InvariantCulture),
                DistanceTo3000Info = distanceInfos[14].ToString(CultureInfo.InvariantCulture),
                DistanceTo4000Info = distanceInfos[15].ToString(CultureInfo.InvariantCulture),
                DistanceTo5000Info = distanceInfos[16].ToString(CultureInfo.InvariantCulture),
                DistanceTo6000Info = distanceInfos[17].ToString(CultureInfo.InvariantCulture),
                DistanceTo7000Info = distanceInfos[18].ToString(CultureInfo.InvariantCulture),
                DistanceTo8000Info = distanceInfos[19].ToString(CultureInfo.InvariantCulture),
                DistanceTo9000Info = distanceInfos[20].ToString(CultureInfo.InvariantCulture),
                DistanceToInfInfo = distanceInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            ICdrCalls stat = new FakeCdrCalls();
            info.ImportCdrCalls(stat);
            AssertDistanceInfos(stat.CdrCallsDistanceInfo, distanceInfos);
            AssertHourInfos(stat.CdrCallsHourInfo, hourInfos);
        }

        [TestCase(3, 4,
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(56, 6,
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(99, 10,
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        public void Test_ImportKpiCalls(int kpiCalls, int kpiDrops, int[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                Calls = kpiCalls.ToString(CultureInfo.InvariantCulture),
                Drops = kpiDrops.ToString(CultureInfo.InvariantCulture),
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            IKpiCalls stat = new FakeKpiCalls();
            info.ImportKpiCalls(stat);
            Assert.AreEqual(stat.KpiCalls, kpiCalls);
            Assert.AreEqual(stat.KpiDrops, kpiDrops);
            AssertHourInfos(stat.KpiCallsHourInfo, hourInfos);
        }

        [TestCase(
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(
            new[] { 1, 2, 3, 4, 5, 67, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 19, 20, 21, 22, 23, 289 })]
        public void Test_ImportKpiDrops(int[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            IKpiDrops stat = new FakeKpiDrops();
            info.ImportKpiDrops(stat);
            AssertHourInfos(stat.KpiDropsHourInfo, hourInfos);
        }

        [TestCase(23.1,
            new[] { 1, 2, 3, 4, 5.3, 6, 7, 8, 9, 10, 11.9, 12, 13, 14, 15, 16, 17, 18.7, 19, 20, 21, 22, 23, 24 })]
        [TestCase(12.1,
            new[] { 1, 2, 3, 4.2, 5, 6.7, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 19.9, 18, 19, 20, 21, 22, 23, 289 })]
        [TestCase(22.4,
            new[] { 1, 2, 3.8, 4, 5, 67, 7.9, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 1.9, 20, 21, 22, 23, 289 })]
        public void Test_ImportMainRssi(double rssi, double[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                AverageRssi = rssi.ToString(CultureInfo.InvariantCulture),
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            IMainRssi stat = new FakeMainRssi();
            info.ImportMainRssi(stat);
            Assert.AreEqual(stat.MainRssi, rssi);
            AssertHourInfos(stat.MainRssiHourInfo, hourInfos);
        }

        [TestCase(23.1,
            new[] {1, 2, 3, 4, 5.3, 6, 7, 8, 9, 10, 11.9, 12, 13, 14, 15, 16, 17, 18.7, 19, 20, 21, 22, 23, 24})]
        [TestCase(12.1,
            new[] {1, 2, 3, 4.2, 5, 6.7, 7, 8, 9, 0, 11, 12, 13, 14, 15, 16, 19.9, 18, 19, 20, 21, 22, 23, 289})]
        [TestCase(22.4,
            new[] {1, 2, 3.8, 4, 5, 67, 7.9, 8, 9, 0, 11, 12, 13, 14, 15, 16, 199, 18, 1.9, 20, 21, 22, 23, 289})]
        public void Test_ImportSubRssi(double rssi, double[] hourInfos)
        {
            TopDrop2GCellCsv info = new TopDrop2GCellCsv
            {
                AverageRssi = rssi.ToString(CultureInfo.InvariantCulture),
                Hour0Info = hourInfos[0].ToString(CultureInfo.InvariantCulture),
                Hour1Info = hourInfos[1].ToString(CultureInfo.InvariantCulture),
                Hour2Info = hourInfos[2].ToString(CultureInfo.InvariantCulture),
                Hour3Info = hourInfos[3].ToString(CultureInfo.InvariantCulture),
                Hour4Info = hourInfos[4].ToString(CultureInfo.InvariantCulture),
                Hour5Info = hourInfos[5].ToString(CultureInfo.InvariantCulture),
                Hour6Info = hourInfos[6].ToString(CultureInfo.InvariantCulture),
                Hour7Info = hourInfos[7].ToString(CultureInfo.InvariantCulture),
                Hour8Info = hourInfos[8].ToString(CultureInfo.InvariantCulture),
                Hour9Info = hourInfos[9].ToString(CultureInfo.InvariantCulture),
                Hour10Info = hourInfos[10].ToString(CultureInfo.InvariantCulture),
                Hour11Info = hourInfos[11].ToString(CultureInfo.InvariantCulture),
                Hour12Info = hourInfos[12].ToString(CultureInfo.InvariantCulture),
                Hour13Info = hourInfos[13].ToString(CultureInfo.InvariantCulture),
                Hour14Info = hourInfos[14].ToString(CultureInfo.InvariantCulture),
                Hour15Info = hourInfos[15].ToString(CultureInfo.InvariantCulture),
                Hour16Info = hourInfos[16].ToString(CultureInfo.InvariantCulture),
                Hour17Info = hourInfos[17].ToString(CultureInfo.InvariantCulture),
                Hour18Info = hourInfos[18].ToString(CultureInfo.InvariantCulture),
                Hour19Info = hourInfos[19].ToString(CultureInfo.InvariantCulture),
                Hour20Info = hourInfos[20].ToString(CultureInfo.InvariantCulture),
                Hour21Info = hourInfos[21].ToString(CultureInfo.InvariantCulture),
                Hour22Info = hourInfos[22].ToString(CultureInfo.InvariantCulture),
                Hour23Info = hourInfos[23].ToString(CultureInfo.InvariantCulture)
            };
            ISubRssi stat = new FakeSubRssi();
            info.ImportSubRssi(stat);
            Assert.AreEqual(stat.SubRssi, rssi);
            AssertHourInfos(stat.SubRssiHourInfo, hourInfos);
        }
    }
}
