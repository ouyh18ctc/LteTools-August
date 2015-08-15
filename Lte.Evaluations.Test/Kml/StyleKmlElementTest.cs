using Lte.Evaluations.Kml;
using System.Xml;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Kml
{
    [TestFixture]
    public class StyleKmlElementTest : FrameworkWriter
    {
        private XmlDocument _doc;
        private StyleKmlElement _element;

        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestStyleKmlElement_EmptyDoc()
        {
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "begin");
            _doc = new XmlDocument();
            _element = new StyleKmlElement(_doc, "222");
            XmlElement element2 = _element.CreateElement();
            Assert.IsNotNull(element2);
            Assert.AreEqual(element2.InnerXml, 
                "<LineStyle><width>0</width><color></color></LineStyle><PolyStyle><color></color></PolyStyle>");
            Assert.AreEqual(element2.Attributes["id"].InnerXml, "222");
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "end");
        }

        [Test]
        public void TestStyleKmlElement_NewDoc()
        {
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "begin");
            Assert.AreEqual(writer.ToString().Replace("\r\n","\n"), (@"<?xml version=""1.0"" encoding=""utf-16""?>
<kml xmlns=""http://earth.google.com/kml/2.1"">
  <Document>
    <name>KML地图</name>
  </Document>
</kml>").Replace("\r\n", "\n"));

            _doc = new XmlDocument();
            _doc.Load(reader);
            XmlNode documentNode = _doc.GetElementsByTagName("Document")[0];

            _element = new StyleKmlElement(_doc, "Red-Grid")
            {
                LineWidth = 1,
                LineColor = "FFFF8080",
                PolyColor = "800000FF"
            };
            XmlElement styleElement = _element.CreateElement();
            documentNode.AppendChild(styleElement);
            if (_doc.DocumentElement != null) _doc.DocumentElement.AppendChild(documentNode);
            Assert.AreEqual(_doc.InnerXml.Replace("\r\n", "\n"), (@"<?xml version=""1.0"" encoding=""utf-16""?>" +
            @"<kml xmlns=""http://earth.google.com/kml/2.1""><Document><name>KML地图</name>"
            + @"<Style id=""Red-Grid"" xmlns=""""><LineStyle><width>1</width><color>FFFF8080</color></LineStyle>"
            + @"<PolyStyle><color>800000FF</color></PolyStyle></Style></Document></kml>").Replace("\r\n", "\n"));
            Assert.AreEqual(KmlTestInfrastructure.StatValueField.IntervalList[0].Color.ColorStringForKml,
                "800A0C80", "end");
        }
    }
}
