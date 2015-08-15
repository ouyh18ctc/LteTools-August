using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Evaluations.Kpi;
using Lte.Parameters.Kpi.Abstract;
using Lte.WinApp.Models;

namespace Lte.WinApp.Test.Models
{
    public class StubFileInfoListImporter : FileInfoListImporter
    {
        public string Message { get; private set; }
        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            if (validFileInfos.Any())
            {
                Message = validFileInfos.Aggregate("", (current, info) => current + (info.FilePath + ","));
            }
            Message = "invalid";
        }
    }

    public class StubTimeStat : ITimeStat
    {
        public DateTime StatTime { get; set; }
    }

    public class FakeTopCellRepository : ITopCellRepository<StubTimeStat>
    {
        private readonly List<StubTimeStat> stats = new List<StubTimeStat>();
 
        public IQueryable<StubTimeStat> Stats {
            get { return stats.AsQueryable(); }
        }
        public void AddOneStat(StubTimeStat stat)
        {
            stats.Add(stat);
        }

        public void SaveChanges()
        {
            
        }
    }

    public class FakeStatDateImporter : IStatDateImporter
    {
        public DateTime Date { get; set; }
        public Task<int> ImportStat(StreamReader reader, CsvFileDescription fileDescriptionNamesUs)
        {
            return Task.Run(()=> reader.ReadToEnd().Length);
        }
    }

    internal class FakeFileInfoListImporterAsync : FileInfoListImporterAsync<StubTimeStat, FakeTopCellRepository>
    {
        protected override IStatDateImporter GenerateImporter()
        {
            return new FakeStatDateImporter();
        }

        public FakeFileInfoListImporterAsync(string contents)
        {
            ReadFile = x => contents.GetStreamReader();
        }
    }

    public class FakeStatDateImporterWithRepository : IStatDateImporter
    {
        public DateTime Date { get; set; }
        private readonly ITopCellRepository<StubTimeStat> _repository;
        public Task<int> ImportStat(StreamReader reader, CsvFileDescription fileDescriptionNamesUs)
        {
            return Task.Run(() =>
            {
                _repository.AddOneStat(new StubTimeStat {StatTime = Date});
                return reader.ReadToEnd().Length;
            });
        }

        public FakeStatDateImporterWithRepository(ITopCellRepository<StubTimeStat> repository)
        {
            _repository = repository;
        }
    }

    internal class FakeFileInfoListImporterAsyncWithRepository : FileInfoListImporterAsync<StubTimeStat, FakeTopCellRepository>
    {
        protected override IStatDateImporter GenerateImporter()
        {
            return new FakeStatDateImporterWithRepository(repository);
        }

        public FakeFileInfoListImporterAsyncWithRepository(string contents)
        {
            ReadFile = x => contents.GetStreamReader();
        }
    }
}
