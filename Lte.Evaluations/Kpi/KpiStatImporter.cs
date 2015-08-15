using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Evaluations.Kpi
{
    public interface IStatDateImporter : IStatImporter
    {
        DateTime Date { get; set; }
    }

    public interface IStatImporter
    {
        Task<int> ImportStat(StreamReader reader, CsvFileDescription fileDescriptionNamesUs);
    }

    public class LteNeighborImporter : IStatImporter
    {
        public Task<int> ImportStat(StreamReader reader, CsvFileDescription fileDescriptionNamesUs)
        {
            throw new NotImplementedException();
        }
    }

    public class KpiStatImporter : IStatDateImporter
    {
        public DateTime Date { get; set; }

        private readonly TopCellRepositoryDropDailyService _service;

        private readonly int _approximateTopnsImport = 100;

        public KpiStatImporter(ITopCellRepository<TopDrop2GCellDaily> repository,
            int approximateTopnsImport)
        {
            _service = new TopCellRepositoryDropDailyService(repository);
            _approximateTopnsImport = approximateTopnsImport;
        }

        public Task<int> ImportStat(StreamReader reader, CsvFileDescription fileDescriptionNamesUs)
        {
            return Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10*(_approximateTopnsImport + 1); i++)
                {
                    string content = reader.ReadLine();
                    if (!string.IsNullOrEmpty(content))
                    {
                        sb.Append(content + "\n");
                    }
                }
                string testInput = sb.ToString();
                List<TopDrop2GCellCsv> stats
                    = CsvContext.ReadString<TopDrop2GCellCsv>(testInput, fileDescriptionNamesUs).ToList();
                return _service.ImportStats(stats, _approximateTopnsImport*10, Date);

            });
        }
    }
}
