using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Evaluations.Kpi
{
    public class Precise4GCoverageImporter : IStatDateImporter
    {
        public DateTime Date { get; set; }

        private readonly SaveTimeSingleKpiStatsService<PreciseCoverage4G, PreciseCoverage4GCsv> cellStatService;
        private readonly SaveTimeTownStatsService<TownPreciseCoverage4GStat, PreciseCoverage4GCsv> _timeTownStatService;

        public Precise4GCoverageImporter(
            ITopCellRepository<PreciseCoverage4G> cellStatRepository,
            ITopCellRepository<TownPreciseCoverage4GStat> townStatRepository,
            IEnumerable<ENodeb> eNodebs)
        {
            cellStatService = new SaveTimeSingleKpiStatsService<PreciseCoverage4G, PreciseCoverage4GCsv>(
                cellStatRepository);
            _timeTownStatService = new SaveTimeTownStatsService<TownPreciseCoverage4GStat, PreciseCoverage4GCsv>(
                townStatRepository, eNodebs);
        }

        public Task<int> ImportStat(StreamReader reader, CsvFileDescription fileDescriptionNamesUs)
        {
            return Task.Run(() =>
            {
                List<PreciseCoverage4GCsv> csvInfos = new List<PreciseCoverage4GCsv>();
                csvInfos.AddRange(CsvContext.Read<PreciseCoverage4GCsv>(reader, fileDescriptionNamesUs));
                int count = cellStatService.Save(csvInfos);
                _timeTownStatService.Save(csvInfos);
                return count;

            });
        }
    }
}
