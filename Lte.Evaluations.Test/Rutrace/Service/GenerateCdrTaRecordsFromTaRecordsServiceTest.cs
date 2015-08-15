using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Rutrace.Service;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Service
{
    [TestFixture]
    public class GenerateCdrTaRecordsFromTaRecordsServiceTest
    {
        private List<CdrRtdRecord> sourceList;
        private const double Eps = 1E-6;

        [SetUp]
        public void SetUp()
        {
            sourceList = new List<CdrRtdRecord>();
        }

        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 1.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 1.0 }, new[] { 1.0 }, new[] { 1.0 },
            new[] { 0 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 10.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 10.0 }, new[] { 10.0 }, new[] { 10.0 },
            new[] { 0 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 100.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 100.0 }, new[] { 100.0 }, new[] { 100.0 },
            new[] { 0 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 1000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 1000.0 }, new[] { 1000.0 }, new[] { 1000.0 },
            new[] { 1 }, new[] { 0 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 1500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 0.0 }, new[] { 0.0 },
            new[] { 1 }, new[] { 0 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 2000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 0.0 }, new[] { 0.0 },
            new[] { 1 }, new[] { 0 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1 }, new byte[] { 2 }, new[] { 2500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 0.0 }, new[] { 0.0 },
            new[] { 0 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 100.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 10.0 }, new[] { 100.0 }, new[] { 55.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 10.0 }, new[] { 500.0 }, new[] { 255.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 1000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 10.0 }, new[] { 1000.0 }, new[] { 505.0 },
            new[] { 1 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 1500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 1490.0 }, new[] { 745.0 },
            new[] { 1 }, new[] { 1 }, new[] { 1 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 2000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 1990.0 }, new[] { 995.0 },
            new[] { 1 }, new[] { 1 }, new[] { 1 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 2500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 2490.0 }, new[] { 1245.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 10.0, 3000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 2990.0 }, new[] { 1495.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 100.0, 500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 100.0 }, new[] { 500.0 }, new[] { 300.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 100.0, 1000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 100.0 }, new[] { 1000.0 }, new[] { 550.0 },
            new[] { 1 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 100.0, 1500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 1400.0 }, new[] { 700.0 },
            new[] { 1 }, new[] { 1 }, new[] { 1 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 100.0, 2000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 1900.0 }, new[] { 950.0 },
            new[] { 1 }, new[] { 1 }, new[] { 1 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 100.0, 2500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 2400.0 }, new[] { 1200.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 100.0, 3000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 2900.0 }, new[] { 1450.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 500.0, 1000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 500.0 }, new[] { 1000.0 }, new[] { 750.0 },
            new[] { 1 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 500.0, 1500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 1000.0 }, new[] { 500.0 },
            new[] { 1 }, new[] { 1 }, new[] { 0 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 500.0, 2000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 1500.0 }, new[] { 750.0 },
            new[] { 1 }, new[] { 1 }, new[] { 1 }, new[] { 0 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 500.0, 2500.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 2000.0 }, new[] { 1000.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 1, 1 }, new byte[] { 2, 2 }, new[] { 500.0, 3000.0 },
            new[] { 1 }, new byte[] { 2 },
            1, new[] { 0.0 }, new[] { 2500.0 }, new[] { 1250.0 },
            new[] { 0 }, new[] { 2 }, new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 1, 1, 1 }, new byte[] { 2, 2, 3 }, new[] { 500.0, 3000.0, 100 },
            new[] { 1, 1 }, new byte[] { 2, 3 },
            2, new[] { 0.0, 100 }, new[] { 2500.0, 100 }, new[] { 1250.0, 100 },
            new[] { 0, 0 }, new[] { 2, 1 }, new[] { 0, 0 }, new[] { 1, 0 })]
        public void Test_Generate(int[] cellId, byte[] sectorId, double[] rtd,
            int[] resultCellId, byte[] resultSectorId,
            int resultCount, double[] min, double[] max, double[] average,
            int[] innerCount, int[] outerCount, int[] innerExcessCount, int[] outerExcessCount)
        {
            for (int i = 0; i < cellId.Length; i++)
            {
                sourceList.Add(new CdrRtdRecord
                {
                    CellId = cellId[i],
                    SectorId = sectorId[i],
                    Rtd = rtd[i]
                });
            }
            GenerateCdrTaRecordsFromTaRecordsService service = new GenerateCdrTaRecordsFromTaRecordsService(
                sourceList);
            List<CdrTaRecord> resultList = service.Generate();
            Assert.AreEqual(resultList.Count, resultCount);
            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(resultList[i].CellId, resultCellId[i]);
                Assert.AreEqual(resultList[i].SectorId, resultSectorId[i]);
                Assert.AreEqual(resultList[i].TaMin, min[i], Eps);
                Assert.AreEqual(resultList[i].TaMax, max[i], Eps);
                Assert.AreEqual(resultList[i].TaAverage, average[i], Eps);
                Assert.AreEqual(resultList[i].TaInnerIntervalNum, innerCount[i]);
                Assert.AreEqual(resultList[i].TaOuterIntervalNum, outerCount[i]);
                Assert.AreEqual(resultList[i].TaInnerIntervalExcessNum, innerExcessCount[i], "Max is:" + max[i]);
                Assert.AreEqual(resultList[i].TaOuterIntervalExcessNum, outerExcessCount[i]);
            }
        }
    }
}
