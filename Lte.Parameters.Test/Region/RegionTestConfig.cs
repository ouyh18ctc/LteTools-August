using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.MockOperations;
using Moq;

namespace Lte.Parameters.Test.Region
{
    public class RegionTestConfig
    {
        protected readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        protected readonly Mock<IRegionRepository> regionRepository = new Mock<IRegionRepository>();

        protected void Initialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(
                new List<Town> {
                    new Town { Id = 1, CityName = "C-1", DistrictName = "D-1", TownName = "T-1" },
                    new Town { Id = 2, CityName = "C-1", DistrictName = "D-2", TownName = "T-2" },
                    new Town { Id = 3, CityName = "C-2", DistrictName = "D-3", TownName = "T-3" },
                    new Town { Id = 4, CityName = "C-2", DistrictName = "D-4", TownName = "T-4" },
                    new Town { Id = 5, CityName = "C-2", DistrictName = "D-4", TownName = "T-5" },
                    new Town { Id = 6, CityName = "C-3", DistrictName = "D-5", TownName = "T-6" },
                    new Town { Id = 7, CityName = "C-3", DistrictName = "D-5", TownName = "T-7" },
                    new Town { Id = 8, CityName = "C-3", DistrictName = "D-6", TownName = "T-8" }
                }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            townRepository.MockAddOneTownOperation();
            townRepository.MockRemoveOneTownOperation();
            regionRepository.Setup(x => x.GetAll()).Returns(
                new List<OptimizeRegion>
                {
                    new OptimizeRegion {Id = 1, City = "C-1", District = "D-1", Region = "R-1"},
                    new OptimizeRegion {Id = 2, City = "C-1", District = "D-2", Region = "R-2"},
                    new OptimizeRegion {Id = 3, City = "C-2", District = "D-2", Region = "R-3"},
                    new OptimizeRegion {Id = 4, City = "C-2", District = "D-3", Region = "R-4"},
                    new OptimizeRegion {Id = 5, City = "C-2", District = "D-4", Region = "R-5"},
                    new OptimizeRegion {Id = 6, City = "C-3", District = "D-5", Region = "R-6"},
                    new OptimizeRegion {Id = 7, City = "C-3", District = "D-6", Region = "R-7"},
                    new OptimizeRegion {Id = 8, City = "C-3", District = "D-7", Region = "R-8"}
                }.AsQueryable());
            regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
            regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
            regionRepository.MockAddOneRegionOperation();
            regionRepository.MockRemoveOneRegionOperation();
        }
    }
}
