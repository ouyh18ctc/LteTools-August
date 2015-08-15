using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Service.Cdma
{
    public abstract class SaveOneBtsService
    {
        protected readonly IBtsRepository _repository;

        protected int _townId;

        protected SaveOneBtsService(IBtsRepository repository)
        {
            _repository = repository;
        }
    }

    public sealed class TownListConsideredSaveOneBtsService : SaveOneBtsService
    {
        private readonly ITownRepository _townRepository;

        public TownListConsideredSaveOneBtsService(IBtsRepository repository, ITownRepository townRepository)
            : base(repository)
        {
            _townRepository = townRepository;
        }

        public bool SaveOneBts(BtsExcel btsInfo, bool updateBts)
        {
            _townId = _townRepository.GetAll().QueryId(btsInfo.DistrictName, btsInfo.TownName);
            string name = btsInfo.Name;
            CdmaBts bts = _repository.QueryBts(_townId, name);
            CdmaBts existedENodebWithSameId = _repository.GetAll().FirstOrDefault(x => x.BtsId == btsInfo.BtsId);
            bool addENodeb = false;

            if (bts == null)
            {
                bts = existedENodebWithSameId;
                if (bts == null)
                {
                    bts = new CdmaBts();
                    addENodeb = true;
                }
            }
            else if (bts != existedENodebWithSameId) { return false; }

            if (addENodeb)
            {
                bts.TownId = _townId;
                bts.Import(btsInfo, true);
                _repository.Insert(bts);
            }
            else if (updateBts)
            {
                const double tolerance = 1E-6;
                if (Math.Abs(bts.Longtitute) < tolerance && Math.Abs(bts.Lattitute) < tolerance)
                {
                    bts.TownId = _townId;
                    bts.Import(btsInfo, false);
                    _repository.Update(bts);
                }
                else
                { return false; }
            }
            return true;
        }
    }

    public sealed class TownIdAssignedSaveOneBtsService : SaveOneBtsService
    {
        private readonly ENodebBaseRepository _baseRepository;
        private readonly List<ENodeb> _eNodebList;

        public TownIdAssignedSaveOneBtsService(IBtsRepository repository,
            ENodebBaseRepository baseRepository, int townId,
            List<ENodeb> eNodebList = null)
            : base(repository)
        {
            _baseRepository = baseRepository;
            _eNodebList = eNodebList;
            _townId = townId;
        }

        public int TownId
        {
            set { _townId = value; }
        }

        public bool SaveOneBts(BtsExcel btsInfo, bool updateBts)
        {
            ENodebBase eNodebBase = _baseRepository.QueryENodeb(btsInfo.BtsId);

            CdmaBts bts;
            if (eNodebBase == null)
            {
                bts = new CdmaBts();
                bts.TownId = _townId;
                bts.Import(btsInfo, true);
                bts.ImportLteInfo(_eNodebList);
                _repository.Insert(bts);
                return true;
            }
            if (!updateBts) return false;
            bts = _repository.GetAll().FirstOrDefault(x => x.BtsId == btsInfo.BtsId);
            if (bts != null)
            {
                bts.TownId = _townId;
                bts.Import(btsInfo, false);
                bts.ImportLteInfo(_eNodebList);
                _repository.Update(bts);
            }
            return true;
        }
    }
}
