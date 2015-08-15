using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.MockOperations;
using Lte.Parameters.Service.Cdma;
using Lte.Parameters.Test.Repository.ENodebRepository;
using Moq;

namespace Lte.Parameters.Test.Repository.BtsRepository
{
    public class BtsRepositoryTestConfig : ENodebRepositoryTestConfig
    {
        protected readonly Mock<IBtsRepository> repository = new Mock<IBtsRepository>();

        protected readonly List<BtsExcel> btsInfos = new List<BtsExcel>
        {
            new BtsExcel
            {
                BtsId = 2,
                Name = "First bts",
                DistrictName = "Chancheng",
                TownName = "Qinren",
                Longtitute = 112.9986,
                Lattitute = 23.1233
            },
            new BtsExcel
            {
                BtsId = 3,
                Name = "Second bts",
                DistrictName = "Chancheng",
                TownName = "Zumiao",
                Longtitute = 112.9987,
                Lattitute = 23.2233
            }
        };

        protected override void Initialize()
        {
            CdmaBts bts = new CdmaBts
            {
                BtsId = 1,
                Name = "FoshanZhaoming",
                Address = "FenjiangZhonglu",
                TownId = 122,
                Longtitute = 112.9987,
                Lattitute = 23.1233
            };
            repository.Setup(x => x.GetAll()).Returns(new List<CdmaBts> 
            {
                bts
            }.AsQueryable());
            repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
            repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
            repository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<CdmaBts, bool>>>()))
                .Returns<Func<CdmaBts, bool>>(Predicate =>
            {
                IEnumerable<CdmaBts> btss = repository.Object.GetAll();
                return btss.FirstOrDefault(Predicate);
            });
            base.Initialize();
            repository.MockBtsRepositorySaveBts();
            repository.MockBtsRepositoryDeleteBts();

        }

        protected bool SaveOneBts(BtsExcel btsInfo)
        {
            TownListConsideredSaveOneBtsService service = new TownListConsideredSaveOneBtsService(repository.Object,
                townRepository.Object);
            return service.SaveOneBts(btsInfo, true);
        }
    }
}