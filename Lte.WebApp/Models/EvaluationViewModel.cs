using System.ComponentModel.DataAnnotations;
using Lte.Evaluations.ViewHelpers;

namespace Lte.WebApp.Models
{
    public class EvaluationViewModel : ENodebQueryViewModel
    {
        [Display(Name = "PDSCH/PDCCH负荷（%）"),
        Range(0, 100, ErrorMessage = "输入参数超出范围！必须在0到100之间"), UIHint("Number"), Required]
        public double TrafficLoad { get; set; }

        [Display(Name = "仿真栅格边长（米）"),
        Range(20, 1000, ErrorMessage = "输入参数超出范围！必须在20到1000之间"), UIHint("Number"), Required]
        public double DistanceInMeter { get; set; }

        [Display(Name = "最大小区覆盖距离（米）"),
        Range(3000, 20000, ErrorMessage = "输入参数超出范围！必须在3000到20000之间"), UIHint("Number"), Required]
        public double CellCoverage { get; set; }

        public PagingInfo PagingInfo { get; set; }

    }

}
