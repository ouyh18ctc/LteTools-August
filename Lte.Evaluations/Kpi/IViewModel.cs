using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;
using Lte.Parameters.Service;
using Lte.Parameters.Service.Public;

namespace Lte.Evaluations.Kpi
{
    public interface IStatDateViewModel
    {
        [Display(Name = "统计日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime StatDate { get; set; }
    }

    public interface IDateSpanViewModel
    {
        [Display(Name = "结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime EndDate { get; set; }

        [Display(Name = "开始日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime StartDate { get; set; }

    }

    public interface IDateSpanAndTopCountViewModel
    {
        [Display(Name = "结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime EndDate { get; set; }

        [Display(Name = "开始日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime StartDate { get; set; }

        [Display(Name = "显示的TOP载频数")]
        int TopCounts { get; set; }
    }

    public interface ITopViewModel<TView> : IStatDateViewModel, IDateSpanAndTopCountViewModel
    {
        IEnumerable<string> Cities { get; set; }

        IEnumerable<TView> StatList { get; set; }

    }

    public static class ITopViewModelQuereis
    {
        public static TViewModel GenerateView<TViewModel, TView>(
            this IEnumerable<TView> cellViews, DateTime endDate, IEnumerable<string> cities)
            where TViewModel : class, ITopViewModel<TView>, new()
        {
            return new TViewModel
            {
                StatList = cellViews,
                EndDate = endDate,
                StatDate = endDate,
                StartDate = endDate.AddDays(-7),
                TopCounts = 20,
                Cities = cities
            };
        }

        public static TViewModel GenerateView<TViewModel, TStat, TView>(
            this IEnumerable<TStat> stats, DateTime statDate, IEnumerable<string> cities,
            IEnumerable<CdmaBts> btss, IEnumerable<ENodeb> eNodebs)
            where TStat : class, ITimeStat, ICdmaCell, new()
            where TViewModel : class, ITopViewModel<TView>, new()
            where TView : class, ICdmaLteNames, new()
        {
            IEnumerable<TStat> lastDateCells
                = stats.GetLastDateStatsConsideringIllegalDate(statDate);
            CdmaLteNamesService<TStat> service = new CdmaLteNamesService<TStat>(lastDateCells,
                btss, eNodebs);
            DateTime endDate = (lastDateCells.Any()) ?
                lastDateCells.First().StatTime.Date : DateTime.Today.AddDays(-1);
            IEnumerable<TView> cellViews = service.Clone<TView>();
            return cellViews.GenerateView<TViewModel, TView>(
                endDate, cities);
        }
    }
}
