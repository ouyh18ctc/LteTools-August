using System.Collections.Generic;
using System.Linq;

namespace Lte.Domain.TypeDefs
{
    public enum FrequencyBand : byte
    {
        Fdd2100 = 0,
        Fdd1800 = 1,
        Tdd2600 = 2
    }

    public enum FrequencyBandType
    {
        Downlink2100,

        Uplink2100,

        Downlink1800,

        Uplink1800,

        Tdd2600,

        Undefined
    }

    class FrequencyBandDef
    {
        public FrequencyBandType FrequencyBandType { get; set; }

        public int FcnStart { get; set; }

        public int FcnEnd { get; set; }

        public double FrequencyStart { get; set; }

        public double FrequencyEnd { get; set; }
    }

    public static class FrequencyBandQueries
    {
        private static readonly List<FrequencyBandDef> frequencyBands = new List<FrequencyBandDef>{
            new FrequencyBandDef {
                FrequencyBandType = FrequencyBandType.Downlink1800,
                FrequencyStart = 1805,
                FrequencyEnd = 1880,
                FcnStart = 1200,
                FcnEnd = 1950 },
            new FrequencyBandDef {
                FrequencyBandType = FrequencyBandType.Uplink1800,
                FrequencyStart = 1710,
                FrequencyEnd = 1785,
                FcnStart = 19200,
                FcnEnd = 19950 },
            new FrequencyBandDef {
                FrequencyBandType = FrequencyBandType.Tdd2600,
                FrequencyStart = 2620,
                FrequencyEnd = 2690,
                FcnStart = 2750,
                FcnEnd = 3450 },
            new FrequencyBandDef {
                FrequencyBandType = FrequencyBandType.Downlink2100,
                FrequencyStart = 2110,
                FrequencyEnd = 2170,
                FcnStart = 0,
                FcnEnd = 600 },
            new FrequencyBandDef {
                FrequencyBandType = FrequencyBandType.Uplink2100,
                FrequencyStart = 1920,
                FrequencyEnd = 1980,
                FcnStart = 18000,
                FcnEnd = 18600 }
        };

        public static FrequencyBandType GetBandFromFrequency(this double frequency)
        {
            FrequencyBandDef def = frequencyBands.FirstOrDefault(
                x => frequency >= x.FrequencyStart && frequency <= x.FrequencyEnd);

            return (def != null) ? def.FrequencyBandType : FrequencyBandType.Undefined;
        }

        public static FrequencyBandType GetBandFromFcn(this int fcn)
        {
            FrequencyBandDef def = frequencyBands.FirstOrDefault(
                x => fcn >= x.FcnStart && fcn <= x.FcnEnd);

            return (def != null) ? def.FrequencyBandType : FrequencyBandType.Undefined;
        }

        public static int GetEarfcn(this double frequency)
        {
            FrequencyBandDef def = frequencyBands.FirstOrDefault(
                x => x.FrequencyBandType == frequency.GetBandFromFrequency());
            return (def != null) ?
                (int)(def.FcnStart + 10 * (frequency - def.FrequencyStart)) :
                int.MinValue;
        }

        public static double GetFrequency(this int fcn)
        {
            FrequencyBandDef def = frequencyBands.FirstOrDefault(
                x => x.FrequencyBandType == fcn.GetBandFromFcn());
            return (def != null) ?
                def.FrequencyStart + 0.1 * (fcn - def.FcnStart) :
                double.MinValue;
        }
    }

    public enum AntennaPortsConfigure
    {
        Antenna2T2R,
        Antenna2T4R,
        Antenna1T1R,
        Antenna2T8R,
        Antenna4T4R
    }

    public enum MeasurePointKpiSelection
    {
        NominalSinr = 0,
        StrongestCellRsrp = 1,
        StrongestInterferenceRsrp = 2,
        TotalInterferencePower = 3,
    }

}
