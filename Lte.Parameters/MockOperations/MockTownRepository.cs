using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Moq;

namespace Lte.Parameters.MockOperations
{
    public static class MockTownRepository
    {
        public static void MockAddOneTownOperation(this Mock<ITownRepository> townRepository,
            IEnumerable<Town> towns)
        {
            townRepository.Setup(x => x.Insert(It.Is<Town>(t => t != null))).Callback<Town>(
                town =>
                {
                    townRepository.Setup(y => y.GetAll()).Returns(
                        towns.Concat(new List<Town> {town}).AsQueryable());
                    townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
                    townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
                });
        }

        public static void MockAddOneTownOperation(this Mock<ITownRepository> townRepository)
        {
            IEnumerable<Town> towns = townRepository.Object.GetAll();
            townRepository.Setup(x => x.Insert(It.Is<Town>(t => t != null))).Callback<Town>(
                town =>
                {
                    townRepository.Setup(y => y.GetAll()).Returns(
                        towns.Concat(new List<Town> { town }).AsQueryable());
                    townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
                    townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
                });
        }

        public static void MockRemoveOneTownOperation(this Mock<ITownRepository> townRepository,
            IEnumerable<Town> towns)
        {
            townRepository.Setup(x => x.Delete(It.Is<Town>(
                t => t != null && towns.FirstOrDefault(y => y == t) != null))).Callback<Town>(
                    t1 =>
                    {
                        townRepository.Setup(z => z.GetAll()).Returns(towns.Where(t2 => t2 != t1).AsQueryable());
                        townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
                        townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
                    });
        }

        public static void MockRemoveOneTownOperation(this Mock<ITownRepository> townRepository)
        {
            IEnumerable<Town> towns = townRepository.Object.GetAll();
            townRepository.Setup(x => x.Delete(It.Is<Town>(
                t => t != null && towns.FirstOrDefault(y => y == t) != null))).Callback<Town>(
                    t1 =>
                    {
                        townRepository.Setup(z => z.GetAll()).Returns(towns.Where(t2 => t2 != t1).AsQueryable());
                        townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
                        townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
                    });
        }
    }

    public static class MockOptimizeRegionRepository
    {
        public static void MockAddOneRegionOperation(this Mock<IRegionRepository> regionRepository,
            IEnumerable<OptimizeRegion> regions)
        {
            regionRepository.Setup(x => x.Insert(It.Is<OptimizeRegion>(o => o != null))).Callback<OptimizeRegion>(
                region =>
                {
                    regionRepository.Setup(y => y.GetAll()).Returns(
                        regions.Concat(new List<OptimizeRegion> { region }).AsQueryable());
                    regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
                    regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
                });
        }

        public static void MockAddOneRegionOperation(this Mock<IRegionRepository> regionRepository)
        {
            IEnumerable<OptimizeRegion> regions = regionRepository.Object.GetAll();
            regionRepository.Setup(x => x.Insert(It.Is<OptimizeRegion>(o => o != null))).Callback<OptimizeRegion>(
                region =>
                {
                    regionRepository.Setup(y => y.GetAll()).Returns(
                        regions.Concat(new List<OptimizeRegion> { region }).AsQueryable());
                    regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
                    regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
                });
        }

        public static void MockRemoveOneRegionOperation(this Mock<IRegionRepository> regionRepository,
            IEnumerable<OptimizeRegion> regions)
        {
            regionRepository.Setup(x => x.Delete(It.Is<OptimizeRegion>(
                t => t != null && regions.FirstOrDefault(y => y == t) != null))).Callback<OptimizeRegion>(
                t1 =>
                {
                    regionRepository.Setup(z => z.GetAll()).Returns(
                        regions.Where(t2 => t2 != t1).AsQueryable());
                    regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
                    regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
                });
        }

        public static void MockRemoveOneRegionOperation(this Mock<IRegionRepository> regionRepository)
        {
            IEnumerable<OptimizeRegion> regions = regionRepository.Object.GetAll();
            regionRepository.Setup(x => x.Delete(It.Is<OptimizeRegion>(
                t => t != null && regions.FirstOrDefault(y => y == t) != null))).Callback<OptimizeRegion>(
                t1 =>
                {
                    regionRepository.Setup(z => z.GetAll()).Returns(
                        regions.Where(t2 => t2 != t1).AsQueryable());
                    regionRepository.Setup(x => x.GetAllList()).Returns(regionRepository.Object.GetAll().ToList());
                    regionRepository.Setup(x => x.Count()).Returns(regionRepository.Object.GetAll().Count());
                });
        }
    }
}
