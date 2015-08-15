using System.Collections.Generic;
using System.Linq;
using Lte.Domain.LinqToCsv.Description;
using Lte.Evaluations.Dingli;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class LogRecordsMergeTest
    {
        private CsvFileDescription fileDescription_namesUs;
        private string testInput;
        private List<LogRecord> originalLogs;

        [SetUp]
        public void TestInitialize()
        {
            fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = '\t',
                IgnoreUnknownColumns = true,
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            testInput = @"Index	Time	Longitude	Latitude	eNodeBID	SectorID	PCI	SINR (dB)	RSRP (dBm)	WideBand CQI	MCS Average DL /s	PDCP Throughput DL (bps)	Event	Message Type	
0	16:04:05:328	-9999	-9999	488507	2	278	13.10	-97.75	8	10	13075264		Inherit Params Set(Event)	
1	16:04:05:328	-9999	-9999	488507	2	278	13.10	-97.75	11	10	13075264		LTE LL1 PUCCH CSF log	
2	16:04:05:328	-9999	-9999	488507	2	278	13.10	-97.75	11	10	13075264		LTE ML1 PUSCH power control	
3	16:04:05:328	-9999	-9999	488507	2	278	13.10	-97.75	11	10	13075264		LTE LL1 PCFICH decoding results	
4	16:04:05:328	-9999	-9999	488507	2	278	13.10	-97.75	11	10	13075264	RRC Connection ReconfigurationLTE Handover Request;	LTE ML1 PUCCH power control	
5	16:04:05:328	-9999	-9999	488507	2	278	13.10	-97.75	11	10	13075264		LTE ML1 Uplink PKT build indication	
6	16:04:05:328	-9999	-9999	488507	2	278	19.40	-97.06	11	10	13075264		LTE_Cell_List	
7	16:04:05:390	-9999	-9999	488507	2	278	19.40	-97.06	11	10	13075264		LTE LL1 PSS results	
8	16:04:05:390	-9999	-9999	488507	2	278	19.40	-97.06	11	10	13075264		LTE LL1 SSS results	
9	16:04:05:453	-9999	-9999	488507	2	278	19.40	-97.06	11	10	13075264		LTE ML1 Uplink PKT build indication	
10	16:04:05:531	-9999	-9999	488507	2	278	19.40	-97.06	11	10	13075264		LTE ML1 Uplink PKT build indication	
11	16:04:05:531	114.298796666667	22.6054766666667	488507	2	278	16.20	-95.93	11	10	13075264	FTP Download Connect StartFTP Download Connect Success;	LTE_Cell_List	
12	16:04:05:718	114.298793809524	22.605475	488507	2	278	16.20	-95.93	11	10	13075264	FTP Download Login SuccessFTP Download Reset Support;	LTE ML1 Uplink PKT build indication	
13	16:04:05:781	114.298790952381	22.6054733333333	488507	2	278	16.20	-95.93	11	10	13075264		LTE ML1 Uplink PKT build indication	
14	16:04:05:781	114.298788095238	22.6054716666667	488507	2	278	15.10	-96.68	11	10	13075264		LTE_Cell_List	
15	16:04:05:843	114.298785238095	22.60547	488507	2	278	15.10	-96.68	8	10	13075264		LTE LL1 PUCCH CSF log	
16	16:04:05:843	114.298782380952	22.6054683333333	488507	2	278	15.10	-96.68	8	10	13075264		LTE LL1 PCFICH decoding results	
17	16:04:05:843	114.29877952381	22.6054666666667	488507	2	278	15.10	-96.68	8	10	13075264	FTP Download Send RETR	LTE ML1 Uplink PKT build indication	
18	16:04:05:906	114.298776666667	22.605465	488507	2	278	15.10	-96.68	8	10	13075264		LTE ML1 Uplink PKT build indication	
19	16:04:05:906	114.298773809524	22.6054633333333	488507	2	278	15.10	-96.68	8	10	13075264		LTE LL1 PSS results	
20	16:04:05:906	114.298770952381	22.6054616666667	488507	2	278	15.10	-96.68	8	10	13075264	FTP Download First Data	LTE LL1 SSS results	
21	16:04:05:968	114.298768095238	22.60546	488507	2	278	19.20	-96.93	8	10	13075264		LTE_Cell_List	
22	16:04:06:031	114.298765238095	22.6054583333333	488507	2	278	19.20	-96.93	8	10	13075264		LTE ML1 PUCCH power control	
23	16:04:06:031	114.298762380952	22.6054566666667	488507	2	278	19.20	-96.93	8	10	13075264		LTE ML1 Uplink PKT build indication	
24	16:04:06:031	114.29875952381	22.605455	488507	2	278	19.20	-96.93	8	10	13075264		LTE ML1 PUSCH power control	
25	16:04:06:031	114.298756666667	22.6054533333333	488507	2	278	19.20	-96.93	8	10	13075264		LTE ML1 Uplink PKT build indication";
            originalLogs = CsvContext.ReadString<LogRecord>(testInput, fileDescription_namesUs).ToList();
        }

        [Test]
        public void TestLogRecordsMerge_BeforeMerge()
        {
            Assert.IsNotNull(originalLogs);
            Assert.AreEqual(originalLogs.Count, 26);
        }

        [Test]
        public void TestLogRecordsMerge_AfterMerge()
        {
            List<LogRecord> resultRecords = originalLogs.Merge();
            Assert.IsNotNull(resultRecords);
            Assert.AreEqual(resultRecords.Count, 10);
            Assert.AreEqual(resultRecords[0].Time.Millisecond, 328);
            Assert.AreEqual(resultRecords[0].Longtitute, -9999);
            Assert.AreEqual(resultRecords[0].Event, ":-:-:-:-RRC Connection ReconfigurationLTE Handover Request;:-:-");
            Assert.AreEqual(resultRecords[0].Event.GetSplittedFields(":-")[0], 
                "RRC Connection ReconfigurationLTE Handover Request;");
            Assert.AreEqual(resultRecords[1].Event.GetSplittedFields(":-").Count(), 0);
            Assert.AreEqual(resultRecords[0].MessageType, 
                "Inherit Params Set(Event):-LTE LL1 PUCCH CSF log:-LTE ML1 PUSCH power control:-" +
                "LTE LL1 PCFICH decoding results:-LTE ML1 PUCCH power control:-LTE ML1 Uplink PKT build indication:-" +
                "LTE_Cell_List");
            Assert.AreEqual(resultRecords[3].Longtitute, 114.298796666667);
        }
    }
}
