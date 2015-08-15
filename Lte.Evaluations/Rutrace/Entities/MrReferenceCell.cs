using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Entities
{
    public abstract class MrCell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public byte Rsrp { get; set; }

        public double Strength {
            get { return Rsrp - 140; }
        }

        public short Frequency { get; set; }
    }

    public class MrReferenceCell : MrCell, IRefCell
    {
        public byte Ta { get; set; }

        public MroRsrpTa GenerateStat()
        {
            MroRsrpTa stat = new MroRsrpTa
            {
                SectorId = SectorId,
                RsrpInterval = Rsrp.GetInterval()
            };
            return stat;
        }
    }

    public class MrNeighborCell : MrCell, INeiCell
    {
        public short Pci { get; set; }
       
        public bool NeedUpdateRtdStat {
            get { return false; }
        }

        public double Rtd { get { return 0; } }
    }

    public static class MrsCellDateOperations
    {
        public static void Import(this List<MrsCellDate> statList, MrsRecordSet recordSet)
        {
            foreach (MrsCell cell in recordSet.MrsCells)
            {
                MrsCellDate stat = statList.FirstOrDefault(x => 
                    x.RecordDate == recordSet.RecordDate && x.CellId == cell.CellId && x.SectorId == cell.SectorId);
                if (stat == null)
                {
                    statList.Add(new MrsCellDate
                    {
                        RecordDate = recordSet.RecordDate,
                        CellId = cell.CellId,
                        SectorId = cell.SectorId,
                        RsrpCounts = cell.RsrpCounts
                    });
                }
                else
                    for (int i = 0; i < 48; i++)
                        stat.RsrpCounts[i] += cell.RsrpCounts[i];
            }
        }

        public static void Import(this List<MrsCellTa> statList, MrsRecordSet recordSet)
        {
            foreach (MrsCell cell in recordSet.MrsCells)
            {
                MrsCellTa stat = statList.FirstOrDefault(x =>
                    x.RecordDate == recordSet.RecordDate && x.CellId == cell.CellId && x.SectorId == cell.SectorId);
                if (stat == null)
                {
                    statList.Add(new MrsCellTa
                    {
                        RecordDate = recordSet.RecordDate,
                        CellId = cell.CellId,
                        SectorId = cell.SectorId,
                        TaCounts = cell.TaCounts
                    });
                }
                else
                    for (int i = 0; i < 45; i++)
                        stat.TaCounts[i] += cell.TaCounts[i];
            }
        }
    }

    public static class MroRsrpTaOpertions
    {
        public static void Import(this List<MroRsrpTa> statList, MroRecordSet recordSet)
        {
            foreach (MrRecord cell in recordSet.RecordList)
            {
                MrReferenceCell record = cell.RefCell;
                if (record == null || record.Ta == 255) continue;
                RsrpInterval interval = record.Rsrp.GetInterval();
                MroRsrpTa stat = statList.FirstOrDefault(x =>
                    x.RecordDate == recordSet.RecordDate && x.CellId == record.CellId && x.SectorId == record.SectorId
                    && x.RsrpInterval == interval);
                if (stat == null)
                {
                    statList.Add(new MroRsrpTa
                    {
                        RecordDate = recordSet.RecordDate,
                        CellId = record.CellId,
                        SectorId = record.SectorId,
                        RsrpInterval = interval
                    });
                }
                else
                    stat.UpdateTa(record.Ta);
            }
        }
    }
}
