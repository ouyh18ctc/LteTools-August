using Lte.Domain.Geo.Abstract;
using Lte.Domain.LinqToCsv;
using Lte.Parameters.Abstract;
using Lte.Evaluations.Properties;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Record
{
    public class RuInterferenceStat : IInterferenceStat, IInterferenceDb, ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int ENodebId { get; set; }

        [CsvColumn(Name = "C网小区名称", FieldIndex = 1)]
        public string CdmaCellName { get; set; }

        [CsvColumn(Name = "L网小区名称", FieldIndex = 2)]
        public string LteCellName { get; set; }

        [CsvColumn(Name = "干扰小区数", FieldIndex = 3)]
        public int VictimCells { get; set; }

        public int InterferenceCells { get; set; }

        [CsvColumn(Name = "干扰比例", FieldIndex = 4)]
        public double InterferenceRatio { get; set; }

        [CsvColumn(Name = "最小站间距", FieldIndex = 5)]
        public double MinRtd { get; set; }

        [CsvColumn(Name = "平均站间距", FieldIndex = 6)]
        public double AverageRtd { get; set; }

        [CsvColumn(Name = "最大覆盖距离", FieldIndex = 7)]
        public double TaMax { get; set; }

        [CsvColumn(Name = "平均覆盖距离", FieldIndex = 8)]
        public double TaAverage { get; set; }

        [CsvColumn(Name = "超远覆盖比例", FieldIndex = 9)]
        public double TaExcessRate { get; set; }

        private static double _ratioThreshold = Settings.Default.InterferenceRatioThreshold;

        public static double RatioThreshold
        {
            get { return _ratioThreshold; }
            set { _ratioThreshold = value; }
        }

        public static void ResetDefault()
        {
            _ratioThreshold = Settings.Default.InterferenceRatioThreshold;
        }

        public RuInterferenceStat()
        {
            LteCellName = "未定义";
            CdmaCellName = "未定义";
            TaMax = 0;
            TaAverage = 0;
            TaExcessRate = 0;
            InterferenceRatio = 0;
            MinRtd = 0;
            AverageRtd = 0;
        }

        public string StatInfo
        {
            get
            {
                return "；</br>干扰小区数: " + VictimCells + "；干扰比例: " + InterferenceRatio
                       + "；</br>平均站间距: " + AverageRtd + "；</br>平均覆盖距离: " + TaAverage
                       + "；</br>超远覆盖比例: " + TaExcessRate;
            }
        }
    }

    public class MrInterferenceStat : IPureInterferenceStat
    {
        public int VictimCells { get; set; }

        public int InterferenceCells { get; set; }

        public int FirstVictimCellId { get; set; }

        public byte FirstVictimSectorId { get; set; }

        public int FirstVictimTimes { get; set; }

        public int FirstInterferenceTimes { get; set; }

        public int SecondVictimCellId { get; set; }

        public byte SecondVictimSectorId { get; set; }

        public int SecondVictimTimes { get; set; }

        public int SecondInterferenceTimes { get; set; }

        public int ThirdVictimCellId { get; set; }

        public byte ThirdVictimSectorId { get; set; }

        public int ThirdVictimTimes { get; set; }

        public int ThirdInterferenceTimes { get; set; }
    }
}
