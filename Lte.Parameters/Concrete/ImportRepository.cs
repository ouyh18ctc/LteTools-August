using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Concrete
{
    public class ImportExcelListService<TExcel>
        where TExcel : class, IDataReaderImportable, new()
    {
        private readonly List<TExcel> _excelList;
        private readonly DataTableReader _tableReader;

        public ImportExcelListService(List<TExcel> excelList,
            DataTable dataTable)
        {
            _excelList = excelList;
            _tableReader = (dataTable != null) ? dataTable.CreateDataReader() : null;
        }

        public void Import()
        {
            if (_tableReader == null) return;
            while (_tableReader.Read())
            {
                TExcel entity = ReadEntity();
                _excelList.Add(entity);
            }
            _tableReader.Close();
        }

        private TExcel ReadEntity()
        {
            TExcel entity = new TExcel();
            entity.Import(_tableReader);
            return entity;
        }
    }

    public class ImportExcelValueService<TExcel>
        where TExcel : class, IValueImportable, new()
    {
        private readonly List<TExcel> _excelList = new List<TExcel>();

        public ImportExcelValueService(DataTable dataTable, Func<IDataReader, TExcel> EntityConstructor)
        {
            DataTableReader tableReader = (dataTable != null) ? dataTable.CreateDataReader() : null;
            if (tableReader == null) return;
            while (tableReader.Read())
            {
                TExcel entity = EntityConstructor(tableReader);
                _excelList.Add(entity);
            }
            tableReader.Close();
        }

        public List<TExcel> ExcelList
        {
            get { return _excelList; }
        }

        public void Import()
        {
            Parallel.ForEach(_excelList, entity => entity.Import());
        }
    }

    public class ExcelStatsImportRepository<T>
        where T : class, IDataReaderImportable, new()
    {
        public List<T> StatList { get; private set; }

        private ExcelStatsImportRepository()
        {
            StatList = new List<T>();
        }

        public ExcelStatsImportRepository(string excelFileName, string sheetName)
            : this()
        {
            string[] sheetNames = { sheetName };
            using (ExcelImporter excelImporter = new ExcelImporter(excelFileName, sheetNames))
            {
                using (DataTable statTable = excelImporter[sheetName])
                {
                    ImportExcelListService<T> service =
                            new ImportExcelListService<T>(StatList, statTable);
                    service.Import();
                }
            }
        }
    }

    public class ExcelBtsImportRepository<TBts> : IExcelBtsImportRepository<TBts>, IValueImportable
        where TBts : class, IValueImportable, new()
    {
        private readonly ImportExcelValueService<TBts> service;
        public List<TBts> BtsExcelList
        {
            get { return (service == null) ? null : service.ExcelList; }
        }

        public ExcelBtsImportRepository(string excelFileName, Func<IDataReader, TBts> EntityConstructor,
            string sheetName = "基站级")
        {
            using (ExcelImporter excelImporter = new ExcelImporter(excelFileName, new[] { sheetName }))
            {
                using (DataTable eNodebTable = excelImporter[sheetName])
                {
                    service = new ImportExcelValueService<TBts>(eNodebTable, EntityConstructor);
                }
            }
        }

        public void Import()
        {
            if (service == null) return;
            service.Import();
        }
    }

    public class ExcelCellImportRepository<TCell> : IExcelCellImportRepository<TCell>, IValueImportable
        where TCell : class, IValueImportable, new()
    {
        private readonly ImportExcelValueService<TCell> service;

        public List<TCell> CellExcelList
        {
            get { return (service == null) ? null : service.ExcelList; }
        }

        public ExcelCellImportRepository(string excelFileName, Func<IDataReader, TCell> EntityConstructor,
            string sheetName = "小区级")
        {
            using (ExcelImporter excelImporter = new ExcelImporter(excelFileName, new[] { sheetName }))
            {
                using (DataTable cellTable = excelImporter[sheetName])
                {
                    service = new ImportExcelValueService<TCell>(cellTable, EntityConstructor);
                }
            }
        }

        public void Import()
        {
            if (service == null) return;
            service.Import();
        }
    }

    public class ExcelDistributionImportRepository<TDistribution> : IValueImportable
        where TDistribution : class, IValueImportable, new()
    {
        private readonly ImportExcelValueService<TDistribution> service;

        public List<TDistribution> DistributionExcelList
        {
            get { return (service == null) ? null : service.ExcelList; }
        }

        public ExcelDistributionImportRepository(string excelFileName, Func<IDataReader, TDistribution> EntityConstructor,
            string sheetName = "室分")
        {
            using (ExcelImporter excelImporter = new ExcelImporter(excelFileName, new[] { sheetName }))
            {
                using (DataTable cellTable = excelImporter[sheetName])
                {
                    service = new ImportExcelValueService<TDistribution>(cellTable, EntityConstructor);
                }
            }
        }

        public void Import()
        {
            if (service == null) return;
            service.Import();
        }
    }

    public class MmlImportRepository :
        IMmlImportRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>
    {
        public List<CdmaBts> CdmaBtsList { get; private set; }

        public List<CdmaCell> CdmaCellList { get; private set; }

        private MmlImportRepository()
        {
            CdmaBtsList = new List<CdmaBts>();
            CdmaCellList = new List<CdmaCell>();
        }

        public MmlImportRepository(StreamReader reader)
            : this()
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") { continue; }
                MmlLineInfo lineInfo = new MmlLineInfo(line);
                if (lineInfo.KeyWord == "ADD BSCBTSINF")
                {
                    CdmaBtsList.Add(lineInfo.GenerateCdmaBts());
                    continue;
                }
                if (lineInfo.KeyWord == "ADD CELL")
                {
                    CdmaCellList.Add(lineInfo.GenerateCdmaCell());
                }
            }
            reader.Close();
        }
    }
}
