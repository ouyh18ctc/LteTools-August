using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Lte.Domain.Regular;
using NUnit.Framework;
using Moq;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_SetAzimuthAngleTest
    {
        private Mock<IGeoPoint<double>> p;
        private ComparableCell cc;
        private Mock<IOutdoorCell> cell;
        private const double eps = 1E-6;

        [SetUp]
        public void SetUp()
        {
            p = new Mock<IGeoPoint<double>>();
            p.SetupGet(x => x.Longtitute).Returns(112);
            p.SetupGet(x => x.Lattitute).Returns(23);
            cell = new Mock<IOutdoorCell>();
            cell.SetupGet(x=>x.Longtitute).Returns(112.01);
            cell.SetupGet(x => x.Lattitute).Returns(23.01);
            cell.BindGetAndSetAttributes(x => x.Azimuth, (x, v) => x.Azimuth = v);

            cc = new ComparableCell {Cell = cell.Object};

        }

        [Test]
        public void TestComparableCell_SetAzimuthAngle_OnePoint_AzimuthAngle_135()
        {
            cc.SetupComparableCell(p.Object, cell.Object);
            Assert.AreEqual(cc.AzimuthAngle, -135, eps);
            
        }

        [TestCase(180,45,180,45)]
        [TestCase(135,90,135,90)]
        [TestCase(90,135,90,135)]
        [TestCase(45,-180,45,-180)]
        [TestCase(0,-135,0,-135)]
        public void TestComparableCell_SetAzimuthAngle(double azimuthAngle, 
            double expectedAngleFromCellAzimuth, double expectedCellAzimuth, double expectedAzimuthAngle)
        {
            
            cc.SetAzimuthAngle(p.Object, azimuthAngle);
            double angleFromCellAzimuth = cc.Cell.AngleFromCellAzimuth(p.Object);
            Assert.AreEqual(angleFromCellAzimuth, expectedAngleFromCellAzimuth, eps,
                "Angle from cell azimuth: " + angleFromCellAzimuth);
            Assert.AreEqual(cc.Cell.Azimuth, expectedCellAzimuth, eps);
            Assert.AreEqual(cc.AzimuthAngle, expectedAzimuthAngle, eps);
        }
    }
}
