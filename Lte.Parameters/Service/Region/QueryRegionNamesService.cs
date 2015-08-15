using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public abstract class QueryRegionNamesService : QueryNamesService
    {
        protected readonly IEnumerable<OptimizeRegion> _regions;

        protected QueryRegionNamesService(IEnumerable<OptimizeRegion> regions)
        {
            _regions = regions;
        }
    }

    public class QueryRegionCityNamesService : QueryRegionNamesService
    {
        public QueryRegionCityNamesService(IEnumerable<OptimizeRegion> regions)
            : base(regions)
        {
        }

        public override IEnumerable<string> Query()
        {
            return _regions.Select(x => x.City).Distinct();
        }
    }

    public class ByCityDistrictQueryRegionCityNamesService : QueryRegionNamesService
    {
        private readonly string _cityName;
        private readonly string _districtName;

        public ByCityDistrictQueryRegionCityNamesService(IEnumerable<OptimizeRegion> regions,
            string cityName, string districtName) 
            : base(regions)
        {
            _cityName = cityName;
            _districtName = districtName;
        }

        public override IEnumerable<string> Query()
        {
            return _regions.Where(x => x.City == _cityName && x.District == _districtName).Select(
                x => x.Region).Distinct();
        }
    }

    public class QueryOptimizeRegionNamesService : QueryRegionNamesService
    {
        public QueryOptimizeRegionNamesService(IEnumerable<OptimizeRegion> regions)
            : base(regions)
        {
        }

        public override IEnumerable<string> Query()
        {
            return _regions.Select(x => x.Region).Distinct();
        }
    }
}