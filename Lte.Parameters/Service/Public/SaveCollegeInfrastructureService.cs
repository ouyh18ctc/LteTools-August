using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Public
{
    public class SaveCollegeInfrastructureService
    {
        private readonly IInfrastructureRepository _repository;

        public SaveCollegeInfrastructureService(IInfrastructureRepository repository)
        {
            _repository = repository;
        }

        public int SaveENodebs(IEnumerable<CollegeENodebExcel> eNodebExcels, IENodebRepository eNodebRepository)
        {
            int count = 0;
            foreach (CollegeENodebExcel excel in eNodebExcels)
            {
                ENodeb eNodeb = eNodebRepository.GetAll().FirstOrDefault(x => x.ENodebId == excel.ENodebId);
                if (eNodeb==null) continue;
                InfrastructureInfo infrastructure = _repository.InfrastructureInfos.FirstOrDefault(x =>
                    x.HotspotName == excel.CollegeName && x.HotspotType == HotspotType.College
                    && x.InfrastructureType == InfrastructureType.ENodeb && x.InfrastructureId == eNodeb.Id);
                if (infrastructure == null)
                {
                    infrastructure = new InfrastructureInfo
                    {
                        HotspotName = excel.CollegeName,
                        HotspotType = HotspotType.College,
                        InfrastructureType = InfrastructureType.ENodeb,
                        InfrastructureId = eNodeb.Id
                    };
                    _repository.AddOneInfrastructure(infrastructure);
                    _repository.SaveChanges();
                }
                count++;
            }
            return count;
        }

        public int SaveBtss(IEnumerable<CollegeBtsExcel> btsExcels, IBtsRepository btsRepository)
        {
            int count = 0;
            foreach (CollegeBtsExcel excel in btsExcels)
            {
                CdmaBts bts = btsRepository.GetAll().FirstOrDefault(x => x.BtsId == excel.BtsId);
                if (bts==null) continue;
                InfrastructureInfo infrastructure = _repository.InfrastructureInfos.FirstOrDefault(x =>
                    x.HotspotName == excel.CollegeName && x.HotspotType == HotspotType.College
                    && x.InfrastructureType == InfrastructureType.CdmaBts && x.InfrastructureId == bts.Id);
                if (infrastructure == null)
                {
                    infrastructure = new InfrastructureInfo
                    {
                        HotspotName = excel.CollegeName,
                        HotspotType = HotspotType.College,
                        InfrastructureType = InfrastructureType.CdmaBts,
                        InfrastructureId = bts.Id
                    };
                    _repository.AddOneInfrastructure(infrastructure);
                    _repository.SaveChanges();
                }
                count++;
            }
            return count;
        }

        public int SaveCells(IEnumerable<CollegeCellExcel> cellExcels, ICellRepository cellRepository)
        {
            int count = 0;
            foreach (CollegeCellExcel excel in cellExcels)
            {
                Cell cell =
                    cellRepository.GetAll().FirstOrDefault(
                        x => x.ENodebId == excel.ENodebId && x.SectorId == excel.SectorId);
                if (cell==null) continue;
                InfrastructureInfo infrastructure = _repository.InfrastructureInfos.FirstOrDefault(x =>
                    x.HotspotName == excel.CollegeName && x.HotspotType == HotspotType.College
                    && x.InfrastructureType == InfrastructureType.Cell && x.InfrastructureId == cell.Id);
                if (infrastructure == null)
                {
                    infrastructure = new InfrastructureInfo
                    {
                        HotspotName = excel.CollegeName,
                        HotspotType = HotspotType.College,
                        InfrastructureType = InfrastructureType.Cell,
                        InfrastructureId = cell.Id
                    };
                    _repository.AddOneInfrastructure(infrastructure);
                    _repository.SaveChanges();
                }
                count++;
            }
            return count;
        }

        public int SaveCdmaCells(IEnumerable<CollegeCdmaCellExcel> cellExcels, ICdmaCellRepository cellRepository)
        {
            int count = 0;
            foreach (CollegeCdmaCellExcel excel in cellExcels)
            {
                CdmaCell cell =
                    cellRepository.GetAll().FirstOrDefault(
                        x => x.BtsId == excel.BtsId && x.SectorId == excel.SectorId);
                if (cell == null) continue;
                InfrastructureInfo infrastructure = _repository.InfrastructureInfos.FirstOrDefault(x =>
                    x.HotspotName == excel.CollegeName && x.HotspotType == HotspotType.College
                    && x.InfrastructureType == InfrastructureType.CdmaCell && x.InfrastructureId == cell.Id);
                if (infrastructure == null)
                {
                    infrastructure = new InfrastructureInfo
                    {
                        HotspotName = excel.CollegeName,
                        HotspotType = HotspotType.College,
                        InfrastructureType = InfrastructureType.CdmaCell,
                        InfrastructureId = cell.Id
                    };
                    _repository.AddOneInfrastructure(infrastructure);
                    _repository.SaveChanges();
                }
                count++;
            }
            return count;
        }

        private int SaveIndoorDistributions(IEnumerable<CollegeIndoorExcel> indoorExcels,
            IIndoorDistributioinRepository distributioinRepository, InfrastructureType type)
        {
            int count = 0;
            foreach (CollegeIndoorExcel excel in indoorExcels)
            {
                IndoorDistribution distribution = distributioinRepository.IndoorDistributions.FirstOrDefault(x =>
                    x.Name == excel.Name && x.Range == excel.Range && x.SourceName == excel.SourceName);
                if (distribution == null)
                {
                    distribution = new IndoorDistribution
                    {
                        Name = excel.Name,
                        Range = excel.Range,
                        SourceName = excel.SourceName,
                        SourceType = excel.SourceType,
                        Longtitute = excel.Longtitute,
                        Lattitute = excel.Lattitute
                    };
                    distribution = distributioinRepository.AddOneDistribution(distribution);
                    distributioinRepository.SaveChanges();
                }
                InfrastructureInfo infrastructure = _repository.InfrastructureInfos.FirstOrDefault(x =>
                    x.HotspotName == excel.CollegeName && x.HotspotType == HotspotType.College
                    && x.InfrastructureType == type && x.InfrastructureId == distribution.Id);
                if (infrastructure == null)
                {
                    infrastructure = new InfrastructureInfo
                    {
                        HotspotName = excel.CollegeName,
                        HotspotType = HotspotType.College,
                        InfrastructureType = type,
                        InfrastructureId = distribution.Id
                    };
                    _repository.AddOneInfrastructure(infrastructure);
                    _repository.SaveChanges();
                }
                count++;
            }
            return count;
        }

        public int SaveLteIndoorDistributions(IEnumerable<CollegeIndoorExcel> indoorExcels,
            IIndoorDistributioinRepository distributioinRepository)
        {
            return SaveIndoorDistributions(indoorExcels, distributioinRepository, InfrastructureType.LteIndoor);
        }

        public int SaveCdmaIndoorDistributions(IEnumerable<CollegeIndoorExcel> indoorExcels,
            IIndoorDistributioinRepository distributioinRepository)
        {
            return SaveIndoorDistributions(indoorExcels, distributioinRepository, InfrastructureType.CdmaIndoor);
        }
    }
}
