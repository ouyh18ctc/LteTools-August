using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Interference
{
    public class CalculateDifferentModTestOneElementNullStrongestCell : InterferenceTester
    {
        public CalculateDifferentModTestOneElementNullStrongestCell(SfMeasurePointResult result,
            List<MeasurableCell> cellList)
        {
            result.StrongestCell = null;
            cellList.Add(new MeasurableCell());
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNull(interference);
        }
    }

    public class CalculateDifferentModTestOneElementSameStrongestCell : InterferenceTester
    {
        public CalculateDifferentModTestOneElementSameStrongestCell(SfMeasurePointResult result,
            List<MeasurableCell> cellList)
        {
            MeasurableCell mcell = new MeasurableCell();
            cellList.Add(mcell);
            result.StrongestCell = mcell;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }

    public class CalculateDifferentModTestOneElementDifferentStrongestCellsSameMod3
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateDifferentModTestOneElementDifferentStrongestCellsSameMod3(SfMeasurePointResult result,
            List<MeasurableCell> cellList)
            : base(0, 0)
        {
            cellList.Add(Mcell1);
            result.StrongestCell = Mcell2;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }

    public class CalculateDifferentModTestOneElementDifferentStrongestCellsDifferentMod3
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateDifferentModTestOneElementDifferentStrongestCellsDifferentMod3(
            SfMeasurePointResult result, List<MeasurableCell> cellList)
            : base(0, 1)
        {
            cellList.Add(Mcell1);
            result.StrongestCell = Mcell2;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            IEnumerable<MeasurableCell> cells = interference as MeasurableCell[] ?? interference.ToArray();
            Assert.AreEqual(cells.Count(), 1);
            Assert.AreEqual(cells.ElementAt(0), Mcell1);
        }
    }

    public class CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellSameMod3
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellSameMod3(
            SfMeasurePointResult result, List<MeasurableCell> cellList)
            : base(0, 0)
        {
            cellList.Add(Mcell1);
            cellList.Add(Mcell2);
            result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }

    public class CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateDifferentModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3(
            SfMeasurePointResult result, List<MeasurableCell> cellList)
            : base(0, 1)
        {
            cellList.Add(Mcell1);
            cellList.Add(Mcell2);
            result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            var measurableCells = interference as MeasurableCell[] ?? interference.ToArray();
            Assert.AreEqual(measurableCells.Count(), 1);
            Assert.AreEqual(measurableCells.ElementAt(0), Mcell2);
        }
    }

    public class CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3
        : ThreeCellCalculateSameModInterferenceTester
    {
        public CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3(
            SfMeasurePointResult result, List<MeasurableCell> cellList)
            : base(0, 1, 0)
        {
            cellList.Add(Mcell1);
            cellList.Add(Mcell2);
            cellList.Add(Mcell3);
            result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            var measurableCells = interference as MeasurableCell[] ?? interference.ToArray();
            Assert.AreEqual(measurableCells.Count(), 1);
            Assert.AreEqual(measurableCells.ElementAt(0), Mcell2);
        }
    }

    public class CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3
        : ThreeCellCalculateSameModInterferenceTester
    { 
        public CalculateDifferentModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3(
            SfMeasurePointResult result, List<MeasurableCell> cellList)
            : base(0, 1, 2)
        {
            cellList.Add(Mcell1);
            cellList.Add(Mcell2);
            cellList.Add(Mcell3);
            result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            var measurableCells = interference as MeasurableCell[] ?? interference.ToArray();
            Assert.AreEqual(measurableCells.Count(), 2);
            Assert.AreEqual(measurableCells.ElementAt(1), Mcell3);
        }
    }
}
