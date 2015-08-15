using System.Xml.Linq;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Entities
{
    [TestFixture]
    public class MrRecordTest
    {
        private void AssertRefCell(MrReferenceCell cell, int eNodebId, byte sectorId, byte rsrp)
        {
            Assert.AreEqual(cell.CellId, eNodebId);
            Assert.AreEqual(cell.SectorId, sectorId);
            Assert.AreEqual(cell.Rsrp, rsrp);
        }

        private void AssertRefCell(MrReferenceCell cell, int eNodebId, byte sectorId, byte rsrp, short ta)
        {
            AssertRefCell(cell, eNodebId, sectorId, rsrp);
            Assert.AreEqual(cell.Ta, ta);
        }

        private void AssertNbCell(MrNeighborCell cell, short pci, short frequency, byte rsrp)
        {
            Assert.AreEqual(cell.Pci, pci);
            Assert.AreEqual(cell.Frequency, frequency);
            Assert.AreEqual(cell.Rsrp, rsrp);
        }

        [TestCase(483309, "100 123 123727107 30 15 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL", 3, 30)]
        [TestCase(483309, "100 121 123727105 38 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL", 1, 38)]
        public void TestMreRecord_FromLine_NoNeighbor(int eNodebId, string line, byte sectorId, byte mainRsrp)
        {
            MrRecord record = new MreRecord(eNodebId, line);
            AssertRefCell(record.RefCell, eNodebId, sectorId, mainRsrp);
            Assert.AreEqual(record.NbCells.Count, 0);
        }

        [TestCase(483309, "100 121 123727105 35 21 100 224 41 26 NIL NIL NIL NIL NIL NIL NIL NIL", 1, 35,
            224, 100, 41)]
        [TestCase(483309, "100 121 123727105 28 11 100 321 31 15 NIL NIL NIL NIL NIL NIL NIL NIL", 1, 28,
            321, 100, 31)]
        public void TestMreRecord_FromLine_OneNeighbor(int eNodebId, string line, byte sectorId, byte mainRsrp,
            short pci, short frequency, byte rsrp)
        {
            MrRecord record = new MreRecord(eNodebId, line);
            AssertRefCell(record.RefCell, eNodebId, sectorId, mainRsrp);
            Assert.AreEqual(record.NbCells.Count, 1);
            AssertNbCell(record.NbCells[0], pci, frequency, rsrp);
        }

        [TestCase(483309,
            @"<object id=""123727105"" MmeUeS1apId=""54598626"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 121 123727105 NIL NIL 96 23 NIL 36 NIL 3 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 1, 255, 6)]
        [TestCase(483309,
            @"<object id=""123727105"" MmeUeS1apId=""54598626"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 121 123727105 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 1, 255, 255)]
        [TestCase(501392,
            @"<object id=""128356400"" MmeUeS1apId=""306278064"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T04:12:22.024"">
<v>1825 126 128356400 NIL NIL 224 12 NIL 24 NIL 3 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 48, 255, 14)]
        public void TestMroRecord_FromXmlPart_NoNeighbor(int eNodebId, string xmlContents,
            byte sectorId, byte mainRsrp, byte ta)
        {
            XDocument doc = XDocument.Load(xmlContents.GetStreamReader());
            MrRecord record = new MroRecord(eNodebId, doc.Root);
            AssertRefCell(record.RefCell, eNodebId, sectorId, mainRsrp, ta);
            Assert.AreEqual(record.NbCells.Count, 0);
        }

        [TestCase(483309,
            @"<object id=""123727104"" MmeUeS1apId=""54598626"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 120 123727104 28 24 96 0 NIL 13 40 0 100 322 22 11 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 0, 28, 6, 322, 100, 22)]
        [TestCase(483309,
            @"<object id=""123727105"" MmeUeS1apId=""54604239"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:00.448"">
<v>100 121 123727105 32 25 112 6 NIL 14 40 0 100 153 15 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 1, 32, 7, 153, 100, 15)]
        [TestCase(501392,
            @"<object id=""128356400"" MmeUeS1apId=""335606792"" MmeCode=""1"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 78 27 80 49 NIL 30 32 10 1825 128 64 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 48, 78, 5, 128, 1825, 64)]
        public void TestMroRecord_FromXmlPart_OneNeighbor(int eNodebId, string xmlContents,
            byte sectorId, byte mainRsrp, byte ta,
            short pci, short frequency, byte rsrp)
        {
            XDocument doc = XDocument.Load(xmlContents.GetStreamReader());
            MrRecord record = new MroRecord(eNodebId, doc.Root);
            AssertRefCell(record.RefCell, eNodebId, sectorId, mainRsrp, ta);
            Assert.AreEqual(record.NbCells.Count, 1);
            AssertNbCell(record.NbCells[0], pci, frequency, rsrp);
        }

        [TestCase(483309,
            @"<object id=""123727105"" MmeUeS1apId=""54599282"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:05.568"">
<v>100 121 123727105 63 19 NIL NIL NIL NIL 19 NIL 100 321 60 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 63 19 NIL NIL NIL NIL 19 NIL 100 153 38 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 1, 63, 255, 2, new short[] {321, 153}, new short[] {100, 100}, new byte[] {60, 38})]
        [TestCase(483309,
            @"<object id=""123727105"" MmeUeS1apId=""54599282"" MmeCode=""2"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:05.568"">
<v>100 121 123727105 26 23 NIL NIL NIL NIL 39 NIL 100 122 19 7 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 26 23 NIL NIL NIL NIL 39 NIL 100 153 16 7 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 26 23 NIL NIL NIL NIL 39 NIL 100 321 16 2 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 26 23 NIL NIL NIL NIL 39 NIL 100 224 12 8 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 121 123727105 26 23 NIL NIL NIL NIL 39 NIL 100 223 11 6 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 1, 26, 255, 5,
            new short[] {122, 153, 321, 224, 223}, new short[] {100, 100, 100, 100, 100},
            new byte[] {19, 16, 16, 12, 11})]
        [TestCase(483309,
            @"<object id=""123727106"" MmeUeS1apId=""184592286"" MmeCode=""1"" MmeGroupId=""17409"" TimeStamp=""2015-03-31T12:30:05.568"">
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 100 224 39 24 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 100 153 35 10 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 100 154 33 4 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 100 155 32 5 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 100 321 27 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 100 306 1 0 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 1825 31 31 22 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 1825 190 22 5 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>100 122 123727106 42 21 96 6 NIL 28 NIL 8 1825 35 20 5 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 2, 42, 6, 9,
            new short[] {224, 153, 154, 155, 321, 306, 31, 190, 35},
            new short[] {100, 100, 100, 100, 100, 100, 1825, 1825, 1825},
            new byte[] {39, 35, 33, 32, 27, 1, 31, 22, 20})]
        [TestCase(501392,
            @"<object id=""128356400"" MmeUeS1apId=""155213578"" MmeCode=""1"" MmeGroupId=""17409"" TimeStamp=""2015-06-28T18:00:06.264"">
<v>1825 126 128356400 36 22 208 16 NIL 30 96 19 1825 339 35 16 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
<v>1825 126 128356400 36 22 208 16 NIL 30 96 19 1825 340 32 10 NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL NIL</v>
</object>", 48, 36, 13, 2,
          new short[]{339, 340}, new short[]{1825,1825}, new byte[]{35,32})]
        public void TestMroRecord_FromXmlPart_MultiNeighbors(int eNodebId, string xmlContents,
            byte sectorId, byte mainRsrp, byte ta, int neighbors,
            short[] pcis, short[] frequencies, byte[] rsrps)
        {
            XDocument doc = XDocument.Load(xmlContents.GetStreamReader());
            MrRecord record = new MroRecord(eNodebId, doc.Root);
            AssertRefCell(record.RefCell, eNodebId, sectorId, mainRsrp, ta);
            Assert.AreEqual(record.NbCells.Count, neighbors);

            for (int i = 0; i < neighbors; i++)
            {
                AssertNbCell(record.NbCells[i], pcis[i], frequencies[i], rsrps[i]);
            }
        }
    }
}
