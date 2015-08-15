using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Interference
{
    public class UpdateSameModInterferenceTestOneElementInCellList : InterferenceTester
    {
        public UpdateSameModInterferenceTestOneElementInCellList()
        {
            MeasurableCell mcell = new MeasurableCell();
            CellList = new List<MeasurableCell> {mcell};
            Result.StrongestCell = mcell;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
            Assert.IsNull(Result.StrongestInterference);
            Assert.AreEqual(Result.SameModInterferenceLevel, Double.MinValue);
        }
    }

    public class UpdateSameModInterferenceTestTwoSameModElementsInCellList
        : TwoCellCalculateSameModInterferenceTester
    {
        public UpdateSameModInterferenceTestTwoSameModElementsInCellList()
            : base(0, 0)
        {
            Mcell2.ReceivedRsrp = -12.3;
            CellList = new List<MeasurableCell> {Mcell1, Mcell2};
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 1);
            Assert.AreEqual(Result.StrongestInterference, Mcell2);
            Assert.AreEqual(Result.SameModInterferenceLevel, -12.3);
        }
    }

    public class UpdateSameModInterferenceTestTwoDifferentModElementsInCellList
        : TwoCellCalculateSameModInterferenceTester
    {
        public UpdateSameModInterferenceTestTwoDifferentModElementsInCellList()
            : base(0, 1)
        {
            Mcell2.ReceivedRsrp = -12.3;
            CellList = new List<MeasurableCell> { Mcell1, Mcell2 };
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 0);
            Assert.IsNull(Result.StrongestInterference);
            Assert.AreEqual(Result.SameModInterferenceLevel, Double.MinValue);
        }
    }

    public class UpdateSameModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod
        : ThreeCellCalculateSameModInterferenceTester
    {
        public UpdateSameModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod()
            : base(0, 1, 1)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2, Mcell3 };
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 0);
            Assert.IsNull(Result.StrongestInterference);
            Assert.AreEqual(Result.SameModInterferenceLevel, Double.MinValue);
        }
    }

    public class UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod
        : ThreeCellCalculateSameModInterferenceTester
    {
        public UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod()
            : base(0, 0, 1)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2, Mcell3 };
            Mcell2.ReceivedRsrp = -12.3;
            Mcell3.ReceivedRsrp = -12.3;
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 1);
            Assert.AreEqual(Result.StrongestInterference, Mcell2);
            Assert.AreEqual(Result.SameModInterferenceLevel, -12.3);
        }
    }

    public class UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod
        : ThreeCellCalculateSameModInterferenceTester
    {
        public UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod()
            : base(0, 0, 0)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2, Mcell3 };
            Mcell2.ReceivedRsrp = -10.3;
            Mcell3.ReceivedRsrp = -12.3;
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 2);
            Assert.AreEqual(Result.StrongestInterference, Mcell2);
            Assert.AreEqual(Result.SameModInterferenceLevel, -8.175574, 1E-6);
        }
    }
}
