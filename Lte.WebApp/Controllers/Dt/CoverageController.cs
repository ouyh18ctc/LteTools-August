using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lte.Domain.Geo.Service;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Service;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Coverage;

namespace Lte.WebApp.Controllers.Dt
{
    public class CoverageController : Controller
    {
        private readonly IENodebRepository eNodebRepository;
        private readonly ICellRepository cellRepository;

        public CoverageController(IENodebRepository eNodebRepository,
            ICellRepository cellRepositroy)
        {
            this.eNodebRepository = eNodebRepository;
            cellRepository = cellRepositroy;
        }

        public JsonResult CoverageAdjust(CoverageStatChart chart)
        {
            List<ENodeb> eNodebList = eNodebRepository.GetAllList();
            IEnumerable<CoverageAdjustment> adjustments =
                chart.StatList.GenerateAdjustmentList(cellRepository, eNodebList);
            var result = from a in adjustments
                         join e in eNodebList
                         on a.ENodebId equals e.ENodebId
                         select new
                         {
                             N = e.Name + "-" + a.SectorId,
                             F = a.Frequency,
                             F165m = (int)(100 * a.Factor165m) / (double)(100),
                             F135m = (int)(100 * a.Factor135m) / (double)(100),
                             F105m = (int)(100 * a.Factor105m) / (double)(100),
                             F75m = (int)(100 * a.Factor75m) / (double)(100),
                             F45m = (int)(100 * a.Factor45m) / (double)(100),
                             F15m = (int)(100 * a.Factor15m) / (double)(100),
                             F15 = (int)(100 * a.Factor15) / (double)(100),
                             F45 = (int)(100 * a.Factor45) / (double)(100),
                             F75 = (int)(100 * a.Factor75) / (double)(100),
                             F105 = (int)(100 * a.Factor105) / (double)(100),
                             F135 = (int)(100 * a.Factor135) / (double)(100),
                             F165 = (int)(100 * a.Factor165) / (double)(100)
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CoverageIntervalPercentage(string fieldName, CoverageStatChart chart)
        {
            StatValueField field = new StatValueField { FieldName = fieldName };
            field.AutoGenerateIntervals(8);
            IEnumerable<double> values = (fieldName == "信号RSRP") ?
                chart.StatList.Select(x => x.Rsrp) : chart.StatList.Select(x => x.Sinr);
            Dictionary<string, double> result = field.GetPercentageStat(values);
            return Json(result.Select(x => new { N = x.Key, V = 100 * x.Value }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportPoints(string fieldName, StatValueFieldRepository repository,
            CoverageStatChart chart)
        {
            if (chart.StatList.Count == 0)
            {
                TempData["warning"] = "覆盖数据为空，请先导入数据分析。";
                return RedirectToAction("CoverageImport");
            }
            StatValueField field = repository.GenerateDefaultField(fieldName);
            TempData["centerX"] = chart.StatList.Average(x=>x.BaiduLongtitute);
            TempData["centerY"] = chart.StatList.Average(x=>x.BaiduLattitute);
            return View(field);
        }

        public JsonResult GetStatValueField(StatValueFieldRepository repository,
            string fieldName)
        {
            StatValueField field = repository.GenerateDefaultField(fieldName);
            return Json(field.IntervalList.Select(x => new
            {
                L = x.IntervalLowLevel,
                H = x.IntervalUpLevel,
                C = x.Color.ColorStringForHtml,
                K = x.Color.ColorStringForKml
            }).ToArray(),
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCoveragePoints(string fieldName,
            StatValueFieldRepository repository, CoverageStatChart chart)
        {
            StatValueField field = repository.GenerateDefaultField(fieldName);
            return Json(chart.StatList.Select(x => new
            {
                X = x.Longtitute + GeoMath.BaiduLongtituteOffset,
                Y = x.Lattitute + GeoMath.BaiduLattituteOffset,
                C = field.GetColor(fieldName == "信号RSRP" ? x.Rsrp : x.Sinr, "FFFFFF")
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CoverageImport()
        {
            TempData["StatLength"] = 0;
            return View();
        }

        public ActionResult CoverageAnalyze(HttpPostedFileBase[] fileUpload, CoverageStatChart chart)
        {
            List<CoverageStat> coverageStatList = new List<CoverageStat>();
            if (fileUpload[0] != null)
            {
                foreach (HttpPostedFileBase file in fileUpload)
                {
                    using (HttpFileImporter importer = new HttpFileImporter(file))
                    {
                        if (!importer.Success)
                        {
                            if (TempData["warning"] == null)
                            {
                                TempData["warning"] = "请选择合适的路测数据导入！读取以下文件失败："
                                                      + importer.FilePath;
                            }
                            else
                            {
                                TempData["warning"] += "; " + importer.FilePath;
                            }
                        }
                        else
                        {
                            string extension = Path.GetExtension(importer.FileName);
                            if (extension != null)
                            {
                                string fileExt = extension.ToLower();
                                coverageStatList.AddRange(
                                    (fileExt == ".txt") ?
                                        CsvContext.Read<LogRecord>(
                                            importer.Reader, CsvFileDescription.TabDescription).Select(x =>
                                            {
                                                CoverageStat stat = new CoverageStat(); stat.Import(x); return stat;
                                            }).ToList() :
                                        CsvContext.Read<HugelandRecord>(
                                            importer.Reader, CsvFileDescription.CommaDescription).Select(x =>
                                            {
                                                CoverageStat stat = new CoverageStat(); stat.Import(x); return stat;
                                            }));
                            }
                            chart.Import(coverageStatList);
                            if (TempData["warning"] == null)
                            {
                                TempData["success"] = "导入路测数据成功！";
                            }
                        }
                    }
                }
            }

            ViewBag.Title = "路测覆盖指标分析";
            TempData["StatLength"] = chart.StatList.Count;
            return View("CoverageImport");
        }

        public ActionResult FileRecords2G(string fileName)
        {
            FileRecordsRepository.Update2GList(DCTestService.Query2GFileRecords(fileName).ToList());
            TestFileRecordsViewModel viewModel = new TestFileRecordsViewModel
            {
                CenterX = FileRecordsRepository.FileRecords2GList.Average(x => x.BaiduLongtitute),
                CenterY = FileRecordsRepository.FileRecords2GList.Average(x => x.BaiduLattitute),
                FileName = fileName
            };
            return View(viewModel);
        }

        public ActionResult FileRecords3G(string fileName)
        {
            FileRecordsRepository.Update3GList(DCTestService.Query3GFileRecords(fileName).ToList());
            TestFileRecordsViewModel viewModel = new TestFileRecordsViewModel
            {
                CenterX = FileRecordsRepository.FileRecords3GList.Average(x => x.BaiduLongtitute),
                CenterY = FileRecordsRepository.FileRecords3GList.Average(x => x.BaiduLattitute),
                FileName = fileName
            };
            return View(viewModel);
        }

        public ActionResult FileRecords4G(string fileName)
        {
            FileRecordsRepository.Update4GList(DCTestService.Query4GFileRecords(fileName).ToList());
            TestFileRecordsViewModel viewModel = new TestFileRecordsViewModel
            {
                CenterX = FileRecordsRepository.FileRecords4GList.Average(x => x.BaiduLongtitute),
                CenterY = FileRecordsRepository.FileRecords4GList.Average(x => x.BaiduLattitute),
                FileName = fileName
            };
            return View(viewModel);
        }
    }
}
