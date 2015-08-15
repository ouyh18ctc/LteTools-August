using System;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Rutrace.Entities
{
    public interface IInterference : ICell
    {
        int InterferenceTimes { get; set; }
    }

    public class MrInterference : IInterference
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int InterferenceTimes { get; set; }

        public MrInterference()
        {
            InterferenceTimes = 0;
        }

        public MrInterference(ICell cell) : this()
        {
            CellId = cell.CellId;
            SectorId = cell.SectorId;
        }
    }

    public class RuInterference : IInterference
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int InterferenceTimes { get; set; }

        public double SumRtds { get; set; }

        public int TotalRtds { get; set; }

        public double AverageRtd
        {
            get { return TotalRtds == 0 ? 0 : SumRtds / TotalRtds; }
        }

        public RuInterference(ICell cell)
        {
            CellId = cell.CellId;
            SectorId = cell.SectorId;
            InterferenceTimes = 0;
            SumRtds = 0;
            TotalRtds = 0;
        }
    }

    public static class InterferenceRecordOperations
    {
        public static double GetInterferenceRatio<TInterference>(this IInterferenceRecord<TInterference> record, ICell cell)
            where TInterference : class, IInterference
        {
            TInterference interference =
                record.Interferences.FirstOrDefault(x => x.CellId == cell.CellId && x.SectorId == cell.SectorId);
            return interference == null ? 0 :
                interference.InterferenceTimes / (double)record.MeasuredTimes;
        }

        public static double GetInterferenceRatio<TInterference>(this IInterferenceRecord<TInterference> record, int i)
            where TInterference : class, IInterference
        {
            return record.MeasuredTimes == 0 ? 0 :
                record.Interferences[i].InterferenceTimes / (double)record.MeasuredTimes;
        }

        public static TInterference Import<TInterference, TRef, TNei>(
            this IInterferenceRecord<TInterference> record,
            TNei neiCell, TRef refCell, Func<TNei, bool> FrequencyValidation,
            Func<TNei, TInterference> InterferenceGenerator)
            where TInterference : class, IInterference
            where TRef : class, IRefCell, new()
            where TNei : class, INeiCell, new()
        {
            record.MeasuredTimes++;

            //这里的逻辑是邻区信号强度足够强的时候才算干扰小区，这时才会纳入干扰小区列表
            if (!FrequencyValidation(neiCell) ||
                !(neiCell.Strength > refCell.Strength -
                  RuInterferenceRecord.InterferenceThreshold)) return null;
            TInterference interference =
                record.Interferences.FirstOrDefault(x =>
                    x.CellId == neiCell.CellId && x.SectorId == neiCell.SectorId);
            if (interference == null)
            {
                interference = InterferenceGenerator(neiCell);
                record.Interferences.Add(interference);
            }
            interference.InterferenceTimes++;
            return interference;
        }
    }
}
