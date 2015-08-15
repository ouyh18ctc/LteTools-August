using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Antenna
{
    public class HorizontalPropertyUnitTest
    {
        protected HorizontalProperty Property;

        const double Eps = 1E-6;

        protected void SetupProperty(double halfPowerAzimuth = 32, double frontBackRatio = 30)
        {
            Property = new HorizontalProperty(halfPowerAzimuth, frontBackRatio);
        }

        protected void AssertTest(double para, double expected)
        {
            Assert.AreEqual(Property.CalculateFactor(para), expected, Eps);
        }

        protected void AssertBreakPoint(double attenuation)
        {
            for (double angle = 0; angle < 174; angle += 5.1)
            {
                if (angle < 90)
                {
                    if (attenuation >= 10)
                    {
                        Assert.IsTrue(Property.CalculateFactor(angle) < attenuation,
                            "_result: " + Property.CalculateFactor(angle).ToString(CultureInfo.InvariantCulture) 
                            + " is greater than "
                            + attenuation.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        Assert.IsTrue(Property.CalculateFactor(angle) <= attenuation,
                            "_result: " + Property.CalculateFactor(angle).ToString(CultureInfo.InvariantCulture) 
                            + " is greater than "
                            + attenuation.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    AssertTest(angle, attenuation);
                }
            }
        }
    }

    [TestFixture]
    public class DefaultHorizontalPropertyUnitTest : HorizontalPropertyUnitTest
    {
        [SetUp]
        public void Setup()
        {
            SetupProperty();
        }

        [Test]
        public void TestValues()
        {
            AssertTest(0, 0);
        }

        [Test]
        public void TestHalfPowerAzimuth()
        {
            AssertTest(32, 3);
        }

        [Test]
        public void TestBreakPoint_Is90()
        {
            AssertBreakPoint(30);
        }
    }

    [TestFixture]
    public class VariateHpaHorizontalPropertyUnitTest : HorizontalPropertyUnitTest
    {
        private void AssertHpaBreakPoint(double hpa)
        {
            Property = new HorizontalProperty(halfPowerAzimuth: hpa);
            Assert.IsTrue(Property.CalculateFactor(hpa / 2) < 3);
            Assert.IsTrue(Property.CalculateFactor(hpa - 1E-6) < 3);
            AssertTest(hpa, 3);
            Assert.IsTrue(Property.CalculateFactor(hpa + 1E-6) > 3);
            Assert.IsTrue(Property.CalculateFactor(hpa * 2) > 3);
            Assert.IsTrue(Property.CalculateFactor(hpa * 3) > 3);
        }

        [Test]
        public void Assert_HPA10()
        {
            AssertHpaBreakPoint(10);
        }

        [Test]
        public void Assert_HPA20()
        {
            AssertHpaBreakPoint(20);
        }

        [Test]
        public void Assert_HPA30()
        {
            AssertHpaBreakPoint(30);
        }

        [Test]
        public void Assert_HPA50()
        {
            AssertHpaBreakPoint(50);
        }

        [Test]
        public void Assert_HPA80()
        {
            AssertHpaBreakPoint(80);
        }
    }

    [TestFixture]
    public class VariateFbrHorizontalPropertyUnitTest : HorizontalPropertyUnitTest
    {
        private void AssertFbrBreakPoint(double fbr)
        {
            Property = new HorizontalProperty(frontBackRatio: fbr);
            AssertBreakPoint(fbr);
        }

        [Test]
        public void Assert_FBR90()
        {
            AssertFbrBreakPoint(90);
        }

        [Test]
        public void Assert_FBR60()
        {
            AssertFbrBreakPoint(60);
        }

        [Test]
        public void Assert_FBR40()
        {
            AssertFbrBreakPoint(40);
        }

        [Test]
        public void Assert_FBR30()
        {
            AssertFbrBreakPoint(30);
        }

        [Test]
        public void Assert_FBR20()
        {
            AssertFbrBreakPoint(20);
        }

        [Test]
        public void Assert_FBR10()
        {
            AssertFbrBreakPoint(10);
        }

        [Test]
        public void Assert_FBR5()
        {
            AssertFbrBreakPoint(5);
        }
    }
}
