using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public class RegionOperationService
    {
        private readonly IRegionRepository _repository;
        private readonly string _cityName;
        private readonly string _districtName;
        private readonly string _regionName;
        private readonly QueryRegionService _service;

        public RegionOperationService(IRegionRepository repository,
            string cityName, string districtName, string regionName)
        {
            _repository = repository;
            _cityName = cityName;
            _districtName = districtName;
            _regionName = regionName;
            _service = new ByRegionQueryRegionService(_repository.GetAll(),
                _cityName, _districtName, _regionName);
        }

        private bool InputIsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(_cityName) 
                    || string.IsNullOrEmpty(_districtName) || string.IsNullOrEmpty(_regionName);
            }
        }

        public bool SaveOneRegion(bool forceChangeExisted = false)
        {
            if (InputIsEmpty)
            {
                return false;
            }

            QueryRegionService districtService = new ByDistrictQueryRegionService(_repository.GetAll(),
                _cityName, _districtName);
            OptimizeRegion existedRegion = districtService.Query();
            if (existedRegion == null)
            {
                if (_service.Query() == null)
                {
                    _repository.Insert(new OptimizeRegion
                    {
                        City = _cityName,
                        District = _districtName,
                        Region = _regionName
                    });
                    return true;
                }
            }
            else if (_regionName != existedRegion.Region && forceChangeExisted)
            {
                existedRegion.Region = _regionName;
                _repository.Update(existedRegion);
                return true;
            }
            return false;
        }

        public bool DeleteOneRegion()
        {
            if (InputIsEmpty)
            {
                return false;
            }

            OptimizeRegion region = _service.Query();
            if (region == null)
            {
                return false;
            }
            _repository.Delete(region);
            return true;
        }
    }
}
