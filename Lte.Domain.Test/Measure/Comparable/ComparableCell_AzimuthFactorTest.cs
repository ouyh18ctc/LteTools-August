using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_AzimuthFactorTest
    {
        private readonly ComparableCell mockCell = new ComparableCell();
        const double eps = 1E-6;

        [Test]
        public void TestAzimuthAngleProperty_0()
        {
            mockCell.AzimuthAngle = 0;          
            Assert.AreEqual(mockCell.AzimuthAngle, 0);
        }

        [Test]
        public void TestAzimuthFactor_0()
        {
            mockCell.AzimuthAngle = 0;
            Assert.AreEqual(mockCell.AzimuthFactor(), 0);          
        }

        [Test]
        public void TestAzimuthAngleProperty_30()
        {
            mockCell.AzimuthAngle = 30;
            Assert.AreEqual(mockCell.AzimuthAngle, 30);
        }

        [Test]
        public void TestAzimuthFactor_32()
        {
            mockCell.AzimuthAngle = 32;
            Assert.AreEqual(mockCell.AzimuthFactor(), 3, eps);
        }

        [Test]
        public void TestAzimuthFactor_SetProperty_25_30()
        {
            HorizontalProperty property = new HorizontalProperty(25);
            mockCell.AzimuthAngle = 25;
            Assert.AreEqual(mockCell.AzimuthFactor(property), 3, eps);
        }
    }
}
