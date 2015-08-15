using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Antenna
{
    [TestFixture]
    public class VerticalPropertyTest
    {
        [Test]
        public void Test_Default()
        {
            VerticalProperty property = new VerticalProperty();
            Assert.AreEqual(property.CalculateFactor(0), 0);
            Assert.AreEqual(property.CalculateFactor(7), 3);
            Assert.AreEqual(property.CalculateFactor(21), 9);
            Assert.AreEqual(property.CalculateFactor(70), 30);
            Assert.AreEqual(property.CalculateFactor(90), 30);
        }

        [Test]
        public void Test_half10()
        {
            VerticalProperty property = new VerticalProperty(10);
            Assert.AreEqual(property.CalculateFactor(0), 0);
            Assert.AreEqual(property.CalculateFactor(7), 2.1);
            Assert.AreEqual(property.CalculateFactor(10), 3);
            Assert.AreEqual(property.CalculateFactor(30), 9);
            Assert.AreEqual(property.CalculateFactor(90), 27);
        }
    }
}
