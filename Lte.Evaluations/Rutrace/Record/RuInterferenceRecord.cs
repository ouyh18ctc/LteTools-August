using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Properties;

namespace Lte.Evaluations.Rutrace.Record
{
    public interface IInterferenceRecord<TInterference> : ICell
    {
        int MeasuredTimes { get; set; }

        List<TInterference> Interferences { get; set; }

    }

    public interface IInterferenceRecord<TInterference, in TRef, TNei> : IInterferenceRecord<TInterference>
        where TInterference : class, IInterference
        where TRef : class, IRefCell, new()
        where TNei : class, INeiCell, new()
    {
        void Import(IRuRecord<TRef, TNei> record, Func<TNei, bool> FrequencyValidation);

        DateTime RecordDate { get; set; }
    }

    public class RuInterferenceRecord : IInterferenceRecord<RuInterference, ReferenceCell, NeighborCell>
    {
        private static double interferenceThreshold = Settings.Default.RuInterferenceThreshold;

        public static void ResetDefault()
        {
            interferenceThreshold = Settings.Default.RuInterferenceThreshold;
        }

        public static double InterferenceThreshold
        {
            get { return interferenceThreshold; }
            set { interferenceThreshold = value; }
        }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int MeasuredTimes { get; set; }

        public List<RuInterference> Interferences { get; set; }

        public double MinRtd
        {
            get
            {
                IEnumerable<double> values = Interferences.Where(x => x.TotalRtds > 0).Select(x => x.AverageRtd);
                return !values.Any() ? 0 : values.Min();
            }
        }

        public RuInterferenceRecord()
        {
            MeasuredTimes = 0;
            Interferences = new List<RuInterference>();
        }

        public void Import(IRuRecord<ReferenceCell, NeighborCell> record, 
            Func<NeighborCell, bool> FrequencyValidation)
        {
            foreach (NeighborCell neiCell in record.NbCells)
            {
                RuInterference interference 
                    = this.Import(neiCell, record.RefCell, FrequencyValidation, x=>new RuInterference(x));
                if (interference == null || !neiCell.NeedUpdateRtdStat) continue;
                interference.SumRtds += neiCell.Rtd;
                interference.TotalRtds++;
            }
        }

        public DateTime RecordDate { get; set; }

        public void Import(IRuRecord<MrReferenceCell, MrNeighborCell> record,
            Func<MrNeighborCell, bool> FrequencyValidation)
        {
            foreach (MrNeighborCell neiCell in record.NbCells)
            {
                MeasuredTimes++;
                if (!FrequencyValidation(neiCell) ||
                    !(neiCell.Strength > record.RefCell.Strength - InterferenceThreshold)) continue;
                RuInterference interference =
                    Interferences.FirstOrDefault(x =>
                        x.CellId == neiCell.CellId && x.SectorId == neiCell.SectorId);
                if (interference == null)
                {
                    interference = new RuInterference(neiCell);
                    Interferences.Add(interference);
                }
                interference.InterferenceTimes++;
                if (!neiCell.NeedUpdateRtdStat) continue;
                interference.SumRtds += neiCell.Rtd;
                interference.TotalRtds++;
            }
        }
    }

    public class MrInterferenceRecord : IInterferenceRecord<MrInterference, MrReferenceCell, MrNeighborCell>
    {
        public void Import(IRuRecord<MrReferenceCell, MrNeighborCell> record, 
            Func<MrNeighborCell, bool> FrequencyValidation)
        {
            foreach (MrNeighborCell neiCell in record.NbCells)
            {
                this.Import(neiCell, record.RefCell, FrequencyValidation, x => new MrInterference(x));
            }
        }

        public DateTime RecordDate { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int MeasuredTimes { get; set; }

        public List<MrInterference> Interferences { get; set; }

        public MrInterferenceRecord()
        {
            MeasuredTimes = 0;
            Interferences = new List<MrInterference>();
        }
    }
}
