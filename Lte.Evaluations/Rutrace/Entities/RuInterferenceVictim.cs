using Lte.Domain.Geo;
using Lte.Domain.Geo.Abstract;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class RuInterferenceVictim : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int MeasuredTimes { get; set; }

        public int InterferenceTimes { get; set; }

        public double InterferenceRatio
        {
            get { return (double)InterferenceTimes / MeasuredTimes; }
        }

        public RuInterferenceVictim(ICell cell)
        {
            CellId = cell.CellId;
            SectorId = cell.SectorId;
            MeasuredTimes = 0;
            InterferenceTimes = 0;
        }
    }
}
