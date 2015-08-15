using NUnit.Framework;
using Lte.Domain.Measure;
using System.Collections.Generic;

namespace Lte.Domain.Test.Measure.Interference
{
    [TestFixture]
    public class UpdateDifferentModInterferenceTest
    {
        [Test]
        public void TestUpdateDifferentModInterference_OneElementInCellList()
        {
            UpdateDifferentModInterferenceTestOneElementInCellList tester
                = new UpdateDifferentModInterferenceTestOneElementInCellList();
            IEnumerable<MeasurableCell> interference = tester.UpdateDifferentModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateDifferentModInterference_TwoSameModElementsInCellList()
        {
            UpdateDifferentModInterferenceTestTwoSameModElementsInCellList tester
                = new UpdateDifferentModInterferenceTestTwoSameModElementsInCellList();
            IEnumerable<MeasurableCell> interference = tester.UpdateDifferentModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateDifferentModInterference_TwoDifferentModElementsInCellList()
        {
            UpdateDifferentModInterferenceTestTwoDifferentModElementsInCellList tester
                = new UpdateDifferentModInterferenceTestTwoDifferentModElementsInCellList();
            IEnumerable<MeasurableCell> interference = tester.UpdateDifferentModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateDifferentModInterference_ThreeElements_First0Mod_Second1Mod_Third1Mod()
        {
            UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod tester
                = new UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod();
            IEnumerable<MeasurableCell> interference = tester.UpdateDifferentModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateDifferentModInterference_ThreeElements_First0Mod_Second0Mod_Third1Mod()
        {
            UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod tester
                = new UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod();
            IEnumerable<MeasurableCell> interference = tester.UpdateDifferentModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateDifferentModInterference_ThreeElements_First0Mod_Second0Mod_Third0Mod()
        {
            UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod tester
                = new UpdateDifferentModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod();
            IEnumerable<MeasurableCell> interference = tester.UpdateDifferentModInterference();
            tester.AssertValues(interference);
        }
    }
}
