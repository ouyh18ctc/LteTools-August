using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Service.Cdma
{
    public abstract class SaveCdmaCellInfoListService
    {
        protected readonly ICdmaCellRepository _repository;
        protected readonly IEnumerable<CdmaCellExcel> _cellInfoList;

        protected SaveCdmaCellInfoListService(ICdmaCellRepository repository,
            IEnumerable<CdmaCellExcel> cellInfoList)
        {
            _repository = repository;
            _cellInfoList = cellInfoList;
        }

        public abstract void Save(ParametersDumpInfrastructure infrastructure);
    }

    public class SimpleSaveCdmaCellInfoListService : SaveCdmaCellInfoListService
    {
        private readonly ENodebBaseRepository _btsBaseRepository;

        public SimpleSaveCdmaCellInfoListService(ICdmaCellRepository repository,
            IEnumerable<CdmaCellExcel> cellInfoList, IBtsRepository btsRepository)
            : base(repository, cellInfoList)
        {
            _btsBaseRepository = new ENodebBaseRepository(btsRepository);
        }

        public override void Save(ParametersDumpInfrastructure infrastructure)
        {
            infrastructure.CdmaCellsInserted = 0;

            using (CdmaCellBaseRepository baseRepository = new CdmaCellBaseRepository(_repository))
            {
                foreach (CdmaCellExcel cellInfo in _cellInfoList)
                {
                    SaveOneCdmaCellService service = new ByENodebQuickSaveOneCdmaCellService(
                        _repository, baseRepository, cellInfo, _btsBaseRepository);
                    if (service.Save())
                    {
                        baseRepository.ImportNewCellInfo(cellInfo);
                        infrastructure.CdmaCellsInserted++;
                    }
                }
            }
        }
    }

    public class UpdateConsideredSaveCdmaCellInfoListService : SaveCdmaCellInfoListService
    {
        private readonly bool _updateExisted;
        private readonly IEnumerable<CdmaCellExcel> _validInfos;

        public UpdateConsideredSaveCdmaCellInfoListService(ICdmaCellRepository repository,
            IEnumerable<CdmaCellExcel> cellInfoList, IBtsRepository btsRepository,
            bool updateExisted = false)
            : base(repository, cellInfoList)
        {
            _updateExisted = updateExisted;
            IEnumerable<CdmaCellExcel> distinctInfos = cellInfoList.Distinct(
                p => new { p.BtsId, p.SectorId, p.Frequency });

            _validInfos
                = from d in distinctInfos
                  join e in btsRepository.GetAll()
                  on d.BtsId equals e.BtsId
                  select d;
        }

        public override void Save(ParametersDumpInfrastructure infrastructure)
        {
            CdmaCell.UpdateFirstFrequency = true;
            List<CdmaCell> validCells = new List<CdmaCell>();
            foreach (CdmaCellExcel info in _validInfos)
            {
                CdmaCell cell = validCells.FirstOrDefault(x =>
                    x.BtsId == info.BtsId && x.SectorId == info.SectorId && x.CellType == info.CellType);
                if (cell == null)
                {
                    cell = new CdmaCell();
                    validCells.Add(cell);
                }
                cell.Import(info, true);
            }
            IEnumerable<CdmaCell> updateCells
                = from v in validCells
                  join c in _repository.GetAllList()
                  on new { v.BtsId, v.SectorId, v.CellType }
                  equals new { c.BtsId, c.SectorId, c.CellType }
                  select v;
            infrastructure.CdmaCellsInserted = 0;
            infrastructure.CdmaCellsUpdated = 0;
            if (_updateExisted)
            {
                foreach (CdmaCell cell in updateCells)
                {
                    CdmaCell objectCell = _repository.GetAll().FirstOrDefault(
                        x => x.BtsId == cell.BtsId && x.SectorId == cell.SectorId && x.CellType == cell.CellType);
                    if (objectCell == null) continue;
                    cell.CloneProperties<CdmaCell>(objectCell);
                    _repository.Update(cell);
                    infrastructure.CdmaCellsUpdated++;
                }
            }
            IEnumerable<CdmaCell> insertCells = validCells.Except(updateCells, new CdmaCellComperer());
            SaveCdmaCellListService service = new SimpleSaveCdmaCellListService(
                _repository, insertCells);
            infrastructure.CdmaCellsInserted += insertCells.Count();
            service.Save(infrastructure);
        }
    }
}
