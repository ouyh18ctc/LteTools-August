using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lte.Evaluations.Kml
{
    public abstract class KmlElement
    {
        protected readonly XmlDocument doc;

        protected KmlElement(XmlDocument doc)
        {
            this.doc = doc;
        }

        public abstract XmlElement CreateElement();
    }

    public class FolderKmlElement : KmlElement
    {
        private readonly string name;

        public FolderKmlElement(XmlDocument doc, string name)
            : base(doc)
        {
            this.name = name;
        }

        public override XmlElement CreateElement()
        {
            XmlElement element = doc.CreateElement("Folder");
            element.AppendChild(doc.CreateNameElement(name));
            return element;
        }
    }

    public static class FolderKmlOperations
    {
        public static void AddPlacemarkElement(this XmlDocument doc, XmlElement placemarkElement)
        {
            XmlNode folderNode = doc.GetElementsByTagName("Folder")[0];
            folderNode.AppendChild(placemarkElement);
        }
    }

    public class PlacemarkKmlElement : KmlElement
    {
        private readonly string name;

        private string styleUrl;

        public string StyleUrl
        {
            get { return styleUrl; }
            set { styleUrl = value; }
        }

        private string coordinatesInfo;

        public string CoordinatesInfo
        {
            get { return coordinatesInfo; }
            set { coordinatesInfo = value; }
        }

        public PlacemarkKmlElement(XmlDocument doc, string name)
            : base(doc)
        {
            this.name = name;
        }

        public override XmlElement CreateElement()
        {
            XmlElement element = doc.CreatePlacemarkElement(name, styleUrl,
                "1", "relativeToGround", coordinatesInfo);
            return element;
        }
    }

    public class StyleKmlElement : KmlElement
    {
        private readonly string id;

        public StyleKmlElement(XmlDocument doc, string id)
            : base(doc)
        {
            this.id = id;
        }

        private short lineWidth;

        public short LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        private string lineColor;

        public string LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        private string polyColor;

        public string PolyColor
        {
            get { return polyColor; }
            set { polyColor = value; }
        }

        public override XmlElement CreateElement()
        {
            XmlElement styleElement = doc.CreateElement("Style");
            styleElement.SetAttribute("id", id);
            styleElement.AppendChild(
                doc.CreateLineStyleElement(lineWidth.ToString(CultureInfo.InvariantCulture), lineColor));
            styleElement.AppendChild(
                doc.CreatePolyStyleElement(polyColor));
            return styleElement;
        }
    }
}
