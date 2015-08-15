using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Lte
{
    public abstract class SaveOneCellService
    {
        protected ICellRepository _repository;
        protected CellExcel _cellInfo;

        protected SaveOneCellService(ICellRepository repository, CellExcel cellInfo)
        {
            _repository = repository;
            _cellInfo = cellInfo;
        }

        protected abstract bool SaveWhenENodebNotExisted();

        protected abstract bool ENodebNotExisted();

        public bool Save()
        {
            if (ENodebNotExisted())
            {
                return false;
            }
            return SaveWhenENodebNotExisted();
        }
    }

    public abstract class QuickSaveOneCellService : SaveOneCellService
    {
        private readonly CellBaseRepository _baseRepository;
        private readonly bool _updateExisted;

        protected QuickSaveOneCellService(ICellRepository repository, CellExcel cellInfo,
            CellBaseRepository baseRepository, bool updateExisted = false)
            : base(repository, cellInfo)
        {
            _baseRepository = baseRepository;
            _updateExisted = updateExisted;
        }

        protected override bool SaveWhenENodebNotExisted()
        {
            CellBase cellBase = _baseRepository.QueryCell(_cellInfo.ENodebId, _cellInfo.SectorId);

            if (cellBase == null)
            {
                Cell cell = new Cell();
                cell.Import(_cellInfo);
                _repository.Insert(cell);
                return true;
            }
            if (_updateExisted)
            {
                Cell cell = _repository.GetAll().FirstOrDefault(x => 
                        x.ENodebId == _cellInfo.ENodebId && x.SectorId == _cellInfo.SectorId);
                if (cell == null) return false;
                cell.Import(_cellInfo);
                _repository.Update(cell);
                return true;
            }
            return false;
        }
    }

    public class SimpleSaveOneCellService : SaveOneCellService
    {
        private readonly ENodeb _eNodeb;

        public SimpleSaveOneCellService(ICellRepository repository,
            CellExcel cellInfo, IENodebRepository eNodebRepository)
            : base(repository, cellInfo)
        {
            _eNodeb = eNodebRepository.GetAll().FirstOrDefault(x => x.ENodebId == cellInfo.ENodebId);
        }

        protected override bool ENodebNotExisted()
        {
            return _eNodeb == null;
        }

        protected override bool SaveWhenENodebNotExisted()
        {
            Cell cell = _repository.GetAll().FirstOrDefault(x =>
                        x.ENodebId == _cellInfo.ENodebId && x.SectorId == _cellInfo.SectorId);
            bool addCell = false;
            if (cell == null)
            {
                cell = new Cell();
                addCell = true;
            }
            cell.Import(_cellInfo);
            if (addCell)
            {
                _repository.Insert(cell);
            }
            else
            {
                _repository.Update(cell);
            }
            return true;
        }
    }

    public class ByENodebQuickSaveOneCellService : QuickSaveOneCellService
    {
        private readonly ENodeb _eNodeb;

        public ByENodebQuickSaveOneCellService(ICellRepository repository, CellBaseRepository baseRepository,
            CellExcel cellInfo, IENodebRepository eNodebRepository, bool updateExisted = false)
            : base(repository, cellInfo, baseRepository, updateExisted)
        {
            _eNodeb = eNodebRepository.GetAll().FirstOrDefault(x => x.ENodebId == cellInfo.ENodebId);
        }

        protected override bool ENodebNotExisted()
        {
            return _eNodeb == null;
        }
    }

    public class ByENodebBaseQuickSaveOneCellService : QuickSaveOneCellService
    {
        private readonly ENodebBase _eNodeb;

        public ByENodebBaseQuickSaveOneCellService(ICellRepository repository, CellBaseRepository baseRepository,
            CellExcel cellInfo, ENodebBaseRepository eNodebRepository, bool updateExisted = false)
            : base(repository, cellInfo, baseRepository, updateExisted)
        {
            _eNodeb = eNodebRepository.QueryENodeb(cellInfo.ENodebId);
        }

        protected override bool ENodebNotExisted()
        {
            return _eNodeb == null;
        }
    }
}
