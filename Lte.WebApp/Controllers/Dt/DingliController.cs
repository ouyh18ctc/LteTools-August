using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Coverage;

namespace Lte.WebApp.Controllers.Dt
{
    public class DingliController : Controller
    {
        private static IEnumerable<Town> towns;

        public DingliController(ITownRepository townRepository)
        {
            LogRecordRepository.ThroughputCalculationDelay = 1;
            if (towns == null)
                towns = townRepository.GetAllList();
        }

        public ActionResult HandoverImport()
        {
            TempData["StatLength"] = 0;
            return View();
        }

        public ActionResult HandoverAnalyze()
        {
            using (HttpFileImporter importer = new HttpFileImporter(Request.Files["fileUpload"]))
            {
                if (!importer.Success)
                {
                    TempData["error"] = "请选择合适的路测数据导入！";
                }
                else
                {
                    LogRecordRepository recordRepository = new LogRecordRepository
                    {
                        LogRecordList = CsvContext.Read<LogRecord>(
                            new StreamReader(importer.FilePath), 
                            CsvFileDescription.TabDescription).ToList().Merge()
                    };
                    recordRepository.GetHandoverInfoList();

                    ViewBag.Title = "导入路测数据:" + importer.FileName;
                }
            }
            return View("HandoverImport");
        }

        public ActionResult ExportLogs(string fileName)
        {
            if (TempData["HandoverList"] == null) { return Redirect("HandoverImport"); }
            string absoluFilePath
                = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "uploads\\", fileName);
            CsvFileDescription fileDescription = CsvFileDescription.ChineseExportDescription;
            CsvContext.Write(TempData["HandoverList"] as List<HandoverInfo>, 
                absoluFilePath, fileDescription);
            return File(new FileStream(absoluFilePath, FileMode.Open),
                "application/octet-stream", Server.UrlEncode(fileName));
        }

        public ActionResult List()
        {
            TestListViewModel viewModel = new TestListViewModel
            {
                AreaTestDateList = DCTestService.QueryTestDateInfos().ToList(),
                TownList = towns
            };
            return View(viewModel);
        }

    }
}
