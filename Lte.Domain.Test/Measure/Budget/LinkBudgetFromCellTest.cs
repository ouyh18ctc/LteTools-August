using System.Globalization;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Budget
{
    [TestFixture]
    public class LinkBudgetFromCellTest
    {
        private readonly IOutdoorCell cell = new StubOutdoorCell(112, 23);
        private ILinkBudget<double> budget;
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            cell.Frequency = 1825;
            cell.AntennaGain = 18;
            cell.RsPower = 16.2;
            budget = new LinkBudget(cell);
        }

        [Test]
        public void TestLinkBudgetFromCell()
        {
            Assert.IsNotNull(budget);
            Assert.AreEqual(budget.AntennaGain, 18);
            Assert.AreEqual(budget.TransmitPower, 16.2);
            Assert.AreEqual(budget.Model.Frequency, 1867.5);
            cell.Frequency = 100;
            cell.RsPower = 15.2;
            budget = new LinkBudget(cell);
            Assert.AreEqual(budget.TransmitPower, 15.2);
            Assert.AreEqual(budget.Model.Frequency, 2120);
        }

        [Test]
        public void TestBudgetFromCell_Calculate()
        {
            double x = budget.CalculateReceivedPower(1, 1);
            Assert.AreEqual(x, -123.717734, eps, x.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestBudgetFromCell_Calculate_FromCellHeight()
        {
            double x = budget.CalculateReceivedPower(1, cell.Height);
            Assert.AreEqual(cell.Height, 30);
            Assert.AreEqual(x, -103.303919, eps, x.ToString(CultureInfo.InvariantCulture));
            x = budget.CalculateReceivedPower(1, 20);
            Assert.AreEqual(x, -105.7375, eps, x.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestBudgetFromCell_Calculate_AdjustFrequencyAndRsPower()
        {
            cell.Frequency = 100;
            cell.RsPower = 15.2;
            budget = new LinkBudget(cell);
            double x = budget.CalculateReceivedPower(1, cell.Height);
            Assert.AreEqual(cell.Height, 30);
            Assert.AreEqual(x, -105.758561, eps, x.ToString(CultureInfo.InvariantCulture));
            x = budget.CalculateReceivedPower(1, 20);
            Assert.AreEqual(x, -108.192142, eps, x.ToString(CultureInfo.InvariantCulture));
        }
    }
}
