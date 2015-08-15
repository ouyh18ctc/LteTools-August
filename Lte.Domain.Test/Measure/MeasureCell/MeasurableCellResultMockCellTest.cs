using System;
using System.Collections.Generic;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.MeasureCell
{
    [TestFixture]
    public class MeasurableCellResultMockCellTest
    {
        private MeasurableCell strongestCell = new MeasurableCell();
        private SfMeasurePointResult result = new SfMeasurePointResult();
        private List<MeasurableCell> cellList = new List<MeasurableCell>();
        const double Eps = 1E-6;

        private void ConstructStrongestCell()
        {
            strongestCell.Cell.PciModx = 0;
            cellList = new List<MeasurableCell> {strongestCell};
        }

        private MeasurableCell AddSameModInterference(double strength = -12.3)
        {

            MeasurableCell sameModInterference = new MeasurableCell();
            sameModInterference.Cell.PciModx = 0;
            sameModInterference.ReceivedRsrp = strength;
            cellList.Add(sameModInterference);
            return sameModInterference;
        }

        private void AddDifferentModInterference()
        {

            MeasurableCell diffModInterference = new MeasurableCell();
            diffModInterference.Cell.PciModx = 1;
            diffModInterference.ReceivedRsrp = -12.3;

            cellList.Add(diffModInterference);
        }

        [SetUp]
        public void TestInitialize()
        {
            strongestCell.ReceivedRsrp = -12.3;
            result.StrongestCell = strongestCell;
        }

        [Test]
        public void TestMeasurableCellResult_EmptyCellList()
        {
            result.CalculateInterference(cellList, 0.1);
            Assert.IsNull(result.StrongestInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.TotalInterferencePower, Double.MinValue);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell()
        {
            ConstructStrongestCell();
            result.CalculateInterference(cellList, 0.1);
            Assert.IsNull(result.StrongestInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.TotalInterferencePower, Double.MinValue);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_OneSameModInterference()
        {
            ConstructStrongestCell();
            MeasurableCell sameModInterference = AddSameModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -12.3);
            Assert.AreEqual(result.DifferentModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.TotalInterferencePower, -12.3);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_TwoSameModInterferencesWithFirstBigger()
        {
            ConstructStrongestCell();
            MeasurableCell sameModInterference = AddSameModInterference();
            AddSameModInterference(-13.3);

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -9.760981, Eps);
            Assert.AreEqual(result.DifferentModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.TotalInterferencePower, -9.760981, Eps);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_TwoSameModInterferencesWithSecondBigger()
        {
            ConstructStrongestCell();
            AddSameModInterference(-13.3);
            MeasurableCell sameModInterference = AddSameModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -9.760981, Eps);
            Assert.AreEqual(result.DifferentModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.TotalInterferencePower, -9.760981, Eps);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_OneDifferentModInterference()
        {
            ConstructStrongestCell();
            AddDifferentModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.IsNull(result.StrongestInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -12.3);
            Assert.AreEqual(result.TotalInterferencePower, -22.3);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_TwoDifferentModInterferences()
        {
            ConstructStrongestCell();
            AddDifferentModInterference();
            AddDifferentModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.IsNull(result.StrongestInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -9.2897, Eps);
            Assert.AreEqual(result.TotalInterferencePower, -19.2897, Eps);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_OneSameModInterference_OneDifferentModInterference()
        {
            ConstructStrongestCell();
            MeasurableCell sameModInterference = AddSameModInterference();
            AddDifferentModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -12.3);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -12.3);
            Assert.AreEqual(result.TotalInterferencePower, -11.886073, Eps);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_TwoSameModInterferences_OneDifferentModInterference()
        {
            ConstructStrongestCell();
            MeasurableCell sameModInterference = AddSameModInterference();
            AddSameModInterference(-13.3);
            AddDifferentModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -9.760981, Eps);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -12.3);
            Assert.AreEqual(result.TotalInterferencePower, -9.525448, Eps);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_OneSameModInterference_TwoDifferentModInterferences()
        {
            ConstructStrongestCell();
            MeasurableCell sameModInterference = AddSameModInterference();
            AddDifferentModInterference();
            AddDifferentModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -12.3);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -9.2897, Eps);
            Assert.AreEqual(result.TotalInterferencePower, -11.508188, Eps);
        }

        [Test]
        public void TestMeasurableCellResult_OneSameStrongestCell_TwoSameModInterferences_TwoDifferentModInterferences()
        {
            ConstructStrongestCell();
            MeasurableCell sameModInterference = AddSameModInterference();
            AddSameModInterference(-13.3);
            AddDifferentModInterference();
            AddDifferentModInterference();

            result.CalculateInterference(cellList, 0.1);

            Assert.AreEqual(result.StrongestInterference, sameModInterference);
            Assert.AreEqual(result.SameModInterferenceLevel, -9.760981, Eps);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -9.2897, Eps);
            Assert.AreEqual(result.TotalInterferencePower, -9.302034, Eps);
        }

    }
}
