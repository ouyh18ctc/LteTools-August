using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Lte.Domain.Geo.Entities;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Infrastructure.Entities;
using Lte.Evaluations.Service;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Coverage;

namespace Lte.WebApp.Controllers.Rutrace
{
    public class RutraceController : Controller
    {
        private readonly IENodebRepository _eNodebRepository;
        private readonly ICellRepository _cellRepository;
        private readonly ITownRepository _townRepository;
        private readonly IMrsCellRepository _mrsRepository;
        private static IEnumerable<EvaluationOutdoorCell> outdoorCellList;

        private int PageSize { get; set; }

        public RutraceController(IENodebRepository eNodebRepository,
            ICellRepository cellRepositroy,
            ITownRepository townRepository,
            IMrsCellRepository mrsRepository)
        {
            PageSize = 10;
            _eNodebRepository = eNodebRepository;
            _cellRepository = cellRepositroy;
            _townRepository = townRepository;
            _mrsRepository = mrsRepository;
        }

        public ActionResult Import(int page = 1)
        {
            IEnumerable<ENodebBase> eNodebBases = _eNodebRepository.GetAllList().Select(x =>
                new ENodebBase
                {
                    ENodebId = x.ENodebId,
                    Name = x.Name
                });
            MrInterferenceViewModel model = new MrInterferenceViewModel
            {
                StartDate = DateTime.Today.AddDays(-7),
                EndDate = DateTime.Today
            };
            model.InitializeTownList(_townRepository, model);
            model.UpdateStats(RutraceStatContainer.MrsStats, eNodebBases, PageSize, page);
            return View(model);
        }

        [HttpPost]
        public ActionResult ImportResult(MrInterferenceViewModel model)
        {
            IEnumerable<ENodeb> eNodebs = _eNodebRepository.GetAllWithNames(_townRepository,
                model.CityName, model.DistrictName, model.TownName, model.ENodebName, model.Address);
            IEnumerable<ENodebBase> eNodebBases = eNodebs.ToList().Select(x =>
                new ENodebBase
                {
                    ENodebId = x.ENodebId,
                    Name = x.Name
                });
            if (eNodebs.Any())
            {
                MrsCellDate[] stats 
                    = _mrsRepository.QueryItems(eNodebs, model.StartDate, model.EndDate).ToArray();
                Array.Sort(stats, new MrsCellDateComparer());
                if (stats.Any())
                {
                    RutraceStatContainer.MrsStats = stats.Select(x =>
                    {
                        MrsCellDateView view = new MrsCellDateView();
                        x.CloneProperties(view);
                        ENodebBase eNodeb = eNodebBases.FirstOrDefault(e => e.ENodebId == x.CellId);
                        view.CellName = eNodeb == null ? "Unknown" : eNodeb.Name + "-" + x.SectorId;
                        return view;
                    }).ToList();
                    outdoorCellList = RutraceStatContainer.QueryOutdoorCellsFromMrs(
                        _eNodebRepository, _cellRepository);
                }
            }
            model.InitializeTownList(_townRepository, model);
            model.UpdateStats(RutraceStatContainer.MrsStats, eNodebBases, PageSize);
            return View("Import", model);
        }

        public ActionResult ExportStat(string fileName)
        {
            IEnumerable<MrsCellDateView> results = RutraceStatContainer.MrsStats;
            if (results == null) { return Redirect("Import"); }
            string absoluFilePath
                = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "uploads/", fileName);
            CsvFileDescription fileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true,
                TextEncoding = Encoding.GetEncoding("GB2312")
            };
            CsvContext.Write(results, absoluFilePath, fileDescription);
            return File(new FileStream(absoluFilePath, FileMode.Open),
                "application/octet-stream", Server.UrlEncode(fileName));
        }

        public ActionResult DisplayDef(StatComplexFieldRepository repository)
        {
            return View(new StatFieldViewModel(repository.FieldList[0])
            {
                FieldLength = 8,
                ColorThemeDescription = "喷漆"
            });
        }

        [HttpPost]
        public ActionResult DisplayDef(StatFieldViewModel viewModel,
            StatComplexFieldRepository repository)
        {
            for (int i = 0; i < repository.FieldList.Count; i++)
            {
                if (i == (int)viewModel.FieldName.GetStatRuIndex()
                    + StatValueChoiceQueries.Choices.Count()
                    || i == (int)viewModel.FieldName.GetStatValueIndex()
                    || repository.FieldList[i].IntervalList.Count == 0)
                {
                    repository.FieldList[i].AutoGenerateIntervals(
                        viewModel.FieldLength, viewModel.ColorThemeDescription.GetColorThemeIndex());
                }
            }
            GenerateValuesMrsService service = new GenerateValuesMrsService(RutraceStatContainer.MrsStats);
            return View(new StatFieldViewModel(repository[viewModel.FieldName],
                service.GenerateValues(viewModel.FieldName))
            {
                FieldLength = viewModel.FieldLength,
                ColorThemeDescription = viewModel.ColorThemeDescription
            });
        }

        public ActionResult ExportSectors(StatComplexFieldRepository repository)
        {
            if (RutraceStatContainer.MrsStats == null || RutraceStatContainer.MrsStats.Count == 0)
            {
                TempData["warning"] = "小区统计数据为空！请先导入并统计数据。";
                return RedirectToAction("Import");
            }

            for (int i = 0; i < StatRuChoiceQueries.Choices.Count(); i++)
            {
                StatValueField field = repository.FieldList[i + StatRuChoiceQueries.Choices.Count()];
                if (field.IntervalList.Count == 0)
                {
                    TempData["warning"] = "显示区间：" + StatRuChoiceQueries.Choices.ElementAt(0)
                        + "设置为空！请先设置区间。";
                    return RedirectToAction("DisplayDef");
                }
            }

            return View(new StatFieldsSelectionViewModel
            {
                RuFieldName = StatRuChoiceQueries.Choices.ElementAt(0),
                ValueFieldName = StatValueChoiceQueries.Choices.ElementAt(0),
                Longtitute = outdoorCellList.Where(x => x.Height > 1).Select(x => x.Longtitute).Distinct().Average(),
                Lattitute = outdoorCellList.Where(x => x.Height > 1).Select(x => x.Lattitute).Distinct().Average()
            });
        }

        public JsonResult GetStatRuField(StatComplexFieldRepository repository,
            string fieldName)
        {
            StatValueField field = repository.FieldList[(int)fieldName.GetStatRuIndex()
                + StatValueChoiceQueries.Choices.Count()];
            return Json(field.IntervalList.Select(x => new
            {
                L = x.IntervalLowLevel,
                H = x.IntervalUpLevel,
                C = x.Color.ColorStringForHtml,
                K = x.Color.ColorStringForKml
            }).ToArray(),
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatValueField(StatComplexFieldRepository repository,
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

        public JsonResult GetStatField(StatComplexFieldRepository repository,
            string fieldName)
        {
            StatValueChoice valueChoice = fieldName.GetStatValueIndex();
            StatRuChoice ruChoice = fieldName.GetStatRuIndex();
            int index = (valueChoice == StatValueChoice.Undefined) ?
                (int)ruChoice + StatValueChoiceQueries.Choices.Count() :
                (int)valueChoice;
            StatValueField field = repository.FieldList[index];
            return Json(field.IntervalList.Select(x => new
            {
                L = x.IntervalLowLevel,
                H = x.IntervalUpLevel,
                C = x.Color.ColorStringForHtml,
                K = x.Color.ColorStringForKml
            }).ToArray(),
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatSectors(StatComplexFieldRepository repository, string fieldName)
        {
            List<SectorTriangle> info = repository.GenerateSectors(RutraceStatContainer.MrsStats,
                outdoorCellList, fieldName);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}
