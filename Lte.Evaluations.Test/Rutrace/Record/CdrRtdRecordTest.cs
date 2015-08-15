using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Record
{
    [TestFixture]
    public class CdrRtdRecordTest
    {
        private const double Eps = 1E-6;

        [TestCase("0,1,2,3,4,5,6,8", 3, 4, 244)]
        [TestCase("0,1,2,5,2,5,6,16", 5, 2, 488)]
        [TestCase("-1,1,2,5,2,5,6,16", 5, 2, 488)]
        [TestCase("-1,1,s,a,b,5,6,16", -1, 15, 488)]
        [TestCase("0,1,2,5,2,5,6,er", 5, 2, 0)]
        public void Test_Contructor_FromFields(string fields, int cellId, byte sectorId, double rtd)
        {
            CdrRtdRecord record = new CdrRtdRecord(fields.Split(','));
            Assert.AreEqual(record.CellId, cellId);
            Assert.AreEqual(record.SectorId, sectorId);
            Assert.AreEqual(record.Rtd, rtd, Eps);
        }

        [TestCase(1, 2, 7)]
        [TestCase(5, 9, 14)]
        [TestCase(1233, 48, 8)]
        [TestCase(75, 49, 7)]
        [TestCase(87, 50, 7)]
        [TestCase(90, 4, 7)]
        public void Test_Constructor_FromMrRecord(int cellId, byte sectorId, byte ta)
        {
            MrRecord mrRecord = new MroRecord
            {
                RefCell = new MrReferenceCell
                {
                    CellId = cellId,
                    SectorId = sectorId,
                    Ta = ta
                }
            };
            CdrRtdRecord record = new CdrRtdRecord(mrRecord);
            Assert.AreEqual(record.CellId, cellId);
            Assert.AreEqual(record.SectorId, sectorId);
            Assert.AreEqual(record.Rtd, ta * 78.12, Eps);
        }
    }
}
