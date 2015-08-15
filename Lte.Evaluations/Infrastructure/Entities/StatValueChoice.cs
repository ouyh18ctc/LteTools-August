using System.Collections.Generic;
using System.Linq;

namespace Lte.Evaluations.Infrastructure.Entities
{
    public enum StatValueChoice : int
    {
        SameModInterference,
        DifferentModInterference,
        TotalInterference,
        NominalSinr,
        Rsrp,
        Undefined
    }

    public static class StatValueChoiceQueries
    {
        private static Dictionary<StatValueChoice, string> list = new Dictionary<StatValueChoice, string>(){
            { StatValueChoice.SameModInterference, "同模干扰电平" },
            { StatValueChoice.DifferentModInterference, "不同模干扰电平" },
            { StatValueChoice.TotalInterference, "总干扰电平" },
            { StatValueChoice.NominalSinr, "标称SINR" },
            { StatValueChoice.Rsrp, "信号RSRP" }
        };

        public static IEnumerable<string> Choices
        {
            get { return list.Select(x => x.Value); }
        }

        public static string GetStatValueDescription(this StatValueChoice choice)
        {
            return list[choice];
        }

        public static StatValueChoice GetStatValueIndex(this string statValueDescription)
        {
            return (list.ContainsValue(statValueDescription)) ?
                list.FirstOrDefault(x => x.Value == statValueDescription).Key :
                StatValueChoice.Undefined;
        }
    }
}
