using System;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Concrete;
using Lte.Parameters.Kpi.Service;

namespace Lte.Parameters.Service.Public
{
    public class ParametersDumpGenerator
    {
        public Func<IParametersDumpController, ParametersDumpInfrastructure,
            IBtsDumpRepository<ENodebExcel>> LteENodebDumpGenerator { get; set; }

        public Func<IParametersDumpController, ParametersDumpInfrastructure,
            ICellDumpRepository<CellExcel>> LteCellDumpGenerator { get; set; }

        public Func<IParametersDumpController, ParametersDumpInfrastructure,
            IBtsDumpRepository<BtsExcel>> CdmaBtsDumpGenerator { get; set; }

        public Func<IParametersDumpController, ParametersDumpInfrastructure,
            ICellDumpRepository<CdmaCellExcel>> CdmaCellDumpGenerator { get; set; }

        public Func<IParametersDumpController, ParametersDumpInfrastructure,
            IMmlDumpRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>> MmlDumpGenerator { get; set; }

        public void DumpLteData(ParametersDumpInfrastructure infrastructure,
            IParametersDumpController controller,
            IParametersDumpConfig config)
        {
            IBtsDumpRepository<ENodebExcel> btsDumpRepository
                = LteENodebDumpGenerator(controller, infrastructure);
            btsDumpRepository.ImportBts = config.ImportENodeb;
            btsDumpRepository.UpdateBts = config.UpdateENodeb;
            btsDumpRepository.InvokeAction(infrastructure.LteENodebRepository);

            ICellDumpRepository<CellExcel> cellDumpRepository
                = LteCellDumpGenerator(controller, infrastructure);
            cellDumpRepository.ImportCell = config.ImportLteCell;
            cellDumpRepository.UpdateCell = config.UpdateLteCell;
            LteCellDumpRepository repository = cellDumpRepository as LteCellDumpRepository;
            if (repository != null)
                repository.UpdatePci = config.UpdatePci;
            cellDumpRepository.InvokeAction(infrastructure.LteCellRepository);
        }

        public void DumpMmlData(ParametersDumpInfrastructure infrastructure,
            IParametersDumpController controller)
        {
            if (infrastructure.MmlListIsEmpty)
            {
                return;
            }
            IMmlDumpRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel> dumpRepository
                = MmlDumpGenerator(controller, infrastructure);

            foreach (IMmlImportRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel> repository 
                in infrastructure.MmlRepositoryList)
            {
                dumpRepository.InvokeAction(repository);
            }
        }

        public void DumpCdmaData(ParametersDumpInfrastructure infrastructure,
            IParametersDumpController controller,
            IParametersDumpConfig config)
        {
            IBtsDumpRepository<BtsExcel> dumpBtsRepository
                = CdmaBtsDumpGenerator(controller, infrastructure);
            dumpBtsRepository.ImportBts = config.ImportBts;
            dumpBtsRepository.UpdateBts = config.UpdateBts;
            dumpBtsRepository.InvokeAction(infrastructure.CdmaBtsRepository);

            ICellDumpRepository<CdmaCellExcel> dumpCellRepository
                = CdmaCellDumpGenerator(controller, infrastructure);
            dumpCellRepository.ImportCell = config.ImportCdmaCell;
            dumpCellRepository.UpdateCell = config.UpdateCdmaCell;
            dumpCellRepository.InvokeAction(infrastructure.CdmaCellRepository);
        }
    }
}
