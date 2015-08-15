using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;
using Lte.Parameters.Service.Region;

namespace Lte.WebApp.Controllers.Parameters
{
    public class DistrictListController : ApiController
    {
        private readonly ITownRepository townRepository;

        public DistrictListController(ITownRepository townRepository)
        {
            this.townRepository = townRepository;
        }

        [Route("api/DistrictList/{cityName}")]
        public IEnumerable<string> GetDistrictListByCityName(string cityName)
        {
            QueryNamesService service = new QueryDistinctDistrictNamesService(
                townRepository.GetAll(), cityName);
            return service.Query().ToList();
        }
    }

    public class TownListController : ApiController
    {
        private readonly ITownRepository townRepository;

        public TownListController(ITownRepository townRepository)
        {
            this.townRepository = townRepository;
        }

        [Route("api/TownList/{cityName}/{districtName}")]
        public IEnumerable<string> GetTownListByCityAndDistrictName(string cityName, string districtName)
        {
            QueryNamesService service = new QueryDistinctTownNamesService(
                townRepository.GetAll(), cityName, districtName);
            return service.Query().ToList();
        }
    }

    public class RegionNameController : ApiController
    {
        private readonly IRegionRepository regionRepository;

        public RegionNameController(IRegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        [Route("api/RegionName/{cityName}/{districtName}")]
        public string GetName(string cityName, string districtName)
        {
            QueryRegionService service = new ByDistrictQueryRegionService(
                regionRepository.GetAll(), cityName, districtName);
            OptimizeRegion region = service.Query();
            return (region == null) ? "" : region.Region;
        }
    }

    public class QueryENodebController : ApiController
    {
        [Route("api/QueryENodeb")]
        public IEnumerable<int> GetIds()
        {
            return (ParametersContainer.QueryENodebs == null)?new List<int>():
                ParametersContainer.QueryENodebs.Select(x => x.ENodebId).Distinct();
        }
    }

    public class QueryENodebsController : ApiController
    {
        [Route("api/QueryENodebs")]
        public IEnumerable<ENodeb> Get()
        {
            return ParametersContainer.QueryENodebs;
        }
    }

    public class QueryBtssController : ApiController
    {
        [Route("api/QueryBtss")]
        public IEnumerable<CdmaBts> Get()
        {
            return ParametersContainer.QueryBtss;
        }
    }

    public class QueryLteSectorsController : ApiController
    {
        private readonly IENodebRepository eNodebRepository;

        public QueryLteSectorsController(IENodebRepository eNodebRepository)
        {
            this.eNodebRepository = eNodebRepository;
        }

        [Route("api/QueryLteSectors")]
        public IEnumerable<SectorTriangle> Get()
        {
            if (ParametersContainer.QueryCells == null) return new List<SectorTriangle>();
            IEnumerable<EvaluationOutdoorCell> outdoorCells =
                from cell in ParametersContainer.QueryCells
                let eNodeb = eNodebRepository.GetAll().FirstOrDefault(x => x.ENodebId == cell.ENodebId)
                where eNodeb != null
                select new EvaluationOutdoorCell(eNodeb, cell);
            return outdoorCells.GetSectors();
        }
    }

    public class QueryCdmaSectorsController : ApiController
    {
        private readonly IBtsRepository btsRepository;

        public QueryCdmaSectorsController(IBtsRepository btsRepository)
        {
            this.btsRepository = btsRepository;
        }

        [Route("api/QueryCdmaSectors")]
        public IEnumerable<SectorTriangle> Get()
        {
            if (ParametersContainer.QueryCdmaCells == null) return new List<SectorTriangle>();
            IEnumerable<EvaluationOutdoorCell> outdoorCells =
                from cell in ParametersContainer.QueryCdmaCells
                let bts = btsRepository.GetAll().FirstOrDefault(x => x.BtsId == cell.BtsId)
                where bts != null
                select new EvaluationOutdoorCell(bts, cell);
            return outdoorCells.GetSectors();
        }
    }

    public class QueryLteDistributionsController : ApiController
    {
        [Route("api/QueryLteDistributions")]
        public IEnumerable<IndoorDistribution> Get()
        {
            return ParametersContainer.QueryLteDistributions;
        }
    }

    public class QueryCdmaDistributionsController : ApiController
    {
        [Route("api/QueryCdmaDistributions")]
        public IEnumerable<IndoorDistribution> Get()
        {
            return ParametersContainer.QueryCdmaDistributions;
        }
    }

    public class SectorListController : ApiController
    {
        private readonly IENodebRepository eNodebRepository;
        private readonly ICellRepository cellRepository;

        public SectorListController(IENodebRepository eNodebRepository, ICellRepository cellRepository)
        {
            this.eNodebRepository = eNodebRepository;
            this.cellRepository = cellRepository;
        }

        [Route("api/SectorList/{id}")]
        public IEnumerable<SectorTriangle> Get(int id)
        {
            ENodeb eNodeb = eNodebRepository.GetAll().FirstOrDefault(x => x.ENodebId == id);
            IQueryOutdoorCellsService<EvaluationOutdoorCell> service 
                = new ByENodebQueryOutdoorCellService<EvaluationOutdoorCell>(
                cellRepository, eNodeb, (c,e)=>new EvaluationOutdoorCell(c,e));
            List<EvaluationOutdoorCell> outdoorCells = service.Query();
            return outdoorCells.GetSectors();
        }
    }
}
