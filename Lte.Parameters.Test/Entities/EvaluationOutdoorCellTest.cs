using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class EvaluationOutdoorCellTest
    {
        private Cell cell;

        [SetUp]
        public void TestInitialize()
        {
            cell = new Cell()
            {
                Height = 1,
                Azimuth = 2,
                MTilt = 3,
                ETilt = 4,
                Longtitute = 5,
                Lattitute = 6,
                AntennaGain = 7,
                RsPower = 8,
                SectorId = 9,
                Frequency = 10,
                Pci = 11
            };
        }

        [Test]
        public void TestEvaluationOutdoorCell()
        {
            ENodeb eNodeb = new ENodeb { Name = "OMG" };
            EvaluationOutdoorCell eodCell = new EvaluationOutdoorCell(eNodeb, cell);
            Assert.IsNotNull(eodCell);
            Assert.AreEqual(eodCell.Height, 1);
            Assert.AreEqual(eodCell.Lattitute, 6);
            Assert.AreEqual(eodCell.Pci, 11);
            Assert.AreEqual(eodCell.CellName, "OMG-9");
        }
    }
}
