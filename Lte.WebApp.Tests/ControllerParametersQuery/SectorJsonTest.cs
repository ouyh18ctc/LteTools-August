using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.WebApp.Controllers.Parameters;
using Moq;
using NUnit.Framework;

namespace Lte.WebApp.Tests.ControllerParametersQuery
{
    [TestFixture]
    public class SectorJsonTest
    {
        private readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        private readonly Mock<ICellRepository> cellRepository = new Mock<ICellRepository>();
        private SectorListController controller;

        [SetUp]
        public void TestInitialize()
        {
            eNodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb>{
                new ENodeb{ENodebId=1,Longtitute=112.1,Lattitute=23.1}
            }.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());
            eNodebRepository.Setup(x => x.Count()).Returns(eNodebRepository.Object.GetAll().Count());
            cellRepository.Setup(x => x.GetAll()).Returns(new List<Cell>{
                new Cell{ENodebId=1,SectorId=0,Azimuth=30,Height=10},
                new Cell{ENodebId=1,SectorId=1,Azimuth=150,Height=10},
                new Cell{ENodebId=1,SectorId=2,Azimuth=270,Height=10}
            }.AsQueryable());
            cellRepository.Setup(x => x.GetAllList()).Returns(cellRepository.Object.GetAll().ToList());
            cellRepository.Setup(x => x.Count()).Returns(cellRepository.Object.GetAll().Count());
            controller = new SectorListController(eNodebRepository.Object,cellRepository.Object);
        }

        [Test]
        public void TestGetSectorList()
        {
            IEnumerable<SectorTriangle> result = controller.Get(1);
            Assert.IsNotNull(result);
            List<SectorTriangle> data = result.ToList();
            Assert.IsNotNull(data);
            const double Eps = 1E-6;
            Assert.AreEqual(data.Count, 3);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(data[i].X1, GeoMath.BaiduLongtituteOffset, Eps);
                Assert.AreEqual(data[i].Y1,GeoMath.BaiduLattituteOffset, Eps);
            }
        }
    }
}
