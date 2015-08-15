using System.Collections.Generic;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Parameters.Service.Cdma
{
    public abstract class SaveCdmaCellListService
    {
        protected readonly ICdmaCellRepository _repository;
        protected readonly IEnumerable<CdmaCell> _cells;

        protected SaveCdmaCellListService(ICdmaCellRepository repository,
            IEnumerable<CdmaCell> cells)
        {
            _repository = repository;
            _cells = cells;
        }

        public abstract void Save(ParametersDumpInfrastructure infrastructure);
    }

    public class SimpleSaveCdmaCellListService : SaveCdmaCellListService
    {
        public SimpleSaveCdmaCellListService(ICdmaCellRepository repository,
            IEnumerable<CdmaCell> cells)
            : base(repository, cells)
        {
        }

        public override void Save(ParametersDumpInfrastructure infrastructure)
        {
            foreach (CdmaCell cell in _cells)
            {
                _repository.Insert(cell);
                infrastructure.CdmaCellsInserted++;
            }
        }
    }

    public class BtsConsideredSaveCdmaListService : SaveCdmaCellListService
    {
        private readonly ENodebBaseRepository _baseBtsRepository;

        public BtsConsideredSaveCdmaListService(ICdmaCellRepository repository,
            IEnumerable<CdmaCell> cells, IBtsRepository btsRepository)
            : base(repository, cells)
        {
            _baseBtsRepository = new ENodebBaseRepository(btsRepository);
        }

        public override void Save(ParametersDumpInfrastructure infrastructure)
        {
            using (CdmaCellBaseRepository baseRepository = new CdmaCellBaseRepository(_repository))
            {
                foreach (CdmaCell cell in _cells)
                {
                    if (_baseBtsRepository.QueryENodeb(cell.BtsId) != null
                        && baseRepository.QueryCell(
                            cell.BtsId, cell.SectorId, cell.CellType) == null)
                    {
                        _repository.Insert(cell);
                        infrastructure.CdmaCellsInserted++;
                    }
                }
            }
        }
    }
}
