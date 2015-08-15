using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Coverage
{
    public static class InterferenceStatService
    {
        public static IEnumerable<InterferenceStat> QueryItems(
            this IInterferenceStatRepository repository,
            IEnumerable<ENodeb> eNodebs, IEnumerable<CdmaLteNames> namesInfoList)
        {
            IEnumerable<CdmaLteNames> names = from e in eNodebs
                                              join n in namesInfoList
                                                  on e.ENodebId equals n.ENodebId
                                              select n;
            IEnumerable<InterferenceStat> stats = from n in names
                                                  join s in repository.GetAll()
                                                      on new { CellId = n.CdmaCellId, n.SectorId }
                                                  equals new { s.CellId, s.SectorId }
                                                  select s;
            return stats;
        }

        public static IEnumerable<MrsCellDate> QueryItems(
            this IMrsCellRepository repository,
            IEnumerable<ENodeb> eNodebs, DateTime startDate, DateTime endDate)
        {
            return from e in eNodebs
                   join s in repository.GetAll().Where(x => x.RecordDate >= startDate && x.RecordDate <= endDate)
                   on e.ENodebId equals s.CellId
                   select s;
        }
    }
}
