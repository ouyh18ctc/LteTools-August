using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public abstract class QueryRegionService
    {
        protected readonly IEnumerable<OptimizeRegion> _regions;

        public abstract OptimizeRegion Query();

        protected QueryRegionService(IEnumerable<OptimizeRegion> regions)
        {
            _regions = regions;
        }
    }

    public class ByDistrictQueryRegionService : QueryRegionService
    {
        private readonly string _cityName;
        private readonly string _districtName;

        public ByDistrictQueryRegionService(IEnumerable<OptimizeRegion> regions,
            string cityName, string districtName)
            : base(regions)
        {
            _cityName = cityName;
            _districtName = districtName;
        }

        public override OptimizeRegion Query()
        {
            return _regions.FirstOrDefault(
                x => x.City == _cityName && x.District == _districtName);
        }
    }

    public class ByRegionQueryRegionService : QueryRegionService
    {
        private readonly string _cityName;
        private readonly string _districtName;
        private readonly string _regionName;

        public ByRegionQueryRegionService(IEnumerable<OptimizeRegion> regions,
            string cityName, string districtName, string regionName) : base(regions)
        {
            _cityName = cityName;
            _districtName = districtName;
            _regionName = regionName;
        }

        public override OptimizeRegion Query()
        {
            return _regions.FirstOrDefault(
                x => x.City == _cityName && x.District == _districtName && x.Region == _regionName);
        }
    }
}
