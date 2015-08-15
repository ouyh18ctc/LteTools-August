using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Controllers.Parameters
{
    public class ENodebController : ApiController
    {
        private readonly IENodebRepository _repository;

        public ENodebController(IENodebRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ENodeb> Get()
        {
            return _repository.GetAllList();
        }

        [Route("api/ENodeb/{id:int}")]
        public ENodeb Get(int id)
        {
            return _repository.GetAll().FirstOrDefault(x => x.ENodebId == id);
        }

        public IEnumerable<ENodeb> Get(double west, double east, double south, double north)
        {
            return _repository.GetAll().Where(x =>
                x.Longtitute >= west && x.Longtitute <= east && x.Lattitute >= south && x.Lattitute <= north);
        }

        public void Put(ENodeb eNodeb)
        {
            _repository.Insert(eNodeb);
        }

        public void Post(ENodeb eNodeb)
        {
            ENodeb item = _repository.GetAll().FirstOrDefault(x => x.ENodebId == eNodeb.ENodebId);
            if (item != null)
            {
                item.Name = eNodeb.Name;
                item.Address = eNodeb.Address;
            }
        }

        public void Delete(int id)
        {
            ENodeb eNodeb = _repository.GetAll().FirstOrDefault(x => x.ENodebId == id);
            if (eNodeb != null)
            {
                _repository.Delete(eNodeb);
            }
        }
    }

    public class BtsController : ApiController
    {
        private readonly IBtsRepository _repository;

        public BtsController(IBtsRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CdmaBts> Get()
        {
            return _repository.GetAllList();
        }

        [Route("api/Bts/{id:int}")]
        public CdmaBts Get(int id)
        {
            return _repository.GetAll().FirstOrDefault(x => x.ENodebId == id);
        }

        public IEnumerable<CdmaBts> Get(double west, double east, double south, double north)
        {
            return _repository.GetAll().Where(x =>
                x.Longtitute >= west && x.Longtitute <= east && x.Lattitute >= south && x.Lattitute <= north);
        }

        public void Put(CdmaBts bts)
        {
            _repository.Insert(bts);
        }

        public void Post(CdmaBts bts)
        {
            CdmaBts item = _repository.GetAll().FirstOrDefault(x => x.ENodebId == bts.ENodebId);
            if (item != null)
            {
                item.Name = bts.Name;
                item.Address = bts.Address;
                _repository.Update(item);
            }
        }

        public void Delete(int id)
        {
            CdmaBts bts = _repository.GetAll().FirstOrDefault(x => x.ENodebId == id);
            if (bts != null)
            {
                _repository.Delete(bts);
            }
        }
    }

    public class CellController : ApiController
    {
        private readonly ICellRepository _repository;

        public CellController(ICellRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Cell> GetCells(double west, double east, double south, double north)
        {
            return _repository.GetAll().Where(x =>
                x.Longtitute >= west && x.Longtitute <= east && x.Lattitute >= south && x.Lattitute <= north);
        } 
    }

    public class CdmaCellController : ApiController
    {
        private readonly ICdmaCellRepository _repository;

        public CdmaCellController(ICdmaCellRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CdmaCell> GetCells(double west, double east, double south, double north)
        {
            return _repository.GetAll().Where(x =>
                x.Longtitute >= west && x.Longtitute <= east && x.Lattitute >= south && x.Lattitute <= north);
        } 
    }
}
