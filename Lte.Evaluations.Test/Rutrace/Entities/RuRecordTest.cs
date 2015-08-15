using Lte.Evaluations.Rutrace.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Entities
{
    [TestFixture]
    public class RuRecordTest
    {
        [Test]
        public void TestRuRecord_NoNeighbor()
        {
            byte[] contents = { 
                                  0x25, 0xC0, 0xA8, 0x04, 0x08, 0x1F, 0x81, 0x43, 0x00, 0x27 
                              };
            RuRecord record = new RuRecord(contents, 0);
            Assert.IsNotNull(record);
            Assert.AreEqual(record.NbCells.Count, 0);
            Assert.AreEqual(record.RefCell.Frequency, 37);
            Assert.AreEqual(record.RefCell.CellId, 504);
            Assert.AreEqual(record.RefCell.SectorId, 1);
            Assert.AreEqual(record.RefCell.EcIo, 3);
            Assert.AreEqual(record.RefCell.Rtd, 1189.5);
        }

        [Test]
        public void TestRuRecord_OneNeighborDiffFrequency()
        {
            byte[] contents = { 
                                  0x77, 0xC0, 0xA8, 0x04, 0x08, 0x2C, 0x82, 0x41, 0x00, 0x2F, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x2C, 0x82, 0x8A, 0xEA, 0x80, 0x01, 0x00, 0x00, 0x25
                              };
            RuRecord record = new RuRecord(contents, 0);
            Assert.IsNotNull(record);
            Assert.AreEqual(record.NbCells.Count, 1);
            Assert.AreEqual(record.RefCell.Frequency, 119);
            Assert.AreEqual(record.RefCell.CellId, 712);
            Assert.AreEqual(record.RefCell.SectorId, 2);
            Assert.AreEqual(record.RefCell.EcIo, 1);
            Assert.AreEqual(record.RefCell.Rtd, 1433.5);
            Assert.AreEqual(record.NbCells[0].CellId, 712);
            Assert.AreEqual(record.NbCells[0].SectorId, 2);
            Assert.AreEqual(record.NbCells[0].EcIo, 10);
            Assert.AreEqual(record.NbCells[0].Pn, 426);
            Assert.AreEqual(record.NbCells[0].Rtd, 0);
            Assert.AreEqual(record.NbCells[0].Frequency, 37);
        }

        [Test]
        public void TestRuRecord_TwoNeighbors()
        {
            byte[] contents = { 
                                  0x25, 0xC0, 0xA8, 0x04, 0x08, 0xA5, 0xB2, 0x50, 0x00, 0x91, 
                                  0xC0, 0xA8, 0x04, 0x08, 0xA5, 0xB0, 0x54, 0x9B, 0x7C, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x72, 0xD0, 0x60, 0x98, 0x81, 0x00 
                              };
            RuRecord record = new RuRecord(contents, 0);
            Assert.IsNotNull(record);
            Assert.AreEqual(record.NbCells.Count, 2);
            Assert.AreEqual(record.RefCell.Frequency, 37);
            Assert.AreEqual(record.RefCell.CellId, 2651);
            Assert.AreEqual(record.RefCell.SectorId, 2);
            Assert.AreEqual(record.RefCell.EcIo, 16);
            Assert.AreEqual(record.RefCell.Rtd, 4422.5);
            Assert.AreEqual(record.NbCells[0].CellId, 2651);
            Assert.AreEqual(record.NbCells[0].SectorId, 0);
            Assert.AreEqual(record.NbCells[0].EcIo, 20);
            Assert.AreEqual(record.NbCells[0].Pn, 110);
            Assert.AreEqual(record.NbCells[0].Rtd, 976);
            Assert.AreEqual(record.NbCells[0].Frequency, 37);
            Assert.AreEqual(record.NbCells[1].CellId, 1837);
            Assert.AreEqual(record.NbCells[1].SectorId, 0);
            Assert.AreEqual(record.NbCells[1].EcIo, 32);
            Assert.AreEqual(record.NbCells[1].Pn, 98);
            Assert.AreEqual(record.NbCells[1].Rtd, 244);
            Assert.AreEqual(record.NbCells[1].Frequency, 37);
        }

        [Test]
        public void TestRuRecord_ThreeNeighbors_DiffFrequencies()
        {
            byte[] contents = { 
                                  0x77, 0xC0, 0xA8, 0x04, 0x08, 0x1E, 0x60, 0x44, 0x00, 0x0B, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x29, 0x71, 0x0D, 0xAC, 0x80, 0x00, 
                                  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x8F, 0xAC, 0x80, 0x01, 0x00, 0x00, 0x4E, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x1E, 0x61, 0x8C, 0xAB, 0x81, 0x01, 0x00, 0x00, 0x4E 
                              };
            RuRecord record = new RuRecord(contents, 0);
            Assert.IsNotNull(record);
            Assert.AreEqual(record.NbCells.Count, 3);
            Assert.AreEqual(record.RefCell.Frequency, 119);
            Assert.AreEqual(record.RefCell.CellId, 486);
            Assert.AreEqual(record.RefCell.SectorId, 0);
            Assert.AreEqual(record.RefCell.EcIo, 4);
            Assert.AreEqual(record.RefCell.Rtd, 335.5);
            Assert.AreEqual(record.NbCells[0].CellId, 663);
            Assert.AreEqual(record.NbCells[0].SectorId, 1);
            Assert.AreEqual(record.NbCells[0].EcIo, 13);
            Assert.AreEqual(record.NbCells[0].Pn, 178);
            Assert.AreEqual(record.NbCells[0].Rtd, 0);
            Assert.AreEqual(record.NbCells[0].Frequency, 119);
            Assert.AreEqual(record.NbCells[1].CellId, 4095);
            Assert.AreEqual(record.NbCells[1].SectorId, 15);
            Assert.AreEqual(record.NbCells[1].EcIo, 15);
            Assert.AreEqual(record.NbCells[1].Pn, 178);
            Assert.AreEqual(record.NbCells[1].Rtd, 0);
            Assert.AreEqual(record.NbCells[1].Frequency, 78);
            Assert.AreEqual(record.NbCells[2].CellId, 486);
            Assert.AreEqual(record.NbCells[2].SectorId, 1);
            Assert.AreEqual(record.NbCells[2].EcIo, 12);
            Assert.AreEqual(record.NbCells[2].Pn, 174);
            Assert.AreEqual(record.NbCells[2].Rtd, 244);
            Assert.AreEqual(record.NbCells[2].Frequency, 78);
        }

        [Test]
        public void TestRuRecord_ManyNeighbors()
        {
            byte[] contents = { 
                                  0x25, 0xC0, 0xA8, 0x04, 0x08, 0xAD, 0x51, 0x40, 0x00, 0x28, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x6A, 0x12, 0x59, 0xEB, 0x80, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x60, 0x91, 0x54, 0xCA, 0x5D, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0xDC, 0xE1, 0x51, 0xAA, 0xA4, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x1B, 0x51, 0x0C, 0xCE, 0x8E, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x51, 0x90, 0x51, 0xA7, 0xFE, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x69, 0xE1, 0x08, 0xCB, 0xC4, 0x00, 
                                  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x93, 0x9D, 0x65, 0x00, 
                                  0xC0, 0xA8, 0x04, 0x08, 0x6A, 0x11, 0x55, 0xC1, 0x64, 0x00
                              };
            RuRecord record = new RuRecord(contents, 0);
            Assert.IsNotNull(record);
            Assert.AreEqual(record.NbCells.Count, 8);
            Assert.AreEqual(record.RefCell.Frequency, 37);
            Assert.AreEqual(record.RefCell.CellId, 2773);
            Assert.AreEqual(record.RefCell.SectorId, 1);
            Assert.AreEqual(record.RefCell.EcIo, 0);
            Assert.AreEqual(record.RefCell.Rtd, 1220);
            Assert.AreEqual(record.NbCells[0].CellId, 1697);
            Assert.AreEqual(record.NbCells[0].SectorId, 2);
            Assert.AreEqual(record.NbCells[0].EcIo, 25);
            Assert.AreEqual(record.NbCells[0].Pn, 430);
            Assert.AreEqual(record.NbCells[0].Rtd, 0);
            Assert.AreEqual(record.NbCells[0].Frequency, 37);
            Assert.AreEqual(record.NbCells[1].CellId, 1545);
            Assert.AreEqual(record.NbCells[1].SectorId, 1);
            Assert.AreEqual(record.NbCells[1].EcIo, 20);
            Assert.AreEqual(record.NbCells[1].Pn, 298);
            Assert.AreEqual(record.NbCells[1].Rtd, 8540);
            Assert.AreEqual(record.NbCells[1].Frequency, 37);
        }
    }
}
