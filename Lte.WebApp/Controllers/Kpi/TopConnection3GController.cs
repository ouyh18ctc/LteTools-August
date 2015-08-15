using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Service.Public;

namespace Lte.WebApp.Controllers.Kpi
{
    public class TopConnection3GController : Controller
    {
        private readonly ITopCellRepository<TopConnection3GCell> statRepository;
        private readonly IBtsRepository btsRepository;
        private readonly IENodebRepository eNodebRepository;

        public TopConnection3GController(
            ITopCellRepository<TopConnection3GCell> statRepository,
            IBtsRepository btsRepository,
            IENodebRepository eNodebRepository)
        {
            this.statRepository = statRepository;
            this.btsRepository = btsRepository;
            this.eNodebRepository = eNodebRepository;
        }

        public JsonResult Query(DateTime begin, DateTime end, string city, int topCounts = 20)
        {
            DateTime endDate = end.AddDays(1);
            IEnumerable<TopConnection3GCell> stats = statRepository.Stats.QueryTimeStats(begin, endDate, city).ToList();
            if (stats.Any())
            {
                CdmaLteNamesService<TopConnection3GCell> service = new CdmaLteNamesService<TopConnection3GCell>(
                    stats, btsRepository.GetAllList(), eNodebRepository.GetAllList());
                IEnumerable<TopConnection3GCellView> cellViews
                    = service.Clone<TopConnection3GCellView>();
                var statCounts = from v in cellViews
                                 group v by new { v.CdmaName, v.SectorId } into g
                                 select new
                                 {
                                     CarrierName = g.Key.CdmaName + "-" + g.Key.SectorId,
                                     TopDates = g.Count(),
                                     SumOfTimes= g.Sum(v => v.ConnectionFails)
                                 };
                return Json(statCounts.OrderByDescending(x => x.SumOfTimes).Take(topCounts), 
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new List<int>(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueryHistory(int cellId, byte sectorId, DateTime end, int days = 20)
        {
            DateTime beginDate = end.AddDays(-days).Date;
            DateTime endDate = end.AddDays(1).Date;
            IEnumerable<TopConnection3GCell> cells = new List<TopConnection3GCell>();
            while (cells.Count() < 3 && beginDate > endDate.AddDays(-300))
            {
                cells = statRepository.Stats.Where(
                    x => x.StatTime >= beginDate && x.StatTime < endDate
                    && x.CellId == cellId && x.SectorId == sectorId).ToList();
                beginDate = beginDate.AddDays(-days);
            }
            return Json(cells.OrderBy(x => x.StatTime).Select(x => new
            {
                StatDate = x.StatTime.Date.ToShortDateString(),
                ConnectionFails = x.ConnectionFails,
                ConnectionRate = x.ConnectionRate * 100,
                LinkBusyRate = x.LinkBusyRate,
                DropRate = x.DropRate * 100
            }),
                JsonRequestBehavior.AllowGet);
        }
	}
}