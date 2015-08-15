using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Lte;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Kpi.Concrete
{
    public class LteENodebDumpRepository : IBtsDumpRepository<ENodebExcel>
    {
        private readonly ITownRepository townRepository;

        private readonly IENodebRepository eNodebRepository;

        private readonly ParametersDumpInfrastructure infrastructure;

        public bool ImportBts { get; set; }
        public bool UpdateBts { get; set; }

        public LteENodebDumpRepository(
            ITownRepository townRepository,
            IENodebRepository eNodebRepository, 
            ParametersDumpInfrastructure infrastructure)
        {
            this.townRepository = townRepository;
            this.eNodebRepository = eNodebRepository;
            this.infrastructure = infrastructure;
        }

        public void InvokeAction(IExcelBtsImportRepository<ENodebExcel> importRepository)
        {
            if (!ImportBts || importRepository.BtsExcelList.Count <= 0) return;
            SaveENodebListService service = new SaveENodebListService(eNodebRepository, infrastructure, townRepository);
            service.Save(importRepository.BtsExcelList, UpdateBts);
        }
    }

    public class LteCellDumpRepository : ICellDumpRepository<CellExcel>
    {
        private readonly ICellRepository cellRepository;

        private readonly IENodebRepository eNodebRepository;

        private readonly IBtsRepository btsRepository;

        private readonly ICdmaCellRepository cdmaCellRepository;

        private readonly ParametersDumpInfrastructure infrastructure;

        public bool ImportCell { get; set; }

        public bool UpdateCell { get; set; }

        public bool UpdatePci { get; set; }

        public LteCellDumpRepository(
            ICellRepository cellRepository,
            IENodebRepository eNodebRepository,
            IBtsRepository btsRepository,
            ICdmaCellRepository cdmaCellRepository,
            ParametersDumpInfrastructure infrastructure)
        {
            this.cellRepository = cellRepository;
            this.eNodebRepository = eNodebRepository;
            this.btsRepository = btsRepository;
            this.cdmaCellRepository = cdmaCellRepository;
            this.infrastructure = infrastructure;
        }

        public void InvokeAction(IExcelCellImportRepository<CellExcel> importRepository)
        {
            if (!ImportCell || importRepository.CellExcelList.Count <= 0) return;
            SaveCellInfoListService lteService = new UpdateConsideredSaveCellInfoListService(
                cellRepository, eNodebRepository, UpdateCell, UpdatePci);
            lteService.Save(importRepository.CellExcelList.Distinct(new CellExcelComparer()), infrastructure);

            CdmaLteIdsService idService = new CdmaLteIdsService(importRepository.CellExcelList);
            IEnumerable<CdmaLteIds> ids = idService.Query();
            UpdateCdmaLteIdService service = new UpdateCdmaLteIdService(btsRepository, cdmaCellRepository, ids);
            service.Update();
        }
    }
}
