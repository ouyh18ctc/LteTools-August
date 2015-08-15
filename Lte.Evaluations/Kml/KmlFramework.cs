using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Lte.Evaluations.Kml
{
    public class KmlFramework
    {
        public string KmlFileName { get; set; }

        public KmlFramework(string fileName)
        {
            KmlFileName = fileName;
            if (!File.Exists(KmlFileName))
            {
                TextWriter stream = new StreamWriter(KmlFileName);

                XmlWriter writer = GenerateWriter(stream);
                writer.Close();
            }
        }

        public XmlWriter GenerateWriter(TextWriter stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter writer = XmlWriter.Create(stream, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("kml", "http://earth.google.com/kml/2.1");
            writer.WriteStartElement("Document");
            writer.WriteElementString("name", "KML地图");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return writer;
        }

    }
}
