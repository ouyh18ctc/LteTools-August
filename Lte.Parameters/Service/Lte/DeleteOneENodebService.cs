using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Service.Lte
{
    public class DeleteOneENodebService
    {
        private readonly IENodebRepository _repository;
        private readonly ENodeb _eNodeb;

        private DeleteOneENodebService(IENodebRepository repository)
        {
            _repository = repository;
        }

        public DeleteOneENodebService(IENodebRepository repository, int eNodebId)
            : this(repository)
        {
            _eNodeb = repository.GetAll().FirstOrDefault(x => x.ENodebId == eNodebId);
        }

        public DeleteOneENodebService(IENodebRepository repository, int townId, string eNodebName)
            : this(repository)
        {
            _eNodeb = repository.GetAll().FirstOrDefault(x => x.TownId == townId && x.Name == eNodebName);
        }

        public DeleteOneENodebService(IENodebRepository repository, ITownRepository townRepository,
            string cityName, string districtName, string townName, string eNodebName)
            : this(repository)
        {
            if (townRepository == null)
            {
                _eNodeb = null;
            }
            else
            {
                int townId = townRepository.GetAll().QueryId(cityName, districtName, townName);
                _eNodeb = repository.GetAll().FirstOrDefault(x => x.TownId == townId && x.Name == eNodebName);
            }
        }

        public bool Delete()
        {
            if (_eNodeb == null) return false;
            _repository.Delete(_eNodeb);
            return true;
        }
    }
}
