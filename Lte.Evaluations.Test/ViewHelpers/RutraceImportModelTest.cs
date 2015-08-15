using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.ViewHelpers
{
    [TestFixture]
    public class RutraceImportModelTest
    {
        private readonly RutraceParametersModel model = new RutraceParametersModel();

        [SetUp]
        public void SetUp()
        {
            model.ResetDefaultValues();
        }

        [Test]
        public void TestRutraceImportModel_ReadData_DefaultValues()
        {
            model.ReadData();
            Assert.AreEqual(model.InterferenceThreshold, 6.0);
        }

        [TestCase(2.0)]
        [TestCase(3.0)]
        [TestCase(4.0)]
        [TestCase(7.0)]
        [TestCase(10.0)]
        [TestCase(100.0)]
        public void TestRutraceImportModel_ReadData_InterferenceThreshold(double threshold)
        {
            RuInterferenceRecord.InterferenceThreshold = threshold;
            model.ReadData();
            Assert.AreEqual(model.InterferenceThreshold, threshold);
            Assert.AreEqual(model.TaLowerBound, 780);
            Assert.AreEqual(model.TaUpperRatio, 3);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestRutraceImportModel_ReadData_InterferenceStat_UpperBound(int ratio)
        {
            InterferenceStat.UpperBound = 780 * ratio;
            model.ReadData();
            Assert.AreEqual(model.TaUpperRatio, ratio, 1E-6);
        }

        [TestCase(55, 1000, 5)]
        [TestCase(550, 1000, 5)]
        [TestCase(5, 1000, 5)]
        [TestCase(55, 100, 5)]
        [TestCase(66, 2300, 5)]
        public void TestRutraceImportModel_WriteData(double threshold, double low, double upperRatio)
        {
            model.RatioThreshold = threshold;
            model.TaLowerBound = low;
            model.TaUpperRatio = upperRatio;
            model.WriteData();
            Assert.AreEqual(RuInterferenceStat.RatioThreshold, threshold/100, 1E-6);
            Assert.AreEqual(InterferenceStat.LowerBound, low);
            Assert.AreEqual(InterferenceStat.UpperBound, low*upperRatio);
        }

        [TearDown]
        public void TearDown()
        {
            model.ResetDefaultValues();
        }
    }
}
