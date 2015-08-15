using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.WebApp.Controllers.Kpi
{
    public class KpiStatController : Controller
    {
        private readonly ITopCellRepository<CdmaRegionStat> cdmaStatRepository;

        public KpiStatController(ITopCellRepository<CdmaRegionStat> cdmaStatRepository)
        {
            this.cdmaStatRepository = cdmaStatRepository;
        }

        public JsonResult Query(DateTime begin, DateTime end, AllCdmaDailyStatList statList)
        {
            statList.Import(cdmaStatRepository, begin, end);
            return Json(statList.Stats.Count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDateCategories(string city, AllCdmaDailyStatList statList)
        {
            return Json(statList.DateCategories(city), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityErlang2GSeries(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetSummaryStats(x => x.ErlangExcludingSwitch);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityDrop2GRateSeries(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetSummaryStats(x => x.Drop2GRate * 100);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityEcio2GSeries(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetSummaryStats(x => x.Ecio * 100);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityFlow3GSeries(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetSummaryStats(x => x.Flow / 1024);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityDownSwitchRateSeries(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetSummaryStats(x => x.DownSwitchRate);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityCi3GSeries(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetSummaryStats(x => x.Ci * 100);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionErlang2GSeries(string city, string region, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetRegionStats(
                x => x.ErlangExcludingSwitch, x => x.Region, region);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionDrop2GRateSeries(string city, string region, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetRegionStats(
                x => x.Drop2GRate * 100, x => x.Region, region);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionEcio2GSeries(string city, string region, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetRegionStats(
                x => x.Ecio * 100, x => x.Region, region);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionFlow3GSeries(string city, string region, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetRegionStats(
                x => x.Flow / 1024, x => x.Region, region);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionCi3GSeries(string city, string region, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetRegionStats(
                x => x.Ci * 100, x => x.Region, region);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionDownSwitchRateSeries(string city, string region, AllCdmaDailyStatList statList)
        {
            IEnumerable<double> stats = statList.Stats[city].GetRegionStats(
                x => x.DownSwitchRate, x => x.Region, region);
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionList(string city, AllCdmaDailyStatList statList)
        {
            IEnumerable<string> regions = statList.Stats[city].Regions;
            return Json(regions, JsonRequestBehavior.AllowGet);
        }
	}
}