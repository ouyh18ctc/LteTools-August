using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Service
{
    public static class ImportInterferenceStatsService
    {
        public static void ImportByTa(this List<InterferenceStat> stats, IEnumerable<CdrTaRecord> details)
        {
            foreach (CdrTaRecord detail in details.Where(x => x.TaMax > 0))
            {
                InterferenceStat stat =
                    stats.FirstOrDefault(x => x.CellId == detail.CellId && x.SectorId == detail.SectorId);
                if (stat == null)
                {
                    stat = new InterferenceStat
                    {
                        CellId = detail.CellId,
                        SectorId = detail.SectorId
                    };
                    stats.Add(stat);
                }
                detail.CloneProperties<ITaDb>(stat);
            }
        }

        private static void ImportRuRtdRecords(this List<InterferenceStat> stats, 
            IEnumerable<RuInterferenceRecord> records)
        {
            foreach (RuInterferenceRecord record in records.Where(x => x.MinRtd > 0))
            {
                InterferenceStat stat =
                    stats.FirstOrDefault(x => x.CellId == record.CellId && x.SectorId == record.SectorId);
                if (stat == null)
                {
                    stat = new InterferenceStat
                    {
                        CellId = record.CellId,
                        SectorId = record.SectorId
                    };
                    stats.Add(stat);
                }
                stat.MinRtd = record.MinRtd;
                stat.SumRtds = record.Interferences.Select(x => x.SumRtds).Sum();
                stat.TotalRtds = record.Interferences.Select(x => x.TotalRtds).Sum();
            }
        }

        public static void Import(this List<InterferenceStat> stats, List<RuRecordSet> recordSets)
        {
            List<RuInterferenceRecord> records 
                = recordSets.GenerateInterferenceRecords<RuInterferenceRecord, RuRecordSet, 
                RuInterference, RuRecord, ReferenceCell, NeighborCell>();

            List<InterferenceDetails> details = new List<InterferenceDetails>();
            details.Import(records);

            stats.AddRange(details.Select(x => new InterferenceStat
            {
                CellId = x.CellId,
                SectorId = x.SectorId,
                VictimCells = x.Victims.Count,
                InterferenceCells = x.Victims.Count(v => v.InterferenceRatio > RuInterferenceStat.RatioThreshold)
            }));
            stats.ImportRuRtdRecords(records);
        }

        public static void Import(this List<PureInterferenceStat> stats, List<MrRecordSet> recordSets)
        {
            List<MrInterferenceRecord> records
                = recordSets.GenerateInterferenceRecords<MrInterferenceRecord, MrRecordSet,
                    MrInterference, MrRecord, MrReferenceCell, MrNeighborCell>();

            List<InterferenceDetails> details = new List<InterferenceDetails>();
            details.Import(records);

            foreach (InterferenceDetails detail in details)
            {
                InterferenceVictim[] victims = detail.Victims.ToArray();
                Array.Sort(victims, new InterferenceVictimComparer());
                PureInterferenceStat stat = new PureInterferenceStat
                {
                    CellId = detail.CellId,
                    SectorId = detail.SectorId,
                    VictimCells = victims.Length,
                    InterferenceCells = victims.Count(v => v.InterferenceRatio > RuInterferenceStat.RatioThreshold),
                    RecordDate = detail.RecordDate
                };
                if (victims.Length > 0)
                {
                    stat.FirstVictimCellId = victims[0].CellId;
                    stat.FirstVictimSectorId = victims[0].SectorId;
                    stat.FirstVictimTimes = victims[0].MeasuredTimes;
                    stat.FirstInterferenceTimes = victims[0].InterferenceTimes;
                }
                if (victims.Length > 1)
                {
                    stat.SecondVictimCellId = victims[1].CellId;
                    stat.SecondVictimSectorId = victims[1].SectorId;
                    stat.SecondVictimTimes = victims[1].MeasuredTimes;
                    stat.SecondInterferenceTimes = victims[1].InterferenceTimes;
                }
                if (victims.Length > 2)
                {
                    stat.ThirdVictimCellId = victims[2].CellId;
                    stat.ThirdVictimSectorId = victims[2].SectorId;
                    stat.ThirdVictimTimes = victims[2].MeasuredTimes;
                    stat.ThirdInterferenceTimes = victims[2].InterferenceTimes;
                }
                stats.Add(stat);
            }
        }

        private static List<TInterferenceRecord> 
            GenerateInterferenceRecords<TInterferenceRecord, TRecordSet, TInterference, TRecord, TRef, TNei>(
            this IEnumerable<TRecordSet> recordSets)
            where TInterferenceRecord : class, ICell, IInterferenceRecord<TInterference, TRef, TNei>, new()
            where TRecordSet : class, IRecordSet<TRecord, TRef, TNei>
            where TInterference : class, IInterference
            where TRecord : IRuRecord<TRef, TNei>
            where TRef : class, IRefCell, new()
            where TNei : class, INeiCell, new()
        {
            List<TInterferenceRecord> records = new List<TInterferenceRecord>();
            foreach (TRecordSet recordSet in recordSets)
            {
                foreach (TRecord record in recordSet.RecordList)
                {
                    if (record.NbCells.Count <= 0) continue;

                    TInterferenceRecord interferenceRecord =
                        records.FirstOrDefault(
                            x => x.CellId == record.RefCell.CellId && x.SectorId == record.RefCell.SectorId
                            && x.RecordDate == recordSet.RecordDate);
                    if (interferenceRecord == null)
                    {
                        interferenceRecord = new TInterferenceRecord
                        {
                            CellId = record.RefCell.CellId,
                            SectorId = record.RefCell.SectorId,
                            RecordDate = recordSet.RecordDate
                        };
                        records.Add(interferenceRecord);
                    }
                    interferenceRecord.Import(record,
                        neiCell => neiCell.Frequency == record.RefCell.Frequency && neiCell.SectorId != 15);
                }
            }
            return records;
        }
    }
}
