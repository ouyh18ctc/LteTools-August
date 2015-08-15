using System;
using Lte.Evaluations.Dingli;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class CoverageStatTest
    {
        private LogRecord record;
        private HugelandRecord hRecord;
        private CoverageStat stat;

        [Test]
        public void TestCoverageStat()
        {
            record = new LogRecord()
            {
                Pci = 111,
                Sinr = 12,
                Rsrp = -110,
                PdschTbCode0 = 12345,
                PdschTbCode1 = 23456,
                Longtitute = 112.1,
                Lattitute = 23.2,
                Time = DateTime.Parse("11:30:04"),
                ENodebId = 1,
                SectorId = 1,
                Earfcn =100
            };
            hRecord = new HugelandRecord()
            {
                Pci = 111,
                Sinr = 12,
                Rsrp = -95,
                Longtitute = 112.3,
                Lattitute = 23.4,
                PdschTbCode0 = 12345,
                PdschTbCode1 = 23456,
                Time = DateTime.Parse("2012-11-22 11:30:04"),
                ENodebId = 1,
                SectorId = 2,
                Earfcn = 1825
            };
            stat = new CoverageStat();
            stat.Import(record);
            Assert.AreEqual(stat.Rsrp, -110);
            Assert.AreEqual(stat.Sinr, 12);
            Assert.AreEqual(stat.Longtitute, 112.1);
            Assert.AreEqual(stat.Lattitute, 23.2);
            Assert.AreEqual(stat.ENodebId, 1);
            Assert.AreEqual(stat.SectorId, 1);
            Assert.AreEqual(stat.Earfcn, 100);
            stat.Import(hRecord);
            Assert.AreEqual(stat.Rsrp, -95);
            Assert.AreEqual(stat.Sinr, 12);
            Assert.AreEqual(stat.Longtitute, 112.3);
            Assert.AreEqual(stat.Lattitute, 23.4);
            Assert.AreEqual(stat.ENodebId, 1);
            Assert.AreEqual(stat.SectorId, 2);
            Assert.AreEqual(stat.Earfcn, 1825);
        }
    }
}
