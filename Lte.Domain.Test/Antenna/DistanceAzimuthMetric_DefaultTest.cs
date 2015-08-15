using System;
using NUnit.Framework;
using Lte.Domain.Measure;

namespace Lte.Domain.Test.Antenna
{
    [TestFixture]
    public class DistanceAzimuthMetric_DefaultTest
    {
        private readonly DistanceAzimuthMetric metric = new DistanceAzimuthMetric();

        private const double Eps = 1E-6;

        [TestCase(0, 0, -70)]
        [TestCase(0.001, 0, -70)]
        [TestCase(0.01, 0, -70)]
        [TestCase(0.001, 5, -70)]
        [TestCase(0.002, 5, -70)]
        [TestCase(0.005, 5, -70)]
        [TestCase(0.005, 10, -70)]
        [TestCase(0.005, 15, -65.53605)]
        [TestCase(0.005, 20, -60.53605)]
        [TestCase(0.005, 25, -55.53605)]
        [TestCase(0.01, 5, -65)]
        [TestCase(0.01, 10, -60)]
        [TestCase(0.01, 15, -55)]
        [TestCase(0.01, 20, -50)]
        [TestCase(0.01, 25, -45)]
        [TestCase(0.01, 30, -40)]
        [TestCase(0.1, 0, -35)]
        public void Test_DistanceAzimuthMetric(double distance, double azimuthFactor, double result)
        {
            Assert.AreEqual(metric.Calculate(distance, azimuthFactor), result, Eps);
        }
    }

    [TestFixture]
    public class DistanceAzimuthMetric_SquareTest
    {
        private readonly DistanceAzimuthMetric metric = new DistanceAzimuthMetric(20);

        private const double Eps = 1E-6;

        [TestCase(0, 0, -70)]
        [TestCase(0.001, 0, -60)]
        [TestCase(0.01, 0, -40)]
        [TestCase(0.001, 5, -55)]
        [TestCase(0.01, 10, -30)]
        [TestCase(0.1, 0, -20)]
        [TestCase(0.1, 10, -10)]
        public void Test_distance0_azimuthFactor0(double distance, double azimuthFactor, double result)
        {
            Assert.AreEqual(metric.Calculate(distance, azimuthFactor), result, Eps);
        }
    }
}
