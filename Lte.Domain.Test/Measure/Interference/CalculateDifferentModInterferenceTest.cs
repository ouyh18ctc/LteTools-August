using Lte.Domain.Measure;
using System.Collections.Generic;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Interference
{
    [TestFixture]
    public class CalculateDifferentModInterferenceTest
    {
        private SfMeasurePointResult result;
        private List<MeasurableCell> cellList;

        [SetUp]
        public void SetUp()
        {
            result = new SfMeasurePointResult();
            cellList = new List<MeasurableCell>();
        }

        [Test]
        public void TestDifferentModInterference_EmptyList()
        {
            IEnumerable<MeasurableCell> interferences = result.CalculateDifferentModInterferences(cellList);
            Assert.IsNull(interferences);
        }

        [Test]
        public void TestDifferentModInterference_OneElement_NullStrongestCell()
        {
            CalculateDifferentModTestOneElementNullStrongestCell tester
                = new CalculateDifferentModTestOneElementNullStrongestCell(result, cellList);
            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_OneElement_SameStrongestCell()
        {
            CalculateDifferentModTestOneElementSameStrongestCell tester
                = new CalculateDifferentModTestOneElementSameStrongestCell(result, cellList);
            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_OneElement_DifferentStrongestCells_SameMod3()
        {
            CalculateDifferentModTestOneElementDifferentStrongestCellsSameMod3 tester
                = new CalculateDifferentModTestOneElementDifferentStrongestCellsSameMod3(result, cellList);

            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_OneElement_DifferentStrongestCells_DifferentMod3()
        {
            CalculateDifferentModTestOneElementDifferentStrongestCellsDifferentMod3 tester
                = new CalculateDifferentModTestOneElementDifferentStrongestCellsDifferentMod3(result, cellList);

            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_TwoElements_OneSameStrongestCell_OtherCellSameMod3()
        {
            CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellSameMod3 tester
                = new CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellSameMod3(result, cellList);

            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_TwoElements_OneSameStrongestCell_OtherCellDifferentMod3()
        {
            CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3 tester
                = new CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3(result, cellList);

            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_ThreeElements_OneSameStrongestCell_SecondCellDifferentMod3_ThirdCellSameMod3()
        {
            CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3 tester
                = new CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3(
                    result, cellList);

            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

        [Test]
        public void TestDifferentModInterference_ThreeElements_OneSameStrongestCell_SecondCellDifferentMod3_ThirdCellDifferentMod3()
        {
            CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3 tester
                = new CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3(
                    result, cellList);

            IEnumerable<MeasurableCell> interference = result.CalculateDifferentModInterferences(cellList);
            tester.AssertValues(interference);
        }

    }
}
