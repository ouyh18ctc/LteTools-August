using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public abstract class QueryNamesService
    {
        public abstract IEnumerable<string> Query();

        public int QueryCount()
        {
            return Query().Count();
        }
    }

    public abstract class QueryDistinctNamesService : QueryNamesService
    {
        protected readonly IEnumerable<Town> _towns;

        protected QueryDistinctNamesService(IEnumerable<Town> towns)
        {
            _towns = towns;
        }
    }

    public class QueryDistinctCityNamesService : QueryDistinctNamesService
    {
        public QueryDistinctCityNamesService(IEnumerable<Town> towns) : base(towns)
        {
        }

        public override IEnumerable<string> Query()
        {
            return _towns.Select(x => x.CityName).Distinct();
        }
    }

    public class QueryDistinctDistrictNamesService : QueryDistinctNamesService
    {
        private readonly string _cityName;

        public QueryDistinctDistrictNamesService(IEnumerable<Town> towns, string cityName) 
            : base(towns)
        {
            _cityName = cityName;
        }

        public QueryDistinctDistrictNamesService(IEnumerable<Town> towns)
            : base(towns)
        {
            _cityName = towns.Any() ? towns.First().CityName : "";
        }

        public override IEnumerable<string> Query()
        {
            return _towns.Where(x => x.CityName == _cityName).Select(x => x.DistrictName).Distinct();
        }
    }

    public class QueryDistinctTownNamesService : QueryDistinctNamesService
    {
        private readonly string _cityName;
        private readonly string _districtName;

        public QueryDistinctTownNamesService(IEnumerable<Town> towns, string cityName, string districtName) 
            : base(towns)
        {
            _cityName = cityName;
            _districtName = districtName;
        }

        public override IEnumerable<string> Query()
        {
            return _towns.Where(x => x.CityName == _cityName && x.DistrictName == _districtName).Select(
                x => x.TownName).Distinct();
        }
    }
}
