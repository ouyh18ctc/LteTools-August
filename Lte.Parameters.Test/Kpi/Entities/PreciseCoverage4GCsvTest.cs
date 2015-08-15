using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Entities
{
    [TestFixture]
    public class PreciseCoverage4GCsvTest
    {
        private List<PreciseCoverage4GCsv> stats;

        [SetUp]
        public void SetUp()
        {
            const string testInput = @"时间,地市,BTS,BTSNAME,SECTOR,SECTORNAME,MR总数,第三强邻区MR重叠覆盖率,第二强邻区MR重叠覆盖率,第一强邻区MR重叠覆盖率
2015-04-26,佛山,501117,陈村电信机楼LBBU6,3,,2637,0,0,0
2015-04-26,佛山,499742,大良府又,2,大良府又_2,12524,2.2120,9.7410,34.1820
2015-04-26,佛山,499738,大良五坊,0,大良五坊_0,40910,0.2570,1.8550,9.3740
2015-04-26,佛山,501204,乐从电信LBBU3,1,乐从财神花园酒店主楼,1912,2.6670,5.5960,8.1590
2015-04-26,佛山,499740,大良金榜,1,大良金榜_1,7042,0.4120,5.4530,20.4630
2015-04-26,佛山,499754,大良李兆基中学,0,大良李兆基中学_0,12541,1.9620,11.8650,35.6510
2015-04-26,佛山,501171,伦教电信LBBU3,1,伦教中富房产有限公司_1,585,0,0,0.1710
2015-04-26,佛山,501182,容桂马岗接入机房LBBU1,2,容桂马岗乡府,3,0,0,0
2015-04-26,佛山,501230,大良南区电信LBBU3,0,大良新桂营业厅,5849,0.8380,7.1470,15.8320
2015-04-26,佛山,501209,德胜机楼LBBU2,2,大良顺德区报建中心_1,4,0,0,0
2015-04-26,佛山,499725,容桂红旗,2,容桂红旗_2,14323,1.2570,7.4150,26.3770
2015-04-26,佛山,501134,容奇机楼LBBU5,1,容桂泓都名邸,1387,0.0720,0.7210,17.4480
2015-04-26,佛山,499732,大良桂畔,0,大良桂畔_0,9178,0.73,2.2770,8.7060
2015-04-26,佛山,501190,乐从电信LBBU4,0,乐从钜隆商业城_0,3225,0.1860,4.5580,13.24";

            stats = CsvContext.ReadString<PreciseCoverage4GCsv>(testInput, CsvFileDescription.CommaDescription).ToList();
        }

        [Test]
        public void Test_Length()
        {
            Assert.AreEqual(stats.Count, 14);
        }

        [TestCase(0, 501117, 3, 2637, 0, 0, 0)]
        [TestCase(1, 499742, 2, 12524, 2.2120, 9.7410, 34.1820)]
        [TestCase(2, 499738, 0, 40910, 0.257, 1.855, 9.374)]
        [TestCase(3, 501204, 1, 1912, 2.667, 5.596, 8.159)]
        [TestCase(4, 499740, 1, 7042, 0.412, 5.453, 20.463)]
        [TestCase(5, 499754, 0, 12541, 1.962, 11.865, 35.651)]
        [TestCase(6, 501171, 1, 585, 0, 0, 0.171)]
        [TestCase(7, 501182, 2, 3, 0, 0, 0)]
        [TestCase(8, 501230, 0, 5849, 0.838, 7.147, 15.832)]
        [TestCase(9, 501209, 2, 4, 0, 0, 0)]
        [TestCase(10, 499725, 2, 14323, 1.257, 7.415, 26.377)]
        [TestCase(11, 501134, 1, 1387, 0.072, 0.721, 17.448)]
        [TestCase(12, 499732, 0, 9178, 0.73, 2.277, 8.706)]
        [TestCase(13, 501190, 0, 3225, 0.186, 4.558, 13.24)]
        public void Test_Details(int index, int eNodebId, byte sectorId, int totalMrs,
            double third, double second, double first)
        {
            PreciseCoverage4GCsv stat = stats[index];
            Assert.AreEqual(stat.StatTime, new DateTime(2015, 4, 26));
            Assert.AreEqual(stat.CellId, eNodebId);
            Assert.AreEqual(stat.SectorId, sectorId);
            Assert.AreEqual(stat.ThirdNeighborRate, third);
            Assert.AreEqual(stat.SecondNeighborRate, second);
            Assert.AreEqual(stat.FirstNeighborRate, first);
        }
    }
}
