using System;
using Lte.Domain.Measure;
using NUnit.Framework;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class AdjustDistanceCalculationTest
    {
        private IBroadcastModel model;

        [SetUp]
        public void TestInitialize()
        {
            model = new BroadcastModel();
        }

        [TestCase(40)]
        [TestCase(20)]
        [TestCase(65)]
        public void TestDefaultModel(double height)
        {
            TestDifferentDistancesWithBsHeight(height);
        }

        [TestCase(40)]
        [TestCase(20)]
        [TestCase(65)]
        public void TestDenseModel(double height)
        {
            model = new BroadcastModel(utype: UrbanType.Dense);
            TestDifferentDistancesWithBsHeight(height);
        }

        [TestCase(40)]
        [TestCase(20)]
        [TestCase(65)]
        public void TestLargeModel_Downlink1800(double height)
        {
            model = new BroadcastModel(FrequencyBandType.Downlink1800);
            TestDifferentDistancesWithBsHeight(height);
        }

        private void TestDifferentDistancesWithBsHeight(double bsHeight)
        {
            double d1 = model.CalculatePathLoss(0.01, bsHeight);
            double d2 = model.CalculatePathLoss(0.05, bsHeight);
            double d3 = model.CalculatePathLoss(0.2, bsHeight);
            Assert.IsTrue(d1 < d2);
            Assert.IsTrue(d2 < d3);
        }
    }
}
