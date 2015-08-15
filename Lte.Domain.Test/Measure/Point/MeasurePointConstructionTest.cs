using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Point
{
    [TestFixture]
    public class MeasurePointConstructionTest
    {
        private readonly IGeoPoint<double> position = new GeoPoint(112, 23);
        private MeasurePoint measurePoint;
        private readonly IBroadcastModel model = new BroadcastModel();
        private ILinkBudget<double> budget;
        private readonly List<IOutdoorCell> cellList = new List<IOutdoorCell>();
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            measurePoint = new MeasurePoint(position);
            budget = new LinkBudget(model);
        }

        [Test]
        public void TestMeasurePointConstruction_MockOneOutdoorCell()
        {
            Mock<IOutdoorCell> oc = new Mock<IOutdoorCell>();
            oc.MockStandardOutdoorCell(112.001, 23.001, 225);
            cellList.Add(oc.Object);
            measurePoint.ImportCells(cellList, budget);
            Assert.AreEqual(measurePoint.Longtitute, position.Longtitute);
            Assert.AreEqual(measurePoint.Lattitute, position.Lattitute);
            Assert.AreEqual(measurePoint.ComparableCellAt(0).Cell, oc.Object);
            Assert.AreEqual(measurePoint.ComparableCellAt(0).Distance, 0.157253, eps);
            Assert.AreEqual(measurePoint.ComparableCellAt(0).AzimuthAngle, 0, eps);
            Assert.AreEqual(measurePoint.ReceivedRsrpAt(0), -80.363205, eps);
        }

    }
}
