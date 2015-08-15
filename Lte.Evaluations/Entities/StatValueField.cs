using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Infrastructure;
using Lte.Evaluations.Infrastructure.Entities;
using Lte.Evaluations.Service;

namespace Lte.Evaluations.Entities
{
    public class StatValueField : IStatValueIntervalList
    {
        private string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
                switch (value)
                {
                    case "同模干扰电平":
                    case "不同模干扰电平":
                    case "总干扰电平":
                    case "信号RSRP":
                        _generator = new RsrpIntervalsGenerator(IntervalList);
                        break;
                    case "标称SINR":
                        _generator = new SinrIntervalsGenerator(IntervalList);
                        break;
                    case "干扰源分析":
                        _generator = new InterferenceIntervalsGenerator(IntervalList);
                        break;
                    case "干扰距离分析":
                        _generator = new DistanceIntervalsGenerator(IntervalList);
                        break;
                    default:
                        _generator = new ExcessIntervalsGenerator(IntervalList);
                        break;
                }
            }
        }

        private IntervalsGenerator _generator;

        public List<StatValueInterval> IntervalList { get; set; }

        public StatValueField()
        {
            IntervalList = new List<StatValueInterval>();
            FieldName = "同模干扰电平";
        }

        public StatValueField(XElement fieldElement)
        {
            IntervalList = new List<StatValueInterval>();
            FieldName = fieldElement.Attribute("ID").Value;
            foreach (XElement element in fieldElement.Descendants("Interval"))
            {
                IntervalList.Add(new StatValueInterval(element));
            }
        }

        public void AutoGenerateIntervals(int intervalLength, ColorTheme theme = ColorTheme.Jet)
        {
            IntervalList.Clear();
            _generator.Generate(intervalLength);
            GenerateIntervalColors(intervalLength, theme);
        }

        private void GenerateIntervalColors(int intervalLength, ColorTheme theme)
        {
            ColormapGenerator generator;
            switch (theme)
            {
                case ColorTheme.Autumn:
                    generator = new AutumnColormapGenerator(intervalLength, 128);
                    break;
                case ColorTheme.Cool:
                    generator = new CoolColormapGenerator(intervalLength, 128);
                    break;
                case ColorTheme.Gray:
                    generator = new GrayColormapGenerator(intervalLength, 128);
                    break;
                case ColorTheme.Hot:
                    generator = new HotColormapGenerator(intervalLength, 128);
                    break;
                case ColorTheme.Jet:
                    generator = new JetColormapGenerator(intervalLength, 128);
                    break;
                case ColorTheme.Spring:
                    generator = new SpringColormapGenerator(intervalLength, 128);
                    break;
                case ColorTheme.Summer:
                    generator = new SummerColormapGenerator(intervalLength, 128);
                    break;
                default:
                    generator = new WinterColormapGenerator(intervalLength, 128);
                    break;
            }
            int length = Math.Max(intervalLength, IntervalList.Count);
            for (int i = 0; i < length; i++)
            {
                IntervalList[i].Color = new Color
                {
                    ColorA = generator.Cmap[i].ColorA,
                    ColorB = generator.Cmap[i].ColorB,
                    ColorG = generator.Cmap[i].ColorG,
                    ColorR = generator.Cmap[i].ColorR
                };
            }
        }

        public XElement GetFieldElement()
        {
            XElement fieldElement = new XElement("Field", new XAttribute("ID", FieldName));
            foreach (StatValueInterval interval in IntervalList)
            {
                fieldElement.Add(interval.XElement);
            }
            return fieldElement;
        }

        public Dictionary<string, double> GetPercentageStat(IEnumerable<double> values)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            int totals = values.Count();
            for (int i = 0; i < IntervalList.Count; i++)
            {
                double low = IntervalList[i].IntervalLowLevel;
                double high = IntervalList[i].IntervalUpLevel;
                int counts = values.Count(x => x >= low && x < high);
                result.Add("[ " + low + " , " + high + " )",
                    (double)counts / totals);
            }
            return result;
        }

        public void UpdateIntervalLowLevel(int selectedIndex, double lowLevel)
        {
            double minLowLevel = (selectedIndex == 0) ? -10000
                : IntervalList[selectedIndex - 1].IntervalLowLevel + 0.01;

            IntervalList[selectedIndex].IntervalLowLevel
                = Math.Min(IntervalList[selectedIndex].IntervalUpLevel - 0.01, Math.Max(lowLevel, minLowLevel));

            if (selectedIndex > 0)
            {
                IntervalList[selectedIndex - 1].IntervalUpLevel
                    = IntervalList[selectedIndex].IntervalLowLevel;
            }
        }

        public void UpdateIntervalUpLevel(int selectedIndex, double upLevel)
        {
            double maxUpLevel = (selectedIndex == IntervalList.Count - 1) ? 10000
                : IntervalList[selectedIndex + 1].IntervalUpLevel - 0.01;

            IntervalList[selectedIndex].IntervalUpLevel
                = Math.Max(IntervalList[selectedIndex].IntervalLowLevel + 0.01, Math.Min(upLevel, maxUpLevel));

            if (selectedIndex < IntervalList.Count - 1)
            {
                IntervalList[selectedIndex + 1].IntervalLowLevel
                    = IntervalList[selectedIndex].IntervalUpLevel;
            }
        }
    }
}
