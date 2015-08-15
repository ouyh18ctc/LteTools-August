using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Evaluations.ViewHelpers
{
    public interface IKpiImportController
    {
        SaveTimeCityKpiStatsService<TopDrop2GCell, TopDrop2GCellExcel> SaveTimeKpiStatsService { get; set; }

        SaveTimeCityKpiStatsService<TopConnection3GCell, TopConnection3GCellExcel> Save3GStatsService { get; set; }
    }

    public static class KpiImportControllerOperations
    {
        public static void Initialize(this IKpiImportController controller,
            ITopCellRepository<TopDrop2GCell> topDrop2GRepository,
            ITopCellRepository<TopConnection3GCell> topConnection3GRepository)
        {
            controller.SaveTimeKpiStatsService =
                new SaveTimeCityKpiStatsService<TopDrop2GCell, TopDrop2GCellExcel>(topDrop2GRepository);
            controller.Save3GStatsService =
                new SaveTimeCityKpiStatsService<TopConnection3GCell, TopConnection3GCellExcel>(topConnection3GRepository);
        }

        public static KpiImportResult Import(this IKpiImportController controller,
            HttpPostedFileBase httpPostedFileBase, string[] sheetNames,
            IRegionRepository regionRepository, ITopCellRepository<CdmaRegionStat> cdmaStatRepository)
        {
            List<CdmaRegionStat> statList =
                httpPostedFileBase.ImportInfo(
                    x =>
                    {
                        List<CdmaRegionStat> stats = new List<CdmaRegionStat>();
                        using (ExcelImporter excelImporter = new ExcelImporter(x, sheetNames))
                        {
                            foreach (string sheetName in sheetNames)
                            {
                                using (DataTable statTable = excelImporter[sheetName])
                                {
                                    ImportExcelListService<CdmaRegionStat> service =
                                        new ImportExcelListService<CdmaRegionStat>(stats, statTable);
                                    service.Import();
                                }
                            }
                        }
                        return stats;
                    }, "佛山");
            if (statList.Count > 0)
            {
                OptimizeRegion firstOrDefault = regionRepository.GetAllList().FirstOrDefault(
                    x => x.Region == statList[0].Region);
                if (firstOrDefault != null)
                {
                    controller.SaveTimeKpiStatsService.CurrentCity = firstOrDefault.City;
                    controller.Save3GStatsService.CurrentCity = firstOrDefault.City;
                }
            }
            int regionStatsSaved = cdmaStatRepository.SaveStats(statList);
            ExcelStatsImportRepository<TopDrop2GCellExcel> drop2GExcelRepository =
                httpPostedFileBase.ImportInfo(
                    x => new ExcelStatsImportRepository<TopDrop2GCellExcel>(x, "掉话TOP30小区"), "佛山");
            int topDrop2GSaved = controller.SaveTimeKpiStatsService.Save(drop2GExcelRepository.StatList);
            ExcelStatsImportRepository<TopConnection3GCellExcel> connection3GExcelRepository =
                httpPostedFileBase.ImportInfo(
                    x => new ExcelStatsImportRepository<TopConnection3GCellExcel>(x, "连接TOP30小区"), "佛山");
            int topConnection3GSaved = controller.Save3GStatsService.Save(connection3GExcelRepository.StatList);

            return new KpiImportResult
            {
                RegionStatsSaved = regionStatsSaved,
                TopDrop2GSaved = topDrop2GSaved,
                TopConnection3GSaved = topConnection3GSaved
            };
        }
    }

    public class KpiImportResult
    {
        public int RegionStatsSaved { get; set; }

        public int TopDrop2GSaved { get; set; }

        public int TopConnection3GSaved { get; set; }
    }
}
