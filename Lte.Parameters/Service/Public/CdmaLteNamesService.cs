using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Public
{
    internal class NamesComparer<TNames> : IEqualityComparer<TNames>
            where TNames : class, ICdmaLteNames, new()
    {
        public bool Equals(TNames p1, TNames p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.CdmaCellId == p2.CdmaCellId && p1.SectorId == p2.SectorId;
        }

        public int GetHashCode(TNames p)
        {
            if (p == null)
                return 0;
            return (p.CdmaCellId + p.SectorId).GetHashCode();
        }
    }

    public class CdmaLteNamesService<TCell>
        where TCell : class, ICdmaCell, new()
    {
        private readonly IEnumerable<TCell> _cellList;
        private readonly IEnumerable<CdmaBts> _btsList;
        private readonly IEnumerable<ENodeb> _eNodebList;

        public CdmaLteNamesService(IEnumerable<TCell> cellList,
            IEnumerable<CdmaBts> btsList, IEnumerable<ENodeb> eNodebList)
        {
            _cellList = cellList;
            _btsList = btsList;
            _eNodebList = eNodebList;
        }

        public IEnumerable<TNames> Query<TNames>()
            where TNames : class, ICdmaLteNames, new()
        {
            var infoList
                = from a in _cellList
                  join b in _btsList on a.BtsId equals b.BtsId
                  join c in _eNodebList on b.ENodebId equals c.ENodebId into bc
                  from c2 in bc.DefaultIfEmpty(new ENodeb { ENodebId = -1, Name = "未定义" })
                  select new { CdmaName = b.Name, LteName = c2.Name, CdmaCellId = a.CellId, a.SectorId, c2.ENodebId };

            return infoList.Select(x =>
                new TNames
                {
                    CdmaCellId = x.CdmaCellId,
                    CdmaName = x.CdmaName,
                    ENodebId = x.ENodebId,
                    LteName = x.LteName,
                    SectorId = x.SectorId
                }).Distinct(new NamesComparer<TNames>());
        }

        public IEnumerable<TNames> Clone<TNames>()
            where TNames : class, ICdmaLteNames, new()
        {
            var infoList
                = from a in _cellList
                  join b in _btsList on a.BtsId equals b.BtsId
                  join c in _eNodebList on b.ENodebId equals c.ENodebId into bc
                  from c2 in bc.DefaultIfEmpty(new ENodeb { ENodebId = -1, Name = "未定义" })
                  select new
                  {
                      Infos = a,
                      CdmaName = b.Name,
                      LteName = c2.Name,
                      CdmaCellId = a.CellId,
                      a.SectorId,
                      c2.ENodebId
                  };

            return infoList.Select(x =>
            {
                TNames result = new TNames
                {
                    CdmaCellId = x.CdmaCellId,
                    CdmaName = x.CdmaName,
                    ENodebId = x.ENodebId,
                    LteName = x.LteName,
                    SectorId = x.SectorId
                };
                x.Infos.CloneProperties(result);
                return result;
            });
        }
    }
}
