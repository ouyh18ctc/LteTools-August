using System.Collections.Generic;
using System.Linq;
using Lte.Domain.LinqToCsv;
using Lte.Domain.Measure;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Entities
{
    public class CsvCellResult
    {
        [CsvColumn(FieldIndex = 2, OutputFormat = "0.000000", Name = "经度")]
        public double Longtitute
        { get; set; }

        [CsvColumn(FieldIndex = 3, OutputFormat = "0.000000", Name = "纬度")]
        public double Lattitute
        { get; set; }

        [CsvColumn(FieldIndex = 4, OutputFormat = "0.000000", Name = "平均接收RSRP")]
        public double StrongestCellRsrp
        { get; set; }

        [CsvColumn(FieldIndex = 5, OutputFormat = "0.000000", Name = "平均覆盖距离（米）")]
        public double StrongestCellDistanceInMeter
        { get; set; }

        [CsvColumn(FieldIndex = 1, Name = "小区名称")]
        public string StrongestCellName
        { get; set; }

        [CsvColumn(FieldIndex = 6, OutputFormat = "0.000000", Name = "总干扰电平")]
        public double TotalInterferencePower
        { get; set; }

        [CsvColumn(FieldIndex = 7, OutputFormat = "0.000000", Name = "标称SINR")]
        public double NominalSinr
        { get; set; }

        public CsvCellResult() { }

        public CsvCellResult(IEnumerable<MeasurePoint> pointList)
        { 
            Longtitute = pointList.ElementAt(0).Result.StrongestCell.Cell.Cell.Longtitute;
            Lattitute = pointList.ElementAt(0).Result.StrongestCell.Cell.Cell.Lattitute;
            StrongestCellName = pointList.ElementAt(0).Result.StrongestCell.CellName;
            StrongestCellRsrp = pointList.Select(x => x.Result.StrongestCell.ReceivedRsrp).SumOfPowerLevel(x => x);
            StrongestCellDistanceInMeter = pointList.Select(x => x.Result.StrongestCell.DistanceInMeter).Average();
            TotalInterferencePower = pointList.Select(x => x.Result.TotalInterferencePower).SumOfPowerLevel(x => x);
            NominalSinr = StrongestCellRsrp - TotalInterferencePower;
        }
    }
}
