using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Entities
{
    public class Color
    {
        public byte ColorA { get; set; }

        public byte ColorR { get; set; }

        public byte ColorG { get; set; }

        public byte ColorB { get; set; }

        public string ColorStringForKml
        {
            get
            {
                return ColorA.ToString("X2") + ColorB.ToString("X2")
                + ColorG.ToString("X2") + ColorR.ToString("X2");
            }
        }

        public string ColorStringForHtml
        {
            get
            {
                return ColorR.ToString("X2") + ColorB.ToString("X2")
                + ColorG.ToString("X2");
            }
        }

        public void SetColor(XElement element)
        {
            ColorA = element.Element("A").Value.ConvertToByte(0);
            ColorB = element.Element("B").Value.ConvertToByte(0);
            ColorG = element.Element("G").Value.ConvertToByte(0);
            ColorR = element.Element("R").Value.ConvertToByte(0);
        }
    }
}
