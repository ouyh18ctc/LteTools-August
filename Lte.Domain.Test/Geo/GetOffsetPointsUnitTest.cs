using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Geo;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using NUnit.Framework;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class GetOffsetPointsUnitTest
    {
        private IEnumerable<IGeoPoint<double>> _pointList;
        private readonly IGeoPoint<double> _center = new GeoPoint(1.05, 1.05);
        private const double Eps = 1E-6;

        [SetUp]
        public void Setup()
        {
            _pointList = new IGeoPoint<double>[]
            {
                new GeoPoint(1, 1),
                new GeoPoint(1.1, 1),
                new GeoPoint(1, 1.1),
                new GeoPoint(1.1, 1.1)
            };
        }

        private void AssertSamePoints(IGeoPoint<double> point1, IGeoPoint<double> point2)
        {
            Assert.AreEqual(point1.Longtitute, point2.Longtitute, Eps, "Longtitute should be " + point1.Longtitute);
            Assert.AreEqual(point1.Lattitute, point2.Lattitute, Eps, "Lattitute should be " + point1.Lattitute);
        }

        [Test]
        public void TestGetLeftBottomOffsetPoint()
        {
            IGeoPoint<double> point = _center.GetLeftBottomOffsetPoint(_pointList, 0, 0);
            IGeoPoint<double> expected = new GeoPoint(0.95, 0.95);
            AssertSamePoints(point, expected);
        }

        [Test]
        public void TestGetRightTopOffsetPoint()
        {
            IGeoPoint<double> point = _center.GetRightTopOffsetPoint(_pointList, 0, 0);
            IGeoPoint<double> expected = new GeoPoint(1.15, 1.15);
            AssertSamePoints(point, expected);
        }
    }
}
