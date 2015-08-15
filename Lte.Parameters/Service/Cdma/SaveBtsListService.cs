using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Parameters.Service.Cdma
{
    public abstract class SaveBtsListService
    {
        protected readonly IBtsRepository _repository;

        protected SaveBtsListService(IBtsRepository repository)
        {
            _repository = repository;
        }

        public abstract void Save(ParametersDumpInfrastructure infrastructure);
    }

    public class ByDbInfoSaveBtsListService : SaveBtsListService
    {
        private readonly IEnumerable<CdmaBts> _btsInfoList;

        public ByDbInfoSaveBtsListService(IBtsRepository repository,
            IEnumerable<CdmaBts> btsInfoList)
            : base(repository)
        {
            _btsInfoList = btsInfoList;
        }

        public override void Save(ParametersDumpInfrastructure infrastructure)
        {
            using (var baseRepository = new ENodebBaseRepository(_repository))
            {
                foreach (var cdmaBts in from cdmaBts in _btsInfoList 
                                        let bts = baseRepository.QueryENodeb(cdmaBts.BtsId) 
                                        where bts == null select cdmaBts)
                {
                    _repository.Insert(cdmaBts);
                }
            }
            infrastructure.CdmaBtsUpdated = 0;
        }
    }

}
