using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Parameters.Service.Cdma
{
    public class ByExcelInfoSaveBtsListService
    {
        private readonly ParametersDumpInfrastructure _infrastructure;
        private readonly ITownRepository _townRepository;
        private readonly IENodebRepository _lteRepository;
        private readonly ENodebBaseRepository _baseRepository;
        private readonly IBtsRepository _repository;

        public ByExcelInfoSaveBtsListService(IBtsRepository repository,
            ParametersDumpInfrastructure infrastructure, ITownRepository townRepository,
            IENodebRepository lteRepository = null)
        {
            _infrastructure = infrastructure;
            _townRepository = townRepository;
            _lteRepository = lteRepository;
            _repository = repository;
            _baseRepository = new ENodebBaseRepository(repository);
        }

        public void Save(IEnumerable<BtsExcel> btsInfoList, bool updateBts)
        {
            IEnumerable<Town> townList = _townRepository.GetAllList();
            List<ENodeb> eNodebList = (_lteRepository == null) ? null : _lteRepository.GetAllList();
            TownIdAssignedSaveOneBtsService service = new TownIdAssignedSaveOneBtsService(
                _repository, _baseRepository, 0, eNodebList);

            foreach (BtsExcel btsExcel in btsInfoList.Distinct(new BtsExcelComparer()))
            {
                var town = townList.FirstOrDefault(x => x.DistrictName == btsExcel.DistrictName
                                                        && x.TownName == btsExcel.TownName);
                var townId = (town == null) ? -1 : town.Id;
                service.TownId = townId;
                if (service.SaveOneBts(btsExcel, updateBts))
                {
                    _infrastructure.CdmaBtsUpdated++;
                }
            }
        }
    }
}
