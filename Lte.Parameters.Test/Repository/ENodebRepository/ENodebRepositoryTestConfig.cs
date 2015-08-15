using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.MockOperations;
using Lte.Parameters.Service.Lte;
using Moq;

namespace Lte.Parameters.Test.Repository.ENodebRepository
{
    public class ENodebRepositoryTestConfig
    {
        protected readonly Mock<IENodebRepository> lteRepository = new Mock<IENodebRepository>();

        protected List<ENodebExcel> eNodebInfos;

        protected readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        protected ENodebExcel eNodebInfo;

        protected virtual void Initialize()
        {
            eNodebInfo = new ENodebExcel
            {
                ENodebId = 2,
                Name = "First eNodeb",
                Factory = "Huawei",
                Gateway = new IpAddress("10.17.165.100"),
                Ip = new IpAddress("10.17.165.121"),
                CityName = "Foshan",
                DistrictName = "Chancheng",
                TownName = "Qinren"
            };
            eNodebInfos = new List<ENodebExcel>
            {
                new ENodebExcel
                {
                    ENodebId = 4,
                    Name = "First eNodeb",
                    Factory = "Huawei",
                    Gateway = new IpAddress("10.17.165.100"),
                    Ip = new IpAddress("10.17.165.121"),
                    CityName = "Foshan",
                    DistrictName = "Chancheng",
                    TownName = "Qinren"
                },
                new ENodebExcel
                {
                    ENodebId = 3,
                    Name = "Second eNodeb",
                    Factory = "Zte",
                    Gateway = new IpAddress("10.17.165.100"),
                    Ip = new IpAddress("10.17.165.122"),
                    CityName = "Foshan",
                    DistrictName = "Chancheng",
                    TownName = "Zumiao"
                }
            };
            lteRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb> 
            {
                new ENodeb
                {
                    ENodebId = 1,
                    Name = "FoshanZhaoming",
                    Address = "FenjiangZhonglu",
                    TownId = 122,
                    Gateway = (new IpAddress("10.17.165.100")).AddressValue,
                    SubIp = 23,
                    IsFdd = true,
                    Longtitute = 112.9987,
                    Lattitute = 23.1233
                }
            }.AsQueryable());
            lteRepository.Setup(x => x.GetAllList()).Returns(lteRepository.Object.GetAll().ToList());
            lteRepository.Setup(x => x.Count()).Returns(lteRepository.Object.GetAll().Count());

            townRepository.Setup(x => x.GetAll()).Returns(new List<Town>
            {
                new Town
                {
                    CityName = "Foshan",
                    DistrictName = "Chancheng",
                    TownName = "Qinren",
                    Id = 122
                }
            }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            lteRepository.MockENodebRepositorySaveENodeb();
            lteRepository.MockENodebRepositoryDeleteENodeb();
            SaveENodebListService.InfoFilter = x => true;
        }

        protected bool DeleteOneENodeb(int eNodebId)
        {
            DeleteOneENodebService service = new DeleteOneENodebService(lteRepository.Object, eNodebId);
            return service.Delete();
        }

        protected bool DeleteOneENodeb(string city, string district, string town, string name)
        {
            DeleteOneENodebService service = new DeleteOneENodebService(lteRepository.Object, townRepository.Object,
                city, district, town, name);
            return service.Delete();
        }

        protected int SaveENodebs(bool update)
        {
            ParametersDumpInfrastructure infrastructure = new ParametersDumpInfrastructure();
            SaveENodebListService service = new SaveENodebListService(
                lteRepository.Object, infrastructure, townRepository.Object);
            service.Save(eNodebInfos, update);
            return infrastructure.ENodebInserted;
        }

    }
}
