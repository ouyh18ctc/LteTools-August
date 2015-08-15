using System;
using Lte.Domain.Measure;
using NUnit.Framework;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class AdjustFrequencyCalculationTest
    {
        private IBroadcastModel model;

        [TestCase(0.05)]
        [TestCase(0.1)]
        [TestCase(0.2)]
        [TestCase(0.5)]
        [TestCase(0.8)]
        [TestCase(1.3)]
        [TestCase(2.1)]
        public void TestDenseModel(double distance)
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Dense, distance);
        }

        [TestCase(0.05)]
        [TestCase(0.1)]
        [TestCase(0.2)]
        [TestCase(0.5)]
        [TestCase(0.8)]
        [TestCase(1.3)]
        [TestCase(2.1)]
        public void TestLargeModel(double distance)
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Large, distance);
        }

        [TestCase(0.05)]
        [TestCase(0.1)]
        [TestCase(0.2)]
        [TestCase(0.5)]
        [TestCase(0.8)]
        [TestCase(1.3)]
        [TestCase(2.1)]
        public void TestMiddleModel(double distance)
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Middle, distance);
        }

        private void TestDifferentFrequenciesWithUrbanType(UrbanType utype, double distance)
        {
            model = new BroadcastModel(utype: utype);
            double d1 = model.CalculatePathLoss(distance, 40);
            model = new BroadcastModel(FrequencyBandType.Uplink2100, utype);
            double d2 = model.CalculatePathLoss(distance, 40);
            model = new BroadcastModel(FrequencyBandType.Downlink1800, utype);
            double d3 = model.CalculatePathLoss(distance, 40);
            model = new BroadcastModel(FrequencyBandType.Uplink1800, utype);
            double d4 = model.CalculatePathLoss(distance, 40);
            model = new BroadcastModel(FrequencyBandType.Tdd2600, utype);
            double d5 = model.CalculatePathLoss(distance, 40);
            Assert.IsTrue(d5 > d1);
            Assert.IsTrue(d1 > d2);
            Assert.IsTrue(d2 > d3);
            Assert.IsTrue(d3 > d4);
        }
    }
}
