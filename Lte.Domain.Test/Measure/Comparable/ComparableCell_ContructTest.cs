using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestFixture]
    public class ComparableCell_ContructTest
    {
        private IList<ILinkBudget<double>> budgetList;
        const double eps = 1E-6;

        [SetUp]
        public void SetUp()
        {
            budgetList = new List<ILinkBudget<double>>();
        }

        [Test]
        public void TestComparableCell_ConstructOnePoint()
        {
            StubOutdoorCell cell = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubGeoPoint point = new StubGeoPoint(cell, 0.01);

            ComparableCell comparableCell = new ComparableCell(point, cell, budgetList);
            Assert.AreEqual(comparableCell.Cell, cell);
            Assert.AreEqual(comparableCell.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell.AzimuthAngle, 90);
            Assert.AreEqual(comparableCell.Budget.AntennaGain, 17.5);
            Assert.AreEqual(comparableCell.Budget.TransmitPower, 15.2);
            Assert.AreEqual(comparableCell.PciModx, 0);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell.Budget);
        }

        [Test]
        public void TestComparableCell_ConstructTwoPoints_WithSameCell()
        {
            StubOutdoorCell cell = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubGeoPoint point1 = new StubGeoPoint(cell, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(cell, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, cell, budgetList);
            Assert.AreEqual(comparableCell1.Cell, cell);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);
            Assert.AreEqual(budgetList.Count, 1, "listCount");
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);

            ComparableCell comparableCell2 = new ComparableCell(point2, cell, budgetList);
            Assert.AreEqual(comparableCell2.Cell, cell);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_ConstructTwoPoints_WithDifferentCells_SameBudget()
        {
            StubOutdoorCell cell1 = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubOutdoorCell cell2 = new StubOutdoorCell(112.5, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubGeoPoint point1 = new StubGeoPoint(cell1, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(cell2, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, cell1, budgetList);
            Assert.AreEqual(comparableCell1.Cell, cell1);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);

            ComparableCell comparableCell2 = new ComparableCell(point2, cell2, budgetList);
            Assert.AreEqual(comparableCell2.Cell, cell2);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_ConstructTwoPoints_WithDifferentCells_DifferentRsPower()
        {
            StubOutdoorCell cell1 = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubOutdoorCell cell2 = new StubOutdoorCell(112.5, 23)
            {
                RsPower = 16.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubGeoPoint point1 = new StubGeoPoint(cell1, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(cell2, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, cell1, budgetList);
            Assert.AreEqual(comparableCell1.Cell, cell1);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);

            ComparableCell comparableCell2 = new ComparableCell(point2, cell2, budgetList);
            Assert.AreEqual(comparableCell2.Cell, cell2);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);
            Assert.AreEqual(budgetList.Count, 2);
            Assert.AreEqual(budgetList.ElementAt(1), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_ConstructTwoPoints_WithDifferentCells_DifferentAntennaGain()
        {
            StubOutdoorCell cell1 = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubOutdoorCell cell2 = new StubOutdoorCell(112.5, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 18.5,
                Frequency = 100
            };
            StubGeoPoint point1 = new StubGeoPoint(cell1, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(cell2, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, cell1, budgetList);
            Assert.AreEqual(comparableCell1.Cell, cell1);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);

            ComparableCell comparableCell2 = new ComparableCell(point2, cell2, budgetList);
            Assert.AreEqual(comparableCell2.Cell, cell2);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);
            Assert.AreEqual(budgetList.Count, 2);
            Assert.AreEqual(budgetList.ElementAt(1), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_ConstructTwoPoints_WithDifferentCells_DifferentFrequency()
        {
            StubOutdoorCell cell1 = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubOutdoorCell cell2 = new StubOutdoorCell(112.5, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 1825
            };
            StubGeoPoint point1 = new StubGeoPoint(cell1, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(cell2, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, cell1, budgetList);
            Assert.AreEqual(comparableCell1.Cell, cell1);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);

            ComparableCell comparableCell2 = new ComparableCell(point2, cell2, budgetList);
            Assert.AreEqual(comparableCell2.Cell, cell2);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);
            Assert.AreEqual(budgetList.Count, 2);
            Assert.AreEqual(budgetList.ElementAt(1), comparableCell2.Budget);
        }

        [Test]
        public void TestComparableCell_ConstructTwoPoints_WithDifferentCells_DifferentPci()
        {
            StubOutdoorCell cell1 = new StubOutdoorCell(112, 23)
            {
                RsPower = 15.2,
                Pci = 30,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubOutdoorCell cell2 = new StubOutdoorCell(112.5, 23)
            {
                RsPower = 15.2,
                Pci = 31,
                AntennaGain = 17.5,
                Frequency = 100
            };
            StubGeoPoint point1 = new StubGeoPoint(cell1, 0.01);
            StubGeoPoint point2 = new StubGeoPoint(cell2, 0.01, 90);

            ComparableCell comparableCell1 = new ComparableCell(point1, cell1, budgetList);
            Assert.AreEqual(comparableCell1.Cell, cell1);
            Assert.AreEqual(comparableCell1.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell1.AzimuthAngle, 90);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell1.Budget);

            ComparableCell comparableCell2 = new ComparableCell(point2, cell2, budgetList);
            Assert.AreEqual(comparableCell2.Cell, cell2);
            Assert.AreEqual(comparableCell2.Distance, 1.111949, eps);
            Assert.AreEqual(comparableCell2.AzimuthAngle, 0);
            Assert.AreEqual(budgetList.Count, 1);
            Assert.AreEqual(budgetList.ElementAt(0), comparableCell2.Budget);
        }
    }
}
