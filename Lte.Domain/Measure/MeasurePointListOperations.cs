using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Measure
{
    
    public static class MeasurePointListOperations
    {
        private static readonly double[] defaultThreshold = { -30, -200, -200, -200 };

        public static IEnumerable<MeasurePoint> FilterNormalPoints(this IEnumerable<MeasurePoint> sourceList,
            MeasurePointKpiSelection selectedIndex, double[] filterThreshold = null)
        {
            double[] filterValues = filterThreshold ?? defaultThreshold;
            switch (selectedIndex)
            {
                case MeasurePointKpiSelection.NominalSinr:
                    return sourceList.Where(x => x.Result.NominalSinr > filterValues[0]);
                case MeasurePointKpiSelection.StrongestCellRsrp:
                    return sourceList.Where(x => x.Result.StrongestCell.ReceivedRsrp > filterValues[1]);
                case MeasurePointKpiSelection.StrongestInterferenceRsrp:
                    return sourceList.Where(x => x.Result.StrongestInterference.ReceivedRsrp > filterValues[2]);
                default:
                    return sourceList.Where(x => x.Result.TotalInterferencePower > filterValues[3]);
            }
        }

        public static IEnumerable<double> GetValues(this IEnumerable<MeasurePoint> sourceList,
            MeasurePointKpiSelection selectedIndex)
        {
            switch (selectedIndex)
            {
                case MeasurePointKpiSelection.NominalSinr:
                    return sourceList.Select(x => x.Result.NominalSinr);
                case MeasurePointKpiSelection.StrongestCellRsrp:
                    return sourceList.Select(x => x.Result.StrongestCell.ReceivedRsrp);
                case MeasurePointKpiSelection.StrongestInterferenceRsrp:
                    return sourceList.Select(x => x.Result.StrongestInterference.ReceivedRsrp);
                default:
                    return sourceList.Select(x => x.Result.TotalInterferencePower);
            }
        }

    }
}
