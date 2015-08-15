using System;
using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;

namespace Lte.Evaluations.Dingli
{
    public static class LogsOperations
    {
        private const double Eps = 1E-6;

        public static double RateEvaluationInterval = 0.5;
        public static List<LogRecord> Merge(this IEnumerable<LogRecord> sourceRecords)
        {
            List<LogRecord> resultRecords = new List<LogRecord>();
            int j = -1;
            bool cellProper = false;
            foreach (LogRecord record in sourceRecords)
            {
                if (resultRecords.Count == 0 || record.Time > resultRecords[j].Time)
                {
                    resultRecords.Add(new LogRecord());
                    record.CloneProperties<LogRecord>(resultRecords[++j], false);
                    cellProper = record.Pci > 0 && record.ENodebId > 0
                                 && record.SectorId > 0;
                }
                else
                {
                    if (Math.Abs(resultRecords[j].Longtitute - record.Longtitute) > Eps 
                        && record.Longtitute > 0)
                    {
                        resultRecords[j].Longtitute = record.Longtitute;
                        resultRecords[j].Lattitute = record.Lattitute;
                    }
                    if (!cellProper && record.Pci > 0 && record.ENodebId > 0
                        && record.SectorId > 0)
                    {
                        resultRecords[j].Pci = record.Pci;
                        resultRecords[j].ENodebId = record.ENodebId;
                        resultRecords[j].SectorId = record.SectorId;
                        resultRecords[j].Earfcn = record.Earfcn;
                        cellProper = true;
                    }
                    resultRecords[j].Event += ":-" + record.Event;
                    resultRecords[j].MessageType += ":-" + record.MessageType;
                }
            }
            return resultRecords;
        }

        public static List<RateStat> MergeStat<TLogRecord>(this List<TLogRecord> records)
            where TLogRecord : class, ILogRecord, new()
        {
            List<RateStat> stats = new List<RateStat>();

            for (int i = 0; i < records.Count; i++)
            {
                if (i == 0 || records[i].Time > records[i - 1].Time
                    || records[i].DlThroughput != records[i - 1].DlThroughput)
                {
                    RateStat stat = new RateStat();
                    stat.Import(records[i]);
                    stats.Add(stat);
                }
            }
            for (int j = 1; j < stats.Count; j++)
            {
                if (stats[j].DlThroughput == stats[j - 1].DlThroughput &&
                    stats[j].PdschRbRate < stats[j - 1].PdschRbRate)
                { 
                    stats[j].PdschRbRate = stats[j - 1].PdschRbRate; 
                }
            }
            return stats;
        }

        public static List<BasicRateStat> Merge(this List<RateStat> stats)
        {
            List<BasicRateStat> results = new List<BasicRateStat>();
            List<RateStat> partsOfStat = new List<RateStat>();
            int intervals = 1;
            foreach (RateStat stat in stats)
            {
                if (stat.Time < stats[0].Time.AddSeconds(RateEvaluationInterval * intervals))
                {
                    partsOfStat.Add(stat);
                }
                else
                {
                    if (partsOfStat.Count > 0) 
                    {
                        results.Add(partsOfStat.Average<BasicRateStat>());
                        partsOfStat.Clear(); 
                    }
                    partsOfStat.Add(stat);
                    intervals 
                        = (int)Math.Floor((stat.Time - stats[0].Time).TotalSeconds / RateEvaluationInterval) + 1;
                }
            }
            results.Add(partsOfStat.Average<BasicRateStat>());
            return results;
        }
    }
}
