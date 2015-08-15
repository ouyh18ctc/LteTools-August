using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Controllers.Rutrace
{
    public class RutraceAnalysisController : Controller
    {
        public JsonResult GetInterferenceRatioToVictimCellsLine(double factor)
        {
            Dictionary<int, double> values = new Dictionary<int, double>();

            for (int i = 1; i <= 100; i++)
            {
                values.Add(i, Math.Pow(i, -1 / factor));
            }

            return Json(values.Select(x => new { X = x.Key, Y = x.Value }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAverageRtdToTaLine(double factor)
        {
            Dictionary<double, int> values = new Dictionary<double, int>{
                { 0, 0 }, { 6000 / factor, 6000 } };
            return Json(values.Select(x => new { X = x.Key, Y = x.Value }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaDistanceRing()
        {
            Dictionary<double, double> values = new Dictionary<double, double>();
            double radius = InterferenceStat.LowerBound;
            for (int i = 90; i >= 0; i--)
            {
                values.Add(radius * Math.Cos(i * Math.PI / 180), radius * Math.Sin(i * Math.PI / 180));
            }
            return Json(values.Select(x => new { X = x.Key, Y = x.Value }), JsonRequestBehavior.AllowGet);
        }

    }
}
