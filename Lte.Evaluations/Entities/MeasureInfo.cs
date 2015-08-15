using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Measure;

namespace Lte.Evaluations.Entities
{
    public class MeasurePointInfo
    {
        public double X1 { get; set; }

        public double X2 { get; set; }

        public double Y1 { get; set; }

        public double Y2 { get; set; }

        public string ColorString { get; set; }

        public string ColorStringForKml { get; set; }

        private short Height { get; set; }

        public MeasurePointInfo()
        { Height = 10; }

        public MeasurePointInfo(MeasurePoint point, StatValueField field, double degreeSpan) : this()
        {
            X1 = point.Longtitute - degreeSpan / 2;
            X2 = point.Longtitute + degreeSpan / 2;
            Y1 = point.Lattitute - degreeSpan / 2;
            Y2 = point.Lattitute + degreeSpan / 2;
            double value;
            switch (field.FieldName)
            {
                case "同模干扰电平":
                    value = point.Result.SameModInterferenceLevel;
                    break;
                case "不同模干扰电平":
                    value = point.Result.DifferentModInterferenceLevel;
                    break;
                case "总干扰电平":
                    value = point.Result.TotalInterferencePower;
                    break;
                case "信号RSRP":
                    value = point.Result.StrongestCell.ReceivedRsrp;
                    break;
                default:
                    value = point.Result.NominalSinr;
                    break;
            }
            if (field.IntervalList.Count > 0)
            {
                StatValueInterval interval = field.IntervalList.FirstOrDefault(
                    x => x.IntervalLowLevel < value && value < x.IntervalUpLevel);
                if (interval != null)
                {
                    interval.Color.ColorA = 128;
                    ColorString = interval.Color.ColorStringForHtml;
                    ColorStringForKml = interval.Color.ColorStringForKml;
                }
                else
                { 
                    ColorString = "ffffff";
                    ColorStringForKml = "80FFFFFF";
                }
            }
        }

        public string CoordinatesInfo
        {
            get
            {
                return X1 + "," + Y1 + "," + Height + " "
                    + X2 + "," + Y1 + "," + Height + " "
                    + X2 + "," + Y2 + "," + Height + " "
                    + X1 + "," + Y2 + "," + Height;
            }
        }
    }

    public class MeasureCellResult
    {
        public CsvCellResult CellResult { get; set; }

        public MeasurePlanCellRelation CellRelation { get; set; }

        public void GenerateMeasureCellRelation(IList<MeasurePoint> pointList, double trafficLoad)
        {
            if (pointList.Count > 0)
            {
                CellRelation =
                    pointList[0].GenerateMeasurePlanCellRelation(trafficLoad);
                for (int j = 1; j < pointList.Count; j++)
                {
                    CellRelation.ImportMeasurePoint(pointList[j]);
                }
            }
        }
    }

    public static class MeasureCellResultQueries
    {
        public static List<MeasureCellResult> GenerateMeasureResultList(
            this List<MeasurePoint> measurePointList, double trafficLoad)
        {
            List<MeasureCellResult> resultList = new List<MeasureCellResult>();
            List<IOutdoorCell> cellList
                = measurePointList.Select(x => x.Result.StrongestCell.Cell.Cell).Distinct().ToList();

            for (int i = 0; i < cellList.Count; i++)
            {
                IEnumerable<MeasurePoint> measurePoints = measurePointList.Where(
                    x => x.Result.StrongestCell.Cell.Cell == cellList[i]);
                List<MeasurePoint> pointList = measurePoints.ToList();
                resultList.Add(new MeasureCellResult());
                resultList[i].CellResult = new CsvCellResult(measurePoints);
                resultList[i].GenerateMeasureCellRelation(pointList, trafficLoad);
            }

            return resultList;
        }
    }
}
