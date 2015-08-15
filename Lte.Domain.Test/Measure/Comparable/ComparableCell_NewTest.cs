using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_NewTest
    {
        private IList<ILinkBudget<double>> budgetList;
        private IBroadcastModel model;
        const double eps = 1E-6;

        [SetUp]
        public void SetUp()
        {
            budgetList = new List<ILinkBudget<double>>();
            model = new BroadcastModel();
        }

        [Test]
        public void TestComparableCell_OnePoint()
        {
            Mock<IOutdoorCell> outdoorCell = new Mock<IOutdoorCell>();
            outdoorCell.MockOutdoorCell(112, 23, 0, 15.2, 18);
            StubGeoPoint point = new StubGeoPoint(outdoorCell.Object, 0.01);

            ComparableCell comparableCell = new ComparableCell(point, outdoorCell.Object, budgetList, model);
            Assert.AreEqual(comparableCell.Cell, outdoorCell.Object);
            Assert.AreEqual(comparableCell.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell.AzimuthAngle, 90);
            Assert.AreEqual(comparableCell.Budget.AntennaGain, 18);
            Assert.AreEqual(comparableCell.Budget.TransmitPower, 15.2);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell.Budget);
        }

        [Test]
        public void TestComparableCell_TwoPoints_WithSameCell()
        {
            Mock<IOutdoorCell> outdoorCell = new Mock<IOutdoorCell>();
            outdoorCell.MockOutdoorCell(112, 23, 0, 15.2, 18);
            StubGeoPoint point1 = new StubGeoPoint(outdoorCell.Object, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(outdoorCell.Object, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, outdoorCell.Object, budgetList, model);
            Assert.AreEqual(comparableCell1.Cell, outdoorCell.Object);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);

            ComparableCell comparableCell2 = new ComparableCell(point2, outdoorCell.Object, budgetList, model);
            Assert.AreEqual(comparableCell2.Cell, outdoorCell.Object);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);

            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_TwoPoints_WithDifferentCells_SameBudget()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112.5, 23, 0, 15.2, 18);
            StubGeoPoint point1 = new StubGeoPoint(outdoorCell1.Object, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(outdoorCell2.Object, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, outdoorCell1.Object, budgetList, model);
            Assert.AreEqual(comparableCell1.Cell, outdoorCell1.Object);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);

            ComparableCell comparableCell2 = new ComparableCell(point2, outdoorCell2.Object, budgetList, model);
            Assert.AreEqual(comparableCell2.Cell, outdoorCell2.Object);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);

            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_TwoPoints_WithDifferentCells_DifferentBudgets()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112.5, 23, 0, 16.2, 18);
            StubGeoPoint point1 = new StubGeoPoint(outdoorCell1.Object, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(outdoorCell2.Object, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, outdoorCell1.Object, budgetList, model);
            Assert.AreEqual(comparableCell1.Cell, outdoorCell1.Object);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);

            ComparableCell comparableCell2 = new ComparableCell(point2, outdoorCell2.Object, budgetList, model);
            Assert.AreEqual(comparableCell2.Cell, outdoorCell2.Object);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);

            Assert.AreEqual(budgetList.Count, 2);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);
            Assert.AreEqual(budgetList.ElementAt(1), comparableCell2.Budget);
        }
        
    }
}
