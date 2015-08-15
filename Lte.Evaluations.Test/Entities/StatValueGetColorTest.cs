using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Service;
using NUnit.Framework;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueGetColorTest
    {
        private RuInterferenceStat _stat;
        private StatValueField _field;
        private InterferenceService<RuInterferenceStat> _service;

        [SetUp]
        public void TestInitialize()
        {
            _stat = new RuInterferenceStat();
            _field = new StatValueField();
        }

        private void AsserOriginalColors()
        {
            Assert.AreEqual(_field.IntervalList[0].Color.ColorStringForHtml, "00FF00");
            Assert.AreEqual(_field.IntervalList[1].Color.ColorStringForHtml, "00FF7F");
            Assert.AreEqual(_field.IntervalList[2].Color.ColorStringForHtml, "00FFFF");
            Assert.AreEqual(_field.IntervalList[3].Color.ColorStringForHtml, "7F7FFF");
            Assert.AreEqual(_field.IntervalList[4].Color.ColorStringForHtml, "FF00FF");
            Assert.AreEqual(_field.IntervalList[5].Color.ColorStringForHtml, "FF007F");
            Assert.AreEqual(_field.IntervalList[6].Color.ColorStringForHtml, "FF0000");
            Assert.AreEqual(_field.IntervalList[7].Color.ColorStringForHtml, "7F0000");
        }

        [TestCase(0.2, 5, "00FF00")]
        [TestCase(0.5, 5, "00FFFF")]
        [TestCase(0.7, 5, "FF00FF")]
        [TestCase(0.3, 10, "00FFFF")]
        [TestCase(0.01, 15, "00FF00")]
        [TestCase(0.03, 15, "00FF00")]
        [TestCase(0.07, 15, "00FF00")]
        [TestCase(0.15, 15, "00FF7F")]
        [TestCase(0.3, 15, "00FFFF")]
        [TestCase(0.07, 30, "00FF00")]
        public void TestStatValueGetColor_InterferenceSource(double ratio, int victimCells,
            string expectedColor)
        {
            _field.FieldName = "干扰源分析";
            _service = new RuInterferenceSourceService(_stat);
            _field.AutoGenerateIntervals(8);
            AsserOriginalColors();
            _stat.InterferenceRatio = ratio;
            _stat.VictimCells = victimCells;
            string colorString = _service.GetColor(_field);
            Assert.AreEqual(colorString, expectedColor);
        }

        [TestCase(80, 20, "FFFFFF")]
        [TestCase(40, 20, "7F0000")]
        [TestCase(21, 20, "FF0000")]
        [TestCase(15, 20, "FF007F")]
        [TestCase(12, 20, "FF00FF")]
        [TestCase(10, 20, "7F7FFF")]
        [TestCase(7, 20, "00FFFF")]
        [TestCase(3, 20, "00FF7F")]
        [TestCase(21, 40, "7F7FFF")]
        [TestCase(15, 40, "00FFFF")]
        public void TestStatValueGetColor_InterferenceDistance(double taAverage, double averageRtd,
            string expectedColor)
        {
            _field.FieldName = "干扰距离分析";
            _service = new RuInterferenceDistanceService(_stat);
            _field.AutoGenerateIntervals(8);
            AsserOriginalColors();
            _stat.TaAverage = taAverage;
            _stat.AverageRtd = averageRtd;
            string colorString = _service.GetColor(_field);
            Assert.AreEqual(colorString, expectedColor, "colorString");
        }

        [TestCase(0.8, "7F0000")]
        [TestCase(0.5, "7F0000")]
        [TestCase(0.25, "FF007F")]
        [TestCase(0.19, "FF00FF")]
        [TestCase(0.1, "7F7FFF")]
        [TestCase(0.05, "00FFFF")]
        [TestCase(0.032, "00FFFF")]
        [TestCase(0.03, "00FFFF")]
        public void TestStatValueGetColor_TaExcessRate(double taExcessRate, string expectedColor)
        {
            _field.FieldName = "邻区距离分析";
            _service = new RuInterferenceTaService(_stat);
            _field.AutoGenerateIntervals(8);
            AsserOriginalColors();
            _stat.TaExcessRate = taExcessRate;
            string colorString = _service.GetColor(_field);
            Assert.AreEqual(colorString, expectedColor);
        }
    }
}
