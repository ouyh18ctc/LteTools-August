using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lte.Evaluations.Kml
{
    public static class KmlDocumentOperations
    {
        public static XmlElement CreateWidthElement(this XmlDocument doc, string widthValue)
        {
            return doc.CreateGeneralElement("width", widthValue);
        }

        public static XmlElement CreateColorElement(this XmlDocument doc, string colorValue)
        {
            return doc.CreateGeneralElement("color", colorValue);
        }

        public static XmlElement CreateGeneralElement(this XmlDocument doc, string elementName, string value)
        {
            XmlElement element = doc.CreateElement(elementName);
            element.InnerText = value;
            return element;
        }

        public static XmlElement CreatePolyStyleElement(this XmlDocument doc, string polyColorValue)
        {
            XmlElement polyStyleElement = doc.CreateElement("PolyStyle");
            polyStyleElement.AppendChild(doc.CreateColorElement(polyColorValue));
            return polyStyleElement;
        }

        public static XmlElement CreateLineStyleElement(this XmlDocument doc, string lineWidthValue, string lineColorValue)
        {
            XmlElement lineStyleElement = doc.CreateElement("LineStyle");
            lineStyleElement.AppendChild(doc.CreateWidthElement(lineWidthValue));
            lineStyleElement.AppendChild(doc.CreateColorElement(lineColorValue));
            return lineStyleElement;
        }

        public static XmlElement CreatePlacemarkElement(this XmlDocument doc, string name, string styleUrl
            , string extrudeInfo, string modeInfo, string coordinatesInfo)
        {
            XmlElement placeMarkElement = doc.CreateElement("Placemark");
            placeMarkElement.AppendChild(doc.CreateNameElement(name));
            placeMarkElement.AppendChild(doc.CreateStyleUrlElement(styleUrl));
            placeMarkElement.AppendChild(doc.CreatePolygonElement(extrudeInfo, modeInfo, coordinatesInfo));
            return placeMarkElement;
        }

        public static XmlElement CreatePolygonElement(this XmlDocument doc, string extrudeInfo,
            string modeInfo, string coordinatesInfo)
        {
            XmlElement polygonElement = doc.CreateElement("Polygon");
            polygonElement.AppendChild(doc.CreateExtrudeElement(extrudeInfo));
            polygonElement.AppendChild(doc.CreateAltituteModeElement(modeInfo));
            polygonElement.AppendChild(doc.CreateOuterBoundaryIsElement(coordinatesInfo));
            return polygonElement;
        }

        public static XmlElement CreateOuterBoundaryIsElement(this XmlDocument doc, string coordinatesInfo)
        {
            XmlElement outerBoundaryIsElement = doc.CreateElement("outerBoundaryIs");
            outerBoundaryIsElement.AppendChild(doc.CreateLinearRingElement(coordinatesInfo));
            return outerBoundaryIsElement;
        }

        public static XmlElement CreateLinearRingElement(this XmlDocument doc, string coordinatesInfo)
        {
            XmlElement linearRingElement = doc.CreateElement("LinearRing");
            linearRingElement.AppendChild(doc.CreateCoordinatesElement(coordinatesInfo));
            return linearRingElement;
        }

        public static XmlElement CreateCoordinatesElement(this XmlDocument doc, string coordinatesInfo)
        {
            return doc.CreateGeneralElement("coordinates", coordinatesInfo);
        }

        public static XmlElement CreateAltituteModeElement(this XmlDocument doc, string modeInfo)
        {
            return doc.CreateGeneralElement("altitudeMode", modeInfo);
        }

        public static XmlElement CreateExtrudeElement(this XmlDocument doc, string extrudeInfo)
        {
            return doc.CreateGeneralElement("extrude", extrudeInfo);
        }

        public static XmlElement CreateStyleUrlElement(this XmlDocument doc, string styleUrl)
        {
            return doc.CreateGeneralElement("styleUrl", styleUrl);
        }

        public static XmlElement CreateNameElement(this XmlDocument doc, string name)
        {
            return doc.CreateGeneralElement("name", name);
        }

        public static void InitializeKmlDocument(this XmlDocument doc)
        {
            XmlNode documentNode = doc.GetElementsByTagName("Document")[0];

            StyleKmlElement styleKmlElement = new StyleKmlElement(doc, "Red-Grid")
            {
                LineWidth = 1,
                LineColor = "FFFF8080",
                PolyColor = "800000FF"
            };
            XmlElement styleElement = styleKmlElement.CreateElement();

            FolderKmlElement folderKmlElement = new FolderKmlElement(doc, "测试点序列");
            XmlElement folderElement = folderKmlElement.CreateElement();

            documentNode.AppendChild(styleElement);
            documentNode.AppendChild(folderElement);
            doc.DocumentElement.AppendChild(documentNode);
        }

        public static void InitializeKmlDocument(this XmlDocument doc, IEnumerable<string> colorStringList)
        {
            XmlNode documentNode = doc.GetElementsByTagName("Document")[0];

            FolderKmlElement folderKmlElement = new FolderKmlElement(doc, "测试点序列");
            XmlElement folderElement = folderKmlElement.CreateElement();

            foreach (string colorString in colorStringList)
            {            
                StyleKmlElement styleKmlElement = new StyleKmlElement(doc, "Color-" + colorString)
                {
                    LineWidth = 1,
                    LineColor = "FFFF8080",
                    PolyColor = colorString
                };
                XmlElement styleElement = styleKmlElement.CreateElement();

                documentNode.AppendChild(styleElement);
            }

            documentNode.AppendChild(folderElement);
            doc.DocumentElement.AppendChild(documentNode);
        }
    }
}
