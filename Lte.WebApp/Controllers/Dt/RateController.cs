using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.ViewHelpers;

namespace Lte.WebApp.Controllers.Dt
{
    public class RateController : Controller
    {
        public ActionResult RateImport()
        {
            ViewBag.Title = "导入路测数据";
            TempData["StatLength"] = 0;
            return View();
        }

        public ActionResult RateAnalyze(RateStatChart chart)
        {
            using (HttpFileImporter importer = new HttpFileImporter(Request.Files["fileUpload"]))
            {
                if (!importer.Success)
                {
                    TempData["error"] = "请选择合适的路测数据导入！";
                    ViewBag.Title = "导入路测数据";
                }
                else
                {
                    TempData["Path"] = importer.FilePath;
                    string extension = Path.GetExtension(importer.FileName);
                    if (extension != null)
                    {
                        string fileExt = extension.ToLower();
                        List<BasicRateStat> rateStatList
                            = (fileExt == ".txt") ?
                                CsvContext.Read<LogRecord>(
                                    importer.Reader, CsvFileDescription.TabDescription).ToList().MergeStat().Merge() :
                                CsvContext.Read<HugelandRecord>(
                                    importer.Reader, CsvFileDescription.CommaDescription).Select(
                                        x => x.Normalize()).ToList().MergeStat().Where(
                                            x => x.PdschRbRate > 0).Select(x => (BasicRateStat)x).ToList();
                        chart.Import(rateStatList);
                    }
                    ViewBag.Title = "路测速率指标分析";
                    TempData["success"] = "导入路测数据:" + importer.FileName + "成功！";
                }
            }
            TempData["StatLength"] = chart.StatList.Count;
            return View("RateImport");
        }

        public JsonResult GetPdschRbTimeSeries(RateStatChart chart)
        {
            return Json(chart.StatList.Select(x =>
                new { Time = x.PassedTimeInSeconds, RB = x.PdschRbRate }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDlFrequencyEfficiencyTimeSeries(RateStatChart chart)
        {
            return Json(chart.StatList.Select(x =>
                new { Time = x.PassedTimeInSeconds, FE = x.DlFrequencyEfficiency }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDlPdcpThroughputTimeSeries(RateStatChart chart)
        {
            return Json(chart.StatList.Select(x =>
                new { Time = x.PassedTimeInSeconds, TP = (double)x.DlThroughput / 1024 / 1024 }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRsrpTimeSeries(RateStatChart chart)
        {
            return Json(chart.StatList.Where(x => x.Rsrp > -150).Select(x =>
                new { Time = x.PassedTimeInSeconds, Rsrp = x.Rsrp }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSinrTimeSeries(RateStatChart chart)
        {
            return Json(chart.StatList.Where(x => x.Sinr > -30).Select(x =>
                new { Time = x.PassedTimeInSeconds, Sinr = x.Sinr }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLowRbRateSinrToFrequencyEfficiency(RateStatChart chart)
        {
            return Json(chart.StatList.Where(x => x.DlRbsPerSlot <= 5 && x.Sinr > -30).Select(x =>
                new { Sinr = x.Sinr, FE = x.DlFrequencyEfficiency }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHighRbRateSinrToFrequencyEfficiency(RateStatChart chart)
        {
            return Json(chart.StatList.Where(x => x.DlRbsPerSlot > 5 && x.Sinr > -30).Select(x =>
                new { Sinr = x.Sinr, FE = x.DlFrequencyEfficiency }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTheorySinrToFrequencyEfficiency(RateStatChart chart)
        {
            return Json(chart.TheoryLine.Select(x =>new { Sinr = x.Key, FE = x.Value }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLowRbRateRsrpToFrequencyEfficiency(RateStatChart chart)
        {
            return Json(chart.StatList.Where(x => x.DlRbsPerSlot <= 5 && x.Rsrp > -150).Select(x =>
                new { Rsrp = x.Rsrp, FE = x.DlFrequencyEfficiency }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHighRbRateRsrpToFrequencyEfficiency(RateStatChart chart)
        {
            return Json(chart.StatList.Where(x => x.DlRbsPerSlot > 5 && x.Rsrp > -150).Select(x =>
                new { Rsrp = x.Rsrp, FE = x.DlFrequencyEfficiency }),
                JsonRequestBehavior.AllowGet);
        }
    }
}
