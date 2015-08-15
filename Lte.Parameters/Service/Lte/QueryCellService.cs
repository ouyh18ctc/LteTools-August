using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Lte
{
    public static class QueryCellService
    {
        private static bool DeleteCell(this ICellRepository repository, Cell cell)
        {
            if (cell == null) return false;
            repository.Delete(cell);
            return true;
        }

        public static bool DeleteCell(this ICellRepository repository, int eNodebId, byte sectorId)
        {
            return repository.DeleteCell(repository.GetAll().FirstOrDefault(x => x.ENodebId == eNodebId && x.SectorId == sectorId));
        }
    }

    public static class QueryMrService
    {
        public static void SaveStats(this IMrsCellRepository repository, IEnumerable<MrsCellDate> stats)
        {
            foreach (MrsCellDate stat in 
                from stat in stats let item = repository.GetAll().FirstOrDefault(x =>
                x.RecordDate == stat.RecordDate && x.CellId == stat.CellId && x.SectorId == stat.SectorId) 
                where item == null select stat)
            {
                stat.UpdateStats();
                repository.Insert(stat);
            }
        }

        public static void SaveTaStats(this IMrsCellTaRepository repository, IEnumerable<MrsCellTa> stats)
        {
            foreach (MrsCellTa stat in
                from stat in stats
                let item = repository.GetAll().FirstOrDefault(x =>
                    x.RecordDate == stat.RecordDate && x.CellId == stat.CellId && x.SectorId == stat.SectorId)
                where item == null
                select stat)
            {
                stat.UpdateStats();
                repository.Insert(stat);
            }
        }

        public static void SaveRsrpTaStats(this IMroCellRepository repository, IEnumerable<MroRsrpTa> stats)
        {
            foreach (MroRsrpTa stat in
                from stat in stats
                let item = repository.GetAll().FirstOrDefault(x =>
                    x.RecordDate == stat.RecordDate && x.CellId == stat.CellId && x.SectorId == stat.SectorId)
                where item == null
                select stat)
            {
                repository.Insert(stat);
            }
        }

    }
}
