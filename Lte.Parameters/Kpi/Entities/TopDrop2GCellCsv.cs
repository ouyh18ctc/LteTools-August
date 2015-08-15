using Lte.Domain.Geo.Abstract;
using Lte.Domain.LinqToCsv;
using Lte.Domain.Regular;
using Lte.Parameters.Kpi.Abstract;

namespace Lte.Parameters.Kpi.Entities
{
    public class TopDrop2GCellCsv : IDrop2GDistanceInfo<string>, IDrop2GHourInfo<string>, ICarrierName
    {
        [CsvColumn(FieldIndex = 1)]
        public string FieldName { get; set; }

        [CsvColumn(FieldIndex = 2)]
        public string Carrier { get; set; }

        [CsvColumn(FieldIndex = 3)]
        public string BtsName { get; set; }

        [CsvColumn(FieldIndex = 4)]
        public string Drops { get; set; }

        [CsvColumn(FieldIndex = 5)]
        public string Calls { get; set; }

        [CsvColumn(FieldIndex = 6)]
        public string DropRate { get; set; }

        [CsvColumn(FieldIndex = 7)]
        public string AverageRssi { get; set; }

        [CsvColumn(FieldIndex = 8)]
        public string AverageDropEcio { get; set; }

        [CsvColumn(FieldIndex = 9)]
        public string AverageDropDistance { get; set; }

        [CsvColumn(FieldIndex = 10)]
        public string DistanceTo200Info { get; set; }

        [CsvColumn(FieldIndex = 11)]
        public string DistanceTo400Info { get; set; }

        [CsvColumn(FieldIndex = 12)]
        public string DistanceTo600Info { get; set; }

        [CsvColumn(FieldIndex = 13)]
        public string DistanceTo800Info { get; set; }

        [CsvColumn(FieldIndex = 14)]
        public string DistanceTo1000Info { get; set; }

        [CsvColumn(FieldIndex = 15)]
        public string DistanceTo1200Info { get; set; }

        [CsvColumn(FieldIndex = 16)]
        public string DistanceTo1400Info { get; set; }

        [CsvColumn(FieldIndex = 17)]
        public string DistanceTo1600Info { get; set; }

        [CsvColumn(FieldIndex = 18)]
        public string DistanceTo1800Info { get; set; }

        [CsvColumn(FieldIndex = 19)]
        public string DistanceTo2000Info { get; set; }

        [CsvColumn(FieldIndex = 20)]
        public string DistanceTo2200Info { get; set; }

        [CsvColumn(FieldIndex = 21)]
        public string DistanceTo2400Info { get; set; }

        [CsvColumn(FieldIndex = 22)]
        public string DistanceTo2600Info { get; set; }

        [CsvColumn(FieldIndex = 23)]
        public string DistanceTo2800Info { get; set; }

        [CsvColumn(FieldIndex = 24)]
        public string DistanceTo3000Info { get; set; }

        [CsvColumn(FieldIndex = 25)]
        public string DistanceTo4000Info { get; set; }

        [CsvColumn(FieldIndex = 26)]
        public string DistanceTo5000Info { get; set; }

        [CsvColumn(FieldIndex = 27)]
        public string DistanceTo6000Info { get; set; }

        [CsvColumn(FieldIndex = 28)]
        public string DistanceTo7000Info { get; set; }

        [CsvColumn(FieldIndex = 29)]
        public string DistanceTo8000Info { get; set; }

        [CsvColumn(FieldIndex = 30)]
        public string DistanceTo9000Info { get; set; }

        [CsvColumn(FieldIndex = 31)]
        public string DistanceToInfInfo { get; set; }

        [CsvColumn(FieldIndex = 32)]
        public string Hour0Info { get; set; }

        [CsvColumn(FieldIndex = 33)]
        public string Hour1Info { get; set; }

        [CsvColumn(FieldIndex = 34)]
        public string Hour2Info { get; set; }

        [CsvColumn(FieldIndex = 35)]
        public string Hour3Info { get; set; }

        [CsvColumn(FieldIndex = 36)]
        public string Hour4Info { get; set; }

        [CsvColumn(FieldIndex = 37)]
        public string Hour5Info { get; set; }

        [CsvColumn(FieldIndex = 38)]
        public string Hour6Info { get; set; }

        [CsvColumn(FieldIndex = 39)]
        public string Hour7Info { get; set; }

        [CsvColumn(FieldIndex = 40)]
        public string Hour8Info { get; set; }

        [CsvColumn(FieldIndex = 41)]
        public string Hour9Info { get; set; }

        [CsvColumn(FieldIndex = 42)]
        public string Hour10Info { get; set; }

        [CsvColumn(FieldIndex = 43)]
        public string Hour11Info { get; set; }

        [CsvColumn(FieldIndex = 44)]
        public string Hour12Info { get; set; }

        [CsvColumn(FieldIndex = 45)]
        public string Hour13Info { get; set; }

        [CsvColumn(FieldIndex = 46)]
        public string Hour14Info { get; set; }

        [CsvColumn(FieldIndex = 47)]
        public string Hour15Info { get; set; }

        [CsvColumn(FieldIndex = 48)]
        public string Hour16Info { get; set; }

        [CsvColumn(FieldIndex = 49)]
        public string Hour17Info { get; set; }

        [CsvColumn(FieldIndex = 50)]
        public string Hour18Info { get; set; }

        [CsvColumn(FieldIndex = 51)]
        public string Hour19Info { get; set; }

        [CsvColumn(FieldIndex = 52)]
        public string Hour20Info { get; set; }

        [CsvColumn(FieldIndex = 53)]
        public string Hour21Info { get; set; }

        [CsvColumn(FieldIndex = 54)]
        public string Hour22Info { get; set; }

        [CsvColumn(FieldIndex = 55)]
        public string Hour23Info { get; set; }

        [CsvColumn(FieldIndex = 56)]
        public string DropCause { get; set; }

        public void ImportCdrDrops(ICdrDrops stat)
        {
            stat.CdrDropsDistanceInfo = this.GenerateDistanceInfo<CdrDropsDistanceInfo, int>();
            stat.CdrDrops = Drops.ConvertToInt(0);
            stat.CdrCalls = Calls.ConvertToInt(1);
            stat.AverageRssi = AverageRssi.ConvertToDouble(-120);
            stat.AverageDropEcio = AverageDropEcio.ConvertToDouble(-12);
            stat.AverageDropDistance = AverageDropDistance.ConvertToDouble(500);
            stat.CdrDropsHourInfo = this.GenerateHourInfo<CdrDropsHourInfo, int>();
        }

        public void ImportDropEcio(IDropEcio stat)
        {
            stat.DropEcioDistanceInfo = this.GenerateDistanceInfo<DropEcioDistanceInfo, double>();
            stat.DropEcioHourInfo = this.GenerateHourInfo<DropEcioHourInfo, double>();
        }

        public void ImportGoodEcio(IGoodEcio stat)
        {
            stat.GoodEcioDistanceInfo = this.GenerateDistanceInfo<GoodEcioDistanceInfo, double>();
        }

        public void ImportCdrCalls(ICdrCalls stat)
        {
            stat.CdrCallsDistanceInfo = this.GenerateDistanceInfo<CdrCallsDistanceInfo, int>();
            stat.CdrCallsHourInfo = this.GenerateHourInfo<CdrCallsHourInfo, int>();
        }

        public void ImportKpiCalls(IKpiCalls stat)
        {
            stat.KpiCalls = Calls.ConvertToInt(1);
            stat.KpiDrops = Drops.ConvertToInt(0);
            stat.KpiCallsHourInfo = this.GenerateHourInfo<KpiCallsHourInfo, int>();
        }

        public void ImportKpiDrops(IKpiDrops stat)
        {
            stat.KpiDropsHourInfo = this.GenerateHourInfo<KpiDropsHourInfo, int>();
        }

        public void ImportErasureDrops(IErasureDrops stat)
        {
            stat.ErasureDrops = Drops.ConvertToInt(0);
            stat.ErasureDropsHourInfo = this.GenerateHourInfo<ErasureDropsHourInfo, int>();
        }

        public void ImportAlarm(IAlarm stat)
        {
            stat.AlarmHourInfos = this.GenerateAlarmHourInfos();
        }

        public void ImportMainRssi(IMainRssi stat)
        {
            stat.MainRssi = AverageRssi.ConvertToDouble(-120);
            stat.MainRssiHourInfo = this.GenerateHourInfo<MainRssiHourInfo, double>();
        }

        public void ImportSubRssi(ISubRssi stat)
        {
            stat.SubRssi = AverageRssi.ConvertToDouble(-120);
            stat.SubRssiHourInfo = this.GenerateHourInfo<SubRssiHourInfo, double>();
        }

        public void ImportDropCause(IDropCause stat)
        {
            stat.DropCause = DropCause;
            stat.NeighborHourInfos = this.GenerateNeighborHourInfos();
        }
    }
}
