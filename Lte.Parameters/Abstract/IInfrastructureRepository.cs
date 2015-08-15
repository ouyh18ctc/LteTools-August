using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface IInfrastructureRepository
    {
        IQueryable<InfrastructureInfo> InfrastructureInfos { get; }

        void AddOneInfrastructure(InfrastructureInfo info);

        bool RemoveOneInfrastructure(InfrastructureInfo info);

        void SaveChanges();
    }

    public static class InfrastructureRepositoryQueries
    {
        public static IEnumerable<ENodeb> QueryCollegeENodebs(this IInfrastructureRepository repository,
            IENodebRepository eNodebRepository, CollegeInfo info)
        {
            IEnumerable<int> ids = repository.InfrastructureInfos.Where(x =>
                x.HotspotName == info.Name && x.InfrastructureType == InfrastructureType.ENodeb
                ).Select(x => x.InfrastructureId).ToList();
            return ids.Select(i => eNodebRepository.GetAll().FirstOrDefault(x => x.Id == i)
                ).Where(eNodeb => eNodeb != null).ToList();
        }

        public static IEnumerable<Cell> QueryCollegeCells(this IInfrastructureRepository repository,
            ICellRepository cellRepository, CollegeInfo info)
        {
            IEnumerable<int> ids = repository.InfrastructureInfos.Where(x =>
                x.HotspotName == info.Name && x.InfrastructureType == InfrastructureType.Cell
                ).Select(x => x.InfrastructureId).ToList();
            return ids.Select(cellRepository.Get).Where(cell => cell != null).ToList();
        }

        public static IEnumerable<CdmaBts> QueryCollegeBtss(this IInfrastructureRepository repository,
            IBtsRepository btsRepository, CollegeInfo info)
        {
            IEnumerable<int> ids = repository.InfrastructureInfos.Where(x =>
                x.HotspotName == info.Name && x.InfrastructureType == InfrastructureType.CdmaBts
                ).Select(x => x.InfrastructureId).ToList();
            return ids.Select(btsRepository.Get).Where(bts => bts != null).ToList();
        }

        public static IEnumerable<CdmaCell> QueryCollegeCdmaCells(this IInfrastructureRepository repository,
            ICdmaCellRepository cdmaCellRepository, CollegeInfo info)
        {
            IEnumerable<int> ids = repository.InfrastructureInfos.Where(x =>
                x.HotspotName == info.Name && x.InfrastructureType == InfrastructureType.CdmaCell
                ).Select(x => x.InfrastructureId).ToList();
            return ids.Select(cdmaCellRepository.Get).Where(cell => cell != null).ToList();
        }

        public static IEnumerable<IndoorDistribution> QueryCollegeLteDistributions(
            this IInfrastructureRepository repository,
            IIndoorDistributioinRepository indoorRepository, CollegeInfo info)
        {
            IEnumerable<int> ids = repository.InfrastructureInfos.Where(x =>
                x.HotspotName == info.Name && x.InfrastructureType == InfrastructureType.LteIndoor
                ).Select(x => x.InfrastructureId).ToList();
            return ids.Select(i => indoorRepository.IndoorDistributions.FirstOrDefault(x => x.Id == i)
                ).Where(distribution => distribution != null).ToList();
        }

        public static IEnumerable<IndoorDistribution> QueryCollegeCdmaDistributions(
            this IInfrastructureRepository repository,
            IIndoorDistributioinRepository indoorRepository, CollegeInfo info)
        {
            IEnumerable<int> ids = repository.InfrastructureInfos.Where(x =>
                x.HotspotName == info.Name && x.InfrastructureType == InfrastructureType.CdmaIndoor
                ).Select(x => x.InfrastructureId).ToList();
            return ids.Select(i => indoorRepository.IndoorDistributions.FirstOrDefault(x => x.Id == i)
                ).Where(distribution => distribution != null).ToList();
        }
    }

    public interface IIndoorDistributioinRepository
    {
        IQueryable<IndoorDistribution> IndoorDistributions { get; }

        IndoorDistribution AddOneDistribution(IndoorDistribution distributiion);

        bool RemoveOneDistribution(IndoorDistribution distribution);

        void SaveChanges();
    }
}
