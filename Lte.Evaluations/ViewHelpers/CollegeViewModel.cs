using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Kpi;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.ViewHelpers
{
    public class CollegeViewModel
    {
        public IEnumerable<CollegeDto> Colleges { get; set; } 
    }

    public class CollegeEditViewModel : ITownDefViewModel
    {
        public List<SelectListItem> CityList { get; set; }

        public List<SelectListItem> DistrictList { get; set; }

        public List<SelectListItem> TownList { get; set; }

        public CollegeDto CollegeDto { get; set; }
    }

    public class InfrastructureInfoViewModel : InfrastructureCoverageViewModel, IDateSpanViewModel
    {
        public IEnumerable<ENodeb> ENodebs { get; set; }

        public IEnumerable<Cell> Cells { get; set; }

        public IEnumerable<CdmaBts> Btss { get; set; }

        public IEnumerable<CdmaCell> CdmaCells { get; set; }

        public IEnumerable<IndoorDistribution> LteDistributions { get; set; }

        public IEnumerable<IndoorDistribution> CdmaDistributions { get; set; }

        public InfrastructureInfoViewModel(int id) : base(id)
        {
        }

        public InfrastructureInfoViewModel(int id, string name) : base(id, name)
        {
            ENodebs = ParametersContainer.QueryENodebs;
            Cells = ParametersContainer.QueryCells;
            Btss = ParametersContainer.QueryBtss;
            CdmaCells = ParametersContainer.QueryCdmaCells;
            LteDistributions = ParametersContainer.QueryLteDistributions;
            CdmaDistributions = ParametersContainer.QueryCdmaDistributions;
        }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class InfrastructureCoverageViewModel
    {
        public int InfrastructureId { get; set; }

        public string InfrastructureName { get; set; }

        public double CenterLongtitute { get; set; }

        public double CenterLattitute { get; set; }

        public InfrastructureCoverageViewModel(int id)
        {
            InfrastructureId = id;
            CenterLongtitute = 113;
            CenterLattitute = 23;
        }

        public InfrastructureCoverageViewModel(int id, string name)
        {
            InfrastructureId = id;
            InfrastructureName = name;
            if (ParametersContainer.QueryENodebs.Any())
            {
                CenterLongtitute = ParametersContainer.QueryENodebs.Average(x => x.BaiduLongtitute);
                CenterLattitute = ParametersContainer.QueryENodebs.Average(x => x.BaiduLattitute);
            }
            else
            {
                CenterLongtitute = 113;
                CenterLattitute = 23;
            }
        }
    }

    public class CollegeLteExcelModel
    {
        public List<CollegeENodebExcel> BtsExcels { get; set; }

        public List<CollegeCellExcel> CellExcels { get; set; }

        public List<CollegeIndoorExcel> IndoorExcels { get; set; }
    }

    public class CollegeCdmaExcelImporter
    {
        public List<CollegeBtsExcel> BtsExcels { get; set; }

        public List<CollegeCdmaCellExcel> CellExcels { get; set; }

        public List<CollegeIndoorExcel> IndoorExcels { get; set; }
    }

    public static class CollegeImportOperations
    {
        public static CollegeLteExcelModel ImportLteInfos(this HttpPostedFileBase lteFileBase)
        {
            return lteFileBase.ImportInfo(x =>
            {
                IValueImportable importer =
                    new ExcelBtsImportRepository<CollegeENodebExcel>(x, t => new CollegeENodebExcel(t));
                importer.Import();
                List<CollegeENodebExcel> btsExcels =
                    (importer as ExcelBtsImportRepository<CollegeENodebExcel>).BtsExcelList;
                importer =
                    new ExcelCellImportRepository<CollegeCellExcel>(x, t => new CollegeCellExcel(t));
                importer.Import();
                List<CollegeCellExcel> cellExcels =
                    (importer as ExcelCellImportRepository<CollegeCellExcel>).CellExcelList;
                importer =
                    new ExcelDistributionImportRepository<CollegeIndoorExcel>(x, t => new CollegeIndoorExcel(t));
                importer.Import();
                List<CollegeIndoorExcel> indoorExcels =
                    (importer as ExcelDistributionImportRepository<CollegeIndoorExcel>).DistributionExcelList;

                return new CollegeLteExcelModel
                {
                    BtsExcels = btsExcels,
                    CellExcels = cellExcels,
                    IndoorExcels = indoorExcels
                };
            }, "4G");
        }

        public static CollegeCdmaExcelImporter ImportCdmaInfos(this HttpPostedFileBase cdmaFileBase)
        {
            return cdmaFileBase.ImportInfo(x =>
            {
                IValueImportable importer =
                    new ExcelBtsImportRepository<CollegeBtsExcel>(x, t => new CollegeBtsExcel(t));
                importer.Import();
                List<CollegeBtsExcel> btsExcels =
                    (importer as ExcelBtsImportRepository<CollegeBtsExcel>).BtsExcelList;
                importer =
                    new ExcelCellImportRepository<CollegeCdmaCellExcel>(x, t => new CollegeCdmaCellExcel(t));
                importer.Import();
                List<CollegeCdmaCellExcel> cellExcels =
                    (importer as ExcelCellImportRepository<CollegeCdmaCellExcel>).CellExcelList;
                importer =
                    new ExcelDistributionImportRepository<CollegeIndoorExcel>(x, t => new CollegeIndoorExcel(t));
                importer.Import();
                List<CollegeIndoorExcel> indoorExcels =
                    (importer as ExcelDistributionImportRepository<CollegeIndoorExcel>).DistributionExcelList;

                return new CollegeCdmaExcelImporter
                {
                    BtsExcels = btsExcels,
                    CellExcels = cellExcels,
                    IndoorExcels = indoorExcels
                };
            }, "3G");
        }
    }
}