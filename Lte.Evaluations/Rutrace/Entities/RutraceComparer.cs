using System.Collections.Generic;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class VictimCellsDescendComparer : IComparer<RuInterferenceStat>
    {
        public int Compare(RuInterferenceStat x, RuInterferenceStat y)
        {
            return -x.VictimCells.CompareTo(y.VictimCells);
        }
    }

    public class InterfernceRatioDescendComparer : IComparer<RuInterferenceStat>
    {
        public int Compare(RuInterferenceStat x, RuInterferenceStat y)
        {
            return -x.InterferenceRatio.CompareTo(y.InterferenceRatio);
        }
    }

    public class AverageRtdDescendComparer : IComparer<RuInterferenceStat>
    {
        public int Compare(RuInterferenceStat x, RuInterferenceStat y)
        {
            return -x.AverageRtd.CompareTo(y.AverageRtd);
        }
    }

    public class TaAverageDescendComparer : IComparer<RuInterferenceStat>
    {
        public int Compare(RuInterferenceStat x, RuInterferenceStat y)
        {
            return -x.TaAverage.CompareTo(y.TaAverage);
        }
    }

    public class TaExcessRateDescendComparer : IComparer<RuInterferenceStat>
    {
        public int Compare(RuInterferenceStat x, RuInterferenceStat y)
        {
            return -x.TaExcessRate.CompareTo(y.TaExcessRate);
        }
    }
}
