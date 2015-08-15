using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Service
{
    public static class ImportRuService
    {
        public static void Import(this IEnumerable<RuInterferenceStat> stats,
            IEnumerable<CdmaLteNames> nameList, IEnumerable<Cell> lteList)
        {
            foreach (RuInterferenceStat stat in stats)
            {
                CdmaLteNames name = nameList.FirstOrDefault(x =>
                    x.CdmaCellId == stat.CellId && x.SectorId == stat.SectorId);

                if (name == null) continue;
                byte lteSectorId = stat.SectorId;
                Cell cell = lteList.FirstOrDefault(x =>
                    x.ENodebId == name.ENodebId && x.SectorId == lteSectorId);
                if (cell == null)
                {
                    lteSectorId += 48;
                    cell = lteList.FirstOrDefault(x =>
                        x.ENodebId == name.ENodebId && x.SectorId == lteSectorId);
                }
                stat.CdmaCellName = name.CdmaName + "-" + stat.SectorId;
                stat.LteCellName = (cell == null) ? "无定义" :
                    name.LteName + "-" + lteSectorId;
                stat.ENodebId = name.ENodebId;
            }
        }
    
        public static void Import(this List<InterferenceDetails> ruInterferenceDetails,
            IEnumerable<RuInterferenceRecord> records)
        {
            foreach (RuInterferenceRecord record in records)
            {
                for (int j = 0; j < record.Interferences.Count; j++)
                {
                    RuInterference interference = record.Interferences[j];
                    InterferenceDetails detail =
                        ruInterferenceDetails.FirstOrDefault(
                            x => x.CellId == interference.CellId && x.SectorId == interference.SectorId);
                    if (detail == null)
                    {
                        detail = new InterferenceDetails
                        {
                            CellId = interference.CellId,
                            SectorId = interference.SectorId,
                            RecordDate = record.RecordDate
                        };
                        ruInterferenceDetails.Add(detail);
                    }
                    detail.Import(record, j);
                }
            }
        }

        public static void Import(this List<InterferenceDetails> ruInterferenceDetails,
            IEnumerable<MrInterferenceRecord> records)
        {
            foreach (MrInterferenceRecord record in records)
            {
                for (int j = 0; j < record.Interferences.Count; j++)
                {
                    MrInterference interference = record.Interferences[j];
                    InterferenceDetails detail =
                        ruInterferenceDetails.FirstOrDefault(
                            x => x.CellId == interference.CellId && x.SectorId == interference.SectorId
                            && x.RecordDate == record.RecordDate);
                    if (detail == null)
                    {
                        detail = new InterferenceDetails
                        {
                            CellId = interference.CellId,
                            SectorId = interference.SectorId,
                            RecordDate = record.RecordDate
                        };
                        ruInterferenceDetails.Add(detail);
                    }
                    detail.Import(record, j);
                }
            }
        }

        public static List<RuInterferenceRecord> GenerteRuInterferenceRecords(this IEnumerable<MrRecordSet> recordSets)
        {
            List<RuInterferenceRecord> records = new List<RuInterferenceRecord>();
            foreach (MrRecordSet recordSet in recordSets)
            {
                foreach (MrRecord record in recordSet.RecordList.Where(x => x.NbCells.Count > 0))
                {
                    RuInterferenceRecord interferenceRecord =
                        records.FirstOrDefault(
                            x => x.CellId == record.RefCell.CellId && x.SectorId == record.RefCell.SectorId);
                    if (interferenceRecord == null)
                    {
                        interferenceRecord = new RuInterferenceRecord
                        {
                            CellId = record.RefCell.CellId,
                            SectorId = record.RefCell.SectorId
                        };
                        records.Add(interferenceRecord);
                    }
                    interferenceRecord.Import(record, neiCell => true);
                }
            }
            return records;
        }

        public static List<MrInterferenceRecord> GenerateMrInterferenceRecords(
            this IEnumerable<MrRecordSet> recordSets)
        {
            List<MrInterferenceRecord> records = new List<MrInterferenceRecord>();
            foreach (MrRecordSet recordSet in recordSets)
            {
                foreach (MrRecord record in recordSet.RecordList.Where(x => x.NbCells.Count > 0))
                {
                    MrInterferenceRecord interferenceRecord =
                        records.FirstOrDefault(
                            x => x.CellId == record.RefCell.CellId && x.SectorId == record.RefCell.SectorId);
                    if (interferenceRecord == null)
                    {
                        interferenceRecord = new MrInterferenceRecord
                        {
                            CellId = record.RefCell.CellId,
                            SectorId = record.RefCell.SectorId
                        };
                        records.Add(interferenceRecord);
                    }
                    interferenceRecord.Import(record, neiCell => true);
                }
            }
            return records;
        }
    }
}
