using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Kpi.Service;
using NUnit.Framework;
using Moq;

namespace Lte.Parameters.Test.Kpi.Service
{
    [TestFixture]
    public class QueryTopDrop2GServiceTest
    {
        private Mock<ITopCellRepository<TopDrop2GCellDaily>> mockRepository =
            new Mock<ITopCellRepository<TopDrop2GCellDaily>>();

        private QueryTopDrop2GService _service;
        private List<TopDrop2GCellDaily> statList;

        [SetUp]
        public void SetUp()
        {
            statList = new List<TopDrop2GCellDaily>();
            _service = new QueryTopDrop2GService(mockRepository.Object,
                1, 2, 3, DateTime.Today);
        }

        [TestCase(101, 23)]
        [TestCase(201, 48)]
        [TestCase(1010, 77)]
        [TestCase(2101, 4673)]
        public void Test_QueryStat_CdrDrops(int cdrDrops, int kpiDrops)
        {
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                CdrDrops = cdrDrops,
                KpiDrops = kpiDrops
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            TopDrop2GCellDaily stat = _service.QueryStat();
            Assert.AreEqual(stat.CdrDrops, cdrDrops);
            Assert.AreEqual(stat.KpiDrops, kpiDrops);
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 122, 2, 3, 4, 5543, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1632, 17, 18, 19, 250, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 832, 9, 10, 11, 12, 137, 14, 15, 156, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 932, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        public void Test_GenerateDistanceDistribution_CdrCallsDistanceInfo(
            int[] values)
        {
            CdrCallsDistanceInfo info = new CdrCallsDistanceInfo
            {
                DistanceTo200Info = values[0],
                DistanceTo400Info = values[1],
                DistanceTo600Info = values[2],
                DistanceTo800Info = values[3],
                DistanceTo1000Info = values[4],
                DistanceTo1200Info = values[5],
                DistanceTo1400Info = values[6],
                DistanceTo1600Info = values[7],
                DistanceTo1800Info = values[8],
                DistanceTo2000Info = values[9],
                DistanceTo2200Info = values[10],
                DistanceTo2400Info = values[11],
                DistanceTo2600Info = values[12],
                DistanceTo2800Info = values[13],
                DistanceTo3000Info = values[14],
                DistanceTo4000Info = values[15],
                DistanceTo5000Info = values[16],
                DistanceTo6000Info = values[17],
                DistanceTo7000Info = values[18],
                DistanceTo8000Info = values[19],
                DistanceTo9000Info = values[20],
                DistanceToInfInfo = values[21]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                CdrCallsDistanceInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DistanceDistribution> distribution = _service.GenerateDistanceDistribution();
            Assert.AreEqual(distribution.Count, 22);
            for (int i = 0; i < 22; i++)
            {
                Assert.AreEqual(distribution[i].CdrCalls, values[i]);
            }
        }

        [TestCase(new[] { 1, 9, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 56, 6, 7, 8, 9, 10, 11, 342, 13, 564, 15, 16, 17, 18, 19, 232, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 34, 38, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 68, 13, 14, 15, 134, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        public void Test_GenerateDistanceDistribution_CdrDropsDistanceInfo(
            int[] values)
        {
            CdrDropsDistanceInfo info = new CdrDropsDistanceInfo
            {
                DistanceTo200Info = values[0],
                DistanceTo400Info = values[1],
                DistanceTo600Info = values[2],
                DistanceTo800Info = values[3],
                DistanceTo1000Info = values[4],
                DistanceTo1200Info = values[5],
                DistanceTo1400Info = values[6],
                DistanceTo1600Info = values[7],
                DistanceTo1800Info = values[8],
                DistanceTo2000Info = values[9],
                DistanceTo2200Info = values[10],
                DistanceTo2400Info = values[11],
                DistanceTo2600Info = values[12],
                DistanceTo2800Info = values[13],
                DistanceTo3000Info = values[14],
                DistanceTo4000Info = values[15],
                DistanceTo5000Info = values[16],
                DistanceTo6000Info = values[17],
                DistanceTo7000Info = values[18],
                DistanceTo8000Info = values[19],
                DistanceTo9000Info = values[20],
                DistanceToInfInfo = values[21]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                CdrDropsDistanceInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DistanceDistribution> distribution = _service.GenerateDistanceDistribution();
            Assert.AreEqual(distribution.Count, 22);
            for (int i = 0; i < 22; i++)
            {
                Assert.AreEqual(distribution[i].CdrDrops, values[i]);
            }
        }

        [TestCase(new[] { 1.0, 9, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1.0, 2, 3, 4, 56, 6, 7, 8, 9, 10, 11, 342, 13, 564, 15, 16, 17, 18, 19, 232, 21, 22 })]
        [TestCase(new[] { 1, 2.0, 3, 4, 5, 6, 34, 38, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2.1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 68, 13, 14, 15, 134, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        public void Test_GenerateDistanceDistribution_DropEcioDistanceInfo(
            double[] values)
        {
            DropEcioDistanceInfo info = new DropEcioDistanceInfo
            {
                DistanceTo200Info = values[0],
                DistanceTo400Info = values[1],
                DistanceTo600Info = values[2],
                DistanceTo800Info = values[3],
                DistanceTo1000Info = values[4],
                DistanceTo1200Info = values[5],
                DistanceTo1400Info = values[6],
                DistanceTo1600Info = values[7],
                DistanceTo1800Info = values[8],
                DistanceTo2000Info = values[9],
                DistanceTo2200Info = values[10],
                DistanceTo2400Info = values[11],
                DistanceTo2600Info = values[12],
                DistanceTo2800Info = values[13],
                DistanceTo3000Info = values[14],
                DistanceTo4000Info = values[15],
                DistanceTo5000Info = values[16],
                DistanceTo6000Info = values[17],
                DistanceTo7000Info = values[18],
                DistanceTo8000Info = values[19],
                DistanceTo9000Info = values[20],
                DistanceToInfInfo = values[21]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                DropEcioDistanceInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DistanceDistribution> distribution = _service.GenerateDistanceDistribution();
            Assert.AreEqual(distribution.Count, 22);
            for (int i = 0; i < 22; i++)
            {
                Assert.AreEqual(distribution[i].DropEcio, values[i]);
            }
        }

        [TestCase(new[] { 1.0, 9, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1.0, 2, 3, 4, 56, 6, 7, 8, 9, 10, 11, 342, 13, 564, 15, 16, 17, 18, 19, 232, 21, 22 })]
        [TestCase(new[] { 1, 2.0, 3, 4, 5, 6, 34, 38, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2.1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 68, 13, 14, 15, 134, 17, 18, 19, 20, 21, 22 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        public void Test_GenerateDistanceDistribution_GoodEcioDistanceInfo(
            double[] values)
        {
            GoodEcioDistanceInfo info = new GoodEcioDistanceInfo
            {
                DistanceTo200Info = values[0],
                DistanceTo400Info = values[1],
                DistanceTo600Info = values[2],
                DistanceTo800Info = values[3],
                DistanceTo1000Info = values[4],
                DistanceTo1200Info = values[5],
                DistanceTo1400Info = values[6],
                DistanceTo1600Info = values[7],
                DistanceTo1800Info = values[8],
                DistanceTo2000Info = values[9],
                DistanceTo2200Info = values[10],
                DistanceTo2400Info = values[11],
                DistanceTo2600Info = values[12],
                DistanceTo2800Info = values[13],
                DistanceTo3000Info = values[14],
                DistanceTo4000Info = values[15],
                DistanceTo5000Info = values[16],
                DistanceTo6000Info = values[17],
                DistanceTo7000Info = values[18],
                DistanceTo8000Info = values[19],
                DistanceTo9000Info = values[20],
                DistanceToInfInfo = values[21]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                GoodEcioDistanceInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DistanceDistribution> distribution = _service.GenerateDistanceDistribution();
            Assert.AreEqual(distribution.Count, 22);
            for (int i = 0; i < 22; i++)
            {
                Assert.AreEqual(distribution[i].GoodEcio, values[i] * 100);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 12.3, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 345, 4, 5.8, 6, 7, 8, 9, 10, 11, 12.98, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 332, 4, 5.8, 6, 7, 8, 9, 10, 11, 12.33, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6.47, 7, 8, 9, 10, 11, 12, 133.2, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10.98, 11, 12, 13, 14, 15.4, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        public void Test_GenerateCoverageInterferenceDistribution_DropEcioHourInfo(
            double[] values)
        {
            DropEcioHourInfo info = new DropEcioHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                DropEcioHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<CoverageInterferenceDistribution> distribution = _service.GenerateCoverageInterferenceDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].DropEcio, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 12.3, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 345, 4, 5.8, 6, 7, 8, 9, 10, 11, 12.98, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 332, 4, 5.8, 6, 7, 8, 9, 10, 11, 12.33, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6.47, 7, 8, 9, 10, 11, 12, 133.2, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10.98, 11, 12, 13, 14, 15.4, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        public void Test_GenerateCoverageInterferenceDistribution_MainRssiHourInfo(
            double[] values)
        {
            MainRssiHourInfo info = new MainRssiHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                MainRssiHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<CoverageInterferenceDistribution> distribution = _service.GenerateCoverageInterferenceDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].MainRssi, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 12.3, 2, 3, 4, 5.8, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 345, 4, 5.8, 6, 7, 8, 9, 10, 11, 12.98, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 332, 4, 5.8, 6, 7, 8, 9, 10, 11, 12.33, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6.47, 7, 8, 9, 10, 11, 12, 133.2, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        [TestCase(new[] { 1, 2, 3, 4, 5.8, 6, 7, 8, 9, 10.98, 11, 12, 13, 14, 15.4, 16, 17, 18, 19, 20, 21, 22, 23.5, 24.2 })]
        public void Test_GenerateCoverageInterferenceDistribution_SubRssiHourInfo(
            double[] values)
        {
            SubRssiHourInfo info = new SubRssiHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                SubRssiHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<CoverageInterferenceDistribution> distribution = _service.GenerateCoverageInterferenceDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].SubRssi, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 122, 2, 3, 4, 5543, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1632, 17, 18, 19, 250, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 832, 9, 10, 11, 12, 137, 14, 15, 156, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 932, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void Test_GenerateDropsHourDistribution_CdrCallsHourInfo(
            int[] values)
        {
            CdrCallsHourInfo info = new CdrCallsHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                CdrCallsHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DropsHourDistribution> distribution = _service.GenerateDropsHourDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].CdrCalls, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 122, 2, 3, 4, 5543, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1632, 17, 18, 19, 250, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 832, 9, 10, 11, 12, 137, 14, 15, 156, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 932, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void Test_GenerateDropsHourDistribution_CdrDropsHourInfo(
            int[] values)
        {
            CdrDropsHourInfo info = new CdrDropsHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                CdrDropsHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DropsHourDistribution> distribution = _service.GenerateDropsHourDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].CdrDrops, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 122, 2, 3, 4, 5543, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1632, 17, 18, 19, 250, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 832, 9, 10, 11, 12, 137, 14, 15, 156, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 932, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void Test_GenerateDropsHourDistribution_ErasureDropsHourInfo(
            int[] values)
        {
            ErasureDropsHourInfo info = new ErasureDropsHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                ErasureDropsHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DropsHourDistribution> distribution = _service.GenerateDropsHourDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].ErasureDrops, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 122, 2, 3, 4, 5543, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1632, 17, 18, 19, 250, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 832, 9, 10, 11, 12, 137, 14, 15, 156, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 932, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void Test_GenerateDropsHourDistribution_KpiCallsHourInfo(
            int[] values)
        {
            KpiCallsHourInfo info = new KpiCallsHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                KpiCallsHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DropsHourDistribution> distribution = _service.GenerateDropsHourDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].KpiCalls, values[i]);
            }
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 122, 2, 3, 4, 5543, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1632, 17, 18, 19, 250, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 832, 9, 10, 11, 12, 137, 14, 15, 156, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 932, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void Test_GenerateDropsHourDistribution_KpiDropsHourInfo(
            int[] values)
        {
            KpiDropsHourInfo info = new KpiDropsHourInfo
            {
                Hour0Info = values[0],
                Hour1Info = values[1],
                Hour2Info = values[2],
                Hour3Info = values[3],
                Hour4Info = values[4],
                Hour5Info = values[5],
                Hour6Info = values[6],
                Hour7Info = values[7],
                Hour8Info = values[8],
                Hour9Info = values[9],
                Hour10Info = values[10],
                Hour11Info = values[11],
                Hour12Info = values[12],
                Hour13Info = values[13],
                Hour14Info = values[14],
                Hour15Info = values[15],
                Hour16Info = values[16],
                Hour17Info = values[17],
                Hour18Info = values[18],
                Hour19Info = values[19],
                Hour20Info = values[20],
                Hour21Info = values[21],
                Hour22Info = values[22],
                Hour23Info = values[23]
            };
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                KpiDropsHourInfo = info
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            List<DropsHourDistribution> distribution = _service.GenerateDropsHourDistribution();
            Assert.AreEqual(distribution.Count, 24);
            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(distribution[i].KpiDrops, values[i]);
            }
        }

        [TestCase(2, new short[] { 0, 1 }, new[] { "锁星问题", "锁星问题" }, new[]{1,2}, 1, new[] { "锁星问题" }, 
            new[] { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(3, new short[] { 0, 1, 4 }, new[] { "锁星问题", "锁星问题", "锁星问题" }, 
            new[] { 1, 2, 5 }, 1, new[] { "锁星问题" },
            new[] { 1, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(3, new short[] { 0, 1, 4 }, new[] { "锁星问题", "锁星问题", "RSSI问题" },
            new[] { 1, 2, 5 }, 2, new[] { "锁星问题", "RSSI问题" },
            new[]
            {
                1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            })]
        [TestCase(4, new short[] { 0, 1, 4, 6 }, new[] { "锁星问题", "RSSI问题", "锁星问题", "RSSI问题" },
            new[] { 1, 2, 5, 8 }, 2, new[] { "锁星问题", "RSSI问题" },
            new[]
            {
                1, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 2, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            })]
        [TestCase(5, new short[] { 0, 1, 4, 6, 7 },
            new[] { "锁星问题", "RSSI问题", "锁星问题", "RSSI问题", "驻波比问题" },
            new[] { 1, 2, 5, 8, 9 }, 3, new[] { "锁星问题", "RSSI问题", "驻波比问题" },
            new[]
            {
                1, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 2, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            })]
        public void Test_GenerateAlarmHourDistribution(int length,
            short[] hours, string[] types, int[] alarms, int resultLength,
            string[] resultKeys, int[] resultCounts)
        {
            List<AlarmHourInfo> infos = new List<AlarmHourInfo>();
            for (int i = 0; i < length; i++)
            {
                infos.Add(new AlarmHourInfo {Hour = hours[i], AlarmType = types[i].GetAlarmType(), Alarms = alarms[i]});
            }
            statList.Add(new TopDrop2GCellDaily
            {
                CellId = 1,
                SectorId = 2,
                Frequency = 3,
                StatTime = DateTime.Today,
                AlarmHourInfos = infos
            });
            mockRepository.SetupGet(x => x.Stats).Returns(statList.AsQueryable());
            AlarmHourDistribution distribution = _service.GenerateAlarmHourDistribution();
            Assert.AreEqual(distribution.AlarmRecords.Count, resultLength);
            for (int i = 0; i < resultLength; i++)
            {
                int[] counts = distribution.AlarmRecords[resultKeys[i]];
                for (int j = 0; j < 24; j++)
                {
                    Assert.AreEqual(counts[j], resultCounts[i*24 + j]);
                }
            }
        }
    }
}
