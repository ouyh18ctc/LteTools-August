using System.Collections.Generic;
using Lte.Domain.Measure;
using System.Linq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Interference
{
    public class CalculateSameModTestOneElementNullStrongestCell : InterferenceTester
    {
        public CalculateSameModTestOneElementNullStrongestCell()
        {
            Result.StrongestCell = null;
            MeasurableCell mcell = new MeasurableCell();
            CellList = new List<MeasurableCell> {mcell};
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNull(interference);
        }
    }

    public class CalculateSameModTestOneElementSameStrongestCell : InterferenceTester
    {
        public CalculateSameModTestOneElementSameStrongestCell()
        {
            MeasurableCell mcell = new MeasurableCell();
            CellList = new List<MeasurableCell> { mcell };
            Result.StrongestCell = mcell;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }

    public class CalculateSameModTestOneElementDifferentStrongestCellsSameMod3 
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestOneElementDifferentStrongestCellsSameMod3()
            : base(0, 0)
        {
            CellList = new List<MeasurableCell> { Mcell1 };
            Result.StrongestCell = Mcell2;

        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 1);
            Assert.AreEqual(interference.ElementAt(0).Cell.PciModx, 0);
        }
    }

    public class CalculateSameModTestOneElementDifferentStrongestCellsDifferentMod3 
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestOneElementDifferentStrongestCellsDifferentMod3()
            : base(0, 1)
        {
            CellList = new List<MeasurableCell> {Mcell1};
            Result.StrongestCell = Mcell2;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }

    public class CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellSameMod3 
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellSameMod3()
            : base(0, 0)
        {
            CellList = new List<MeasurableCell> {Mcell1, Mcell2};
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 1);
            Assert.AreEqual(interference.ElementAt(0), Mcell2);
        }
    }

    public class CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3
        : TwoCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestTwoElementsOneSameStrongestCellOtherCellDifferentMod3()
            : base(0, 1)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2 };
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }

    public class CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3
        : ThreeCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellSameMod3()
            : base(0, 1, 0)
        {
            CellList = new List<MeasurableCell> {Mcell1, Mcell2, Mcell3};
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            IEnumerable<MeasurableCell> measurableCells = interference as MeasurableCell[] ?? interference.ToArray();
            Assert.AreEqual(measurableCells.Count(), 1);
            Assert.AreEqual(measurableCells.ElementAt(0), Mcell3);
        }
    }

    public class CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellSameMod3ThirdCellSameMod3
        : ThreeCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellSameMod3ThirdCellSameMod3()
            : base(0, 0, 0)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2, Mcell3 };
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 2);
        }
    }

    public class CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3
        : ThreeCellCalculateSameModInterferenceTester
    {
        public CalculateSameModTestThreeElementsOneSameStrongestCellSecondCellDifferentMod3ThirdCellDifferentMod3()
            : base(0, 1, 2)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2, Mcell3 };
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
        }
    }
}
