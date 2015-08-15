using Lte.Domain.Measure;
using Lte.Domain.Test.Broadcast;
using Lte.Domain.TypeDefs;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Budget
{
    [TestFixture]
    public class CalculateReceivedPower_2100Test
    {
        private readonly Mock<IBroadcastModel> model = new Mock<IBroadcastModel>();
        private readonly Mock<ILinkBudget<double>> budget = new Mock<ILinkBudget<double>>();
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            model.MockFrequencyType(FrequencyBandType.Downlink2100);
            model.MockUrbanTypeAndKValues(UrbanType.Dense);
            budget.SetupGet(x => x.Model).Returns(model.Object);
            budget.SetupGet(x => x.TransmitPower).Returns(15.2);
            budget.SetupGet(x => x.AntennaGain).Returns(18);
        }

        [Test]
        public void Test_10mDistance()
        {
            double p = budget.Object.CalculateReceivedPower(0.01, 40);
            Assert.AreEqual(p, -21.048422, eps);
        }

        [Test]
        public void Test_20mDistance()
        {
            double p = budget.Object.CalculateReceivedPower(0.02, 40);
            Assert.AreEqual(p, -35.951366, eps);
        }

        [Test]
        public void Test_50mDistance()
        {
            double p = budget.Object.CalculateReceivedPower(0.05, 40);
            Assert.AreEqual(p, -55.651985, eps);
        }

        [Test]
        public void Test_100mDistance()
        {
            double p = budget.Object.CalculateReceivedPower(0.1, 40);
            Assert.AreEqual(p, -70.554929, eps);
        }

        [Test]
        public void Test_200mDistance()
        {
            double p = budget.Object.CalculateReceivedPower(0.2, 40);
            Assert.AreEqual(p, -85.457873, eps);
        }

        [Test]
        public void Test_500mDistance()
        {
            double p = budget.Object.CalculateReceivedPower(0.5, 40);
            Assert.AreEqual(p, -105.158492, eps);
        }
    }
}
