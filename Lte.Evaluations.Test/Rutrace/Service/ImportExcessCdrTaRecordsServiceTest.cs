using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Rutrace.Service;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Service
{
    [TestFixture]
    public class ImportExcessCdrTaRecordsServiceTest : ImportCdrTaRecordsServiceTestConfig
    {
        public ImportExcessCdrTaRecordsServiceTest()
        {
            helper = new ImportExcessCdrTaRecordsServiceTestHelper();
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportOneRecord()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record = InitializeRecord(1, 2, 5.5);
            ImportCdrTaRecordsService service = new ImportExcessCdrTaRecordsService(
                details, record);
            service.Import();
            Assert.AreEqual(details.Count, 0);
        }

        [Test]
        public void TestImport_OneElementDetailList_ImportSameRecord()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record = InitializeRecord(1, 2, 5.5);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record);
            service.Import();
            service = new ImportExcessCdrTaRecordsService(
                details, record);
            service.Import();
            helper.AssertImportUniqueRecordResults(details, record);
        }

        [Test]
        public void TestImport_OneElementDetailList_ImportDifferentRecord()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record1 = InitializeRecord(1, 2, InterferenceStat.LowerBound/2);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record1);
            service.Import();
            for (int rtd = 800; rtd < 3000; rtd += 100)
            {

                CdrRtdRecord record2 = InitializeRecord(3, 4, rtd);
                service = new ImportExcessCdrTaRecordsService(
                    details, record2);
                service.Import();
                helper.AssertImportUniqueRecordResults(details, record1);
            }
        }

        [Test]
        public void TestImport_OneElementDetailList_ImportOneRecord_WithSameCell()
        {
            for (int rtd = 800; rtd < 3000; rtd += 100)
            {
                InitializeEmptyDetailsList();
                CdrRtdRecord record1 = InitializeRecord(1, 2, InterferenceStat.LowerBound/2);
                ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                    details, record1);
                service.Import();
                CdrRtdRecord record2 = InitializeRecord(1, 2, rtd);
                service = new ImportExcessCdrTaRecordsService(
                    details, record2);
                service.Import();
                helper.AssertImportUniqueRecordResults(details, record1, record2);
            }
        }
    }
}
