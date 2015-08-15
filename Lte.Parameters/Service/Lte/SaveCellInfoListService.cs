using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;

namespace Lte.Parameters.Service.Lte
{
    public abstract class SaveCellInfoListService
    {
        protected readonly ICellRepository _repository;

        protected SaveCellInfoListService(ICellRepository repository)
        {
            _repository = repository;
        }

        public abstract void Save(IEnumerable<CellExcel> cellInfoList, ParametersDumpInfrastructure infrastructure);
    }

    public class QuickSaveCellInfoListService : SaveCellInfoListService
    {
        private readonly CellBaseRepository _baseRepository;
        private readonly ENodebBaseRepository _baseENodebRepository;

        public QuickSaveCellInfoListService(ICellRepository repository,
            IENodebRepository eNodebRepository)
            : base(repository)
        {
            _baseRepository = new CellBaseRepository(repository);
            _baseENodebRepository = new ENodebBaseRepository(eNodebRepository);
        }

        public override void Save(IEnumerable<CellExcel> cellInfoList, ParametersDumpInfrastructure infrastructure)
        {
            infrastructure.CellsInserted = 0;
            foreach (CellExcel info in cellInfoList)
            {
                ByENodebBaseQuickSaveOneCellService service = 
                    new ByENodebBaseQuickSaveOneCellService(_repository, _baseRepository, info, _baseENodebRepository);
                if (service.Save())
                {
                    _baseRepository.ImportNewCellInfo(info);
                    infrastructure.CellsInserted++;
                }
            }
        }
    }

    public class UpdateConsideredSaveCellInfoListService : SaveCellInfoListService
    {
        private readonly bool _updateExisted;
        private readonly bool _updatePci;
        private readonly IENodebRepository _eNodebRepository;

        public UpdateConsideredSaveCellInfoListService(ICellRepository repository,
            IENodebRepository eNodebRepository,
            bool updateExisted = false, bool updatePci = false)
            : base(repository)
        {
            _updateExisted = updateExisted;
            _updatePci = updatePci;
            _eNodebRepository = eNodebRepository;
        }

        public override void Save(IEnumerable<CellExcel> cellInfoList, ParametersDumpInfrastructure infrastructure)
        {
            IEnumerable<CellExcel> distinctInfos
                = cellInfoList.Distinct(p => new { p.ENodebId, p.SectorId, p.Frequency });
            IEnumerable<CellExcel> _validInfos
                = from d in distinctInfos
                  join e in _eNodebRepository.GetAll()
                  on d.ENodebId equals e.ENodebId
                  select d;
            var updateCells
                = (from v in _validInfos
                  join c in _repository.GetAll()
                  on new { v.ENodebId, v.SectorId, v.Frequency }
                  equals new { c.ENodebId, c.SectorId, c.Frequency }
                  select new { Info = v, Data = c }).ToList();
            infrastructure.CellsUpdated = 0;
            infrastructure.NeighborPciUpdated = 0;
            if (_updateExisted)
            {
                foreach (var cell in updateCells.Where(x=>x.Data.Pci!=x.Info.Pci))
                {
                    cell.Info.CloneProperties(cell.Data);
                    _repository.Update(cell.Data);
                    infrastructure.CellsUpdated++;
                }

                if (_updatePci)
                    infrastructure.NeighborPciUpdated = SaveLteCellRelationService.UpdateNeighborPci(_validInfos);
            }
            IEnumerable<Cell> insertInfos = _validInfos.Except(updateCells.Select(x => x.Info)).Select(x =>
            {
                Cell cell = new Cell();
                cell.Import(x);
                return cell;
            }).ToList();
            _repository.AddCells(insertInfos);
            infrastructure.CellsInserted = insertInfos.Count();
        }
    }
}
