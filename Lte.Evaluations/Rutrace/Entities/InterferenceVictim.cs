using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class InterferenceVictim : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int MeasuredTimes { get; set; }

        public int InterferenceTimes { get; set; }

        public double InterferenceRatio
        {
            get { return (double)InterferenceTimes / MeasuredTimes; }
        }

        public InterferenceVictim(ICell cell)
        {
            CellId = cell.CellId;
            SectorId = cell.SectorId;
            MeasuredTimes = 0;
            InterferenceTimes = 0;
        }
    }

    public class InterferenceVictimComparer : IComparer<InterferenceVictim>
    {
        public int Compare(InterferenceVictim x, InterferenceVictim y)
        {
            return y.InterferenceTimes - x.InterferenceTimes;
        }
    }
}
