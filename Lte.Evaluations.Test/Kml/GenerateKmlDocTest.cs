using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Kml;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Kml
{
    [TestFixture]
    public class GenerateKmlDocTest : FrameworkWriter
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestGenerateKmlDoc_SameModInterference()
        {
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "begin");
            KmlTestInfrastructure.StatValueField.FieldName = "同模干扰电平";
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.FieldName, "同模干扰电平");
            List<MeasurePointInfo> measurePointList =
                KmlTestInfrastructure.MeasurePointList.Select(x =>
                    new MeasurePointInfo(x, KmlTestInfrastructure.StatValueField, 0.1)).ToList();
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "begin1");
            Assert.AreEqual(measurePointList[0].ColorStringForKml, "800A0C80", "middle");
            Assert.AreEqual(measurePointList[1].ColorStringForKml, "80670C0C");
            Assert.AreEqual(measurePointList[2].ColorStringForKml, "80FFFFFF");
            Assert.AreEqual(measurePointList[0].CoordinatesInfo, 
                "112.05,23.15,10 112.15,23.15,10 112.15,23.25,10 112.05,23.25,10");

            XmlDocument doc = GoogleKml.GenerateKmlDoc(measurePointList,
                KmlTestInfrastructure.StatValueField, reader);
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "end");
            Assert.IsNotNull(doc.InnerXml);
            Assert.AreEqual(doc.InnerXml.IndexOf(@"<Style id=""Color-800A0C80"" xmlns="""">", System.StringComparison.Ordinal), 112);
            Assert.AreEqual(doc.InnerXml.IndexOf(@"<Placemark><name>测试点</name><styleUrl>Color-80670C0C</styleUrl>", System.StringComparison.Ordinal), 911);

        }

        [Test]
        public void TestGenerateKmlDoc_DifferentModInterference()
        {
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "begin");
            KmlTestInfrastructure.StatValueField.FieldName = "不同模干扰电平";
            List<MeasurePointInfo> measurePointList =
                KmlTestInfrastructure.MeasurePointList.Select(x =>
                    new MeasurePointInfo(x, KmlTestInfrastructure.StatValueField, 0.1)).ToList();
            Assert.AreEqual(measurePointList[0].ColorStringForKml, "80670C0C");
            Assert.AreEqual(measurePointList[1].ColorStringForKml, "80670C0C");
            Assert.AreEqual(measurePointList[2].ColorStringForKml, "80670C0C");

            XmlDocument doc = GoogleKml.GenerateKmlDoc(measurePointList,
                KmlTestInfrastructure.StatValueField, reader);
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "end");
            Assert.IsNotNull(doc.InnerXml);
            Assert.AreEqual(doc.InnerXml,
                @"<?xml version=""1.0"" encoding=""utf-16""?>"
                + @"<kml xmlns=""http://earth.google.com/kml/2.1"">"
                + @"<Document><name>KML地图</name>"
                + @"<Style id=""Color-800A0C80"" xmlns=""""><LineStyle><width>1</width>"
                + @"<color>FFFF8080</color></LineStyle><PolyStyle><color>800A0C80</color></PolyStyle></Style>"
                + @"<Style id=""Color-80670C0C"" xmlns=""""><LineStyle><width>1</width>"
                + @"<color>FFFF8080</color></LineStyle><PolyStyle><color>80670C0C</color></PolyStyle></Style>"
                + @"<Style id=""Color-800A7B0C"" xmlns=""""><LineStyle><width>1</width>"
                + @"<color>FFFF8080</color></LineStyle><PolyStyle><color>800A7B0C</color></PolyStyle></Style>"
                + @"<Folder xmlns=""""><name>测试点序列</name>"
                + @"<Placemark><name>测试点</name><styleUrl>Color-80670C0C</styleUrl>"
                + @"<Polygon><extrude>1</extrude><altitudeMode>relativeToGround</altitudeMode>"
                + @"<outerBoundaryIs><LinearRing>"
                + @"<coordinates>112.05,23.15,10 112.15,23.15,10 112.15,23.25,10 112.05,23.25,10</coordinates>"
                + @"</LinearRing></outerBoundaryIs></Polygon></Placemark>"
                + @"<Placemark><name>测试点</name><styleUrl>Color-80670C0C</styleUrl>"
                + @"<Polygon><extrude>1</extrude><altitudeMode>relativeToGround</altitudeMode>"
                + @"<outerBoundaryIs><LinearRing>"
                + @"<coordinates>112.15,23.25,10 112.25,23.25,10 112.25,23.35,10 112.15,23.35,10</coordinates>"
                + @"</LinearRing></outerBoundaryIs></Polygon></Placemark>"
                + @"<Placemark><name>测试点</name><styleUrl>Color-80670C0C</styleUrl>"
                + @"<Polygon><extrude>1</extrude><altitudeMode>relativeToGround</altitudeMode>"
                + @"<outerBoundaryIs><LinearRing>"
                + @"<coordinates>112.25,23.35,10 112.35,23.35,10 112.35,23.45,10 112.25,23.45,10</coordinates>"
                + @"</LinearRing></outerBoundaryIs></Polygon></Placemark>"
                + @"</Folder></Document></kml>");

        }
    }
}
