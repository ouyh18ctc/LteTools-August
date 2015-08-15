using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Cdma
{
    public static class QueryBtsService
    {
        public static CdmaBts QueryBts(this IBtsRepository repository,
            int townId, string name)
        {
            return repository.GetAll().FirstOrDefault(x => x.TownId == townId && x.Name == name);
        }
    }
}
