using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.MockOperations;
using Moq;

namespace Lte.Parameters.Test.Repository.CellRepository
{
    public class CdmaCellRepositoryTestConfig
    {
        protected readonly Mock<ICdmaCellRepository> repository = new Mock<ICdmaCellRepository>();

        protected void Initialize()
        {
            repository.Setup(x => x.GetAll()).Returns(new List<CdmaCell> 
            {
                new CdmaCell
                {
                    BtsId = 1,
                    SectorId = 0,
                    IsOutdoor = false,
                    Frequency1 = 283,
                    Frequency = 64,
                    Height = 32,
                    Azimuth = 55,
                    AntennaGain = 18,
                    MTilt = 3,
                    ETilt = 6,
                    RsPower = 15.2
                }
            }.AsQueryable());
            repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
            repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
            repository.MockCdmaCellRepositoryDeleteCell();
            repository.MockCdmaCellRepositorySaveCell();
        }
    }
}