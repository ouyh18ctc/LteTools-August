using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Budget
{
    [TestFixture]
    public class LinkBudgetTest
    {
        private ILinkBudget<double> budget;
        const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            IBroadcastModel model = new BroadcastModel();
            budget = new LinkBudget(model);
        }

        [Test]
        public void TestBudgetModel()
        {
            Assert.IsNotNull(budget.Model);
        }

        [Test]
        public void TestBudgetCalculate()
        {
            double x = budget.CalculateReceivedPower(1, 1);
            Assert.AreEqual(x, -126.172376, eps, x.ToString());
        }
    }
}
