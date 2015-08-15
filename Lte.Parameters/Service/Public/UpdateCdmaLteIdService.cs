using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Public
{
    public class UpdateCdmaLteIdService
    {
        private readonly IEnumerable<BtsENodebIds> _eNodebIds;
        private readonly IBtsRepository _repository;

        private UpdateCdmaLteIdService(IBtsRepository repository)
        {
            _repository = repository;
        }

        public UpdateCdmaLteIdService(IBtsRepository repository,
            ICdmaCellRepository cellRepository, IEnumerable<CdmaLteIds> lteIds)
            : this(repository)
        {
            var cdmaLteInfos =
                (from a in repository.GetAllList()
                    join b in cellRepository.GetAllList()
                        on a.BtsId equals b.BtsId
                    where a.ENodebId == -1
                    select new {a.BtsId, b.CellId}).Distinct();
            _eNodebIds =
                from a in cdmaLteInfos
                join c in lteIds
                    on a.CellId equals c.CdmaCellId
                select new BtsENodebIds
                {
                    ENodebId = c.ENodebId,
                    BtsId = a.BtsId
                };
        }

        public UpdateCdmaLteIdService(IBtsRepository repository,
            IEnumerable<BtsENodebIds> btsENodebIds)
            : this(repository)
        {
            _eNodebIds = btsENodebIds;
        }

        public void Update()
        {
            foreach (BtsENodebIds idDefinition in _eNodebIds)
            {
                var id = idDefinition.BtsId;
                CdmaBts bts = _repository.GetAll().FirstOrDefault(x => x.BtsId == id);
                if (bts == null) continue;
                bts.ENodebId = idDefinition.ENodebId;
                _repository.Update(bts);
            }
        }
    }
}
