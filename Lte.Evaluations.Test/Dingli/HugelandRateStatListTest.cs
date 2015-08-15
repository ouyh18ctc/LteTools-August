using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Dingli;
using Lte.Domain.LinqToCsv.Context;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class HugelandRateStatListTest : TabCsvReader
    {
        private List<RateStat> rateStatList;

        [SetUp]
        public void TestInitialize()
        {
            HugelandDescriptionInitialize();

        }

        [Test]
        public void TestHugelandRateStatList()
        {
            testInput = @"Index,Device,Time,Lon,Lat,Events,UE Timestamp,Extra Info,Direction,Signaling,Signaling Data,eNodeB ID,Cell ID,Frequency DL(MHz),PCI,CRS RSRP,CRS SINR,DL BLER(%),CQI Average,UL MCS Value # Average,DL MCS Value # Average,PDCP Thr'put UL(kb/s),PDCP Thr'put DL(kb/s),PHY Thr'put DL(kb/s),MAC Thr'put DL(kb/s),PUSCH Rb Num/s,PDSCH Rb Num/s,PUSCH TB Size Ave(bits),PDSCH TB Size Ave(bits)
1,UE1,01:11:48.000,,,,,,,,,,,2120.0,38,,,,,,,,,,,,,,
2,UE1,01:11:48.500,,,,,,,,,,,2120.0,38,,,,,,,,,,,,,,
3,UE1,01:11:49.000,,,,,,,,,,,2120.0,38,-84.4,15.9,,,,,,,,,,,,
4,UE1,01:11:49.500,113.14016,23.07023,,,,,,,,,2120.0,38,-84.4,15.9,,,,,0.0,0.0,0.0,0.0,0,,,
5,UE1,01:11:50.000,113.14016,23.07023,,,,,,,,,2120.0,38,-84.4,15.9,,,,,0.0,0.0,0.0,0.0,0,,,
6,UE1,01:11:50.500,113.14016,23.07023,,,,,,,,,2120.0,38,-85.0,15.1,,,,,0.0,0.0,0.0,0.0,0,,,
7,UE1,01:11:51.000,113.14016,23.07023,,,,,,,,,2120.0,38,-85.0,15.1,,,,,0.0,0.0,0.0,0.0,0,,,
8,UE1,01:11:51.500,113.14016,23.07023,,,,,,,,,2120.0,38,-85.2,15.0,,,,,0.0,0.0,0.0,0.0,0,,,
9,UE1,01:11:52.000,113.14016,23.07023,,,,,,,,,2120.0,38,-85.2,15.0,,,,,0.0,0.0,0.0,0.0,0,,,
10,UE1,01:11:52.500,113.14016,23.07023,,,,,,,,,2120.0,38,-85.2,15.0,,,,,0.0,0.0,0.0,0.0,0,,,
11,UE1,01:11:53.000,113.14016,23.07023,,,,,,,,,2120.0,38,-84.3,15.5,,,,,0.0,0.0,0.0,0.0,0,,,
12,UE1,01:11:53.500,113.14016,23.07023,,,,,,,,,2120.0,38,-84.3,15.5,,,,,0.0,0.0,0.0,0.0,0,,,";
            rateStatList = CsvContext.ReadString<HugelandRecord>(testInput, fileDescription_namesUs).ToList().MergeStat();
            Assert.AreEqual(rateStatList.Count, 12);
            Assert.AreEqual(rateStatList[0].Earfcn, 100);
            Assert.AreEqual(rateStatList[0].Pci, 38);
            Assert.AreEqual(rateStatList[3].Rsrp, -84.4);
            Assert.AreEqual(rateStatList[3].Sinr, 15.9);
            Assert.AreEqual(rateStatList[3].Time.ToString("HH:mm:ss.fff"), "01:11:49.500");
        }

        [Test]
        public void TestHugelandRateStatList_2()
        {
            testInput = TabCsvReader.HugelandRecordExample;
            rateStatList = CsvContext.ReadString<HugelandRecord>(testInput, fileDescription_namesUs).ToList().MergeStat();
            Assert.AreEqual(rateStatList.Count, 19);
            Assert.AreEqual(rateStatList[0].Earfcn, 100);
            Assert.AreEqual(rateStatList[0].Pci, 99);
            Assert.AreEqual(rateStatList[3].Rsrp, -94.2);
            Assert.AreEqual(rateStatList[3].Sinr, 2.1);
            Assert.AreEqual(rateStatList[3].AverageCqi, 5.7);
            Assert.AreEqual(rateStatList[3].UlMcs, 22);
            Assert.AreEqual(rateStatList[3].DlMcs, 10);
            Assert.AreEqual(rateStatList[3].UlThroughput, 203161);
            Assert.AreEqual(rateStatList[3].DlThroughput, 13436006);
            Assert.AreEqual(rateStatList[3].PhyThroughputCode0, 15638220);
            Assert.AreEqual(rateStatList[3].PuschRbRate, 960);
            Assert.AreEqual(rateStatList[3].PdschRbRate, 176304);
            Assert.AreEqual(rateStatList[3].PdschTbCode0, 15241);
            Assert.AreEqual(rateStatList[3].Time.ToString("HH:mm:ss.fff"), "01:14:01.500");
        }
    }
}
