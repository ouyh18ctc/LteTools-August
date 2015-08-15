using System;
using Lte.Domain.Measure;
using NUnit.Framework;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class AdjustUrbanCalculationTest
    {
        private IBroadcastModel model;

        [Test]
        public void TestDownlink1800Model_50mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink1800, 0.05, false);
        }

        [Test]
        public void TestDownlink1800Model_100mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink1800, 0.1, true);
        }

        [Test]
        public void TestDownlink1800Model_200mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink1800, 0.2, true);
        }

        [Test]
        public void TestDownlink1800Model_500mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink1800, 0.5, true);
        }

        [Test]
        public void TestDownlink2100Model_50mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink2100, 0.05, false);
        }

        [Test]
        public void TestDownlink2100Model_100mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink2100, 0.1, true);
        }

        [Test]
        public void TestDownlink2100Model_200mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink2100, 0.2, true);
        }

        [Test]
        public void TestDownlink2100Model_500mDistance()
        {
            TestDifferentUrbanTypesWithFrequency(FrequencyBandType.Downlink2100, 0.5, true);
        }

        private void TestDifferentUrbanTypesWithFrequency(FrequencyBandType ftype, double distance,
            bool farEnough)
        {
            model = new BroadcastModel(ftype, UrbanType.Dense);
            double d1 = model.CalculatePathLoss(distance, 40);
            model = new BroadcastModel(ftype);
            double d2 = model.CalculatePathLoss(distance, 40);
            model = new BroadcastModel(ftype, UrbanType.Middle);
            double d3 = model.CalculatePathLoss(distance, 40);
            if (farEnough)
            { Assert.IsTrue(d1 > d2, "d1 = {0}, d2 = {1}", d1, d2); }
            else
            { Assert.IsTrue(d1 < d2, "d1 = {0}, d2 = {1}", d1, d2); }
            Assert.IsTrue(d2 > d3, "d2 = {0}, d3 = {1}", d2, d3);
        }
    }
}
