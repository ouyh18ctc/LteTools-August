using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Cdma
{
    public static class DeleteOneCdmaCellService
    {
        public static bool Delete(this ICdmaCellRepository repository,
            int btsId, byte sectorId, string cellType)
        {
            CdmaCell _cell = repository.Query(btsId, sectorId, cellType);
            if (_cell == null) return false;
            repository.Delete(_cell);
            return true;
        }
    }
}
