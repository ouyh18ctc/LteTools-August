using System.Collections.Generic;
using System.Reflection;

namespace Lte.Parameters.Kpi.Entities
{
    public class DistanceDistribution
    {
        public string DistanceDescription { get; set; }

        public int CdrCalls { get; set; }

        public int CdrDrops { get; set; }

        public double DropEcio { get; set; }

        public double GoodEcio { get; set; }
    }

    public class CoverageInterferenceDistribution
    {
        public int Hour { get; set; }

        public double DropEcio { get; set; }

        public double MainRssi { get; set; }

        public double SubRssi { get; set; }
    }

    public static class DistanceDistributionQueries
    {
        public static void Import(this List<DistanceDistribution> result,
            CdrCallsDistanceInfo cdrCallInfo, CdrDropsDistanceInfo cdrDropInfo,
            DropEcioDistanceInfo dropEcioInfo, GoodEcioDistanceInfo goodEcioInfo)
        {
            int[] distanceRange =
            {
                0,200,400,600,800,1000,1200,1400,1600,1800,2000,2200,2400,2600,2800,3000,4000,
                5000,6000,7000,8000,9000 };
            for (int i = 0; i < distanceRange.Length - 1; i++)
            {
                string propertyName = "DistanceTo" + distanceRange[i + 1] + "Info";
                PropertyInfo cdrCallsProperty = (typeof(CdrCallsDistanceInfo)).GetProperty(propertyName);
                PropertyInfo cdrDropsProperty = (typeof(CdrDropsDistanceInfo)).GetProperty(propertyName);
                PropertyInfo dropEcioProperty = (typeof(DropEcioDistanceInfo)).GetProperty(propertyName);
                PropertyInfo goodEcioProperty = (typeof(GoodEcioDistanceInfo)).GetProperty(propertyName);
                result.Add(new DistanceDistribution
                {
                    DistanceDescription = distanceRange[i] + " -> " + distanceRange[i + 1] + "m",
                    CdrCalls = (cdrCallInfo == null) ? 0 : (int) cdrCallsProperty.GetValue(cdrCallInfo),
                    CdrDrops = (cdrDropInfo == null) ? 0 : (int) cdrDropsProperty.GetValue(cdrDropInfo),
                    DropEcio = (dropEcioInfo == null) ? 0 : (double) dropEcioProperty.GetValue(dropEcioInfo),
                    GoodEcio = (goodEcioInfo == null) ? 0 : (double) goodEcioProperty.GetValue(goodEcioInfo)*100
                });
            }
            result.Add(
                new DistanceDistribution
                {
                    DistanceDescription = "9000m -> inf",
                    CdrCalls = (cdrCallInfo == null) ? 0 : cdrCallInfo.DistanceToInfInfo,
                    CdrDrops = (cdrDropInfo == null) ? 0 : cdrDropInfo.DistanceToInfInfo,
                    DropEcio = (dropEcioInfo == null) ? 0 : dropEcioInfo.DistanceToInfInfo,
                    GoodEcio = (goodEcioInfo == null) ? 0 : goodEcioInfo.DistanceToInfInfo * 100
                });
        }

        public static void Import(this List<CoverageInterferenceDistribution> result,
            DropEcioHourInfo dropEcioInfo, MainRssiHourInfo mainRssiInfo, SubRssiHourInfo subRssiInfo)
        {
            for (int hour = 0; hour < 24; hour++)
            {
                string propertyName = "Hour" + hour + "Info";
                PropertyInfo dropEcioProperty = (typeof(DropEcioHourInfo)).GetProperty(propertyName);
                PropertyInfo mainRssiProperty = (typeof(MainRssiHourInfo)).GetProperty(propertyName);
                PropertyInfo subRssiProperty = (typeof(SubRssiHourInfo)).GetProperty(propertyName);
                result.Add(new CoverageInterferenceDistribution
                {
                    Hour = hour,
                    DropEcio = (dropEcioInfo == null) ? 0 : (double)dropEcioProperty.GetValue(dropEcioInfo),
                    MainRssi = (mainRssiInfo == null) ? 0 : (double)mainRssiProperty.GetValue(mainRssiInfo),
                    SubRssi = (subRssiInfo == null) ? 0 : (double)subRssiProperty.GetValue(subRssiInfo)
                });
            }
        }
    }
}
