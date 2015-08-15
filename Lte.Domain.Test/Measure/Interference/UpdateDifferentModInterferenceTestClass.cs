using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Interference
{
    public class UpdateDifferentModInterferenceTestOneElementInCellList : InterferenceTester
    {
        public UpdateDifferentModInterferenceTestOneElementInCellList()
        {
            MeasurableCell mcell = new MeasurableCell();
            CellList = new List<MeasurableCell> {mcell};
            Result.StrongestCell = mcell;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.IsNotNull(interference);
            Assert.AreEqual(interference.Count(), 0);
            Assert.AreEqual(Result.DifferentModInterferenceLevel, Double.MinValue);
        }
    }

    public class UpdateDifferentModInterferenceTestTwoSameModElementsInCellList
        : TwoCellCalculateSameModInterferenceTester
    {
        public UpdateDifferentModInterferenceTestTwoSameModElementsInCellList()
            : base(0, 0)
        {
            Mcell2.ReceivedRsrp = -12.3;
            CellList = new List<MeasurableCell> {Mcell1, Mcell2};
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 0);
            Assert.AreEqual(Result.DifferentModInterferenceLevel, Double.MinValue);
        }
    }

    public class UpdateDifferentModInterferenceTestTwoDifferentModElementsInCellList
        : TwoCellCalculateSameModInterferenceTester
    {
        public UpdateDifferentModInterferenceTestTwoDifferentModElementsInCellList()
            : base(0, 1)
        {
            Mcell2.ReceivedRsrp = -12.3;
            CellList = new List<MeasurableCell> {Mcell1, Mcell2};
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 1);
            Assert.AreEqual(interference.ElementAt(0), Mcell2);
            Assert.AreEqual(Result.DifferentModInterferenceLevel, -12.3);
        }
    }

    public class UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod
        : ThreeCellCalculateSameModInterferenceTester
    {
        public UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod()
            : base(0, 1, 1)
        {
            CellList = new List<MeasurableCell> {Mcell1, Mcell2, Mcell3};
            Mcell2.ReceivedRsrp = -12.3;
            Mcell3.ReceivedRsrp = -12.3;
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 2);
            Assert.AreEqual(interference.ElementAt(1), Mcell3);
            Assert.AreEqual(Result.DifferentModInterferenceLevel, -9.2897, 1E-6);
        }
    }

    public class UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod
        : ThreeCellCalculateSameModInterferenceTester
    {
        public UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod()
            : base(0, 0, 1)
        {
            Mcell2.ReceivedRsrp = -12.3;
            Mcell3.ReceivedRsrp = -12.3;
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 1);
            Assert.AreEqual(Result.DifferentModInterferenceLevel, -12.3);
        }
    }

    public class UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod
        : ThreeCellCalculateSameModInterferenceTester
    {
        public UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod()
            : base(0, 0, 0)
        {
            CellList = new List<MeasurableCell> { Mcell1, Mcell2, Mcell3 };
            Mcell2.ReceivedRsrp = -10.3;
            Mcell3.ReceivedRsrp = -12.3;
            Result.StrongestCell = Mcell1;
        }

        public override void AssertValues(IEnumerable<MeasurableCell> interference)
        {
            Assert.AreEqual(interference.Count(), 0);
            Assert.AreEqual(Result.DifferentModInterferenceLevel, Double.MinValue);
        }
    }
}
