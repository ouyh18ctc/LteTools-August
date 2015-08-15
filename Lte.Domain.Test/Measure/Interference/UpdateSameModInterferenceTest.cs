using System;
using NUnit.Framework;
using Lte.Domain.Measure;
using System.Collections.Generic;

namespace Lte.Domain.Test.Measure.Interference
{
    [TestFixture]
    public class UpdateSameModInterferenceTest
    {
        [Test]
        public void TestUpdateSameModInterference_OneElementInCellList()
        {
            UpdateSameModInterferenceTestOneElementInCellList tester
                = new UpdateSameModInterferenceTestOneElementInCellList();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();

            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateSameModInterference_TwoSameModElementsInCellList()
        {
            UpdateSameModInterferenceTestTwoSameModElementsInCellList tester
                = new UpdateSameModInterferenceTestTwoSameModElementsInCellList();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();

            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateSameModInterference_TwoDifferentModElementsInCellList()
        {
            UpdateSameModInterferenceTestTwoDifferentModElementsInCellList tester
                = new UpdateSameModInterferenceTestTwoDifferentModElementsInCellList();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateSameModInterference_ThreeElements_First0Mod_Second1Mod_Third1Mod()
        {
            UpdateSameModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod tester
                = new UpdateSameModInterferenceTestThreeElementsFirst0ModSecond1ModThird1Mod();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateSameModInterference_ThreeElements_First0Mod_Second0Mod_Third1Mod()
        {
            UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod tester
                = new UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird1Mod();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }

        [Test]
        public void TestUpdateSameModInterference_ThreeElements_First0Mod_Second0Mod_Third0Mod()
        {
            UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod tester
                = new UpdateSameModInterferenceTestThreeElementsFirst0ModSecond0ModThird0Mod();

            IEnumerable<MeasurableCell> interference = tester.UpdateSameModInterference();
            tester.AssertValues(interference);
        }
    }
}
