using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Cdma;

namespace Lte.Parameters.Kpi.Concrete
{
    public class CdmaBtsDumpRepository : IBtsDumpRepository<BtsExcel>
    {
        private readonly ByExcelInfoSaveBtsListService service;

        public bool ImportBts { get; set; }
        public bool UpdateBts { get; set; }

        public CdmaBtsDumpRepository(
            ITownRepository townRepository,
            IENodebRepository eNodebRepository, 
            IBtsRepository btsRepository,
            ParametersDumpInfrastructure infrastructure)
        {
            service = new ByExcelInfoSaveBtsListService(
                btsRepository, infrastructure, townRepository, eNodebRepository);
        }

        public void InvokeAction(IExcelBtsImportRepository<BtsExcel> importRepository)
        {
            if (!ImportBts || importRepository.BtsExcelList.Count <= 0) return;
            service.Save(importRepository.BtsExcelList, UpdateBts);
        }
    }

    public class CdmaCellDumpRepository : ICellDumpRepository<CdmaCellExcel>
    {
        public bool ImportCell { get; set; }

        public bool UpdateCell { get; set; }

        private readonly ParametersDumpInfrastructure infrastructure;
        private readonly IBtsRepository btsRepository;
        private readonly ICdmaCellRepository cdmaCellRepository;

        public CdmaCellDumpRepository(IBtsRepository btsRepository,
            ICdmaCellRepository cdmaCellRepository,
            ParametersDumpInfrastructure infrastructure)
        {
            this.btsRepository = btsRepository;
            this.cdmaCellRepository = cdmaCellRepository;
            this.infrastructure = infrastructure;
        }

        public void InvokeAction(IExcelCellImportRepository<CdmaCellExcel> importRepository)
        {
            if (ImportCell && importRepository.CellExcelList.Count > 0)
            {
                SaveCdmaCellInfoListService service =
                    new UpdateConsideredSaveCdmaCellInfoListService(cdmaCellRepository,
                        importRepository.CellExcelList, btsRepository, UpdateCell);
                service.Save(infrastructure);
            }
        }
    }
}
