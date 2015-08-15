using System;
using Lte.Domain.Measure;
using NUnit.Framework;
using Moq;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class BroadcastModelDistanceTest
    {
        private readonly Mock<IBroadcastModel> model = new Mock<IBroadcastModel>();

        [Test]
        public void TestLargeModel_Downlink2100_40BsHeight()
        {
            AssertTest(UrbanType.Large, 2120, 40);
        }

        [Test]
        public void TestLargeModel_Downlink2100_20BsHeight()
        {
            AssertTest(UrbanType.Large, 2120, 20);
        }

        [Test]
        public void TestLargeModel_Downlink2100_65BsHeight()
        {
            AssertTest(UrbanType.Large, 2120, 65);
        }

        [Test]
        public void TestDenseModel_Downlink2100_20BsHeight()
        {
            AssertTest(UrbanType.Dense, 2120, 20);
        }

        [Test]
        public void TestDenseModel_Downlink2100_40BsHeight()
        {
            AssertTest(UrbanType.Dense, 2120, 40);
        }

        [Test]
        public void TestDenseModel_Downlink2100_65BsHeight()
        {
            AssertTest(UrbanType.Dense, 2120, 65);
        }

        [Test]
        public void TestLargeModel_Downlink1800_20BsHeight()
        {
            AssertTest(UrbanType.Large, 1860, 20);
        }

        [Test]
        public void TestLargeModel_Downlink1800_40BsHeight()
        {
            AssertTest(UrbanType.Large, 1860, 40);
        }

        [Test]
        public void TestLargeModel_Downlink1800_65BsHeight()
        {
            AssertTest(UrbanType.Large, 1860, 65);
        }

        private void AssertTest(UrbanType type, double frequnecy, double height)
        {
            model.MockUrbanTypeAndKValues(type);
            model.SetupGet(x => x.Frequency).Returns(frequnecy);
            TestDifferentDistancesWithBsHeight(height);
        }

        private void TestDifferentDistancesWithBsHeight(double bsHeight)
        {
            double d1 = model.Object.CalculatePathLoss(0.01, bsHeight);
            double d2 = model.Object.CalculatePathLoss(0.05, bsHeight);
            double d3 = model.Object.CalculatePathLoss(0.2, bsHeight);
            Assert.IsTrue(d1 < d2, "d1 = " + d1.ToString() + ", d2 = " + d2.ToString());
            Assert.IsTrue(d2 < d3, "d2 = " + d2.ToString() + ", d3 = " + d3.ToString());
        }
    }
}
