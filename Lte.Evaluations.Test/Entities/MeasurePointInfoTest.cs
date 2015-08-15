using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Test.Kml;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class MeasurePointInfoTest
    {
        private MeasurePoint _point;
        private readonly StatValueField statValueField = KmlTestInfrastructure.StatValueField;
        private MeasurePointInfo _info;

        [SetUp]
        public void TestInitialize()
        {
            
            _point = new MeasurePoint(new GeoPoint(112.1, 23.2))
            {
                Result = new SfMeasurePointResult
                {
                    SameModInterferenceLevel = 0.5,
                    DifferentModInterferenceLevel = 1.5,
                    TotalInterferencePower = 2.5,
                    NominalSinr = 3.5
                }
            };
        }

        [Test]
        public void TestMeasurePointInfo_SameModInterference()
        {
            statValueField.FieldName = "同模干扰电平";
            _info = new MeasurePointInfo(_point, statValueField, 0.1);
            Assert.IsNotNull(_info);
            Assert.AreEqual(_info.ColorStringForKml, "800A0C80");
            Assert.AreEqual(_info.CoordinatesInfo, "112.05,23.15,10 112.15,23.15,10 112.15,23.25,10 112.05,23.25,10");
        }

        [Test]
        public void TestMeasurePointInfo_DiffModInterference()
        {
            statValueField.FieldName = "不同模干扰电平";
            _info = new MeasurePointInfo(_point, statValueField, 0.1);
            Assert.IsNotNull(_info);
            Assert.AreEqual(_info.ColorStringForKml, "80670C0C");
            Assert.AreEqual(_info.CoordinatesInfo, "112.05,23.15,10 112.15,23.15,10 112.15,23.25,10 112.05,23.25,10");
        }

        [Test]
        public void TestMeasurePointInfo_TotalInterference()
        {
            statValueField.FieldName = "总干扰电平";
            _info = new MeasurePointInfo(_point, statValueField, 0.1);
            Assert.IsNotNull(_info);
            Assert.AreEqual(_info.ColorStringForKml, "800A7B0C");
            Assert.AreEqual(_info.CoordinatesInfo, "112.05,23.15,10 112.15,23.15,10 112.15,23.25,10 112.05,23.25,10");
        }

        [Test]
        public void TestMeasurePointInfo_NominalSinr()
        {
            statValueField.FieldName = "标称SINR";
            _info = new MeasurePointInfo(_point, statValueField, 0.1);
            Assert.IsNotNull(_info);
            Assert.AreEqual(_info.ColorStringForKml, "80FFFFFF");
            Assert.AreEqual(_info.CoordinatesInfo, "112.05,23.15,10 112.15,23.15,10 112.15,23.25,10 112.05,23.25,10");
        }
    }
}
