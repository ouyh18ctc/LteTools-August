using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Abstract
{
    public interface IStatValueIntervalList
    {
        List<StatValueInterval> IntervalList { get; set; }
    }

    public static class StatValueIntervalListQueries
    {
        public static string GetColor(this IStatValueIntervalList field, double value, string color)
        {
            if (field.IntervalList.Count > 0)
            {
                StatValueInterval interval = field.IntervalList.FirstOrDefault(
                    x => x.IntervalLowLevel <= value && value < x.IntervalUpLevel);
                if (interval != null)
                {
                    interval.Color.ColorA = 128;
                    color = interval.Color.ColorStringForHtml;
                }
            }
            return color;
        }
    }
}
