using System.Collections.Generic;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Rutrace.Service;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Service
{
    [TestFixture]
    public class CdrRtdRecordTest
    {
        private const string line = "460030926761151_A00000496E3E4E#;" +
                                    "D0_4096_20141017155007_558_2_78_53_40_32_126_25_26_0;" +
                                    "D0_4096_20141017155102_487_0_37_59_7_26_112_12_12_0";

        private void Import(List<CdrTaRecord> details, CdrRtdRecord record)
        {
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(details, record);
            service.Import();
            service = new ImportExcessCdrTaRecordsService(details, record);
            service.Import();
        }

        private void GenerateDetails(List<CdrTaRecord> details)
        {
            string[] segments = line.GetSplittedFields(';');
            CdrRtdRecord record = new CdrRtdRecord(segments[1].GetSplittedFields('_'));
            Import(details, record);

            record = new CdrRtdRecord(segments[2].GetSplittedFields('_'));
            Import(details, record);
        }

        private static void AssertDetails(List<CdrTaRecord> details)
        {
            Assert.AreEqual(details.Count, 2);
            Assert.AreEqual(details[0].CellId, 558);
            Assert.AreEqual(details[0].SectorId, 2);
            Assert.AreEqual(details[0].TaAverage, 1220);
            Assert.AreEqual(details[0].TaInnerIntervalExcessNum, 0, "inner excess");
            Assert.AreEqual(details[0].TaInnerIntervalNum, 1, "inner");
            Assert.AreEqual(details[0].TaSum, 1220);
            Assert.AreEqual(details[0].TaOuterIntervalExcessNum, 0, "outer excess");
            Assert.AreEqual(details[0].TaOuterIntervalNum, 0, "outer");
            Assert.AreEqual(details[0].TaMin, 1220);
        }

        private static void AssertRecords(List<CdrRtdRecord> records)
        {
            Assert.AreEqual(records.Count, 2);
            Assert.AreEqual(records[0].CellId, 558);
            Assert.AreEqual(records[0].SectorId, 2);
            Assert.AreEqual(records[0].Rtd, 1220);
        }

        [Test]
        public void TestCdrRtdRecord()
        {
            List<CdrRtdRecord> records = new List<CdrRtdRecord>();
            ImportCdrRtdRecordsService service = new ImportCdrRtdRecordsService(records, line);
            service.Import();
            AssertRecords(records);

            List<CdrTaRecord> details = new List<CdrTaRecord>();
            GenerateDetails(details);

            AssertDetails(details);
        }
    }
}
