using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Lte;
using Moq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Test.MockOperations
{
    public class MockENodebTestConfig : MockTownTestConfig
    {
        protected readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        protected override void Initialize()
        {
            base.Initialize();
            eNodebRepository.Setup(x => x.GetAll()).Returns(
                new List<ENodeb> {
                    new ENodeb { TownId = 1, ENodebId = 10001, Name = "E-1" },
                    new ENodeb { TownId = 1, ENodebId = 10002, Name = "E-2" },
                    new ENodeb { TownId = 1, ENodebId = 10003, Name = "E-3" },
                    new ENodeb { TownId = 2, ENodebId = 10004, Name = "E-4" },
                    new ENodeb { TownId = 2, ENodebId = 10005, Name = "E-5" },
                    new ENodeb { TownId = 3, ENodebId = 10006, Name = "E-6" },
                    new ENodeb { TownId = 4, ENodebId = 10007, Name = "E-7" }
                }.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());
            eNodebRepository.Setup(x => x.Count()).Returns(eNodebRepository.Object.GetAll().Count());
        }

        protected int SaveENodebs(List<ENodebExcel> infoList)
        {
            ParametersDumpInfrastructure infrastructure = new ParametersDumpInfrastructure();
            SaveENodebListService service = new SaveENodebListService(
                eNodebRepository.Object, infrastructure, townRepository.Object);
            service.Save(infoList, true);
            return infrastructure.ENodebInserted;
        }

        protected bool DeleteOneENodeb(int eNodebId)
        {
            DeleteOneENodebService service = new DeleteOneENodebService(eNodebRepository.Object, eNodebId);
            return service.Delete();
        }
        protected bool DeleteOneENodeb(ITownRepository repository,
            string city, string district, string town, string name)
        {
            DeleteOneENodebService service = new DeleteOneENodebService(eNodebRepository.Object, repository,
                city, district, town, name);
            return service.Delete();
        }

        protected bool DeleteOneENodeb(string city, string district, string town, string name)
        {
            return DeleteOneENodeb(townRepository.Object, city, district, town, name);
        }
    }

    public class MockTownTestConfig
    { 
        protected readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();

        protected virtual void Initialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(
                new List<Town> {
                    new Town { Id = 1, CityName = "C-1", DistrictName = "D-1", TownName = "T-1" },
                    new Town { Id = 2, CityName = "C-2", DistrictName = "D-2", TownName = "T-2" },
                    new Town { Id = 3, CityName = "C-3", DistrictName = "D-3", TownName = "T-3" },
                    new Town { Id = 4, CityName = "C-4", DistrictName = "D-4", TownName = "T-4" },
                    new Town { Id = 5, CityName = "C-5", DistrictName = "D-5", TownName = "T-5" }
                }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
        }
    }
}
