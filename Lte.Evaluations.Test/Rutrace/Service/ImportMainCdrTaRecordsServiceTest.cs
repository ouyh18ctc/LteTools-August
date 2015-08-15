using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Rutrace;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Rutrace.Service;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Service
{
    [TestFixture]
    public class ImportMainCdrTaRecordsServiceTest : ImportCdrTaRecordsServiceTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            helper = new ImportMainCdrTaRecordsServiceTestHelper();
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportOneRecord_WithSmallRtd()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record = InitializeRecord(1, 2, 5.5);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record);
            service.Import();
            helper.AssertImportUniqueRecordResults(details, record);
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportTwoSameRecords_WithSmallRtd()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record = InitializeRecord(1, 2, 5.5);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record);
            service.Import();
            service.Import();
            helper.AssertImportTwoSameRecordsResults(details, record);
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportOneRecord_WithLargeRtd()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record = InitializeRecord(1, 2, 5500);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record);
            service.Import();
            helper.AssertImportUniqueRecordResults(details, record);
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportTwoSameRecords_WithLargeRtd()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record = InitializeRecord(1, 2, 5500);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record);
            service.Import();
            service.Import();
            helper.AssertImportTwoSameRecordsResults(details, record);
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportTwoRecordsWithSameCell()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record1 = InitializeRecord(1, 2, 5500);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record1);
            service.Import();
            CdrRtdRecord record2 = InitializeRecord(1, 2, 5.5);
            service = new ImportMainCdrTaRecordsService(details, record2);
            service.Import();
            helper.AssertImportTwoRecordsWithSameCellResults(details, record1, record2);
        }

        [Test]
        public void TestImport_EmptyDetailList_ImportTwoDifferentRecords()
        {
            InitializeEmptyDetailsList();
            CdrRtdRecord record1 = InitializeRecord(1, 2, 5500);
            ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                details, record1);
            service.Import();
            CdrRtdRecord record2 = InitializeRecord(2, 3, 5.5);
            service = new ImportMainCdrTaRecordsService(details, record2);
            service.Import();
            helper.AssertImportTwoDifferentRecordsResults(details, record1, record2);
        }
    }
}
