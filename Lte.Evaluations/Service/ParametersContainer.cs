using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Service.Public;

namespace Lte.Evaluations.Service
{
    public interface ICollegeController
    {
        IIndoorDistributioinRepository IndoorDistributioinRepository { get; }

        IENodebRepository ENodebRepository { get; }

        ICellRepository CellRepository { get; }

        IBtsRepository BtsRepository { get; }

        ICdmaCellRepository CdmaCellRepository { get; }

        IInfrastructureRepository InfrastructureRepository { get; }
    }

    public class ParametersContainer
    {
        public IEnumerable<TownENodebStat> TownENodebStats { get; private set; }

        public void ImportTownENodebStats(ITownRepository townRepository, IENodebRepository eNodebRepository,
            IRegionRepository regionRepository)
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.GetAllList();
            TownENodebStats = townRepository.GetAllList().Select(
                x => new TownENodebStat(x, eNodebs, regionRepository)).ToList();
        }

        public Dictionary<string, int> GetENodebsByDistrict(string cityName)
        {
            var result = from s in TownENodebStats.Where(x => x.CityName == cityName)
                         group s by s.DistrictName into g
                         select new { g.Key, Value = g.Sum(s => s.TotalENodebs) };
            return result.ToDictionary(r => r.Key, r => r.Value);
        }

        public Dictionary<string, int> GetENodebsByTown(string cityName, string districtName)
        {
            var result = from s in TownENodebStats.Where(x => x.CityName == cityName && x.DistrictName == districtName)
                         group s by s.TownName into g
                         select new { g.Key, Value = g.Sum(s => s.TotalENodebs) };
            return result.ToDictionary(r => r.Key, r => r.Value);
        }

        public static IEnumerable<ENodeb> QueryENodebs { get; set; }

        public static IEnumerable<Cell> QueryCells { get; private set; }

        public static IEnumerable<CdmaBts> QueryBtss { get; private set; }

        public static IEnumerable<CdmaCell> QueryCdmaCells { get; private set; }

        public static IEnumerable<IndoorDistribution> QueryLteDistributions { get; private set; }

        public static IEnumerable<IndoorDistribution> QueryCdmaDistributions { get; private set; }

        public static void UpdateCollegeInfos(ICollegeController controller, CollegeInfo info)
        {
            QueryENodebs = controller.InfrastructureRepository.QueryCollegeENodebs(
                controller.ENodebRepository, info);
            QueryCells = controller.InfrastructureRepository.QueryCollegeCells(
                controller.CellRepository, info);
            QueryBtss = controller.InfrastructureRepository.QueryCollegeBtss(
                controller.BtsRepository, info);
            QueryCdmaCells = controller.InfrastructureRepository.QueryCollegeCdmaCells(
                controller.CdmaCellRepository, info);
            QueryLteDistributions = controller.InfrastructureRepository.QueryCollegeLteDistributions(
                controller.IndoorDistributioinRepository, info);
            QueryCdmaDistributions = controller.InfrastructureRepository.QueryCollegeCdmaDistributions(
                controller.IndoorDistributioinRepository, info);
        }
    }

    public static class KpiStatContainer
    {
        public static AllCdmaDailyStatList AllCdmaDailyStatList { get; set; }

        public static AllLteDailyStatList AllLteDailyStatList { get; set; }
    }

    public static class RutraceStatContainer
    {
        public static List<MrsCellDateView> MrsStats { get; set; }

        public static IEnumerable<EvaluationOutdoorCell> QueryOutdoorCellsFromMrs(
            IENodebRepository _eNodebRepository, 
            ICellRepository _cellRepository)
        {
            IEnumerable<int> ids = MrsStats.Select(x => x.CellId).Distinct();
            IEnumerable<ENodeb> eNodebs = _eNodebRepository.GetAllWithIds(ids);
            return _cellRepository.Query(MrsStats, eNodebs);
        }
    }
}
