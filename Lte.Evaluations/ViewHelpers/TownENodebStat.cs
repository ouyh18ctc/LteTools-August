using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Evaluations.ViewHelpers
{
    public class TownENodebStat : ITown
    {
        [Display(Name = "基站个数")]
        public int TotalENodebs { get; private set; }

        [Display(Name = "城市")]
        public string CityName { get; set; }

        [Display(Name = "片区")]
        public string RegionName { get; private set; }

        [Display(Name = "区域")]
        public string DistrictName { get; set; }

        [Display(Name = "镇区") ]
        public string TownName { get; set; }

        public int TownId { get; private set; }

        public TownENodebStat() { }

        public TownENodebStat(Town town, IEnumerable<ENodeb> eNodebs, IRegionRepository regionRepository)
        {
            town.CloneProperties(this);
            TownId = town.Id;

            QueryRegionService service = new ByDistrictQueryRegionService(
                regionRepository.GetAll(), town.CityName, town.DistrictName);
            OptimizeRegion region = service.Query();
            RegionName = (region == null) ? "" : region.Region;
            TotalENodebs = eNodebs.Count(x => x.TownId == TownId);
        }
    }
}
