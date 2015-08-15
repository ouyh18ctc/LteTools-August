using System;
using Lte.Evaluations.Dingli;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class RateStatTest
    {
        private LogRecord record;
        private HugelandRecord hRecord;
        private RateStat stat;

        [Test]
        public void TestRateStat_1()
        {
            record = new LogRecord()
            {
                Pci = 111,
                Sinr = 12,
                PdschTbCode0 = 12345,
                PdschTbCode1 = 23456,
                Time = DateTime.Parse("11:30:04")
            };
            hRecord = new HugelandRecord()
            {
                Pci = 111,
                Sinr = 12,
                PdschTbCode0 = 12345,
                PdschTbCode1 = 23456,
                Time = DateTime.Parse("2012-11-22 11:30:04")
            };
            stat = new RateStat();
            stat.Import(record);
            Assert.AreEqual(stat.Pci, 111);
            Assert.AreEqual(stat.Sinr, 12);
            Assert.AreEqual(stat.PdschTbCode0, 12345);
            Assert.AreEqual(stat.PdschTbCode1, 23456);
            Assert.AreEqual(stat.Time.ToLongTimeString(), "11:30:04");
            stat.Import(hRecord);
            Assert.AreEqual(stat.Pci, 111);
            Assert.AreEqual(stat.Sinr, 12);
            Assert.AreEqual(stat.PdschTbCode0, 12345);
            Assert.AreEqual(stat.PdschTbCode1, 0);
            Assert.AreEqual(stat.Time.ToLongTimeString(), "11:30:04");
            Assert.AreEqual(stat.Time.ToLongDateString(), "2012年11月22日");
        }

        [Test]
        public void TestRateStat_2()
        {
            record = new LogRecord()
            {
                DlThroughput = 123567,
                UlMcs = 12,
                PdschScheduledSlots = 17,
                Time = DateTime.Parse("2012-11-22 11:30:04.221")
            };
            hRecord = new HugelandRecord()
            {
                DlThroughput = 123567,
                UlMcs = 12
            };
            stat = new RateStat();
            stat.Import(record);
            Assert.AreEqual(stat.DlThroughput, 123567);
            Assert.AreEqual(stat.UlMcs, 12);
            Assert.AreEqual(stat.Time.Second, 4);
            Assert.AreEqual(stat.Time.Millisecond, 221);
            stat.Import(hRecord);
            Assert.AreEqual(stat.DlThroughput, 123567);
            Assert.AreEqual(stat.UlMcs, 12);
        }

    }
}
