using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Service.Lte
{
    public class SaveENodebListService
    {
        private readonly IENodebRepository _repository;
        private readonly List<Town> _townList; 
        private readonly ENodebBaseRepository _baseRepository;
        private readonly ParametersDumpInfrastructure _infrastructure;

        private static Func<ENodebExcel, bool> infoFilter = x => x.ENodebId > 10000;

        public SaveENodebListService(IENodebRepository repository,
            ParametersDumpInfrastructure infrastructure, ITownRepository townRepository)
        {
            _repository = repository;
            _baseRepository = new ENodebBaseRepository(repository);
            _townList = townRepository.GetAllList();
            _infrastructure = infrastructure;
            _infrastructure.ENodebsUpdated = 0;
        }

        public static Func<ENodebExcel, bool> InfoFilter
        {
            set { infoFilter = value; }
        }

        public void Save(IEnumerable<ENodebExcel> eNodebInfoList, bool update)
        {
            IEnumerable<ENodebExcel> validInfos =
                eNodebInfoList.Where(x => infoFilter(x))
                .Distinct(new ENodebExcelComparer())
                .Distinct(new ENodebExcelNameComparer());

            foreach (ENodebExcel info in validInfos)
            {
                int townId = _townList.QueryId(info);
                ENodebBase existedENodebWithSameName = _baseRepository.QueryENodeb(townId, info.Name);
                ENodebBase existedENodebWithSameId = _baseRepository.QueryENodeb(info.ENodebId);
                if (existedENodebWithSameName == null && existedENodebWithSameId == null)
                {
                    ENodeb eNodeb = new ENodeb();
                    eNodeb.Import(info, townId);
                    _repository.Insert(eNodeb);
                    _infrastructure.ENodebInserted++;
                }
                if (!update) continue;
                if (existedENodebWithSameId != null)
                {
                    ENodeb byIdENodeb = _repository.GetAll().FirstOrDefault(x => x.ENodebId == info.ENodebId);
                    if (byIdENodeb != null)
                    {
                        byIdENodeb.Import(info, townId, false);
                        _repository.Update(byIdENodeb);
                        _infrastructure.ENodebsUpdated++;
                    }
                }
                else if (existedENodebWithSameName != null)
                {
                    ENodeb byNameENodeb =
                        _repository.GetAll().FirstOrDefault(x => x.TownId == townId && x.Name == info.Name);
                    if (byNameENodeb != null)
                    {
                        byNameENodeb.Import(info, townId);
                        _repository.Update(byNameENodeb);
                        _infrastructure.ENodebsUpdated++;
                    }
                }
            }
        }
    }

}
