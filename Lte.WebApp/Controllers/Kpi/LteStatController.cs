using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.WebApp.Controllers.Kpi
{
    public class QueryLteStatController : ApiController
    {
        private readonly ITopCellRepository<TownPreciseCoverage4GStat> lteStatRepository;
        private readonly IEnumerable<Town> towns;

        public QueryLteStatController(ITopCellRepository<TownPreciseCoverage4GStat> lteStatRepository,
            ITownRepository townRepository)
        {
            this.lteStatRepository = lteStatRepository;
            towns = townRepository.GetAllList();
        }

        [Route("api/QueryLteStat/{begin}/{end}")]
        public int Get(DateTime begin, DateTime end)
        {
            DateTime beginDate = end.AddDays(-200);
            DateTime endDate = end.AddDays(1);
            KpiStatContainer.AllLteDailyStatList = new AllLteDailyStatList(towns)
            {
                Stats = new Dictionary<string, DailyStatList<RegionPrecise4GStat>>()
            };
            IEnumerable<TownPreciseCoverage4GStat> stats = lteStatRepository.Stats.Where(
                x => x.StatTime > beginDate && x.StatTime < endDate).ToList();
            IEnumerable<RegionPrecise4GStat> items =
                stats.Select(x => new RegionPrecise4GStat(new TownPrecise4GView(x, towns)));
            KpiStatContainer.AllLteDailyStatList.Import(items, begin, end);
            return KpiStatContainer.AllLteDailyStatList.Stats.Count;
        }
    }

    public class QueryLteDistrictDatesController : ApiController
    {
        [Route("api/QueryLteDistrictDates/{district}")]
        public IEnumerable<string> Get(string district)
        {
            return KpiStatContainer.AllLteDailyStatList.DateCategories(district);
        }

        [Route("api/QueryLteDistrictDates")]
        public IEnumerable<string> Get()
        {
            return KpiStatContainer.AllLteDailyStatList.OverallDates;
        }
    }

    public class QueryLteMrsController : ApiController
    {
        [Route("api/QueryLteMrs/{district}")]
        public IEnumerable<int> Get(string district)
        {
            return KpiStatContainer.AllLteDailyStatList.Stats[district].GetSummaryStats(x => x.TotalMrs).ToList();
        }

        [Route("api/QueryLteMrs/{district}/{town}")]
        public IEnumerable<int> Get(string district, string town)
        {
            return KpiStatContainer.AllLteDailyStatList.Stats[district].GetRegionStats(
                x => x.TotalMrs, x => x.Region, town);
        }
    }

    public class QueryLteMrTableController : ApiController
    {
        [Route("api/QueryLteMrTable/{district}")]
        public IEnumerable<int> Get(string district)
        {
            return KpiStatContainer.AllLteDailyStatList.GetDistrictMrs(district);
        }
    }

    public class QueryPreciseRatesController : ApiController
    {
        [Route("api/QueryPreciseRates/{district}")]
        public IEnumerable<double> Get(string district)
        {
            return KpiStatContainer.AllLteDailyStatList.Stats[district].GetSummaryStats(
                x => x.PreciseRate * 100).ToList();
        }

        [Route("api/QueryPreciseRates/{district}/{town}")]
        public IEnumerable<double> Get(string district, string town)
        {
            return KpiStatContainer.AllLteDailyStatList.Stats[district].GetRegionStats(
                x => x.PreciseRate * 100, x => x.Region, town);
        }
    }

    public class QueryPreciseRateTableController : ApiController
    {
        [Route("api/QueryPreciseRateTable/{district}")]
        public IEnumerable<double> Get(string district)
        {
            return KpiStatContainer.AllLteDailyStatList.GetDistrictRates(district);
        }
    }

    public class QueryLteKpiTownListController : ApiController
    {
        [Route("api/QueryLteKpiTownList/{district}")]
        public IEnumerable<string> Get(string district)
        {
            return KpiStatContainer.AllLteDailyStatList.Stats[district].Regions;
        }
    }

    public class QueryPreciseStatController : ApiController
    {
        private readonly ITopCellRepository<PreciseCoverage4G> _repository;

        public QueryPreciseStatController(ITopCellRepository<PreciseCoverage4G> repository)
        {
            _repository = repository;
        }

        [Route("api/QueryPreciseStat/{cellId}/{sectorId}/{date}")]
        public IEnumerable<PreciseCoverage4G> Get(int cellId, byte sectorId, DateTime date)
        {
            DateTime begin = date.AddDays(-7);
            DateTime end = date.AddDays(7);
            return _repository.Stats.Where(x =>
                x.StatTime >= begin && x.StatTime <= end
                && x.CellId == cellId && x.SectorId == sectorId).ToList();
        }

        public IEnumerable<PreciseCoverage4G> Get(int cellId, byte sectorId, DateTime begin, DateTime end)
        {
            return _repository.Stats.Where(x =>
                x.StatTime >= begin && x.StatTime <= end
                && x.CellId == cellId && x.SectorId == sectorId).ToList();
        }
    }
}