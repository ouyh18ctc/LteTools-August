using System;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;
using Lte.Evaluations.Properties;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Record
{
    public class CdrTaRecord : ICell, ITaDb
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int TaOuterIntervalNum { get; set; }

        public int TaOuterIntervalExcessNum { get; set; }

        public int TaInnerIntervalNum { get; set; }

        public int TaInnerIntervalExcessNum { get; set; }

        public double TaSum { get; set; }

        public double TaAverage
        {
            get { return this.GetTaAverage(); }
        }

        public double TaMin { get; set; }

        public double TaMax { get; set; }

        private static double excessThreshold = Settings.Default.TaExcessThreshold;

        public static double ExcessThreshold
        {
            get { return excessThreshold; }
            set { excessThreshold = value; }
        }

        public static void ResetDefault()
        {
            excessThreshold = Settings.Default.TaExcessThreshold;
        }

        private bool IsRemoteSite
        {
            get { return TaMax > InterferenceStat.UpperBound / 2 && TaAverage/TaMax > 0.5; }
        }

        public double Threshold 
        {
            get
            {
                return IsRemoteSite ? ExcessThreshold + TaMin : ExcessThreshold;
            }
        }

        public void CorrectRemoteFactor()
        {
            if (IsRemoteSite)
            {
                double min = TaMin;
                TaMax -= min;
                TaSum -= min * (TaInnerIntervalNum + TaOuterIntervalNum);
                TaMin = 0;
            }
        }

        public CdrTaRecord()
        {
            TaOuterIntervalNum = 0;
            TaInnerIntervalNum = 0;
            TaSum = 0;
            TaMax = Double.MinValue;
            TaOuterIntervalExcessNum = 0;
            TaInnerIntervalExcessNum = 0;
            TaMin = Double.MaxValue;
        }
    }

}
