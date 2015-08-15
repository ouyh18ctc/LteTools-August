using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;
using Lte.Parameters.Properties;

namespace Lte.Parameters.Entities
{
    public class InterferenceStat : Entity, IInterferenceStat, IInterferenceDb, IRtdDb, ITaDb, ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int VictimCells { get; set; }
        
        public int InterferenceCells { get; set; }

        public double InterferenceRatio 
        {
            get
            {
                return (VictimCells == 0) ? 0 : InterferenceCells /(double)VictimCells;
            }
        }

        public double SumRtds { get; set; }

        public int TotalRtds { get; set; }

        public double AverageRtd 
        {
            get
            {
                return TotalRtds == 0 ? 0 : SumRtds/TotalRtds;
            }
        }

        public double MinRtd { get; set; }

        public int TaOuterIntervalNum { get; set; }

        public int TaInnerIntervalNum { get; set; }

        public double TaSum { get; set; }

        public double TaAverage
        {
            get { return this.GetTaAverage(); }
        }

        private static double lowerBound = Settings.Default.LowerBound;

        public static double LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        private static double upperBound = Settings.Default.UpperBound;

        public static double UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }

        public static void ResetDefault()
        {
            lowerBound = Settings.Default.LowerBound;
            upperBound = Settings.Default.UpperBound;
        }

        public int TaOuterIntervalExcessNum { get; set; }

        public int TaInnerIntervalExcessNum { get; set; }

        public double TaMax { get; set; }

        public static bool IsInnerBound(double rtd)
        {
            return rtd > LowerBound && rtd < UpperBound;
        }

        public double TaExcessRate 
        {
            get
            {
                return (IsInnerBound(TaMax))
                    ? (TaOuterIntervalNum == 0
                        ? 0
                        : TaOuterIntervalExcessNum/(double) TaOuterIntervalNum)
                    : (TaOuterIntervalNum + TaInnerIntervalNum == 0
                        ? 0
                        : (TaInnerIntervalExcessNum + TaOuterIntervalExcessNum)/
                          (double) (TaInnerIntervalNum + TaOuterIntervalNum));
            }
        }

        public InterferenceStat()
        {
            TaInnerIntervalExcessNum = 0;
            TaOuterIntervalExcessNum = 0;
            TaSum = 0;
            TaInnerIntervalNum = 0;
            TaOuterIntervalNum = 0;
            TotalRtds = 0;
            SumRtds = 0;
            InterferenceCells = 0;
            VictimCells = 0;
            TaMax = 0;
            MinRtd = 0;
        }
    }
}
