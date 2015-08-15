using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class MetricCalculate_DefaultTest
    {
        private readonly FakeComparableCell mockCC = new FakeComparableCell();
        const double eps = 1E-6;

        [Test]
        public void TestMetricCalculate_Default_distance10m_angle0()
        {
            mockCC.SetupComparableCellProperties(0.01, 0);
            Assert.AreEqual(mockCC.MetricCalculate(), -70);
        }

        [Test]
        public void TestMetricCalculate_Default_distance10m_angle30()
        {
            mockCC.SetupComparableCellProperties(0.01, 30);
            Assert.AreEqual(mockCC.MetricCalculate(), -67.435345, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance10m_angle60()
        {
            mockCC.SetupComparableCellProperties(0.01, 60);
            Assert.AreEqual(mockCC.MetricCalculate(), -57.435345, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance10m_angle90()
        {
            mockCC.SetupComparableCellProperties(0.01, 90);
            Assert.AreEqual(mockCC.MetricCalculate(), -40, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance10m_angle120()
        {
            mockCC.SetupComparableCellProperties(0.01, 120);
            Assert.AreEqual(mockCC.MetricCalculate(), -40, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance20m_angle0()
        {
            mockCC.SetupComparableCellProperties(0.02, 0);
            Assert.AreEqual(mockCC.MetricCalculate(), -59.46395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance20m_angle30()
        {
            mockCC.SetupComparableCellProperties(0.02, 30);
            Assert.AreEqual(mockCC.MetricCalculate(), -56.899295, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance20m_angle60()
        {
            mockCC.SetupComparableCellProperties(0.02, 60);
            Assert.AreEqual(mockCC.MetricCalculate(), -46.899295, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance20m_angle90()
        {
            mockCC.SetupComparableCellProperties(0.02, 90);
            Assert.AreEqual(mockCC.MetricCalculate(), -29.46395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance20m_angle120()
        {
            mockCC.SetupComparableCellProperties(0.02, 120);
            Assert.AreEqual(mockCC.MetricCalculate(), -29.46395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance50m_angle0()
        {
            mockCC.SetupComparableCellProperties(0.05, 0);
            Assert.AreEqual(mockCC.MetricCalculate(), -45.53605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance50m_angle30()
        {
            mockCC.SetupComparableCellProperties(0.05, 30);
            Assert.AreEqual(mockCC.MetricCalculate(), -42.971395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance50m_angle60()
        {
            mockCC.SetupComparableCellProperties(0.05, 60);
            Assert.AreEqual(mockCC.MetricCalculate(), -32.971395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance50m_angle90()
        {
            mockCC.SetupComparableCellProperties(0.05, 90);
            Assert.AreEqual(mockCC.MetricCalculate(), -15.53605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance50m_angle120()
        {
            mockCC.SetupComparableCellProperties(0.05, 120);
            Assert.AreEqual(mockCC.MetricCalculate(), -15.53605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance100m_angle0()
        {
            mockCC.SetupComparableCellProperties(0.1, 0);
            Assert.AreEqual(mockCC.MetricCalculate(), -35, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance100m_angle30()
        {
            mockCC.SetupComparableCellProperties(0.1, 30);
            Assert.AreEqual(mockCC.MetricCalculate(), -32.435345, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance100m_angle60()
        {
            mockCC.SetupComparableCellProperties(0.1, 60);
            Assert.AreEqual(mockCC.MetricCalculate(), -22.435345, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance100m_angle90()
        {
            mockCC.SetupComparableCellProperties(0.1, 90);
            Assert.AreEqual(mockCC.MetricCalculate(), -5, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance100m_angle120()
        {
            mockCC.SetupComparableCellProperties(0.1, 120);
            Assert.AreEqual(mockCC.MetricCalculate(), -5, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance200m_angle0()
        {
            mockCC.SetupComparableCellProperties(0.2, 0);
            Assert.AreEqual(mockCC.MetricCalculate(), -24.46395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance200m_angle30()
        {
            mockCC.SetupComparableCellProperties(0.2, 30);
            Assert.AreEqual(mockCC.MetricCalculate(), -21.899295, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance200m_angle60()
        {
            mockCC.SetupComparableCellProperties(0.2, 60);
            Assert.AreEqual(mockCC.MetricCalculate(), -11.899295, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance200m_angle90()
        {
            mockCC.SetupComparableCellProperties(0.2, 90);
            Assert.AreEqual(mockCC.MetricCalculate(), 5.53605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance200m_angle120()
        {
            mockCC.SetupComparableCellProperties(0.2, 120);
            Assert.AreEqual(mockCC.MetricCalculate(), 5.53605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance500m_angle0()
        {
            mockCC.SetupComparableCellProperties(0.5, 0);
            Assert.AreEqual(mockCC.MetricCalculate(), -10.53605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance500m_angle30()
        {
            mockCC.SetupComparableCellProperties(0.5, 30);
            Assert.AreEqual(mockCC.MetricCalculate(), -7.971395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance500m_angle60()
        {
            mockCC.SetupComparableCellProperties(0.5, 60);
            Assert.AreEqual(mockCC.MetricCalculate(), 2.028605, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance500m_angle90()
        {
            mockCC.SetupComparableCellProperties(0.5, 90);
            Assert.AreEqual(mockCC.MetricCalculate(), 19.46395, eps);
        }

        [Test]
        public void TestMetricCalculate_Default_distance500m_angle120()
        {
            mockCC.SetupComparableCellProperties(0.5, 120);
            Assert.AreEqual(mockCC.MetricCalculate(), 19.46395, eps);
        }
    }
}
