using System;
using Lte.Domain.Measure;
using NUnit.Framework;
using Lte.Domain.TypeDefs;
using Moq;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class BroadcastModelFrequencyTest
    {
        private readonly Mock<IBroadcastModel> model=new Mock<IBroadcastModel>();

        [Test]
        public void TestDenseModel_50mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Dense, 0.05);
        }

        [Test]
        public void TestDenseModel_100mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Dense, 0.1);
        }

        [Test]
        public void TestDenseModel_200mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Dense, 0.2);
        }

        [Test]
        public void TestDenseModel_500mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Dense, 0.5);
        }

        [Test]
        public void TestLargeModel_50mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Large, 0.05);
        }

        [Test]
        public void TestLargeModel_100mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Large, 0.1);
        }

        [Test]
        public void TestLargeModel_200mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Large, 0.2);
        }

        [Test]
        public void TestLargeModel_500mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Large, 0.5);
        }

        [Test]
        public void TestMiddleModel_50mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Middle, 0.05);
        }

        [Test]
        public void TestMiddleModel_100mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Middle, 0.1);
        }

        [Test]
        public void TestMiddleModel_200mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Middle, 0.2);
        }

        [Test]
        public void TestMiddleModel_500mDistance()
        {
            TestDifferentFrequenciesWithUrbanType(UrbanType.Middle, 0.5);
        }

        private void TestDifferentFrequenciesWithUrbanType(UrbanType utype, double distance)
        {
            model.MockUrbanTypeAndKValues(utype);
            model.MockFrequencyType(FrequencyBandType.Downlink2100);
            double d1 = model.Object.CalculatePathLoss(distance, 40);
            model.MockFrequencyType(FrequencyBandType.Uplink2100);
            double d2 = model.Object.CalculatePathLoss(distance, 40);
            model.MockFrequencyType(FrequencyBandType.Downlink1800);
            double d3 = model.Object.CalculatePathLoss(distance, 40);
            model.MockFrequencyType(FrequencyBandType.Uplink1800);
            double d4 = model.Object.CalculatePathLoss(distance, 40);
            model.MockFrequencyType(FrequencyBandType.Tdd2600);
            double d5 = model.Object.CalculatePathLoss(distance, 40);
            Assert.IsTrue(d5 > d1);
            Assert.IsTrue(d1 > d2);
            Assert.IsTrue(d2 > d3);
            Assert.IsTrue(d3 > d4);
        }
    }
}
