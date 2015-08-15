using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Lte.Domain.Geo;
using Lte.Domain.Regular;
using Lte.Domain.Measure;
using Lte.Evaluations.Infrastructure;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Kml
{
    public class GoogleKml
    {
        private KmlFramework Framework { get; set; }

        public GoogleKml(string fileName)
        {
            Framework = new KmlFramework(fileName);
        }

        public void GenerateKmlFile(string kmlFileName,
            List<MeasurePointInfo> measurePointList, StatValueField field)
        {
            TextReader reader = new StreamReader(Framework.KmlFileName);
            XmlDocument doc = GenerateKmlDoc(measurePointList, field, reader);
            
            XmlTextWriter tr = new XmlTextWriter(kmlFileName, null);
            tr.Formatting = Formatting.Indented;
            doc.WriteContentTo(tr);
            tr.Close();
        }

        public static XmlDocument GenerateKmlDoc(List<MeasurePointInfo> measurePointList, 
            StatValueField field, TextReader reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            doc.InitializeKmlDocument(field.IntervalList.Select(x => x.Color.ColorStringForKml));

            for (int i = 0; i < measurePointList.Count(); i++)
            {
                PlacemarkKmlElement placemarkKmlElement = new PlacemarkKmlElement(doc, "测试点")
                {
                    StyleUrl = "Color-" + measurePointList[i].ColorStringForKml,
                    CoordinatesInfo = measurePointList[i].CoordinatesInfo
                };
                XmlElement placemarkElement = placemarkKmlElement.CreateElement();
                doc.AddPlacemarkElement(placemarkElement);
            }
            return doc;
        }
    }

}
