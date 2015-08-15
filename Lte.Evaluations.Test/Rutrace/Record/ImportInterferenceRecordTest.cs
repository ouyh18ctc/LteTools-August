using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Record
{
    public class FakeInterference : IInterference
    {
        public int CellId { get; set; }
        public byte SectorId { get; set; }
        public int InterferenceTimes { get; set; }

        public FakeInterference()
        {
            InterferenceTimes = 0;
        }

        public FakeInterference(FakeNeighborCell neiCell) : this()
        {
            CellId = neiCell.CellId;
            SectorId = neiCell.SectorId;
        }
    }

    public class FakeReferenceCell : IRefCell
    {
        public int CellId { get; set; }
        public byte SectorId { get; set; }
        public short Frequency { get; set; }
         
        public double Rsrp { get; set; }

        public double Strength
        {
            get { return Rsrp; }
        }
    }

    public class FakeNeighborCell : INeiCell
    {
        public int CellId { get; set; }
        public byte SectorId { get; set; }

        public short Frequency
        {
            get { return SectorId > 30 ? (short)1825 : (short)100; }
        }

        public double Rsrp { get; set; }

        public double Strength
        {
            get { return Rsrp; }
        }
    }

    [TestFixture]
    public class ImportInterferenceRecordTest
    {
        private readonly Mock<IInterferenceRecord<FakeInterference>> mockInterferenceRecord =
            new Mock<IInterferenceRecord<FakeInterference>>();

        [SetUp]
        public void SetUp()
        {
            RuInterferenceRecord.InterferenceThreshold = 6;
            mockInterferenceRecord.MockSetters();
        }

        [TestCase(50001, 0, -102, 0, 0, 50001, 1, -105)]
        [TestCase(50001, 0, -102, 0, 0, 50001, 1, -110)]
        [TestCase(50001, 0, -102, 0, 0, 50001, 0, -105)]
        public void Test_OrinalOneCell(int cellId, byte sectorId, double refRsrp,
            int measuredTimes, int interferenceTimes,
            int neiCellId, byte neiSectorId, double neiRsrp)
        {
            mockInterferenceRecord.SetupGet(x => x.MeasuredTimes).Returns(measuredTimes);
            mockInterferenceRecord.Setup(x => x.Interferences).Returns(new List<FakeInterference>
            {
                new FakeInterference
                {
                    CellId = cellId,
                    SectorId = sectorId,
                    InterferenceTimes = interferenceTimes
                }
            });
            FakeNeighborCell neiCell = new FakeNeighborCell
            {
                CellId = neiCellId,
                SectorId = neiSectorId,
                Rsrp = neiRsrp
            };
            FakeReferenceCell refCell = new FakeReferenceCell
            {
                Rsrp = refRsrp
            };

            FakeInterference interference = mockInterferenceRecord.Object.Import(neiCell, refCell, x => true,
                x => new FakeInterference(x));
            Assert.AreEqual(mockInterferenceRecord.Object.MeasuredTimes, measuredTimes + 1);
            if (neiRsrp > refRsrp - 6)
            {
                if (cellId == neiCellId && sectorId == neiSectorId)
                {
                    Assert.AreEqual(interference.InterferenceTimes, interferenceTimes + 1);
                    Assert.AreEqual(mockInterferenceRecord.Object.Interferences.Count,1);
                }
                else
                {
                    Assert.AreEqual(interference.InterferenceTimes, 1);
                    Assert.AreEqual(mockInterferenceRecord.Object.Interferences.Count, 2);
                }
            }
            else Assert.IsNull(interference);
        }

        [TestCase(new[] { 50001, 50002 }, new byte[] { 0, 0 }, -102, 0, new[] { 0, 0 }, 50001, 1, -105)]
        [TestCase(new[] { 50001, 50003 }, new byte[] { 1, 0 }, -102, 0, new[] { 0, 0 }, 50001, 1, -105)]
        [TestCase(new[] { 50001, 50002 }, new byte[] { 1, 0 }, -102, 0, new[] { 0, 0 }, 50001, 1, -110)]
        [TestCase(new[] { 50001, 50002 }, new byte[] { 0, 0 }, -102, 0, new[] { 0, 0 }, 50002, 1, -105)]
        [TestCase(new[] { 50001, 50002 }, new byte[] { 1, 2 }, -102, 0, new[] { 0, 0 }, 50001, 1, -105)]
        public void Test_OrinalTwoCells(int[] cellIds, byte[] sectorIds, double refRsrp,
            int measuredTimes, int[] interferenceTimes,
            int neiCellId, byte neiSectorId, double neiRsrp)
        {
            mockInterferenceRecord.SetupGet(x => x.MeasuredTimes).Returns(measuredTimes);
            mockInterferenceRecord.Setup(x => x.Interferences).Returns(new List<FakeInterference>
            {
                new FakeInterference
                {
                    CellId = cellIds[0],
                    SectorId = sectorIds[0],
                    InterferenceTimes = interferenceTimes[0]
                },
                new FakeInterference
                {
                    CellId = cellIds[1],
                    SectorId = sectorIds[1],
                    InterferenceTimes = interferenceTimes[1]
                }
            });
            FakeNeighborCell neiCell = new FakeNeighborCell
            {
                CellId = neiCellId,
                SectorId = neiSectorId,
                Rsrp = neiRsrp
            };
            FakeReferenceCell refCell = new FakeReferenceCell
            {
                Rsrp = refRsrp
            };

            FakeInterference interference = mockInterferenceRecord.Object.Import(neiCell, refCell, x => true,
                x => new FakeInterference(x));
            Assert.AreEqual(mockInterferenceRecord.Object.MeasuredTimes, measuredTimes + 1);
            if (neiRsrp > refRsrp - 6)
            {
                if (cellIds[0] == neiCellId && sectorIds[0] == neiSectorId)
                {
                    Assert.AreEqual(interference.InterferenceTimes, interferenceTimes[0] + 1);
                    Assert.AreEqual(mockInterferenceRecord.Object.Interferences.Count, 2);
                }
                else if (cellIds[1] == neiCellId && sectorIds[1] == neiSectorId)
                {
                    Assert.AreEqual(interference.InterferenceTimes, interferenceTimes[1] + 1);
                    Assert.AreEqual(mockInterferenceRecord.Object.Interferences.Count, 2);
                }
                else
                {
                    Assert.AreEqual(interference.InterferenceTimes, 1);
                    Assert.AreEqual(mockInterferenceRecord.Object.Interferences.Count, 3);
                }
            }
            else Assert.IsNull(interference);
        }
    }
}
