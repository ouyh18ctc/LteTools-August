using System;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Infrastructure.Abstract;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class ReferenceCell : IRefCell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public short Frequency { get; set; }

        public byte EcIo { get; set; }

        public double Rtd { get; set; }

        public double Strength{
            get { return (double) EcIo/2; }
        }
    }

    public class NeighborCell : ReferenceCell, INeiCell
    {
        private const double Eps = 1E-8;

        public short Pn { get; set; }

        public bool FollowPn { get; set; }

        public bool NeedUpdateRtdStat {
            get { return (Math.Abs(Rtd) > Eps); }
        }
    }
}
