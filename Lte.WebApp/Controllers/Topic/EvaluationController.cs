using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Domain.Regular;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Infrastructure;
using Lte.Evaluations.Infrastructure.Entities;
using Lte.Evaluations.Kml;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Lte;
using Lte.Parameters.Service.Public;
using Lte.WebApp.Models;

namespace Lte.WebApp.Controllers.Topic
{
    public class EvaluationController : Controller
    {
        private readonly ITownRepository townRepository;
        private readonly IENodebRepository eNodebRepository;
        private readonly ICellRepository cellRepository;
        private static List<ENodeb> eNodebList = new List<ENodeb>();
        private const int PageSize = 10;

        public EvaluationController(ITownRepository townRepository,
            IENodebRepository eNodebRepository, ICellRepository cellRepositroy)
        {
            this.townRepository = townRepository;
            this.eNodebRepository = eNodebRepository;
            cellRepository = cellRepositroy;
            
        }

        public void ResetENodebList()
        {
            eNodebList = eNodebRepository.GetAllList();
        }

        public ActionResult RegionDef(int page = 1)
        {
            EvaluationViewModel viewModel =
                new EvaluationViewModel
                {
                    ENodebs = eNodebList.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = eNodebList.Count()
                    },
                    TrafficLoad = EvaluationSettings.TrafficLoad * 100,
                    CellCoverage = (int)EvaluationSettings.DegreeSpan.GetDistanceInMeter(),
                    DistanceInMeter = EvaluationSettings.DistanceInMeter
                };
            viewModel.InitializeTownList(townRepository);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RegionDef(ENodebQueryViewModel viewModel, 
            EvaluationInfrastructure infrastructure)
        {
            eNodebList = eNodebRepository.GetAllWithNames(townRepository,
                viewModel, viewModel.ENodebName, viewModel.Address);

            EvaluationViewModel model = new EvaluationViewModel
            {
                ENodebs = eNodebList.Take(PageSize).ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = PageSize,
                    TotalItems = eNodebList.Count()
                },
                TrafficLoad = EvaluationSettings.TrafficLoad * 100,
                CellCoverage = (int)EvaluationSettings.DegreeSpan.GetDistanceInMeter(),
                DistanceInMeter = EvaluationSettings.DistanceInMeter
            };
            model.InitializeTownList(townRepository);
            ViewBag.RegionList = infrastructure.Region;
            return View(model);
        }

        public ActionResult PointsCalculate(EvaluationInfrastructure infrastructure)
        {
            infrastructure.InitializeRegion();
            if (infrastructure.Region == null || infrastructure.Region.Length == 0
                || infrastructure.CellList.Count == 0)
            {
                TempData["warning"] = "仿真区域设置为空！请先设置仿真区域。";
                return RedirectToAction("RegionDef");
            }
            infrastructure.Region.CalculatePerformance(EvaluationSettings.TrafficLoad);
            TempData["success"] = "仿真数据处理成功！可以查看仿真结果。";
            return RedirectToAction("PointsAnalysis");
        }

        public ActionResult PointsAnalysis(StatValueFieldRepository repository)
        {
            return View(new StatFieldViewModel(repository.FieldList[0])
            {
                FieldLength = 4,
                ColorThemeDescription = "春"
            });
        }

        [HttpPost]
        public ActionResult PointsAnalysis(StatFieldViewModel viewModel,
            StatValueFieldRepository repository)
        {
            for (int i = 0; i < repository.FieldList.Count; i++)
            {
                if (i == (int)viewModel.FieldName.GetStatValueIndex()
                    || repository.FieldList[i].IntervalList.Count == 0)
                {
                    repository.FieldList[i].AutoGenerateIntervals(
                        viewModel.FieldLength, viewModel.ColorThemeDescription.GetColorThemeIndex());
                }
            }
            return View(new StatFieldViewModel(repository.FieldList[(int)viewModel.FieldName.GetStatValueIndex()])
            {
                FieldLength = viewModel.FieldLength,
                ColorThemeDescription = viewModel.ColorThemeDescription
            });
        }

        public ActionResult ExportPoints(string fieldName,
            EvaluationInfrastructure infrastructure,
            StatValueFieldRepository repository)
        {
            if (infrastructure.Region == null || infrastructure.Region.Length == 0
                || infrastructure.CellList.Count == 0)
            {
                TempData["warning"] = "仿真区域设置为空！请先设置仿真区域。";
                return RedirectToAction("RegionDef");
            }
            StatValueField field = repository.FieldList[(int)fieldName.GetStatValueIndex()];
            if (field.IntervalList.Count == 0)
            { 
                TempData["warning"] = "显示区间设置为空！请先设置区间。";
                return RedirectToAction("PointsAnalysis");
            }

            TempData["centerX"] = infrastructure.CellList.Average(x=>x.BaiduLongtitute);
            TempData["centerY"] = infrastructure.CellList.Average(x=>x.BaiduLattitute);
            return View(field);
        }

        public ActionResult ExportKml(string fieldName,
            EvaluationInfrastructure infrastructure,
            StatValueFieldRepository repository)
        {
            StatValueField field = repository.FieldList[(int)fieldName.GetStatValueIndex()];
            if (field.IntervalList.Count == 0)
            {
                TempData["warning"] = "显示区间设置为空！请先设置区间。";
                return RedirectToAction("PointsAnalysis");
            }

            GoogleKml kml = new GoogleKml("framework.xml");
            string absoluFilePath
                = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "uploads\\", 
                "Kmlresults-" + field.FieldName + DateTime.Today.ToString("yyyyMMdd"));
            kml.GenerateKmlFile(absoluFilePath, 
                infrastructure.Region.GetMeasureInfoList(field, EvaluationSettings.DistanceInMeter), field);
            return File(new FileStream(absoluFilePath, FileMode.Open),
                "application/octet-stream", Server.UrlEncode(
                "Kmlresults-" + field.FieldName + DateTime.Today.ToString("yyyyMMdd") + ".kml"));
        }

        public JsonResult ImportCells(EvaluationInfrastructure infrastructure, 
            string message)
        {
            IEnumerable<ENodeb> eNodebs;
            if (message != "")
            {
                string[] ids = message.GetSplittedFields(',');
                eNodebs
                    = from a in eNodebList
                      join b in ids
                      on a.ENodebId equals int.Parse(b)
                      select a;
            }
            else
            {
                eNodebs = eNodebList;
            }
            IEnumerable<IOutdoorCell> outdoorCellList = cellRepository.Query(eNodebs);

            infrastructure.AddCells(outdoorCellList as IEnumerable<EvaluationOutdoorCell>);

            return Json(infrastructure.CellList.Select(x =>
                new
                {
                    CellName = x.CellName,
                    CellInfo = x.Info()
                }).ToArray(),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImportCellsWithRange(EvaluationInfrastructure infrastructure,
            StatComplexFieldRepository repository,
            string fieldName, double southWestLon, double southWestLat,
            double northEastLon, double northEastLat)
        {
            infrastructure.ImportCellList(
                eNodebRepository, 
                cellRepository,
                new GeoPoint(southWestLon - GeoMath.BaiduLongtituteOffset, southWestLat - GeoMath.BaiduLattituteOffset),
                new GeoPoint(northEastLon - GeoMath.BaiduLongtituteOffset, northEastLat - GeoMath.BaiduLattituteOffset));
            if (string.IsNullOrEmpty(fieldName))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            infrastructure.Region.CalculatePerformance(EvaluationSettings.TrafficLoad);
            StatValueField field = repository.FieldList[(int)fieldName.GetStatValueIndex()];

            List<MeasurePointInfo> infoList
                = infrastructure.Region.GetMeasureMergedList(field, EvaluationSettings.DistanceInMeter);
            return Json(infoList.Select(x =>
                new
                {
                    X1 = x.X1 + GeoMath.BaiduLongtituteOffset,
                    Y1 = x.Y1 + GeoMath.BaiduLattituteOffset,
                    X2 = x.X2 + GeoMath.BaiduLongtituteOffset,
                    Y2 = x.Y2 + GeoMath.BaiduLattituteOffset,
                    C = x.ColorString
                }).ToArray(),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCells(EvaluationInfrastructure infrastructure)
        {
            return Json(infrastructure.CellList.Select(x =>
                new
                {
                    CellName = x.CellName,
                    CellInfo = x.Info()
                }).ToArray(),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPoints(string fieldName,
            EvaluationInfrastructure infrastructure,
            StatValueFieldRepository repository)
        {
            StatValueField field = repository.FieldList[(int)fieldName.GetStatValueIndex()];
            List<MeasurePointInfo> infoList 
                = infrastructure.Region.GetMeasureMergedList(field, EvaluationSettings.DistanceInMeter);
            return Json(infoList.Select(x =>
                new { 
                    X1 = x.X1 + GeoMath.BaiduLongtituteOffset, 
                    Y1 = x.Y1 + GeoMath.BaiduLattituteOffset,
                    X2 = x.X2 + GeoMath.BaiduLongtituteOffset,
                    Y2 = x.Y2 + GeoMath.BaiduLattituteOffset, 
                    C = x.ColorString }).ToArray(), 
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSectors(EvaluationInfrastructure infrastructure)
        {
            List<SectorTriangle> sectors = infrastructure.CellList.GetSectors();
            return Json(sectors, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveOneCell(EvaluationInfrastructure infrastructure, 
            string cellName)
        {
            infrastructure.RemoveCell(cellName);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ModifyParameters(double trafficLoad, double cellCoverage, double distanceInMeter)
        {
            EvaluationSettings.TrafficLoad = trafficLoad / 100;
            EvaluationSettings.DegreeSpan = cellCoverage.GetDegreeInterval();
            EvaluationSettings.DistanceInMeter = distanceInMeter;
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatValueField(StatValueFieldRepository repository,
            string fieldName)
        {
            StatValueField field = repository.FieldList[(int)fieldName.GetStatValueIndex()];
            return Json(field.IntervalList.Select(x => new
            {
                L = x.IntervalLowLevel,
                H = x.IntervalUpLevel,
                C = x.Color.ColorStringForHtml,
                K = x.Color.ColorStringForKml
            }).ToArray(),
            JsonRequestBehavior.AllowGet);
        }
    }
}
