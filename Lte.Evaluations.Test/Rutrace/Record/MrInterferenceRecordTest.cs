using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Record
{
    [TestFixture]
    public class MrInterferenceRecordTest
    {
        private readonly MrInterferenceRecord record=new MrInterferenceRecord();

        [SetUp]
        public void SetUp()
        {
            RuInterferenceRecord.InterferenceThreshold = 6;
        }

        [TestCase(1, 1, 50001, 48, 40, 50002, 48, 35)]
        [TestCase(1, 1, 50002, 48, 40, 50002, 48, 35)]
        [TestCase(1, 1, 50001, 49, 40, 50001, 49, 35)]
        [TestCase(1, 1, 50001, 49, 40, 50001, 49, 30)]
        [TestCase(2, 1, 50001, 49, 40, 50001, 49, 35)]
        [TestCase(3, 2, 50001, 49, 40, 50001, 49, 35)]
        public void Test_Original_OneInterference(
            int interferenceTimes, int measureTimes,
            int cellId, byte sectorId, byte refRsrp,
            int neiCellId, byte neiSectorId, byte neiRsrp)
        {
            record.Interferences = new List<MrInterference>
            {
                new MrInterference
                {
                    CellId = cellId,
                    SectorId = sectorId,
                    InterferenceTimes = interferenceTimes
                }
            };
            record.MeasuredTimes = measureTimes;
            MrRecord mrRecord = new MroRecord
            {
                RefCell = new MrReferenceCell
                {
                    Rsrp = refRsrp
                },
                NbCells = new List<MrNeighborCell>
                {
                    new MrNeighborCell
                    {
                        CellId = neiCellId,
                        SectorId = neiSectorId,
                        Rsrp = neiRsrp
                    }
                }
            };
            record.Import(mrRecord, x => true);
            Assert.AreEqual(record.MeasuredTimes, measureTimes + 1);
            if (neiRsrp > refRsrp - 6)
            {
                if (cellId == neiCellId && sectorId == neiSectorId)
                {
                    Assert.AreEqual(record.Interferences.Count, 1, "interference count");
                    Assert.AreEqual(record.Interferences[0].InterferenceTimes, interferenceTimes + 1);
                }
                else
                {
                    Assert.AreEqual(record.Interferences.Count, 2, "interference count");
                    Assert.AreEqual(record.Interferences[0].InterferenceTimes, interferenceTimes);
                    Assert.AreEqual(record.Interferences[1].InterferenceTimes, 1);
                }
            }
            else
            {
                Assert.AreEqual(record.Interferences.Count,1);
            }
        }
    }
}
