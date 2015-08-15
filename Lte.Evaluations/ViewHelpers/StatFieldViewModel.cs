using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.ViewHelpers
{
    public class StatFieldViewModel : StatValueField
    {
        public int FieldLength { get; set; }

        public string ColorThemeDescription { get; set; }

        public StatFieldViewModel(StatValueField field)
        {
            FieldName = field.FieldName;
            IntervalList = field.IntervalList;
        }

        public StatFieldViewModel(StatValueField field, IEnumerable<double> values)
            : this(field)
        {
            IntervalSettingList = new List<StatValueIntervalSetting>();
            for (int i = 0; i < IntervalList.Count; i++)
            {
                IntervalSettingList.Add(new StatValueIntervalSetting
                {
                    IntervalLowLevel = IntervalList[i].IntervalLowLevel,
                    IntervalUpLevel = IntervalList[i].IntervalUpLevel,
                    SuggestUpLevel = IntervalList[i].IntervalUpLevel,
                    RecordsCount = values.Count(x => x >= IntervalList[i].IntervalLowLevel
                                                     && x < IntervalList[i].IntervalUpLevel)
                });
            }
        }

        public bool AutoAdjustIntevals(IEnumerable<double> values)
        {
            double[] sortedValues = values.Distinct().ToArray();
            Array.Sort(sortedValues);
            if (sortedValues.Length < IntervalList.Count) return false;
            double minLevel = Math.Min(sortedValues[0], IntervalList[0].IntervalLowLevel);
            double maxLevel = Math.Max(sortedValues[sortedValues.Length - 1],
                IntervalList[IntervalList.Count - 1].IntervalUpLevel);
            if (maxLevel - minLevel < 0.1) return false;
            IntervalSettingList[0].IntervalLowLevel = minLevel;
            for (int i = 0; i < IntervalList.Count; i++)
            {
                int expectedCount = (int) ((i + 1)*(double) sortedValues.Length/IntervalList.Count);
                IntervalSettingList[i].SuggestUpLevel = sortedValues[expectedCount - 1];
                IntervalSettingList[i].RecordsCount = values.Count(
                    x => x >= ((i == 0) ? minLevel : IntervalSettingList[i - 1].SuggestUpLevel)
                        && x < IntervalSettingList[i].SuggestUpLevel);
            }
            return true;
        }

        public List<StatValueIntervalSetting> IntervalSettingList { get; set; }
    }

    public class StatFieldsSelectionViewModel
    {
        public string RuFieldName { get; set; }

        public string ValueFieldName { get; set; }

        public double Longtitute { get; set; }

        public double Lattitute { get; set; }
    }
}