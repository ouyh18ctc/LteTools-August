using System;
using System.Collections.Generic;
using System.Web.Http;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Controllers.Topic
{
    public class DrawCollegeRegionController : ApiController
    {
        private readonly ICollegeRepository _repository;

        public DrawCollegeRegionController(ICollegeRepository repository)
        {
            _repository = repository;
        }

        public CollegeRegion Get(int id, double area, string message)
        {
            CollegeInfo info = _repository.Get(id);
            if (info == null) return null;
            UpdateRegion(id, area, message, info, RegionType.Polygon);
            _repository.Update(info);
            return info.CollegeRegion;
        }

        private void UpdateRegion(int id, double area, string message, CollegeInfo info, RegionType type)
        {
            CollegeRegion region = _repository.GetRegion(id);
            if (region == null)
            {
                info.CollegeRegion = new CollegeRegion
                {
                    Area = area,
                    Info = message,
                    RegionType = type
                };
            }
            else
            {
                region.Area = area;
                region.Info = message;
                region.RegionType = type;
            }
        }

        public CollegeRegion Get(int id, double centerX, double centerY, double radius)
        {
            CollegeInfo info = _repository.Get(id);
            if (info == null) return null;
            double area = Math.PI*radius*radius;
            string message = centerX + ";" + centerY + ";" + radius;
            UpdateRegion(id, area, message, info, RegionType.Circle);
            _repository.Update(info);
            return info.CollegeRegion;
        }

        public CollegeRegion Get(int id, double x1, double y1, double x2, double y2, double area)
        {
            CollegeInfo info = _repository.Get(id);
            if (info == null) return null;
            string message = x1 + ";" + y1 + ";" + x2 + ";" + y2;
            UpdateRegion(id, area, message, info, RegionType.Rectangle);
            _repository.Update(info);
            return info.CollegeRegion;
        }
    }

    public class QueryCollegeRegionController : ApiController
    {
        private readonly ICollegeRepository _repository;

        public QueryCollegeRegionController(ICollegeRepository repository)
        {
            _repository = repository;
        }

        public CollegeRegion Get(int id)
        {
            return _repository.GetRegion(id);
        }
    }

    public class CollegeQueryController : ApiController
    {
        private readonly ICollegeRepository _repository;

        public CollegeQueryController(ICollegeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public CollegeInfo Get(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<CollegeInfo> Get()
        {
            return _repository.GetAll();
        }

    }

    public class CollegeStatController : ApiController
    {
        private readonly ICollegeRepository _repository;
        private readonly IInfrastructureRepository _infrastructureRepository;

        public CollegeStatController(ICollegeRepository repository, IInfrastructureRepository infrastructureRepository)
        {
            _repository = repository;
            _infrastructureRepository = infrastructureRepository;
        }

        public CollegeStat Get(int id)
        {
            CollegeStat stat=new CollegeStat(_repository,id);
            stat.UpdateStats(_infrastructureRepository);
            return stat;
        }
    }
}
