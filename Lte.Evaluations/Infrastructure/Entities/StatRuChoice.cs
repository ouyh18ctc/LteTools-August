using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Service;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Infrastructure.Entities
{
    public enum StatRuChoice
    {
        InterferenceSource,
        CoverageRange,
        NeighborDistance,
        Undefined
    }

    public static class StatRuChoiceQueries
    {
        private static readonly Dictionary<StatRuChoice, string> list = new Dictionary<StatRuChoice, string>
        {
            { StatRuChoice.InterferenceSource, "干扰源分析" },
            { StatRuChoice.CoverageRange, "干扰距离分析" },
            { StatRuChoice.NeighborDistance, "邻区距离分析" }
        };

        public static IEnumerable<string> Choices
        {
            get { return list.Select(x => x.Value); }
        }

        public static string GetStatRuDescription(this StatRuChoice choice)
        {
            return list[choice];
        }

        public static StatRuChoice GetStatRuIndex(this string statRuDescription)
        {
            return (list.ContainsValue(statRuDescription)) ?
                list.FirstOrDefault(x => x.Value == statRuDescription).Key :
                StatRuChoice.Undefined;
        }

        public static Func<RuInterferenceStat, InterferenceService<RuInterferenceStat>> 
            GetGenerator(this string fieldName)
        {
            switch (fieldName)
            {
                case "干扰源分析":
                    return x => new RuInterferenceSourceService(x);
                case "干扰距离分析":
                    return x => new RuInterferenceDistanceService(x);
                case "邻区距离分析":
                    return x => new RuInterferenceTaService(x);
                default:
                    return null;
            }
        }

        public static Func<MrsCellDateView, InterferenceService<MrsCellDateView>>
            MrsGenerator(this string fieldName)
        {
            switch (fieldName)
            {
                case "MR总数":
                    return x => new MrsTotalMrsService(x);
                case "覆盖率（>-105dBm）":
                    return x => new MrsCoverageRateService(x);
                case "覆盖率（>-110dBm）":
                    return x => new MrsCoverageTo110Service(x);
                case "覆盖率（>-115dBm）":
                    return x => new MrsCoverageTo115Service(x);
                default:
                    return null;
            }
        }
    }
}
