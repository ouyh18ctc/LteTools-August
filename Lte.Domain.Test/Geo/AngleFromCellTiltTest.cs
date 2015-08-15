using System;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using NUnit.Framework;
using Moq;

namespace Lte.Domain.Test.Geo
{
    [TestFixture]
    public class AngleFromCellTiltTest
    {
        private Mock<IGeoPoint<double>> mockPoint = new Mock<IGeoPoint<double>>();
        private Mock<IOutdoorCell> mockCell = new Mock<IOutdoorCell>();
        private const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            mockPoint.Setup(x => x.Longtitute).Returns(113);
            mockPoint.Setup(x => x.Lattitute).Returns(23);
            mockCell.Setup(x => x.Lattitute).Returns(23.1);
            mockCell.Setup(x => x.Longtitute).Returns(113.1);
        }

        [TestCase(40, 1, 4)]
        [TestCase(30, 2, 3)]
        [TestCase(70, 1, 15)]
        [TestCase(100, 37, 4)]
        public void TestAngleFromCellTilt_BasicAttributes(double height, double mTilt, double eTilt)
        {
            mockCell.Setup(x => x.Height).Returns(height);
            mockCell.Setup(x => x.MTilt).Returns(mTilt);
            mockCell.Setup(x => x.ETilt).Returns(eTilt);
            Assert.AreEqual(mockPoint.Object.Longtitute, 113);
            Assert.AreEqual(mockPoint.Object.Lattitute, 23);
            Assert.AreEqual(mockCell.Object.Longtitute, 113.1);
            Assert.AreEqual(mockCell.Object.Lattitute, 23.1);
            Assert.AreEqual(mockCell.Object.Height, height);
            Assert.AreEqual(mockCell.Object.MTilt, mTilt);
            Assert.AreEqual(mockCell.Object.ETilt, eTilt);
        }

        [TestCase(40, 1, 4, 0.145741, 4.854259)]
        [TestCase(30, 2, 3, 0.109306, 4.890694)]
        [TestCase(70, 1, 15, 0.2550456, 15.7449544)]
        [TestCase(100, 37, 4, 0.364348, 40.635651)]
        public void TestAngleFromCellTilt_FunctionLogic(double height, double mTilt, double eTilt,
            double expectedTiltFromCell, double expectedAngleFromCellTilt)
        {
            mockCell.Setup(x => x.Height).Returns(height);
            mockCell.Setup(x => x.MTilt).Returns(mTilt);
            mockCell.Setup(x => x.ETilt).Returns(eTilt);
            double tiltFromCell = mockCell.Object.TiltFromCell(mockPoint.Object);
            Assert.AreEqual(expectedTiltFromCell, tiltFromCell, eps);
            double angleFromCellTilt = mockCell.Object.AngleFromCellTilt(mockPoint.Object);
            Assert.AreEqual(expectedAngleFromCellTilt, angleFromCellTilt, eps);
        }

    }
}