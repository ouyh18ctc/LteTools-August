using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi
{
    [TestFixture]
    public class AlarmHourDistributionTest
    {
        private AlarmHourDistribution distribution;

        [SetUp]
        public void SetUp()
        {
            distribution = new AlarmHourDistribution();
        }

        [Test]
        public void Test_Original()
        {
            Assert.AreEqual(distribution.AlarmRecords.Count, 0);
        }

        [TestCase(3, 10, "RSSI问题")]
        [TestCase(3, 10, "传输问题")]
        [TestCase(2, 5, "传输问题")]
        [TestCase(7, 12, "小区退服")]
        [TestCase(0, 112, "驻波比问题")]
        public void Test_AddOneAlarm(short hour, int alarms, string type)
        {
            distribution.Import(new List<AlarmHourInfo>
            {
                new AlarmHourInfo
                {
                    Hour = hour,
                    Alarms = alarms,
                    AlarmType = type.GetAlarmType()
                }
            });
            Assert.AreEqual(distribution.AlarmRecords.Count, 1);
            Assert.AreEqual(distribution.AlarmRecords[type][hour], alarms);
        }

        [TestCase(new short[] { 3, 5 }, new[] { 10, 7 }, "传输问题")]
        [TestCase(new short[] { 3, 9 }, new[] { 11, 2 }, "RSSI问题")]
        [TestCase(new short[] { 3, 9, 4 }, new[] { 11, 2, 8 }, "RSSI问题")]
        [TestCase(new short[] { 3, 9, 4, 6 }, new[] { 11, 2, 8, 25 }, "RSSI问题")]
        public void Test_AddSameProblems(short[] hour, int[] alarms, string type)
        {
            distribution.Import(hour.Select((t, i) => new AlarmHourInfo
            {
                Hour = t, Alarms = alarms[i], AlarmType = type.GetAlarmType()
            }).ToList());
            Assert.AreEqual(distribution.AlarmRecords.Count, 1);
            for (int i = 0; i < hour.Length; i++)
            {
                Assert.AreEqual(distribution.AlarmRecords[type][hour[i]], alarms[i]);
            }
        }

        [TestCase(new short[] { 3, 5 }, new[] { 10, 7 }, new[] { "传输问题", "驻波比问题" })]
        [TestCase(new short[] { 3, 5, 2 }, new[] { 10, 7, 28 },
            new[] { "传输问题", "驻波比问题", "驻波比问题" })]
        [TestCase(new short[] { 3, 5, 2 }, new[] { 10, 7, 28 },
            new[] { "传输问题", "驻波比问题", "小区退服" })]
        [TestCase(new short[] { 3, 5, 2, 8 }, new[] { 10, 7, 28, 37 },
            new[] { "传输问题", "驻波比问题", "小区退服", "传输问题" })]
        [TestCase(new short[] { 3, 5, 2, 8, 9 }, new[] { 10, 7, 28, 37, 36 },
            new[] { "传输问题", "驻波比问题", "小区退服", "传输问题", "不影响业务问题" })]
        public void Test_DifferentProblems(short[] hour, int[] alarms, string[] type)
        {
            distribution.Import(hour.Select((t, i) => new AlarmHourInfo
            {
                Hour = t,
                Alarms = alarms[i],
                AlarmType = type[i].GetAlarmType()
            }).ToList());
            Assert.AreEqual(distribution.AlarmRecords.Count, type.Distinct().Count());
            for (int i = 0; i < hour.Length; i++)
            {
                Assert.AreEqual(distribution.AlarmRecords[type[i]][hour[i]], alarms[i]);
            }
        }
    }
}
