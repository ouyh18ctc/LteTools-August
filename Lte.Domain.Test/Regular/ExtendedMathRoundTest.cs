using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class ExtendedMathRoundTest
    {
        const double eps = 1E-6;

        [Test]
        public void TestDecPowerPositive()
        {
            Assert.AreEqual(ExtendedMath.DecPowerPositive(2, 2), 200);
            Assert.AreEqual(ExtendedMath.DecPowerPositive(1.2223, 2), 122.23, eps);
        }

        [Test]
        public void TestDecPowerNegative()
        {
            Assert.AreEqual(ExtendedMath.DecPowerNegative(2, 2), 0.02);
            Assert.AreEqual(ExtendedMath.DecPowerNegative(2.1, 3), 0.0021, eps);
        }

        [Test]
        public void TestExtendedMathCeiling()
        {
            Assert.AreEqual(ExtendedMath.Ceiling(2.1, 2), 2.1, eps);
            Assert.AreEqual(ExtendedMath.Ceiling(1.2233, 2), 1.23, eps);
            Assert.AreEqual(ExtendedMath.Ceiling(1.22, 2), 1.22, eps);
        }

        [Test]
        public void TestExtendeMathFloor()
        {
            Assert.AreEqual(ExtendedMath.Floor(2.1, 2), 2.1, eps);
            Assert.AreEqual(ExtendedMath.Floor(1.2233, 2), 1.22, eps);
            Assert.AreEqual(ExtendedMath.Floor(1.223, 3), 1.223, eps);
        }

        [Test]
        public void TestExtentedMathRound()
        {
            Assert.AreEqual(ExtendedMath.Round(2.1, 2), 2.1, eps);
            Assert.AreEqual(ExtendedMath.Round(2.11, 2), 2.11, eps);
            Assert.AreEqual(ExtendedMath.Round(2.123, 2), 2.12, eps);
            Assert.AreEqual(ExtendedMath.Round(2.125, 2), 2.12, eps);
            Assert.AreEqual(ExtendedMath.Round(2.1251, 2), 2.13, eps);
        }
    }
}
