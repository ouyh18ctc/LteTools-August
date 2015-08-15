using System;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Antenna
{
    [TestFixture]
    public class HorizontalProperty_DefaultTest
    {
        private readonly HorizontalProperty property = new HorizontalProperty();
        const double eps = 1E-6;

        [TestCase(0, 0)]
        [TestCase(32,3)]
        [TestCase(90, 30)]
        [TestCase(91, 30)]
        [TestCase(120, 30)]
        public void Test_azimuth0(double parameter, double factor)
        {
            Assert.AreEqual(property.CalculateFactor(parameter), factor, eps);
        }

        [Test]
        public void Test_azimuth_89()
        {
            Assert.IsTrue(property.CalculateFactor(89) < 30);
        }
    }

    [TestFixture]
    public class HorizontalProperty_OtherTest
    {
        const double eps = 1E-6;

        [TestCase(10, 20)]
        [TestCase(10, 30)]
        [TestCase(10, 40)]
        [TestCase(20, 10)]
        [TestCase(20, 33)]
        [TestCase(35, 47)]
        public void Test(double half, double back)
        {
            HorizontalProperty property = new HorizontalProperty(half, back);
            Assert.AreEqual(property.CalculateFactor(0), 0);
            Assert.AreEqual(property.CalculateFactor(half), 3, eps);
            Assert.IsTrue(property.CalculateFactor(89) <= back);
            Assert.AreEqual(property.CalculateFactor(90), back, eps);
            Assert.AreEqual(property.CalculateFactor(91), back, eps);
            Assert.AreEqual(property.CalculateFactor(120), back, eps);
        }
    }
}
