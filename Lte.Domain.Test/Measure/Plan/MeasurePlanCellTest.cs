using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Plan
{
    [TestFixture]
    public class MeasurePlanCellTest
    {
        private MeasurePlanCell smpCell;

        [SetUp]
        public void TestIntialize()
        {
            FakeMeasurableCell mmCell = new FakeMeasurableCell
            {
                OutdoorCell = new StubOutdoorCell(112, 22, 10),
                PciModx = 2,
                ReceivedRsrp = -10
            };

            smpCell = new MeasurePlanCell(mmCell);
        }

        [Test]
        public void TestGenerateMeasurePlanCell()
        {
            Assert.AreEqual(smpCell.Cell.Azimuth, 10);
            Assert.AreEqual(smpCell.PciModx, 2);
            Assert.AreEqual(smpCell.ReceivePower, 0.1);
        }

        [Test]
        public void TestUpdateRsrp_MeasurePlanCell()
        {
            FakeMeasurableCell mmCell = new FakeMeasurableCell
            {
                ReceivedRsrp = -10
            };

            smpCell.UpdateRsrpPower(mmCell);
            Assert.AreEqual(smpCell.ReceivePower, 0.2);
        }
    }
}
