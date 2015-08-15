using System.Linq;
using System.Xml;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Kml;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Kml
{
    [TestFixture]
    public class InitializeKmlDocumentTest : FrameworkWriter
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestInitializeKmlDocument()
        {
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "begin");
            KmlTestInfrastructure.StatValueField.FieldName = "同模干扰电平";
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.FieldName, "同模干扰电平");
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            StatValueField field = KmlTestInfrastructure.StatValueField;
            doc.InitializeKmlDocument(field.IntervalList.Select(x => x.Color.ColorStringForKml));
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "end");
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
                + @"<Folder xmlns=""""><name>测试点序列</name></Folder>"
                + @"</Document></kml>");
        }
    }
}
