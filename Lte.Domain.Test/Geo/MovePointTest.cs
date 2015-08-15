using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using NUnit.Framework;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class MovePointTest
    {
        [Test]
        public void TestMovePoint_50m_Northeast()
        {
            GeoPoint origin = new GeoPoint(112.1, 23.1);
            IGeoPoint<double> point = origin.Move(50, 45);
            Assert.AreEqual(point.Longtitute, 112.1010860, 1E-6);
            Assert.AreEqual(point.Lattitute, 23.101086, 1E-6);
        }

        [Test]
        public void TestMovePoint_50m_Southeast()
        {
            GeoPoint origin = new GeoPoint(112.1, 23.1);
            IGeoPoint<double> point = origin.Move(50, 135);
            Assert.AreEqual(point.Longtitute, 112.1010860, 1E-6);
            Assert.AreEqual(point.Lattitute, 23.098914, 1E-6);
        }

        [Test]
        public void TestMovePoint_50m_Southwest()
        {
            GeoPoint origin = new GeoPoint(112.1, 23.1);
            IGeoPoint<double> point = origin.Move(50, 225);
            Assert.AreEqual(point.Longtitute, 112.0989140, 1E-6);
            Assert.AreEqual(point.Lattitute, 23.098914, 1E-6);
        }

        [Test]
        public void TestMovePoint_50m_Northwest()
        {
            GeoPoint origin = new GeoPoint(112.1, 23.1);
            IGeoPoint<double> point = origin.Move(50, 315);
            Assert.AreEqual(point.Longtitute, 112.0989140, 1E-6);
            Assert.AreEqual(point.Lattitute, 23.101086, 1E-6);
        }
    }
}
