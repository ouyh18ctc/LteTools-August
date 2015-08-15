using System;
using Lte.Domain.Regular;

namespace Lte.Parameters.Abstract
{
    public interface IInterferenceStat
    {
        double AverageRtd { get; }

        double MinRtd { get; set; }

        double TaAverage { get; }

        double TaMax { get; set; }

        double TaExcessRate { get; }
    }

    public interface IInterferenceDb
    {
        int VictimCells { get; set; }

        int InterferenceCells { get; set; }
    }

    public static class InterferenceDbOperations
    {
        public static void UpdateInterferenceInfo(this IInterferenceDb src, IInterferenceDb dst)
        {
            if (src.VictimCells > dst.VictimCells && src.InterferenceCells >= dst.InterferenceCells)
            {
                src.CloneProperties<IInterferenceDb>(dst);
            }
        }
    }

    public interface IRtdDb
    {
        double SumRtds { get; set; }

        int TotalRtds { get; set; }

        double MinRtd { get; set; }
    }

    public static class IRtdDbOperations
    {
        public static void UpdateRtdInfo(this IRtdDb src, IRtdDb dst)
        {
            if (src.TotalRtds > dst.TotalRtds *10)
            {
                src.CloneProperties<IRtdDb>(dst);
            }
            else if (dst.TotalRtds <= src.TotalRtds*10)
            {
                dst.SumRtds += src.SumRtds;
                dst.TotalRtds += src.TotalRtds;
                dst.MinRtd = Math.Min(src.MinRtd, dst.MinRtd);
            }
        }
    }

    public interface ITaDb
    {
        int TaOuterIntervalNum { get; set; }

        int TaInnerIntervalNum { get; set; }

        double TaSum { get; set; }

        int TaOuterIntervalExcessNum { get; set; }

        int TaInnerIntervalExcessNum { get; set; }

        double TaMax { get; set; }
    }

    public static class TaDbOperations
    {
        public static double GetTaAverage(this ITaDb stat)
        {
            return (stat.TaInnerIntervalNum + stat.TaOuterIntervalNum == 0) ? 0 :
                    stat.TaSum / (stat.TaInnerIntervalNum + stat.TaOuterIntervalNum);
        }

        public static void UpdateTaInfo(this ITaDb src, ITaDb dst)
        {
            if (src.TaSum > dst.TaSum * 10)
            {
                src.CloneProperties<ITaDb>(dst);
            }
            else if (dst.TaSum <= src.TaSum*10)
            {
                dst.TaSum += src.TaSum;
                dst.TaOuterIntervalExcessNum += src.TaOuterIntervalExcessNum;
                dst.TaOuterIntervalNum += src.TaOuterIntervalNum;
                dst.TaInnerIntervalExcessNum += src.TaInnerIntervalExcessNum;
                dst.TaInnerIntervalNum += src.TaInnerIntervalNum;
                dst.TaMax = Math.Max(src.TaMax, dst.TaMax);
            }
        }
    }
}
