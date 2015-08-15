using System;
using Lte.Domain.Geo.Service;
using Lte.Domain.Geo;
using NUnit.Framework;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class GetDegreeIntervalTest
    {
        const double eps = 1E-6;

        [TestCase(20, 0.00018)]
        [TestCase(30, 0.00027)]
        [TestCase(50, 0.00045)]
        [TestCase(60, 0.00054)]
        [TestCase(100, 0.0009)]
        [TestCase(200, 0.0017986)]
        [TestCase(500, 0.0044966)]
        [TestCase(1000, 0.00899322)]
        public void TestGetDegreeInterval_20m(double distanceInMeter, double expectedInterval)
        {
            double degreeInterval = distanceInMeter.GetDegreeInterval();
            Assert.AreEqual(degreeInterval, expectedInterval, eps);
            double distance = degreeInterval.GetDistanceInMeter();
            Assert.AreEqual(distance, distanceInMeter, eps);
        }

    }
}

