using Lte.Domain.Geo.Service;
using NUnit.Framework;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class AngleBetweenAzimuthsTest
    {
        [Test]
        public void TestBasicAngle()
        {
            Assert.AreEqual(30 % 360, 30);
            Assert.AreEqual(30.3 % 360, 30.3);
            Assert.AreEqual(190.7 % 360, 190.7);
            Assert.AreEqual(361 % 360, 1);
            Assert.AreEqual(-30 % 360, -30);
            Assert.AreEqual(-30.3 % 360, -30.3);
            Assert.AreEqual(-190.7 % 360, -190.7);
            Assert.AreEqual(-361 % 360, -1);
        }

        [Test]
        public void TestAngleBetweenAzimuths_zero()
        {
            double angle = GeoMath.AngleBetweenAzimuths(163, 163);
            Assert.AreEqual(angle, 0);
            angle = GeoMath.AllAngleBetweenAzimuths(163, 163);
            Assert.AreEqual(angle, 0);
        }

        [Test]
        public void TestAngleBetweenAzimuths_positive()
        {
            double angle1 = GeoMath.AngleBetweenAzimuths(270, 30);
            double angle2 = GeoMath.AngleBetweenAzimuths(170, 10);

            Assert.AreEqual(angle1, 120);
            Assert.AreEqual(angle2, 160);

            angle1 = GeoMath.AllAngleBetweenAzimuths(270, 30);
            angle2 = GeoMath.AllAngleBetweenAzimuths(170, 10);
            Assert.AreEqual(angle1, -120);
            Assert.AreEqual(angle2, 160);

        }

        [Test]
        public void TestAngleBetweenAzimuths_negative()
        {
            double angle3 = GeoMath.AngleBetweenAzimuths(30, 270);
            double angle4 = GeoMath.AngleBetweenAzimuths(10, 170);
            Assert.AreEqual(angle3, 120);
            Assert.AreEqual(angle4, 160);

            angle3 = GeoMath.AllAngleBetweenAzimuths(30, 270);
            angle4 = GeoMath.AllAngleBetweenAzimuths(10, 170);
            Assert.AreEqual(angle3, 120);
            Assert.AreEqual(angle4, -160);
        }

        [Test]
        public void TestAngleBetweenAzimuths_large()
        {
            double a1 = GeoMath.AngleBetweenAzimuths(720, 20);
            double a2 = GeoMath.AngleBetweenAzimuths(-10, 370);
            Assert.AreEqual(a1, 20);
            Assert.AreEqual(a2, 20);

            a1 = GeoMath.AllAngleBetweenAzimuths(720, 20);
            a2 = GeoMath.AllAngleBetweenAzimuths(-10, 370);
            Assert.AreEqual(a1, -20);
            Assert.AreEqual(a2, -20);
        }
    }
}