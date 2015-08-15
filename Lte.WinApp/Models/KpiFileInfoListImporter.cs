using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Lte.Evaluations.Kpi;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Rutrace.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Concrete;
using Lte.Parameters.Kpi.Entities;
using Lte.WinApp.Service;

namespace Lte.WinApp.Models
{
    public class KpiFileInfoListImporterAsync : FileInfoListImporterAsync<TopDrop2GCellDaily, EFTopDrop2GcellDailyRepository>
    {
        private readonly int _approximateTopnsImport = 100;

        public KpiFileInfoListImporterAsync(int approximateTopnsImport)
        {
            FileType = "KPI-CSV文件";
            _approximateTopnsImport = approximateTopnsImport;
        }

        protected override IStatDateImporter GenerateImporter()
        {
            return new KpiStatImporter(repository, _approximateTopnsImport);
        }
    }

    public class Precise4GFileInfoListImporterAsync : FileInfoListImporterAsync<PreciseCoverage4G, EFPreciseCoverage4GRepository>
    {
        private ITopCellRepository<TownPreciseCoverage4GStat> townStatRepository;
        private IEnumerable<ENodeb> eNodebs;
        protected override IStatDateImporter GenerateImporter()
        {
            townStatRepository
                = new EFTownPreciseCoverage4GStatRepository();
            eNodebs = (new EFENodebRepository()).GetAllList();
            return new Precise4GCoverageImporter(repository, townStatRepository,
                eNodebs);
        }

        public Precise4GFileInfoListImporterAsync()
        {
            FileType = "精确覆盖-CSV文件";
        }
    }

    public class MrFileInfoListImporter : FileInfoListImporter
    {
        private readonly List<InterferenceStat> _stats;
        private readonly IEnumerable<Cell> _cells;
        private readonly ILteNeighborCellRepository _repository;

        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            FinishValidFilesStateService fileService = new FinishValidFilesStateService(validFileInfos);
            string[] paths = validFileInfos.Select(x => x.FilePath).ToArray();
            List<CdrRtdRecord> taRecordList = new List<CdrRtdRecord>();

            List<MrRecordSet> mrRecordSets = taRecordList.Import(_repository, _cells, paths.Where(x =>
                x.IndexOf("MRO", StringComparison.Ordinal) >= 0));

            List<RuInterferenceRecord> records = mrRecordSets.GenerteRuInterferenceRecords();

            GenerateCdrTaRecordsService taService = new GenerateCdrTaRecordsFromTaRecordsService(taRecordList);
            List<CdrTaRecord> taDetails = taService.Generate();

            List<InterferenceDetails> details = new List<InterferenceDetails>();
            details.Import(records);

            _stats.AddRange(details.Select(x => new InterferenceStat
            {
                CellId = x.CellId,
                SectorId = x.SectorId,
                VictimCells = x.Victims.Count,
                InterferenceCells = x.Victims.Count(v => v.InterferenceRatio > RuInterferenceStat.RatioThreshold)
            }));

            _stats.ImportByTa(taDetails);
            fileService.Finish();
            MessageBox.Show("成功导入MR数据文件");
        }

        private MrFileInfoListImporter()
        {
            FileType = "MR文件";
        }

        public MrFileInfoListImporter(List<InterferenceStat> stats, 
            IEnumerable<Cell> cells, ILteNeighborCellRepository repository) : this()
        {
            _stats = stats;
            _cells = cells;
            _repository = repository;
        }
    }

    public class CdrFileInfoListImporter : FileInfoListImporter
    {
        private readonly List<InterferenceStat> _stats;

        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            FinishValidFilesStateService filesService = new FinishValidFilesStateService(validFileInfos);
            string[] paths = validFileInfos.Select(x => x.FilePath).ToArray();
            GenerateCdrTaRecordsService service = new GenerateCdrTaRecordsFromFilesService(
                paths);
            List<CdrTaRecord> cdrDetails = service.Generate();
            _stats.ImportByTa(cdrDetails);
            filesService.Finish();
            MessageBox.Show("\n成功导入CDR数据文件");
        }

        private CdrFileInfoListImporter()
        {
            FileType = "CDR文件";
        }

        public CdrFileInfoListImporter(List<InterferenceStat> stats)
            : this()
        {
            _stats = stats;
        }
    }

    public class RuFileInfoListImporter : FileInfoListImporter
    {
        private readonly List<InterferenceStat> _stats;

        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            IEnumerable<string> paths = validFileInfos.Select(x => x.FilePath);
            FinishValidFilesStateService fileService = new FinishValidFilesStateService(validFileInfos);
            _stats.Import(RecordSetImporter.ImportRuRecordSets(paths));
            fileService.Finish();

            MessageBox.Show("成功导入RU数据文件");
        }

        private RuFileInfoListImporter()
        {
            FileType = "RU文件";
        }

        public RuFileInfoListImporter(List<InterferenceStat> stats)
            : this()
        {
            _stats = stats;
        }
    }

    public class LteFileInfoListImporter : FileInfoListImporter,
        IExcelBtsImportRepository<ENodebExcel>, IExcelCellImportRepository<CellExcel>
    {
        private List<ENodebExcel> _btsExcelList = new List<ENodebExcel>();
        private List<CellExcel> _cellExcelList = new List<CellExcel>();

        public List<ENodebExcel> BtsExcelList
        {
            get { return _btsExcelList; }
        }

        public List<CellExcel> CellExcelList
        {
            get { return _cellExcelList; }
        }

        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            if (validFileInfos.Length == 0) MessageBox.Show("\n请先选择恰当的" + FileType + "！");
            IValueImportable importer 
                = new ExcelBtsImportRepository<ENodebExcel>(validFileInfos[0].FilePath, x=>new ENodebExcel(x));
            importer.Import();
            _btsExcelList = (importer as ExcelBtsImportRepository<ENodebExcel>).BtsExcelList;
            importer = new ExcelCellImportRepository<CellExcel>(validFileInfos[0].FilePath, x=>new CellExcel(x));
            importer.Import();
            _cellExcelList = (importer as ExcelCellImportRepository<CellExcel>).CellExcelList;
            FinishValidFilesStateService fileService = new FinishValidFilesStateService(validFileInfos);
            fileService.Finish();
            MessageBox.Show("\n读取" + FileType + "成功！");
        }

        public LteFileInfoListImporter()
        {
            FileType = "LTE工参文件";
        }

    }

    public class CdmaFileInfoListImporter : FileInfoListImporter,
        IExcelBtsImportRepository<BtsExcel>, IExcelCellImportRepository<CdmaCellExcel>
    {
        private List<BtsExcel> _btsExcelList = new List<BtsExcel>();
        private List<CdmaCellExcel> _cdmaCellExcelList = new List<CdmaCellExcel>();

        public List<BtsExcel> BtsExcelList
        {
            get { return _btsExcelList; }
        }

        public List<CdmaCellExcel> CellExcelList
        {
            get { return _cdmaCellExcelList; }
        }

        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            if (validFileInfos.Length == 0) MessageBox.Show("\n请先选择恰当的" + FileType + "！");
            IValueImportable importer 
                = new ExcelBtsImportRepository<BtsExcel>(validFileInfos[0].FilePath, x=>new BtsExcel(x));
            importer.Import();
            _btsExcelList = (importer as ExcelBtsImportRepository<BtsExcel>).BtsExcelList;
            importer 
                = new ExcelCellImportRepository<CdmaCellExcel>(validFileInfos[0].FilePath, x=>new CdmaCellExcel(x));
            importer.Import();
            _cdmaCellExcelList = (importer as ExcelCellImportRepository<CdmaCellExcel>).CellExcelList;
            FinishValidFilesStateService fileService = new FinishValidFilesStateService(validFileInfos);
            fileService.Finish();
            MessageBox.Show("\n读取" + FileType + "成功！");
        }

        public CdmaFileInfoListImporter()
        {
            FileType = "CDMA工参文件";
        }

    }

    public class MmlFileInfoListImporter : FileInfoListImporter
    {
        public List<IMmlImportRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>> RepositoryList
        {
            get; private set;
        }

        public override void Import(ImportedFileInfo[] validFileInfos)
        {
            if (validFileInfos.Length == 0) MessageBox.Show("\n请先选择恰当的" + FileType + "！");
            RepositoryList
                = validFileInfos.Select(x => new MmlImportRepository(new StreamReader(x.FilePath,
                    Encoding.GetEncoding("GB2312"))) as
                    IMmlImportRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>).ToList();
            FinishValidFilesStateService fileService = new FinishValidFilesStateService(validFileInfos);
            fileService.Finish();
            MessageBox.Show("\n读取" + FileType + "成功！");
        }

        public MmlFileInfoListImporter()
        {
            FileType = "MML文件";
        }
    }
}