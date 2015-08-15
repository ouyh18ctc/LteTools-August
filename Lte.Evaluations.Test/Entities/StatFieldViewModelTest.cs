using System.Collections.Generic;
using Lte.Evaluations.Entities;
using Lte.Evaluations.ViewHelpers;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatFieldViewModelTest
    {
        private StatValueField field = new StatValueField();
        private StatFieldViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            field.IntervalList = new List<StatValueInterval>();
        }

        [TestCase(3, new[] { 1.0, 2, 3, 4 }, new[] { 1, 1.5, 2.1, 2.8, 3.1, 4, 4.1 }, new[] { 2, 2, 1 })]
        [TestCase(4, new[] { 1.0, 2, 3, 4, 5 }, new[] { 1, 1.5, 2.1, 2.8, 3.1, 4, 4.1, 2.2, 3.3, 4.3 }, 
            new[] { 2, 3, 2, 3 })]
        public void Test(int intervals, double[] levels, double[] values, int[] intervalCounts)
        {
            for (int i = 0; i < intervals; i++)
            {
                field.IntervalList.Add(new StatValueInterval
                {
                    IntervalLowLevel = levels[i],
                    IntervalUpLevel = levels[i + 1]
                });
            }
            viewModel = new StatFieldViewModel(field, values);
            List<StatValueIntervalSetting> settingList = viewModel.IntervalSettingList;
            Assert.AreEqual(settingList.Count, intervals);
            for (int i = 0; i < intervals; i++)
            {
                Assert.AreEqual(settingList[i].IntervalLowLevel, field.IntervalList[i].IntervalLowLevel);
                Assert.AreEqual(settingList[i].IntervalUpLevel, field.IntervalList[i].IntervalUpLevel);
                Assert.AreEqual(settingList[i].SuggestUpLevel, field.IntervalList[i].IntervalUpLevel);
                Assert.AreEqual(settingList[i].RecordsCount, intervalCounts[i]);
            }
        }
    }
}
