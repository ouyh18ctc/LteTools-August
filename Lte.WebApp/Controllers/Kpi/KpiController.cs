using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lte.Evaluations.Kpi;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Region;

namespace Lte.WebApp.Controllers.Kpi
{
    public class KpiController : Controller, IKpiImportController
    {
        private readonly IRegionRepository regionRepository;
        private readonly ITownRepository townRepository;
        private readonly ITopCellRepository<CdmaRegionStat> cdmaStatRepository;
        private readonly ITopCellRepository<TopDrop2GCell> topDrop2GRepository;
        public SaveTimeCityKpiStatsService<TopDrop2GCell, TopDrop2GCellExcel> SaveTimeKpiStatsService { get; set; }
        private readonly ITopCellRepository<TopDrop2GCellDaily> topDrop2GDailyRepository;
        private readonly ITopCellRepository<TopConnection3GCell> topConnection3GRepository;
        private readonly ITopCellRepository<TownPreciseCoverage4GStat> townPrecise4GRepository;
        public SaveTimeCityKpiStatsService<TopConnection3GCell, TopConnection3GCellExcel> Save3GStatsService { get; set; }
        private readonly IBtsRepository btsRepository;
        private readonly IENodebRepository eNodebRepository;
        private readonly QueryNamesService regionNamesService;
        private readonly QueryNamesService districtNamesService;

        public KpiController(IRegionRepository regionRepository,
            ITownRepository townRepository,
            ITopCellRepository<CdmaRegionStat> cdmaStatRepository,
            ITopCellRepository<TopDrop2GCell> topDrop2GRepository,
            ITopCellRepository<TopDrop2GCellDaily> topDrop2GDailyRepository,
            ITopCellRepository<TopConnection3GCell> topConnection3GRepository,
            ITopCellRepository<TownPreciseCoverage4GStat> townPrecise4GRepository,
            IBtsRepository btsRepository,
            IENodebRepository eNodebRepository)
        {
            this.regionRepository = regionRepository;
            this.townRepository = townRepository;
            this.cdmaStatRepository = cdmaStatRepository;
            this.topDrop2GRepository = topDrop2GRepository;
            
            this.topDrop2GDailyRepository = topDrop2GDailyRepository;
            this.topConnection3GRepository = topConnection3GRepository;
            
            this.townPrecise4GRepository = townPrecise4GRepository;
            this.btsRepository = btsRepository;
            this.eNodebRepository = eNodebRepository;

            this.Initialize(topDrop2GRepository, topConnection3GRepository);
            regionNamesService = new QueryRegionCityNamesService(this.regionRepository.GetAll());
            districtNamesService = new QueryDistinctDistrictNamesService(this.townRepository.GetAll());
        }

        public ActionResult Index(KpiListViewModel postedModel)
        {
            IEnumerable<CdmaRegionStat> stats 
                = cdmaStatRepository.Stats.ToList().GetLastDateStats(
                regionRepository.GetAllList(), postedModel.StatDate);
            DateTime endDate = (stats.Any()) ? 
                stats.First().StatDate : DateTime.Today.AddDays(-1);
            KpiListViewModel viewModel = new KpiListViewModel
            {
                CdmaStats = stats,
                EndDate = endDate,
                StatDate = endDate,
                StartDate = endDate.AddDays(-7),
                Cities = regionNamesService.Query().ToList()
            };
            return View(viewModel);
        }

        public ActionResult Precise4G(KpiListViewModel postedModel)
        {
            IEnumerable<TownPrecise4GView> stats
                = townPrecise4GRepository.Stats.GetLastDateStats(
                townRepository.GetAllList(), postedModel.StatDate);
            DateTime endDate = (stats.Any()) ?
                stats.First().StatDate : DateTime.Today.AddDays(-1);
            Precise4GViewModel viewModel = new Precise4GViewModel
            {
                EndDate = endDate,
                StatDate = endDate,
                StartDate = endDate.AddDays(-7),
                PreciseStats = stats,
                Districts = districtNamesService.Query()
            };
            return View(viewModel);
        }

        public ViewResult TopDrop2G(TopDrop2GViewModel postedModel)
        {
            TopDrop2GViewModel viewModel 
                = topDrop2GRepository.Stats.GenerateView<TopDrop2GViewModel, TopDrop2GCell, TopDrop2GCellView>(
                postedModel.StatDate, regionNamesService.Query().ToList(),
                btsRepository.GetAllList(), eNodebRepository.GetAllList());
            return View(viewModel);
        }

        public ViewResult TopDrop2GDaily(TopDrop2GDailyViewModel postedModel)
        { 
            TopDrop2GDailyViewModel viewModel
                =topDrop2GDailyRepository.Stats.GenerateView<
                TopDrop2GDailyViewModel,TopDrop2GCellDaily,TopDrop2GCellDailyView>(
                postedModel.StatDate, regionNamesService.Query().ToList(),
                btsRepository.GetAllList(), eNodebRepository.GetAllList());
            return View(viewModel);
        }

        public ViewResult TopConnection3G(TopConnection3GViewModel postedModel)
        {
            TopConnection3GViewModel viewModel
                = topConnection3GRepository.Stats.GenerateView<
                TopConnection3GViewModel, TopConnection3GCell, TopConnection3GCellView>(
                postedModel.StatDate, regionNamesService.Query().ToList(),
                btsRepository.GetAllList(), eNodebRepository.GetAllList());
            return View(viewModel);
        }

        [Authorize]
        public ViewResult Import()
        {
            return View();
        }

        [Authorize]
        public ViewResult KpiImport()
        {
            QueryNamesService regionService = new QueryOptimizeRegionNamesService(regionRepository.GetAll());
            string[] sheetNames = regionService.Query().ToArray();

            HttpPostedFileBase httpPostedFileBase = Request.Files["dailyKpi"];
            if (httpPostedFileBase == null || httpPostedFileBase.FileName == "")
            {
                TempData["error"] = "请选择需要导入的Excel文件！";
                return View("Import");
            }

            KpiImportResult result = this.Import(httpPostedFileBase, sheetNames, regionRepository, cdmaStatRepository);

            TempData["success"] 
                = "成功保存区域指标：" + result.RegionStatsSaved + "条"
                  + "    成功保存TOP掉话指标" + result.TopDrop2GSaved + "条"
                  + "    成功保存TOP连接失败指标" + result.TopConnection3GSaved + "条";
            return View("Import");
        }
	}
}