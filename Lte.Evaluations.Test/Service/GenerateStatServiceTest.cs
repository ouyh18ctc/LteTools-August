using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Service;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Service
{
    [TestFixture]
    public class GenerateStatServiceTest
    {
        private List<RuInterferenceStat> statList;
        private const double Eps = 1E-6;

        [SetUp]
        public void SetUp()
        {
            statList = new List<RuInterferenceStat>();
        }

        [TestCase(1, new[] { 0.1 }, new[] { 5 }, new [] { 0.179176 })]
        [TestCase(1, new[] { 0.2 }, new[] { 5 }, new[] { 0.358352 })]
        [TestCase(1, new[] { 0.3 }, new[] { 5 }, new[] { 0.537528 })]
        [TestCase(1, new[] { 0.1 }, new[] { 10 }, new[] { 0.23979 })]
        [TestCase(1, new[] { 0.2 }, new[] { 10 }, new[] { 0.47958})]
        [TestCase(1, new[] { 0.3 }, new[] { 10 }, new[] { 0.719369 })]
        [TestCase(1, new[] { 0.4 }, new[] { 10 }, new[] { 0.959158 })]
        [TestCase(1, new[] { 0.5 }, new[] { 10 }, new[] { 1.198948 })]
        [TestCase(1, new[] { 0.1 }, new[] { 15 }, new[] { 0.277259 })]
        [TestCase(1, new[] { 0.1 }, new[] { 20 }, new[] { 0.304452 })]
        [TestCase(2, new[] { 0.1, 0.1 }, new[] { 20, 5 }, new[] { 0.304452, 0.179176 })]
        [TestCase(2, new[] { 0.4, 0.5 }, new[] { 10, 10 }, new[] { 0.959158, 1.198948 })]
        [TestCase(3, new[] { 0.1, 0.1, 0.3 }, new[] { 20, 5, 10 }, new[] { 0.304452, 0.179176, 0.719369 })]
        public void Test_InterferenceSource(int length, double[] ratios, int[] cells, double[] expectedValues)
        {
            for (int i = 0; i < length; i++)
            {
                statList.Add(new RuInterferenceStat
                {
                    InterferenceRatio = ratios[i],
                    VictimCells = cells[i]
                });
            }
            GenerateValuesStatService service = new GenerateValuesStatService(statList);
            List<double> results = service.GenerateValues("干扰源分析");
            Assert.AreEqual(results.Count, length);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(results[i], expectedValues[i], Eps);
            }
        }

        [TestCase(1, new double[] { 100 }, new double[] { 200 })]
        [TestCase(1, new double[] { 100 }, new double[] { 220 })]
        [TestCase(1, new double[] { 3540 }, new double[] { 2210 })]
        [TestCase(2, new [] { 100, 1.1 }, new [] { 220, 32.13 })]
        [TestCase(3, new[] { 100, 1.1, 345.7 }, new[] { 220, 32.13, 9867 })]
        public void Test_InterferenceDistance(int length, double[] tas, double[] rtds)
        {
            for (int i = 0; i < length; i++)
            {
                statList.Add(new RuInterferenceStat
                {
                    TaAverage = tas[i],
                    AverageRtd = rtds[i]
                });
            }
            GenerateValuesStatService service = new GenerateValuesStatService(statList);
            List<double> results = service.GenerateValues("干扰距离分析");
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(results[i], tas[i]/rtds[i], Eps);
            }
        }

        [TestCase(1, new [] { 0.11 })]
        [TestCase(1, new [] { 0.23 })]
        [TestCase(1, new [] { 0.354 })]
        [TestCase(2, new[] { 0.11, 0.13 })]
        [TestCase(3, new[] { 0.76, 0.09, 0.003 })]
        public void Test_InterferenceTaExcessRate(int length, double[] rates)
        {
            for (int i = 0; i < length; i++)
            {
                statList.Add(new RuInterferenceStat
                {
                    TaExcessRate = rates[i]
                });
            }
            GenerateValuesStatService service = new GenerateValuesStatService(statList);
            List<double> results = service.GenerateValues("邻区距离分析");
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(results[i], rates[i], Eps);
            }
        }
    }
}
