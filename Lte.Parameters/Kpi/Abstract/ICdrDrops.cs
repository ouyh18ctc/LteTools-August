using System.Collections.Generic;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Abstract
{
    public interface ICdrDrops
    {
        int CdrCalls { get; set; }

        int CdrDrops { get; set; }

        CdrDropsDistanceInfo CdrDropsDistanceInfo { get; set; }

        double AverageRssi { get; set; }

        double AverageDropEcio { get; set; }

        double AverageDropDistance { get; set; }

        CdrDropsHourInfo CdrDropsHourInfo { get; set; }
    }

    public interface IDropEcio
    {
        DropEcioDistanceInfo DropEcioDistanceInfo { get; set; }

        DropEcioHourInfo DropEcioHourInfo { get; set; }
    }

    public interface IGoodEcio
    {
        GoodEcioDistanceInfo GoodEcioDistanceInfo { get; set; }
    }

    public interface ICdrCalls
    {
        CdrCallsDistanceInfo CdrCallsDistanceInfo { get; set; }

        CdrCallsHourInfo CdrCallsHourInfo { get; set; }
    }

    public interface IKpiCalls
    {
        int KpiCalls { get; set; }

        int KpiDrops { get; set; }

        KpiCallsHourInfo KpiCallsHourInfo { get; set; }
    }

    public interface IKpiDrops
    {
        KpiDropsHourInfo KpiDropsHourInfo { get; set; }
    }

    public interface IErasureDrops
    {
        int ErasureDrops { get; set; }

        ErasureDropsHourInfo ErasureDropsHourInfo { get; set; }
    }

    public interface IAlarm
    {
        ICollection<AlarmHourInfo> AlarmHourInfos { get; set; }
    }

    public interface IMainRssi
    {
        double MainRssi { get; set; }

        MainRssiHourInfo MainRssiHourInfo { get; set; }
    }

    public interface ISubRssi
    {
        double SubRssi { get; set; }

        SubRssiHourInfo SubRssiHourInfo { get; set; }
    }

    public interface IDropCause
    {
        string DropCause { get; set; }

        ICollection<NeighborHourInfo> NeighborHourInfos { get; set; }
    }
}
