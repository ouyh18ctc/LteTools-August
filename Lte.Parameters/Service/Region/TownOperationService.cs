using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public class TownOperationService
    {
        private readonly ITownRepository _repository;
        private readonly string _city;
        private readonly string _district;
        private readonly string _town;

        public TownOperationService(ITownRepository townRepository,
            string cityName, string districtName, string townName)
        {
            _repository = townRepository;
            _city = cityName;
            _district = districtName;
            _town = townName;
        }

        public TownOperationService(ITownRepository townRepository, ITown town)
            : this(townRepository, town.CityName, town.DistrictName, town.TownName)
        {
        }

        public void SaveOneTown()
        {
            Town town = _repository.GetAllList().Query(_city, _district, _town);
            if (town != null) return;
            _repository.Insert(new Town
            {
                CityName = _city.Trim(),
                DistrictName = _district.Trim(),
                TownName = _town.Trim()
            });
        }

        public bool DeleteOneTown()
        {
            Town town = _repository.GetAllList().Query(_city.Trim(), _district.Trim(), _town.Trim());
            if (town == null) return false;
            _repository.Delete(town);
            return true;
        }

        public bool DeleteOneTown(IENodebRepository eNodebRepository, IBtsRepository btsRepository)
        {
            Town town = _repository.GetAllList().Query(_city.Trim(), _district.Trim(), _town.Trim());

            if (town == null) return false;
            ENodeb eNodeb
                = (eNodebRepository != null && eNodebRepository.GetAll() != null) ?
                    eNodebRepository.GetAll().FirstOrDefault(x => x.TownId == town.Id) :
                    null;
            CdmaBts bts
                = (btsRepository != null && btsRepository.GetAll() != null)
                    ? btsRepository.GetAll().FirstOrDefault(x => x.TownId == town.Id)
                    : null;
            if (eNodeb == null && bts == null)
            {
                _repository.Delete(town);
                return true;
            }
            return false;
        }
    }
}
