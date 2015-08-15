using System;
using System.IO;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Entities
{
    [TestFixture]
    public class MrRecordSetTest
    {
        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578577"" TimeStamp=""2015-03-31T04:45:10.465"" id=""123727107"">
<v>100 123 123727107 30 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578833"" TimeStamp=""2015-03-31T04:45:20.455"" id=""123727105"">
<v>100 121 123727105 38 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184560043"" TimeStamp=""2015-03-31T04:45:38.385"" id=""123727107"">
<v>100 123 123727107 30 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184584882"" TimeStamp=""2015-03-31T04:45:48.655"" id=""123727105"">
<v>100 121 123727105 38 25 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 0, "2015-03-31")]
        public void TestMreFormat(string contents, int recordCounts, string recordDate)
        {
            using (StreamReader reader = contents.GetStreamReader())
            {
                MrRecordSet recordSet = new MreRecordSet(reader);
                Assert.AreEqual(recordSet.RecordList.Count, recordCounts);
                Assert.AreEqual(recordSet.RecordDate,DateTime.Parse(recordDate));
            }
        }

        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object EventType=""A3"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578577"" TimeStamp=""2015-03-31T04:45:10.465"" id=""123727107"">
<v>100 121 123727105 35 21 100 224 41 26 NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578833"" TimeStamp=""2015-03-31T04:45:20.455"" id=""123727105"">
<v>100 121 123727105 38 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184560043"" TimeStamp=""2015-03-31T04:45:38.385"" id=""123727107"">
<v>100 123 123727107 30 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184584882"" TimeStamp=""2015-03-31T04:45:48.655"" id=""123727105"">
<v>100 121 123727105 38 25 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 1, 483309, new byte[] {1}, new byte[] {35}, new short[] {100},
            new short[] {224}, new byte[] {41}, new short[] {100})]
        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578577"" TimeStamp=""2015-03-31T04:45:10.465"" id=""123727107"">
<v>100 123 123727107 30 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578833"" TimeStamp=""2015-03-31T04:45:20.455"" id=""123727105"">
<v>100 121 123727105 38 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A3"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184560043"" TimeStamp=""2015-03-31T04:45:38.385"" id=""123727107"">
<v>100 123 123727107 26 14 100 118 29 16 NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184584882"" TimeStamp=""2015-03-31T04:45:48.655"" id=""123727105"">
<v>100 121 123727105 38 25 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 1, 483309, new byte[] { 3 }, new byte[] { 26 }, new short[] { 100 },
                    new short[] { 118 }, new byte[] { 29 }, new short[] { 100 })]
        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object EventType=""A3"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578577"" TimeStamp=""2015-03-31T04:45:10.465"" id=""123727107"">
<v>100 121 123727105 35 21 100 224 41 26 NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184578833"" TimeStamp=""2015-03-31T04:45:20.455"" id=""123727105"">
<v>100 121 123727105 38 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A3"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184560043"" TimeStamp=""2015-03-31T04:45:38.385"" id=""123727107"">
<v>100 123 123727107 26 14 100 118 29 16 NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
<object EventType=""A2"" MmeCode=""1"" MmeGroupId=""17409"" MmeUeS1apId=""184584882"" TimeStamp=""2015-03-31T04:45:48.655"" id=""123727105"">
<v>100 121 123727105 38 25 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL </v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 2, 483309, new byte[] { 1, 3 }, new byte[] { 35, 26 }, new short[] { 100, 100 },
                    new short[] { 224, 118 }, new byte[] { 41, 29 }, new short[] { 100, 100 })]
        public void TestMreFormat(string contents, int recordCounts, int eNodebId,
            byte[] mainSectorId, byte[] mainRsrp, short[] mainFrequency,
            short[] nbPci, byte[] nbRsrp, short[] nbFrequency)
        {
            using (StreamReader reader = contents.GetStreamReader())
            {
                MrRecordSet recordSet = new MreRecordSet(reader);
                Assert.AreEqual(recordSet.RecordList.Count, recordCounts);
                for (int i = 0; i < recordCounts; i++)
                {
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.CellId, eNodebId);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.SectorId, mainSectorId[i]);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.Frequency, mainFrequency[i]);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.Rsrp, mainRsrp[i]);
                    Assert.AreEqual(recordSet.RecordList[i].NbCells[0].Pci, nbPci[i]);
                    Assert.AreEqual(recordSet.RecordList[i].NbCells[0].Rsrp, nbRsrp[i]);
                    Assert.AreEqual(recordSet.RecordList[i].NbCells[0].Frequency, nbFrequency[i]);
                }
            }
        }

        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteScTadv MR.LteScPHR MR.LteScAOA MR.LteScSinrUL MR.LteScUeRxTxTD MR.LteSceEuRxTxTD MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.LteFddNcEarfcn MR.LteFddNcPci MR.LteFddNcRSRP MR.LteFddNcRSRQ MR.LteTddNcEarfcn MR.LteTddNcPci MR.LteTddNcRSRP MR.LteTddNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object id=""123727104"" MmeUeS1apId=""54599732"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 120 123727104 77 28 80 45 NIL 36 32 0 100 321 53 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 120 123727104 77 28 80 45 NIL 36 32 0 100 154 52 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 120 123727104 77 28 80 45 NIL 36 32 0 100 153 50 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 120 123727104 77 28 80 45 NIL 36 32 0 1825 444 22 13 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 1, 483309, "2015-03-31",
                    new byte[] { 0 }, new byte[] { 77 }, new short[] { 100 }, new byte[] { 5 }, new[] { 4 },
                    new short[] { 321, 154, 153, 444 }, new byte[] { 53, 52, 50, 22 }, new short[] { 100, 100, 100, 1825 })]
        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteScTadv MR.LteScPHR MR.LteScAOA MR.LteScSinrUL MR.LteScUeRxTxTD MR.LteSceEuRxTxTD MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.LteFddNcEarfcn MR.LteFddNcPci MR.LteFddNcRSRP MR.LteFddNcRSRQ MR.LteTddNcEarfcn MR.LteTddNcPci MR.LteTddNcRSRP MR.LteTddNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object id=""123727105"" MmeUeS1apId=""54599732"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 121 123727105 31 22 NIL NIL NIL NIL 40 NIL 100 321 25 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 31 22 NIL NIL NIL NIL 40 NIL 100 153 14 1 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 1, 483309, "2015-03-31",
                    new byte[] { 1 }, new byte[] { 31 }, new short[] { 100 }, new byte[] { 255 }, new[] { 2 },
                    new short[] { 321, 153 }, new byte[] { 25, 14 }, new short[] { 100, 100 })]
        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-03-31T05:00:00.000"" startTime=""2015-03-31T04:45:00.000"" endTime=""2015-03-31T05:00:00.000"" period=""15""/>
<eNB id=""483309"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteScTadv MR.LteScPHR MR.LteScAOA MR.LteScSinrUL MR.LteScUeRxTxTD MR.LteSceEuRxTxTD MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.LteFddNcEarfcn MR.LteFddNcPci MR.LteFddNcRSRP MR.LteFddNcRSRQ MR.LteTddNcEarfcn MR.LteTddNcPci MR.LteTddNcRSRP MR.LteTddNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object id=""123727105"" MmeUeS1apId=""54599732"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 121 123727105 27 22 112 5 NIL 13 40 -14 100 153 25 20 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 27 22 112 5 NIL 13 40 -14 100 224 16 7 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 27 22 112 5 NIL 13 40 -14 100 154 11 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""123727106"" MmeUeS1apId=""54599732"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 122 123727106 37 23 112 2 NIL 19 48 -4 100 155 33 17 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 37 23 112 2 NIL 19 48 -4 100 198 30 9 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 37 23 112 2 NIL 19 48 -4 100 153 16 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 37 23 112 2 NIL 19 48 -4 1825 31 27 22 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 37 23 112 2 NIL 19 48 -4 1825 191 22 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 2, 483309, "2015-03-31",
                    new byte[] { 1, 2 }, new byte[] { 27, 37 }, new short[] { 100, 100 }, 
                    new byte[] { 7, 7 }, new[] { 3, 5 },
                    new short[] { 153, 224, 154, 155, 198, 153, 31, 191 }, 
                    new byte[] { 25, 16, 11, 33, 30, 16, 27, 22 }, 
                    new short[] { 100, 100, 100, 100, 100, 100, 1825, 1825 })]
        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" reportTime=""2015-06-28T18:15:00.000"" startTime=""2015-06-28T18:00:00.000"" endTime=""2015-06-28T18:15:00.000"" period=""15""/>
<eNB id=""501392"">
<measurement>
<smr>MR.LteScEarfcn MR.LteScPci MR.LteSccgi MR.LteScRSRP MR.LteScRSRQ MR.LteScTadv MR.LteScPHR MR.LteScAOA MR.LteScSinrUL MR.LteScUeRxTxTD MR.LteSceEuRxTxTD MR.LteNcEarfcn MR.LteNcPci MR.LteNcRSRP MR.LteNcRSRQ MR.LteFddNcEarfcn MR.LteFddNcPci MR.LteFddNcRSRP MR.LteFddNcRSRQ MR.LteTddNcEarfcn MR.LteTddNcPci MR.LteTddNcRSRP MR.LteTddNcRSRQ MR.UtraCpichRSCP MR.UtraCarrierRSSI MR.UtraCpichEcNo MR.UtraCellParameterId MR.GsmNcellNcc MR.GsmNcellBcc MR.GsmNcellBcch MR.GsmNcellCarrierRSSI</smr>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 57 26 64 37 NIL 34 NIL 7 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 78 27 80 49 NIL 30 32 10 1825 128 64 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 36 22 208 16 NIL 30 96 19 1825 339 35 16 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>1825 126 128356400 36 22 208 16 NIL 30 96 19 1825 340 32 10 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 53 28 144 33 NIL 36 NIL -1 1825 341 48 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>1825 126 128356400 53 28 144 33 NIL 36 NIL -1 1825 127 38 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
<object id=""128356400"" MmeUeS1apId=""142674905"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 128 128356401 47 25 80 26 NIL 12 32 3 1825 158 36 13 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 5, 501392, "2015-06-28",
                    new byte[]{48,48,48,48,48},new byte[]{57,78,36,53,47},
                    new short[]{1825,1825,1825,1825,1825}, new byte[]{4,5,13,9,5}, new []{0,1,2,2,1},
                    new short[]{128,339,340,341,127,158},new byte[]{64,35,32,48,38,36},
                    new short[]{1825,1825,1825,1825,1825,1825})]
        public void TestMroRecordSet(string contents, int recordCounts, int eNodebId, string recordDate,
            byte[] mainSectorId, byte[] mainRsrp, short[] mainFrequency, byte[] mainTa, int[] nbCounts,
            short[] nbPci, byte[] nbRsrp, short[] nbFrequency)
        {
            using (StreamReader reader = contents.GetStreamReader())
            {
                MrRecordSet recordSet = new MroRecordSet(reader);
                Assert.AreEqual(recordSet.ENodebId, eNodebId);
                Assert.AreEqual(recordSet.RecordDate, DateTime.Parse(recordDate));
                Assert.AreEqual(recordSet.RecordList.Count, recordCounts);
                for (int i = 0, length = 0; i < recordCounts; length += nbCounts[i], i++)
                {
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.CellId, eNodebId);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.SectorId, mainSectorId[i]);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.Frequency, mainFrequency[i]);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.Rsrp, mainRsrp[i]);
                    Assert.AreEqual(recordSet.RecordList[i].RefCell.Ta, mainTa[i]);
                    for (int j = 0; j < nbCounts[i]; j++)
                    {
                        Assert.AreEqual(recordSet.RecordList[i].NbCells[j].Pci, nbPci[length + j]);
                        Assert.AreEqual(recordSet.RecordList[i].NbCells[j].Rsrp, nbRsrp[length + j]);
                        Assert.AreEqual(recordSet.RecordList[i].NbCells[j].Frequency, nbFrequency[length + j]);
                    }
                }
            }
        }

        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" period=""15"" reportTime=""2015-07-13T04:40:45.000"" startTime=""2015-07-13T04:00:00.000"" endTime=""2015-07-13T04:15:00.000""/>
</bulkPmMrDataFile>", "2015-07-13")]
        public void TestEmptyMroRecordSet(string contents, string recordDate)
        {
            using (StreamReader reader = contents.GetStreamReader())
            {
                MrRecordSet recordSet = new MroRecordSet(reader);
                Assert.AreEqual(recordSet.ENodebId, 0);
                Assert.AreEqual(recordSet.RecordDate, DateTime.Parse(recordDate));
                Assert.AreEqual(recordSet.RecordList.Count, 0);
            }
        }

        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
  <fileHeader fileFormatVersion=""V1.0"" period=""15"" reportTime=""2015-06-28T23:30:00.000"" startTime=""2015-06-28T23:15:00.000"" endTime=""2015-06-28T23:30:00.000""/>
<eNB id=""501392"" >
<measurement mrName=""MR.RSRP"">
<smr>MR.RSRP.00 MR.RSRP.01 MR.RSRP.02 MR.RSRP.03 MR.RSRP.04 MR.RSRP.05 MR.RSRP.06 MR.RSRP.07 MR.RSRP.08 MR.RSRP.09 MR.RSRP.10 MR.RSRP.11 MR.RSRP.12 MR.RSRP.13 MR.RSRP.14 MR.RSRP.15 MR.RSRP.16 MR.RSRP.17 MR.RSRP.18 MR.RSRP.19 MR.RSRP.20 MR.RSRP.21 MR.RSRP.22 MR.RSRP.23 MR.RSRP.24 MR.RSRP.25 MR.RSRP.26 MR.RSRP.27 MR.RSRP.28 MR.RSRP.29 MR.RSRP.30 MR.RSRP.31 MR.RSRP.32 MR.RSRP.33 MR.RSRP.34 MR.RSRP.35 MR.RSRP.36 MR.RSRP.37 MR.RSRP.38 MR.RSRP.39 MR.RSRP.40 MR.RSRP.41 MR.RSRP.42 MR.RSRP.43 MR.RSRP.44 MR.RSRP.45 MR.RSRP.46 MR.RSRP.47</smr>
<object id=""128356400"">
<v>0 0 0 0 0 2 2 1 0 2 1 1 0 0 1 1 2 1 4 6 0 0 0 0 0 0 1 2 0 0 0 0 0 0 1 0 0 2 0 0 0 0 1 0 0 0 0 0 </v>
</object>
<object id=""128356401"">
<v>0 0 0 0 0 0 0 0 0 0 1 1 1 3 3 5 10 12 8 14 18 25 9 14 9 7 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 </v>
</object>
<object id=""128356402"">
<v>0 4 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 </v>
</object>
</measurement>
</eNB>
</bulkPmMrDataFile>", 3, 501392, "2015-06-28",
                    new[]{0,0,4}, new []{2,0,0})]
        public void TestMrsRecordSet(string contents, int cellCounts, int eNodebId, string recordDate,
            int[] rsrpCount1, int[] rsrpCount5)
        {
            using (StreamReader reader = contents.GetStreamReader())
            {
                MrsRecordSet recordSet = new MrsRecordSet(reader);
                Assert.AreEqual(recordSet.MrsCells.Count, cellCounts);
                Assert.AreEqual(recordSet.ENodebId, eNodebId);
                Assert.AreEqual(recordSet.RecordDate, DateTime.Parse(recordDate));
                for (int i = 0; i < cellCounts; i++)
                {
                    Assert.AreEqual(recordSet.MrsCells[i].RsrpCounts[1], rsrpCount1[i]);
                    Assert.AreEqual(recordSet.MrsCells[i].RsrpCounts[5], rsrpCount5[i]);
                }
            }
        }

        [TestCase(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<bulkPmMrDataFile>
<fileHeader fileFormatVersion=""V1.0"" period=""15"" reportTime=""2015-07-13T04:40:45.000"" startTime=""2015-07-13T04:00:00.000"" endTime=""2015-07-13T04:15:00.000""/>
</bulkPmMrDataFile>", "2015-07-13")]
        public void TestEmptyMrsRecordSet(string contents, string recordDate)
        {
            using (StreamReader reader = contents.GetStreamReader())
            {
                MrsRecordSet recordSet=new MrsRecordSet(reader);
                Assert.AreEqual(recordSet.MrsCells.Count, 0);
                Assert.AreEqual(recordSet.ENodebId, 0);
                Assert.AreEqual(recordSet.RecordDate, DateTime.Parse(recordDate));
            }
        }
    }
}
