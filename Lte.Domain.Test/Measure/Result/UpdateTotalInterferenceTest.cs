using System;
using System.Collections.Generic;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Result
{
    [TestFixture]
    public class UpdateTotalInterferenceTest
    {
        private IMeasurePointResult result = new SfMeasurePointResult();
        private List<MeasurableCell> _rsInterference;
        private List<MeasurableCell> _trafficInterference;

        [Test]
        public void TestUpdateTotalInterference_BothNullInterferenceList()
        {
            _rsInterference = null;
            _trafficInterference = null;
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.IsNotNull(result.TotalInterferencePower);
            Assert.AreEqual(result.TotalInterferencePower, Double.MinValue);
        }

        [Test]
        public void TestUpdateTotalInterference_rsNull_trafficEmpty()
        {
            _rsInterference = null;
            _trafficInterference = new List<MeasurableCell>();
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.IsNotNull(result.TotalInterferencePower);
            Assert.AreEqual(result.TotalInterferencePower, Double.MinValue);
        }

        [Test]
        public void TestUpdateTotalInterference_rsEmpty_trafficNull()
        {
            _rsInterference = new List<MeasurableCell>();
            _trafficInterference = null;
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.IsNotNull(result.TotalInterferencePower);
            Assert.AreEqual(result.TotalInterferencePower, Double.MinValue);
        }

        [Test]
        public void TestUpdateTotalInterference_rsEmpty_trafficEmpty()
        {
            _rsInterference = new List<MeasurableCell>();
            _trafficInterference = new List<MeasurableCell>();
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.AreEqual(result.TotalInterferencePower, Double.MinValue);
        }

        [Test]
        public void TestUpdateTotalInterference_rsOneElement_trafficEmpty()
        {
            _rsInterference = new List<MeasurableCell>();
            MeasurableCell mcell1 = new MeasurableCell();
            mcell1.ReceivedRsrp = -12.3;
            _rsInterference.Add(mcell1);
            _trafficInterference = new List<MeasurableCell>();
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.AreEqual(result.TotalInterferencePower, -12.3);
        }

        [Test]
        public void TestUpdateTotalInterference_rsEmpty_trafficOneElement()
        {
            _rsInterference = new List<MeasurableCell>();           
            _trafficInterference = new List<MeasurableCell>();
            MeasurableCell mcell1 = new MeasurableCell();
            mcell1.ReceivedRsrp = -12.3;
            _trafficInterference.Add(mcell1);
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.AreEqual(result.TotalInterferencePower, -22.3);
        }

        [Test]
        public void TestUpdateTotalInterference_rsOneElement_trafficOneElement()
        {
            _rsInterference = new List<MeasurableCell>();
            _trafficInterference = new List<MeasurableCell>();
            MeasurableCell mcell1 = new MeasurableCell();
            mcell1.ReceivedRsrp = -12.3;
            MeasurableCell mcell2 = new MeasurableCell();
            mcell2.ReceivedRsrp = -12.3;
            _rsInterference.Add(mcell1);
            _trafficInterference.Add(mcell2);
            result.UpdateTotalInterference(0.1, _rsInterference, _trafficInterference);
            Assert.AreEqual(result.TotalInterferencePower, -11.886073, 1E-6);
        }
    }
}
