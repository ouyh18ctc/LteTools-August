using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;

namespace Lte.Parameters.Kpi.Entities
{
    public sealed class AlarmHourInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("TopDrop2GCellDailyId")]
        public TopDrop2GCellDaily TopDrop2GCellDaily { get; set; }

        public int TopDrop2GCellDailyId { get; set; }

        public short Hour { get; set; }

        public AlarmType AlarmType { get; set; }

        public int Alarms { get; set; }
    }

    public sealed class NeighborHourInfo : ICdmaCarrier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("TopDrop2GCellDailyId")]
        public TopDrop2GCellDaily TopDrop2GCellDaily { get; set; }

        public int TopDrop2GCellDailyId { get; set; }

        public short Hour { get; set; }

        public short BscId { get; set; }

        public int BtsId { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public short Frequency { get; set; }

        [MaxLength(20)]
        public string NeighborInfo { get; set; }

        [MaxLength(20)]
        public string Problem { get; set; }
    }

    public class AlarmHourDistribution
    {
        public Dictionary<string, int[]> AlarmRecords { get; private set; }

        public AlarmHourDistribution()
        {
            AlarmRecords = new Dictionary<string, int[]>();
        }
    }

    public class DropsHourDistribution
    {
        public int Hour { get; set; }

        public int CdrCalls { get; set; }

        public int CdrDrops { get; set; }

        public int ErasureDrops { get; set; }

        public int KpiCalls { get; set; }

        public int KpiDrops { get; set; }
    }

    public static class HourDistributionQueries
    {
        public static void UpdateInfos(this List<AlarmHourInfo> infos, object statContent, short hour)
        {
            if (statContent != null)
            {
                string[] fields = statContent.ToString().GetSplittedFields(';');
                infos.AddRange(from t in fields
                    select t.GetSplittedFields(':')
                    into subFields
                    where subFields.Length > 1
                    select new AlarmHourInfo
                    {
                        Hour = hour,
                        AlarmType = subFields[0].GetAlarmType(),
                        Alarms = subFields[1].ConvertToShort(0)
                    });
            }
        }

        public static void UpdateInfos(this List<NeighborHourInfo> infos, object statContent, short hour)
        {
            if (statContent != null)
            {
                string[] fields = statContent.ToString().GetSplittedFields(';');
                for (int i = 0; i < fields.Length; i++)
                {
                    string[] subFields = fields[i].GetSplittedFields('_');
                    NeighborHourInfo neighborInfo = new NeighborHourInfo { Hour = hour };
                    neighborInfo.ImportCarrierInfo(subFields);
                    if (subFields.Length > 6)
                    {
                        neighborInfo.NeighborInfo = subFields[5];
                        neighborInfo.Problem = subFields[6];
                    }
                    infos.Add(neighborInfo);
                }
            }
        }

        public static void Import(this AlarmHourDistribution distribution, IEnumerable<AlarmHourInfo> infos)
        {
            foreach (AlarmHourInfo info in infos)
            {
                string descrition = info.AlarmType.GetAlarmTypeDescription();
                if (!distribution.AlarmRecords.ContainsKey(descrition))
                {
                    distribution.AlarmRecords.Add(descrition, new int[24]);
                    for (int i = 0; i < 24; i++)
                    {
                        distribution.AlarmRecords[descrition][i] = 0;
                    }
                }
                distribution.AlarmRecords[descrition][info.Hour] += info.Alarms;
            }
        }

        public static void Import(this List<DropsHourDistribution> result,
            CdrCallsHourInfo cdrCallsInfo, CdrDropsHourInfo cdrDropsInfo,
            ErasureDropsHourInfo erasureDropsInfo, KpiCallsHourInfo kpiCallsInfo, KpiDropsHourInfo kpiDropsInfo)
        {
            for (int hour = 0; hour < 24; hour++)
            {
                string propertyName = "Hour" + hour + "Info";
                PropertyInfo cdrCallsProperty = (typeof(CdrCallsHourInfo)).GetProperty(propertyName);
                PropertyInfo cdrDropsProperty = (typeof(CdrDropsHourInfo)).GetProperty(propertyName);
                PropertyInfo erasureDropsProperty = (typeof(ErasureDropsHourInfo)).GetProperty(propertyName);
                PropertyInfo kpiCallsProperty = (typeof(KpiCallsHourInfo)).GetProperty(propertyName);
                PropertyInfo kpiDropsProperty = (typeof(KpiDropsHourInfo)).GetProperty(propertyName);
                result.Add(new DropsHourDistribution
                {
                    Hour = hour,
                    CdrCalls = (cdrCallsInfo == null) ? 0 : (int)cdrCallsProperty.GetValue(cdrCallsInfo),
                    CdrDrops = (cdrDropsInfo == null) ? 0 : (int)cdrDropsProperty.GetValue(cdrDropsInfo),
                    ErasureDrops =
                        (erasureDropsInfo == null) ? 0 : (int)erasureDropsProperty.GetValue(erasureDropsInfo),
                    KpiCalls = (kpiCallsInfo == null) ? 0 : (int)kpiCallsProperty.GetValue(kpiCallsInfo),
                    KpiDrops = (kpiDropsInfo == null) ? 0 : (int)kpiDropsProperty.GetValue(kpiDropsInfo)
                });
            }
        }
    }
}
