using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Lte;

namespace Lte.Parameters.Concrete
{
    public class EFENodebRepository : LightWeightRepositroyBase<ENodeb>, IENodebRepository
    {
        public List<ENodeb> GetAllWithIds(IEnumerable<int> ids)
        {
            return (from a in GetAll()
                join b in ids on a.ENodebId equals b
                select a).OrderBy(x=>x.ENodebId).ToList();
        }

        public List<ENodeb> GetAllWithNames(ITownRepository townRepository, string city, string district, string town, string eNodebName,
            string address)
        {
            return this.QueryWithNames(townRepository, city, district, town, eNodebName, address).ToList();
        }

        public List<ENodeb> GetAllWithNames(ITownRepository townRepository, ITown town, string eNodebName, string address)
        {
            return GetAllWithNames(townRepository, town.CityName, town.DistrictName, town.TownName,
                eNodebName, address);
        }

        protected override DbSet<ENodeb> Entities
        {
            get { return context.ENodebs; }
        }
    }
}