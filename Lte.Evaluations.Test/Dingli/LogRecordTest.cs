using System.Collections.Generic;
using System.Linq;
using Lte.Domain.LinqToCsv.Description;
using Lte.Evaluations.Dingli;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Mapper;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class LogRecordTest
    {
        private CsvFileDescription _fileDescriptionNamesUs;
        private string _testInput;

        [SetUp]
        public void TestInitialize()
        {
            _fileDescriptionNamesUs = new CsvFileDescription
            {
                SeparatorChar = '\t',
                IgnoreUnknownColumns = true,
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            _testInput = @"Index	Time	Longitude	Latitude	eNodeBID	SectorID	Cell ID	PCI	RSRP (dBm)	SINR (dB)	PDSCH BLER	WideBand CQI	MCS Average UL /s	MCS Average DL /s	PDCP Throughput DL (bps)	PDCP Throughput UL (bps)	Event	Message Type	
0	13:58:08:359	-9999	-9999	491308	48	49130848	276	-102.18	10.10	9.60	10	17	14	10749096	223680		Inherit Params Set(Event)";
        }

        [Test]
        public void TestLogRecord_FullFields()
        {
           
            FieldMapperReading<LogRecord> fm = new FieldMapperReading<LogRecord>(_fileDescriptionNamesUs, null, false);
            Assert.IsNotNull(fm);
            Assert.IsNotNull(fm.FieldIndexInfo);
            Assert.IsNotNull(fm.NameToInfo);
            Assert.AreEqual(fm.NameToInfo.Count, 27);
            Assert.IsNotNull(fm.NameToInfo["PCI"]);
            Assert.AreEqual(fm.NameToInfo["PCI"].HasColumnAttribute, true);
            List<int> charLengths = fm.GetCharLengths();
            Assert.IsNull(charLengths);

            FileDataAccess dataAccess = new FileDataAccess(_testInput.GetStreamReader(), _fileDescriptionNamesUs);
            Assert.IsNotNull(dataAccess);
            RowReader<LogRecord> reader = dataAccess.ReadDataPreparation<LogRecord>(null);
            Assert.IsNotNull(reader);

            Assert.IsNotNull(dataAccess.Cs);
            dataAccess.Row = new DataRow();
            Assert.IsTrue(dataAccess.Cs.ReadRow(dataAccess.Row));

            bool readingResult = reader.ReadingOneFieldRow(fm, dataAccess.Row, true);
            Assert.IsFalse(readingResult);

            Assert.IsTrue(dataAccess.Cs.ReadRow(dataAccess.Row));
            Assert.AreEqual(dataAccess.Row[0].Value, "0");
            Assert.AreEqual(dataAccess.Row[1].Value, "13:58:08:359");
            Assert.AreEqual(dataAccess.Row.Count, 18, "row count");
            Assert.AreEqual(fm.FieldIndexInfo.IndexToInfo.Length, 27, "index to info");

            Assert.AreEqual(fm.FieldIndexInfo.GetMaxRowCount(18), 18);

            TypeFieldInfo tfi = fm.FieldIndexInfo.QueryTypeFieldInfo(true, 1);
            Assert.IsNotNull(tfi);
            Assert.AreEqual(tfi.OutputFormat, "HH:mm:ss.fff");

            string value = dataAccess.Row[1].Value;
            Assert.AreEqual(value, "13:58:08:359");

        }

        [Test]
        public void TestLogRecord_FullFields_2()
        {

            FieldMapperReading<LogRecord> fm = new FieldMapperReading<LogRecord>(_fileDescriptionNamesUs, null, false);
            Assert.IsNotNull(fm);
            Assert.IsNotNull(fm.FieldIndexInfo);
            Assert.IsNotNull(fm.NameToInfo);
            Assert.AreEqual(fm.NameToInfo.Count, 27);
            Assert.IsNotNull(fm.NameToInfo["PCI"]);
            Assert.AreEqual(fm.NameToInfo["PCI"].HasColumnAttribute, true);
            List<int> charLengths = fm.GetCharLengths();
            Assert.IsNull(charLengths);

            FileDataAccess dataAccess = new FileDataAccess(_testInput.GetStreamReader(), _fileDescriptionNamesUs);
            Assert.IsNotNull(dataAccess);
            RowReader<LogRecord> reader = dataAccess.ReadDataPreparation<LogRecord>(null);
            Assert.IsNotNull(reader);
            dataAccess.Row = new DataRow();

            List<LogRecord> records = dataAccess.ReadFieldDataRows(reader, null, fm, null).ToList();

            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count, 1);
            Assert.AreEqual(records[0].Id, 0);
            Assert.AreEqual(records[0].Time.Millisecond, 359);
            Assert.AreEqual(records[0].Sinr, 10.1);
            Assert.AreEqual(records[0].UlMcs, 17);
            Assert.AreEqual(records[0].DlThroughput, 10749096);
        }

        [Test]
        public void TestLogRecord_SomeEmptyFields()
        {
            _testInput = @"Index	Time	Longitude	Latitude	eNodeBID	SectorID	Cell ID	PCI	RSRP (dBm)	SINR (dB)	PDSCH BLER	WideBand CQI	MCS Average UL /s	MCS Average DL /s	PDCP Throughput DL (bps)	PDCP Throughput UL (bps)	Event	Message Type	
41874	14:15:41:078	114.322150862471	22.6803021794872	489835	48	48983548	355			10.32			9			RRC Connection Reestablish RequestLTE Handover Failure;	LTE RRC-->Connection Reestablishment Request";
            List<LogRecord> records = CsvContext.ReadString<LogRecord>(_testInput, _fileDescriptionNamesUs).ToList();
            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count, 1);
            Assert.AreEqual(records[0].Pci, 355);
            Assert.AreEqual(records[0].Rsrp, -9999);
        }

        [Test]
        public void TestLogRecord_EmptyPci()
        {
            _testInput = @"Index	Time	Longitude	Latitude	eNodeBID	SectorID	Cell ID	PCI	RSRP (dBm)	SINR (dB)	PDSCH BLER	WideBand CQI	MCS Average UL /s	MCS Average DL /s	PDCP Throughput DL (bps)	PDCP Throughput UL (bps)	Event	Message Type	
41874	14:15:41:078	114.322150862471	22.6803021794872	489835	48	48983548				10.32			9			RRC Connection Reestablish RequestLTE Handover Failure;	LTE RRC-->Connection Reestablishment Request";
            List<LogRecord> records = CsvContext.ReadString<LogRecord>(_testInput, _fileDescriptionNamesUs).ToList();
            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count, 1);
            Assert.AreEqual(records[0].Pci, -1);
            Assert.AreEqual(records[0].Rsrp, -9999);
        }

        [Test]
        public void TestLogRecord_RbScheduled()
        {
            _testInput = @"Index	Time	Longitude	Latitude	PUSCH RB Count /s	PDSCH RB Count /s	PUSCH Scheduled slot Count /s	PDSCH Scheduled slot Count /s	SINR (dB)	PCI	RSRP (dBm)	WideBand CQI	MCS Average UL /s	MCS Average DL /s	PDCP Throughput DL (bps)	PDCP Throughput UL (bps)	Event	Message Type	
0	14:36:52:046	-9999	-9999	4530	198528	260	2000	17.20	262	-92.06	11	19	14	46937376	889008		Inherit Params Set(Event)	
1	14:36:52:046	-9999	-9999	4530	198528	260	2000	17.20	262	-92.06	11	19	14	46937376	889008		LTE LL1 PDSCH demapper configuration	
2	14:36:52:046	-9999	-9999	4530	198528	260	2000	16.50	262	-92.00	11	19	14	46937376	889008		LTE_Cell_List	
3	14:36:52:046	-9999	-9999	4530	198528	260	2000	16.50	262	-92.00	11	19	14	46937376	889008		LTE LL1 PCFICH decoding results	
4	14:36:52:046	-9999	-9999	4530	198528	260	2000	16.50	262	-92.00	11	19	14	46937376	889008		LTE LL1 PUCCH CSF log	
5	14:36:52:046	-9999	-9999	4530	198528	260	2000	16.50	262	-92.00	11	19	14	46937376	889008		LTE ML1 PUCCH power control	
6	14:36:52:046	-9999	-9999	4530	198528	260	2000	16.50	262	-92.00	11	19	14	46937376	889008		LTE LL1 PSS results	
7	14:36:52:046	-9999	-9999	4530	198528	260	2000	16.50	262	-92.00	11	19	14	46937376	889008		LTE LL1 SSS results";
            List<LogRecord> records = CsvContext.ReadString<LogRecord>(_testInput, _fileDescriptionNamesUs).ToList();
            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count, 8);
            Assert.AreEqual(records[0].PdschRbRate, 198528);
            Assert.AreEqual(records[0].PuschScheduledSlots, 260);
            Assert.AreEqual(records[0].PdschScheduledSlots, 2000);
            Assert.AreEqual(records[0].DlThroughput, 46937376);
        }
    }
}
