using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Controllers.Rutrace
{
    public class MrsQueryCoverageController : ApiController
    {
        [Route("api/MrsQueryCoverage/{topNum}")]
        public IEnumerable<MrsCellDateView> Get(int topNum)
        {
            return RutraceStatContainer.MrsStats.Take(topNum);
        }
    }

    public class MrsQueryTaController : ApiController
    {
        private readonly IMrsCellTaRepository _repository;

        public MrsQueryTaController(IMrsCellTaRepository repository)
        {
            _repository = repository;
        }

        [Route("api/MrsQueryTa/{cellId}/{sectorId}/{date}")]
        public MrsCellTa Get(int cellId, byte sectorId, DateTime date)
        {
            return _repository.GetAll().FirstOrDefault(x =>
                x.CellId == cellId && x.SectorId == sectorId && x.RecordDate == date);
        }
    }

    public class MroQueryRsrpTaController : ApiController
    {
        private readonly IMroCellRepository _repository;

        public MroQueryRsrpTaController(IMroCellRepository repository)
        {
            _repository = repository;
        }

        [Route("api/MroQueryRsrpTa/{cellId}/{sectorId}/{date}")]
        public IEnumerable<MroRsrpTa> Get(int cellId, byte sectorId, DateTime date)
        {
            return _repository.GetAll().Where(x =>
                x.CellId == cellId && x.SectorId == sectorId && x.RecordDate == date);
        }
    }
}
