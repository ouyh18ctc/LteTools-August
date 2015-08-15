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
    internal class FakeGenerateCdrTaRecordsService : GenerateCdrTaRecordsService
    {
        protected override void GenerateDetails()
        {
            
        }

        public FakeGenerateCdrTaRecordsService(List<CdrTaRecord> details)
        {
            _details = details;
        }
    }

    [TestFixture]
    public class GenerateCdrTaRecordsServiceTest
    {
        private List<CdrTaRecord> details;

        [SetUp]
        public void SetUp()
        {
            details = new List<CdrTaRecord>();
        }

        [TestCase(new[] { 200.0 }, new[] { 800.0 }, new[] { false })]
        [TestCase(new[] { 200.0, 400 }, new[] { 800.0, 1000 }, new[] { false, false })]
        [TestCase(new[] { 400.0 }, new[] { 1000.0 }, new[] { false })]
        [TestCase(new[] { 200.0, 400, 600 }, new[] { 800.0, 1000, 1400 }, new[] { false, false, true })]
        [TestCase(new[] { 400.0, 600 }, new[] { 1000.0, 1200 }, new[] { false, true })]
        [TestCase(new[] { 600.0 }, new[] { 1200.0 }, new[] { true })]
        [TestCase(new[] { 600.0, 800 }, new[] { 1200.0, 1400 }, new[] { true, true })]
        [TestCase(new[] { 800.0 }, new[] { 1400.0 }, new[] { true })]
        [TestCase(new[] { 1000.0 }, new[] { 1400.0 }, new[] { true })]
        public void Test_TwoElements_Generate(double[] min, double[] max, bool[] result)
        {
            for (int i = 0; i < min.Length; i++)
            {
                details.Add(new CdrTaRecord
                {
                    TaMin = min[i],
                    TaMax = max[i],
                    TaSum = min[i] + max[i],
                    TaInnerIntervalNum = 1,
                    TaOuterIntervalNum = 1
                });
            }
            FakeGenerateCdrTaRecordsService service = new FakeGenerateCdrTaRecordsService(details);
            List<CdrTaRecord> results = service.Generate();
            Assert.AreEqual(results.Count, min.Length);
            for (int i = 0; i < min.Length; i++)
            {
                if (result[i])
                {
                    Assert.AreEqual(results[i].TaMax, max[i] - min[i]);
                    Assert.AreEqual(results[i].TaMin, 0);
                    Assert.AreEqual(results[i].TaAverage, (max[i] - min[i])/2);
                }
                else
                {
                    Assert.AreEqual(results[i].TaMax, max[i]);
                    Assert.AreEqual(results[i].TaMin, min[i]);
                    Assert.AreEqual(results[i].TaAverage, (max[i] + min[i]) / 2);
                }
            }
        }

        [TestCase(new[] { 200.0 }, false)]
        [TestCase(new[] { 600.0 }, false)]
        [TestCase(new[] { 1000.0 }, false)]
        [TestCase(new[] { 200.0, 1400 }, true)]
        [TestCase(new[] { 200.0, 1400, 2000 }, true)]
        [TestCase(new[] { 200.0, 1400, 4000 }, false)]
        [TestCase(new[] { 1400.0 }, true)]
        public void Test_MultiElements_Generate(double[] elements, bool offset)
        {
            double min = elements.Min();
            double max = elements.Max();
            double sum = elements.Sum();
            int counts = elements.Count();
            details.Add(new CdrTaRecord
            {
                TaMin = min,
                TaMax = max,
                TaSum = sum,
                TaInnerIntervalNum = counts,
                TaOuterIntervalNum = 0
            });
            FakeGenerateCdrTaRecordsService service = new FakeGenerateCdrTaRecordsService(details);
            List<CdrTaRecord> results = service.Generate();
            Assert.AreEqual(results.Count, 1);
            if (offset)
            {
                Assert.AreEqual(results[0].TaMax, max - min);
                Assert.AreEqual(results[0].TaMin, 0);
                Assert.AreEqual(results[0].TaAverage, sum/counts - min);
            }
            else
            {
                Assert.AreEqual(results[0].TaMax, max);
                Assert.AreEqual(results[0].TaMin, min);
                Assert.AreEqual(results[0].TaAverage, sum / counts);
            }
        }
    }
}
