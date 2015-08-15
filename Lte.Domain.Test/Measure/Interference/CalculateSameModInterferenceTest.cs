using NUnit.Framework;
using Lte.Domain.Measure;
using System.Collections.Generic;

namespace Lte.Domain.Test.Measure.Interference
{
    [TestFixture]
    public class CalculateSameModInterferenceTest
    {
        [Test]
        public void TestSameModInterference_OneElement_NullStrongestCell()
        {
            CalculateSameModTestOneElementNullStrongestCell tester
                = new CalculateSameModTestOneElementNullStrongestCell();
            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_OneElement_SameStrongestCell()
        {
            CalculateSameModTestOneElementSameStrongestCell tester
                = new CalculateSameModTestOneElementSameStrongestCell();
            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_OneElement_DifferentStrongestCells_SameMod3()
        {
            CalculateSameModTestOneElementDifferentStrongestCellsSameMod3 tester
                = new CalculateSameModTestOneElementDifferentStrongestCellsSameMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_OneElement_DifferentStrongestCells_DifferentMod3()
        {
            CalculateSameModTestOneElementDifferentStrongestCellsDifferentMod3 tester
                = new CalculateSameModTestOneElementDifferentStrongestCellsDifferentMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_TwoElements_OneSameStrongestCell_OtherCellSameMod3()
        {
            CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellSameMod3 tester
                = new CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellSameMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_TwoElements_OneSameStrongestCell_OtherCellDifferentMod3()
        {
            CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3 tester
                = new CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_ThreeElements_OneSameStrongestCell_SecondCellDifferentMod3_ThirdCellSameMod3()
        {
            CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3 tester
                = new CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_ThreeElements_OneSameStrongestCell_SecondCellSameMod3_ThirdCellSameMod3()
        {
            CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellSameMod3ThirdCellSameMod3 tester
                = new CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellSameMod3ThirdCellSameMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestSameModInterference_ThreeElements_OneSameStrongestCell_SecondCellDifferentMod3_ThirdCellDifferentMod3()
        {
            CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3 tester
                = new CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

    }
}
