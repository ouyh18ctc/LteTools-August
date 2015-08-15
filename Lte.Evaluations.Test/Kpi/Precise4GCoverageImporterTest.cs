using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;
using Lte.Evaluations.Kpi;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Kpi
{
    [TestFixture]
    public class Precise4GCoverageImporterTest
    {
        private readonly Mock<ITopCellRepository<PreciseCoverage4G>> cellRepository =
            new Mock<ITopCellRepository<PreciseCoverage4G>>();
        private readonly Mock<ITopCellRepository<TownPreciseCoverage4GStat>> townRepository =
            new Mock<ITopCellRepository<TownPreciseCoverage4GStat>>();

        private Precise4GCoverageImporter importer;

        [SetUp]
        public void SetUp()
        {
            cellRepository.MockOperations();
            townRepository.MockOperations();
            IEnumerable<ENodeb> eNodebs = new List<ENodeb>
            {
               new ENodeb {TownId = 1, ENodebId = 501117},
               new ENodeb {TownId = 1, ENodebId = 499742},
               new ENodeb {TownId = 1, ENodebId = 499738},
               new ENodeb {TownId = 1, ENodebId = 501204},
               new ENodeb {TownId = 1, ENodebId = 499740},
               new ENodeb {TownId = 2, ENodebId = 502885}
            };
            importer = new Precise4GCoverageImporter(cellRepository.Object, townRepository.Object, eNodebs);
        }

        [TestCase(@"时间,地市,BTS,BTSNAME,SECTOR,SECTORNAME,MR总数,第三强邻区MR重叠覆盖率,第二强邻区MR重叠覆盖率,第一强邻区MR重叠覆盖率
2015-04-29,佛山,501117,陈村电信机楼LBBU6,3,,692,0,0,0
2015-04-29,佛山,499742,大良府又,2,大良府又_2,16544,0.5260,6.2860,38.0020
2015-04-29,佛山,499738,大良五坊,0,大良五坊_0,30664,0.1080,2.7430,10.5630
2015-04-29,佛山,501204,乐从电信LBBU3,1,乐从财神花园酒店主楼,1449,3.8650,8.6960,22.8430
2015-04-29,佛山,499740,大良金榜,1,大良金榜_1,5639,0.1420,2.4830,19.4540",
            5,1,
            new []{692,16544,30664,1449,5639},
            new []{0,16544*0.526/100,30664*0.108/100,1449*3.865/100,5639*0.142/100},
            new []{0,16544*6.286/100,30664*2.743/100,1449*8.696/100,5639*2.483/100},
            new []{0,16544*38.002/100,30664*10.563/100,1449*22.843/100,5639*19.454/100},
            new []{692+16544+30664+1449+5639},
            new []{(16544*0.526+30664*0.108+1449*3.865+5639*0.142)/100},
            new []{(16544*6.286+30664*2.743+1449*8.696+5639*2.483)/100},
            new []{(16544*38.002+30664*10.563+1449*22.843+5639*19.454)/100})]
        [TestCase(@"时间,地市,BTS,BTSNAME,SECTOR,SECTORNAME,MR总数,第三强邻区MR重叠覆盖率,第二强邻区MR重叠覆盖率,第一强邻区MR重叠覆盖率
2015-04-29,佛山,502885,陈村电信机楼LBBU6,3,,692,0,0,0
2015-04-29,佛山,499742,大良府又,2,大良府又_2,16544,0.5260,6.2860,38.0020
2015-04-29,佛山,499738,大良五坊,0,大良五坊_0,30664,0.1080,2.7430,10.5630
2015-04-29,佛山,501204,乐从电信LBBU3,1,乐从财神花园酒店主楼,1449,3.8650,8.6960,22.8430
2015-04-29,佛山,499740,大良金榜,1,大良金榜_1,5639,0.1420,2.4830,19.4540",
            5, 2,
            new[] { 692, 16544, 30664, 1449, 5639 },
            new[] { 0, 16544 * 0.526 / 100, 30664 * 0.108 / 100, 1449 * 3.865 / 100, 5639 * 0.142 / 100 },
            new[] { 0, 16544 * 6.286 / 100, 30664 * 2.743 / 100, 1449 * 8.696 / 100, 5639 * 2.483 / 100 },
            new[] { 0, 16544 * 38.002 / 100, 30664 * 10.563 / 100, 1449 * 22.843 / 100, 5639 * 19.454 / 100 },
            new[] {  16544 + 30664 + 1449 + 5639, 692 },
            new[] { (16544 * 0.526 + 30664 * 0.108 + 1449 * 3.865 + 5639 * 0.142) / 100, 0 },
            new[] { (16544 * 6.286 + 30664 * 2.743 + 1449 * 8.696 + 5639 * 2.483) / 100, 0 },
            new[] { (16544 * 38.002 + 30664 * 10.563 + 1449 * 22.843 + 5639 * 19.454) / 100, 0 })]
        public void Test_ImportStat(string csvContents, int cellCount, int townCount,
            int[] totalMrsByCell,
            double[] thirdNeighborCellCountsByCell,
            double[] secondNeighborCellCountsByCell,
            double[] firstNeighborCellCountsByCell,
            int[] totalMrsByTown,
            double[] thirdNeighborCellCountsByTown,
            double[] secondNeighborCellCountsByTown,
            double[] firstNeighborCellCountsByTown)
        {
            StreamReader reader = csvContents.GetStreamReader();
            GetValue(cellCount, townCount, totalMrsByCell, thirdNeighborCellCountsByCell, secondNeighborCellCountsByCell, firstNeighborCellCountsByCell, totalMrsByTown, thirdNeighborCellCountsByTown, secondNeighborCellCountsByTown, firstNeighborCellCountsByTown, reader);
        }

        private async void GetValue(int cellCount, int townCount, int[] totalMrsByCell, double[] thirdNeighborCellCountsByCell,
            double[] secondNeighborCellCountsByCell, double[] firstNeighborCellCountsByCell, int[] totalMrsByTown,
            double[] thirdNeighborCellCountsByTown, double[] secondNeighborCellCountsByTown,
            double[] firstNeighborCellCountsByTown, StreamReader reader)
        {
            Assert.AreEqual(await importer.ImportStat(reader, CsvFileDescription.CommaDescription), cellCount);
            Assert.AreEqual(cellRepository.Object.Stats.Count(), cellCount);
            Assert.AreEqual(townRepository.Object.Stats.Count(), townCount);
            for (int i = 0; i < cellCount; i++)
            {
                PreciseCoverage4G stat = cellRepository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.TotalMrs, totalMrsByCell[i]);
                Assert.AreEqual(stat.ThirdNeighbors, (int) thirdNeighborCellCountsByCell[i]);
                Assert.AreEqual(stat.SecondNeighbors, (int) secondNeighborCellCountsByCell[i]);
                Assert.AreEqual(stat.FirstNeighbors, (int) firstNeighborCellCountsByCell[i]);
            }
            for (int i = 0; i < townCount; i++)
            {
                TownPreciseCoverage4GStat stat = townRepository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.TotalMrs, totalMrsByTown[i]);
                Assert.AreEqual(stat.ThirdNeighbors, (int) thirdNeighborCellCountsByTown[i]);
                Assert.AreEqual(stat.SecondNeighbors, (int) secondNeighborCellCountsByTown[i]);
                Assert.AreEqual(stat.FirstNeighbors, (int) firstNeighborCellCountsByTown[i]);
            }
        }
    }
}
