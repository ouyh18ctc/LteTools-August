using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Entities
{
    public interface IStatValueInterval
    {
        double IntervalLowLevel { get; set; }
        double IntervalUpLevel { get; set; }
    }

    public class StatValueInterval : IStatValueInterval
    {
        public double IntervalLowLevel { get; set; }

        public double IntervalUpLevel { get; set; }

        public Color Color { get; set; }

        public XElement XElement
        {
            get
            {
                return new XElement("Interval",
                    new XElement("LowLevel", IntervalLowLevel.ToString()),
                    new XElement("UpLevel", IntervalUpLevel.ToString()),
                    new XElement("A", Color.ColorA.ToString()),
                    new XElement("B", Color.ColorB.ToString()),
                    new XElement("R", Color.ColorR.ToString()),
                    new XElement("G", Color.ColorG.ToString()));
            }
        }

        public StatValueInterval()
        {
            Color=new Color();
        }

        public StatValueInterval(XElement element) : this()
        {
            IntervalLowLevel = element.Element("LowLevel").Value.ConvertToDouble(-1);
            IntervalUpLevel = element.Element("UpLevel").Value.ConvertToDouble(1);
            Color.SetColor(element);
        }
    }

    public class StatValueIntervalSetting : IStatValueInterval
    {
        public double IntervalLowLevel { get; set; }
        public double IntervalUpLevel { get; set; }

        public double SuggestUpLevel { get; set; }

        public int RecordsCount { get; set; }
    }
}
