using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.MockOperations;
using Lte.Parameters.Service.Lte;
using Moq;

namespace Lte.Parameters.Test.Repository.CellRepository
{
    public class CellRepositoryTestConfig
    {
        protected readonly Mock<ICellRepository> repository = new Mock<ICellRepository>();
        protected readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        protected CellExcel cellInfo;

        protected Cell QueryCell(int eNodebId, byte sectorId)
        {
            return repository.Object.GetAll().FirstOrDefault(x => x.ENodebId == eNodebId && x.SectorId == sectorId);
        }

        protected void Initialize()
        {
            eNodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb>
            {
                new ENodeb
                {
                    ENodebId = 1
                }
            }.AsQueryable());
            repository.Setup(x => x.GetAll()).Returns(new List<Cell> 
            {
                new Cell
                {
                    ENodebId = 1,
                    SectorId = 0,
                    IsOutdoor = false,
                    Frequency = 1750,
                    BandClass = 2,
                    Height = 32,
                    Azimuth = 55,
                    AntennaGain = 18,
                    MTilt = 3,
                    ETilt = 6,
                    RsPower = 15.2,
                    AntennaPorts = AntennaPortsConfigure.Antenna2T2R
                }
            }.AsQueryable());
            repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
            repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
            repository.MockCellRepositoryDeleteCell();
            repository.MockCellRepositorySaveCell();
            cellInfo = new CellExcel
            {
                ENodebId = 1,
                SectorId = 1,
                IsIndoor = "否  ",
                Frequency = 1750,
                BandClass = 1,
                Height = 40,
                Azimuth = 35,
                AntennaGain = 17.5,
                MTilt = 4,
                ETilt = 7,
                RsPower = 16.2,
                TransmitReceive = "2T4R"
            };
        }

        protected bool SaveOneCell()
        {
            SaveOneCellService service = new SimpleSaveOneCellService(repository.Object,
                cellInfo, eNodebRepository.Object);
            return service.Save();
        }

        protected bool SaveOneCell(CellBaseRepository baseRepository, bool update = false)
        {
            SaveOneCellService service = new ByENodebQuickSaveOneCellService(repository.Object,
                baseRepository, cellInfo, eNodebRepository.Object, update);
            return service.Save();
        }

        protected int SaveCells(IEnumerable<CellExcel> cellInfos)
        {
            ParametersDumpInfrastructure infrastructure = new ParametersDumpInfrastructure();
            SaveCellInfoListService service = new QuickSaveCellInfoListService(repository.Object,
                eNodebRepository.Object);
            service.Save(cellInfos, infrastructure);
            return infrastructure.CellsInserted;
        }

        protected int[] SaveCellInfos(IEnumerable<CellExcel> cellInfos)
        {
            ParametersDumpInfrastructure infrastructure = new ParametersDumpInfrastructure();
            SaveCellInfoListService service = new UpdateConsideredSaveCellInfoListService(
                repository.Object, eNodebRepository.Object, true);
            service.Save(cellInfos, infrastructure);
            return new[] {infrastructure.CellsInserted, infrastructure.CellsUpdated};
        }

        protected bool DeleteOneCell(int eNodebId, byte sectorId)
        {
            return repository.Object.DeleteCell(eNodebId, sectorId);
        }
    }
}
