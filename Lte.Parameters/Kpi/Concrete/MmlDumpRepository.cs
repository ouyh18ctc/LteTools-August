using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Cdma;

namespace Lte.Parameters.Kpi.Concrete
{
    public class MmlDumpRepository : IMmlDumpRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>
    {
        private readonly IBtsRepository btsRepository;
        private readonly ICdmaCellRepository cdmaCellRepository;
        private readonly ParametersDumpInfrastructure infrastructure;

        public MmlDumpRepository(IBtsRepository btsRepository,
            ICdmaCellRepository cdmaCellRepository, ParametersDumpInfrastructure infrastructure)
        {
            this.btsRepository = btsRepository;
            this.cdmaCellRepository = cdmaCellRepository;
            this.infrastructure = infrastructure;
        }

        public void InvokeAction(IMmlImportRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel> mmlRepository)
        {
            int totalLength = mmlRepository.CdmaBtsList.Count + mmlRepository.CdmaCellList.Count;
            if (totalLength == 0) { return; }
            SaveBtsListService btsService = new ByDbInfoSaveBtsListService(btsRepository,
                mmlRepository.CdmaBtsList);
            btsService.Save(infrastructure);
            SaveCdmaCellListService saveService=new BtsConsideredSaveCdmaListService(cdmaCellRepository,
                mmlRepository.CdmaCellList, btsRepository);
            saveService.Save(infrastructure);
        }
    }
}
