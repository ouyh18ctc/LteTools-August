using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class GetSectorPointsTest
    {
        [Test]
        public void TestGetSectorPoints()
        {
            Mock<IOutdoorCell> mockCell = new Mock<IOutdoorCell>();
            mockCell.SetupGet(x => x.Longtitute).Returns(0);
            mockCell.SetupGet(x => x.Lattitute).Returns(0);
            mockCell.SetupGet(x => x.Azimuth).Returns(55);
            GeoMath.BaiduLongtituteOffset = 0;
            GeoMath.BaiduLattituteOffset = 0;
            SectorTriangle sector = mockCell.Object.GetSectorPoints(1000);
            Assert.AreEqual(sector.X1, 0);
            Assert.AreEqual(sector.Y1, 0);
            Assert.AreEqual(sector.X2, 0.0281455, 1E-6);
            Assert.AreEqual(sector.Y2, 0.00246241, 1E-6);
            Assert.AreEqual(sector.X3, 0.0119402, 1E-6);
            Assert.AreEqual(sector.Y3, 0.0256059, 1E-6);
        }
    }
}
