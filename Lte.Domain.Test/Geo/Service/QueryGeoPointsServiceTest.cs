using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Geo;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using NUnit.Framework;

namespace Lte.Domain.Test.Geo.Service
{
    [TestFixture]
    public class QueryGeoPointsServiceTest
    {
        [TestCase(new[]{113.0},new[]{23.0}, 36)]
        [TestCase(new[] { 113.0, 113.01 }, new[] { 23.0, 23.0 }, 42)]
        [TestCase(new[] { 113.0, 113.0 }, new[] { 23.0, 23.01 }, 42)]
        [TestCase(new[] { 113.0, 113.01 }, new[] { 23.0, 23.01 }, 47)]
        public void Test_Query(double[] inLon, double[] inLat, int points)
        {
            IEnumerable<StubGeoPoint> inPoints = inLon.Select((x, i) => new StubGeoPoint
                (x, inLat[i]));
            List<GeoPoint> results = inPoints.Query<StubGeoPoint, GeoPoint>(0.03, 0.01);
            Assert.AreEqual(results.Count, points);
        }
    }
}
