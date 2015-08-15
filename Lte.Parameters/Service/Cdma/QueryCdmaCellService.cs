using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Cdma
{
    public static class QueryCdmaCellService
    {
        public static CdmaCell Query(this ICdmaCellRepository repository,
            int btsId, byte sectorId, string cellType)
        {
            return repository.GetAll().FirstOrDefault(
                x => x.BtsId == btsId && x.SectorId == sectorId && x.CellType == cellType);
        }
    }
}
