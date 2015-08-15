using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Abstract;

namespace Lte.Parameters.Kpi.Entities
{
    public sealed class TopDrop2GCellDaily : ICdmaCarrier, ITimeStat, ICdrDrops, IDropEcio, IGoodEcio,
        ICdrCalls, IKpiCalls, IKpiDrops, IErasureDrops, IAlarm, IMainRssi, ISubRssi, IDropCause
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopDrop2GCellDailyId { get; set; }

        public DateTime StatTime { get; set; }

        public short BscId { get; set; }

        public int BtsId { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public short Frequency { get; set; }

        public int CdrCalls { get; set; }

        public int CdrDrops { get; set; }

        public double CdrDropRate
        {
            get
            {
                return CdrCalls == 0 ? 0 : (double)CdrDrops / CdrCalls * 100;
            }
        }

        public int KpiCalls { get; set; }

        public int KpiDrops { get; set; }

        public double KpiDropRate
        {
            get
            {
                return KpiCalls == 0 ? 0 : (double)KpiDrops / KpiCalls * 100;
            }
        }

        public int ErasureDrops { get; set; }

        public double AverageRssi { get; set; }

        public double MainRssi { get; set; }

        public double SubRssi { get; set; }

        public double AverageDropEcio { get; set; }

        public double AverageDropDistance { get; set; }

        public CdrDropsDistanceInfo CdrDropsDistanceInfo { get; set; }

        public DropEcioDistanceInfo DropEcioDistanceInfo { get; set; }

        public GoodEcioDistanceInfo GoodEcioDistanceInfo { get; set; }

        public CdrCallsDistanceInfo CdrCallsDistanceInfo { get; set; }

        public CdrCallsHourInfo CdrCallsHourInfo { get; set; }

        public CdrDropsHourInfo CdrDropsHourInfo { get; set; }

        public DropEcioHourInfo DropEcioHourInfo { get; set; }

        public ErasureDropsHourInfo ErasureDropsHourInfo { get; set; }

        public KpiCallsHourInfo KpiCallsHourInfo { get; set; }

        public KpiDropsHourInfo KpiDropsHourInfo { get; set; }

        public MainRssiHourInfo MainRssiHourInfo { get; set; }

        public SubRssiHourInfo SubRssiHourInfo { get; set; }

        public ICollection<AlarmHourInfo> AlarmHourInfos { get; set; }

        public ICollection<NeighborHourInfo> NeighborHourInfos { get; set; }

        public string DropCause { get; set; }

        public string Import(List<TopDrop2GCellCsv> csvStats,
            ref int beginIndex, string oldCarrier)
        {
            int maxIndex = Math.Min(csvStats.Count, beginIndex + 20);
            for (int i = beginIndex; i < maxIndex; i++)
            {
                if (csvStats[i].Carrier != oldCarrier)
                {
                    beginIndex = i;
                    return csvStats[i].Carrier;
                }
                Import(csvStats[i]);
            }
            beginIndex = maxIndex;
            return "";
        }

        public void Import(TopDrop2GCellCsv csvStat)
        {
            switch (csvStat.FieldName)
            {
                case "CDR掉话次数":
                    csvStat.ImportCdrDrops(this);
                    break;
                case "掉话Ecio":
                    csvStat.ImportDropEcio(this);
                    break;
                case "ECIO优良比":
                    csvStat.ImportGoodEcio(this);
                    break;
                case "呼叫次数":
                    csvStat.ImportCdrCalls(this);
                    break;
                case "性能数据呼叫次数":
                    csvStat.ImportKpiCalls(this);
                    break;
                case "性能数据掉话次数":
                    csvStat.ImportKpiDrops(this);
                    break;
                case "Erasuare掉话次数":
                    csvStat.ImportErasureDrops(this);
                    break;
                case "告警次数":
                    csvStat.ImportAlarm(this);
                    break;
                case "RSSI主集":
                    csvStat.ImportMainRssi(this);
                    break;
                case "RSSI分集":
                    csvStat.ImportSubRssi(this);
                    break;
                case "掉话原因":
                    csvStat.ImportDropCause(this);
                    break;
            }
        }
    }
}
