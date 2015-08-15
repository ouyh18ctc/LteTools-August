using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Cdma
{
    public abstract class SaveOneCdmaCellService
    {
        protected CdmaCellExcel _cellInfo;
        protected ICdmaCellRepository _repository;

        protected SaveOneCdmaCellService(ICdmaCellRepository repository,
            CdmaCellExcel cellInfo)
        {
            _repository = repository;
            _cellInfo = cellInfo;
        }

        protected abstract bool SaveWhenBtsNotExisted();

        protected abstract bool BtsNotExisted();

        public bool Save()
        {
            if (BtsNotExisted())
            {
                return false;
            }
            return SaveWhenBtsNotExisted();
        }
    }

    public abstract class QuickSaveOneCdmaCellService : SaveOneCdmaCellService
    {
        protected CdmaCellBaseRepository _baseRepository;

        protected QuickSaveOneCdmaCellService(ICdmaCellRepository repository,
            CdmaCellBaseRepository baseRepository,
            CdmaCellExcel cellInfo)
            : base(repository, cellInfo)
        {
            _baseRepository = baseRepository;
        }

        protected override bool SaveWhenBtsNotExisted()
        {
            bool result = true;
            CdmaCellBase cellBase = _baseRepository.QueryCell(_cellInfo.BtsId, _cellInfo.SectorId, _cellInfo.CellType);

            if (cellBase == null)
            {
                CdmaCell cell = new CdmaCell();
                cell.Import(_cellInfo, true);
                _repository.Insert(cell);
            }
            else if (cellBase.Frequency < 0 || !cellBase.HasFrequency(_cellInfo.Frequency))
            {
                CdmaCell cell = _repository.Query(_cellInfo.BtsId, _cellInfo.SectorId, _cellInfo.CellType);
                if (cell != null) { cell.Import(_cellInfo, true); }
            }
            else { result = false; }
            return result;
        }
    }

    public class SimpleSaveOneCdmaCellService : SaveOneCdmaCellService
    {
        private readonly CdmaBts _bts;

        public SimpleSaveOneCdmaCellService(ICdmaCellRepository repository,
            CdmaCellExcel cellInfo, IBtsRepository btsRepository)
            : base(repository, cellInfo)
        {
            _bts = btsRepository.GetAll().FirstOrDefault(x => x.BtsId == cellInfo.BtsId);
        }

        protected override bool SaveWhenBtsNotExisted()
        {
            CdmaCell cell = _repository.Query(_cellInfo.BtsId, _cellInfo.SectorId, _cellInfo.CellType);
            bool addCell = false;
            if (cell == null)
            {
                cell = new CdmaCell();
                addCell = true;
            }
            cell.Import(_cellInfo, true);
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

        protected override bool BtsNotExisted()
        {
            return _bts == null;
        }
    }

    public class ByBtsQuickSaveOneCdmaCellService : QuickSaveOneCdmaCellService
    {
        private readonly CdmaBts _bts;

        public ByBtsQuickSaveOneCdmaCellService(ICdmaCellRepository repository,
            CdmaCellBaseRepository baseRepository,
            CdmaCellExcel cellInfo, IBtsRepository btsRepository)
            : base(repository, baseRepository, cellInfo)
        {
            _bts = btsRepository.GetAll().FirstOrDefault(x => x.BtsId == cellInfo.BtsId);
        }

        protected override bool BtsNotExisted()
        {
            return _bts == null;
        }
    }

    public class ByENodebQuickSaveOneCdmaCellService : QuickSaveOneCdmaCellService
    {
        private readonly ENodebBase _eNodeb;

        public ByENodebQuickSaveOneCdmaCellService(ICdmaCellRepository repository,
            CdmaCellBaseRepository baseRepository,
            CdmaCellExcel cellInfo, ENodebBaseRepository eNodebRepository)
            : base(repository, baseRepository, cellInfo)
        {
            _eNodeb = eNodebRepository.QueryENodeb(cellInfo.BtsId);
        }

        protected override bool BtsNotExisted()
        {
            return _eNodeb == null;
        }
    }
}
