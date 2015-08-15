using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Service.Cdma
{
    public static class DeleteOneBtsService
    {
        public static bool DeleteOneBts(this IBtsRepository repository, int btsId)
        {
            CdmaBts bts = repository.GetAll().FirstOrDefault(x=>x.BtsId==btsId);
            if (bts == null) return false;
            repository.Delete(bts);
            return true;
        }

        public static bool DeleteOneBts(this IBtsRepository repository, ITownRepository townRepository,
            string districtName, string townName, string btsName)
        {
            int townId = townRepository.GetAllList().QueryId(districtName, townName);
            CdmaBts bts = repository.QueryBts(townId, btsName);
            if (bts == null) return false;
            repository.Delete(bts);
            return true;
        }
    }
}
