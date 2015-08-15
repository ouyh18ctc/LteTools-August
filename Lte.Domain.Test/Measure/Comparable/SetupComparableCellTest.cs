using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class SetupComparableCellTest
    {
        readonly ComparableCell mockCC = new ComparableCell();
        readonly Mock<IGeoPoint<double>> mockPoint = new Mock<IGeoPoint<double>>();
        readonly Mock<IOutdoorCell> mockCell = new Mock<IOutdoorCell>();
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            mockPoint.SetupGet(x => x.Longtitute).Returns(113);
            mockPoint.SetupGet(x => x.Lattitute).Returns(23);
            mockCell.SetupGet(x => x.Longtitute).Returns(113.01);
            mockCell.SetupGet(x => x.Lattitute).Returns(23.01);
        }

        [Test]
        public void TestSetupComparableCell_Azimuth_180()
        {
            mockCell.SetupGet(x => x.Azimuth).Returns(180);
            mockCC.SetupComparableCell(mockPoint.Object, mockCell.Object);
            Assert.AreSame(mockCell.Object, mockCC.Cell);
            Assert.AreEqual(mockCC.Distance, mockPoint.Object.SimpleDistance(mockCell.Object));
            Assert.AreEqual(mockCC.AzimuthAngle, 45, eps);
        }

        [Test]
        public void TestSetupComparableCell_Azimuth_OtherAngles()
        {
            mockCell.SetupGet(x => x.Azimuth).Returns(200);
            mockCC.SetupComparableCell(mockPoint.Object, mockCell.Object);
            Assert.AreEqual(mockCC.AzimuthAngle, 25, eps);
            mockCell.SetupGet(x => x.Azimuth).Returns(270);
            mockCC.SetupComparableCell(mockPoint.Object, mockCell.Object);
            Assert.AreEqual(mockCC.AzimuthAngle, -45, eps);
        }
    }
}
