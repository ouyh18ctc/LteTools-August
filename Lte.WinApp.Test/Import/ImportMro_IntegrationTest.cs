using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.WinApp.Import;
using Moq;
using NUnit.Framework;

namespace Lte.WinApp.Test.Import
{
    [TestFixture]
    public class ImportMro_IntegrationTest
    {
        private readonly string testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XmlFiles");
        private MroFilesImporter importer;
        private readonly Mock<ICellRepository> cellRepository = new Mock<ICellRepository>();
        private readonly Mock<ILteNeighborCellRepository> neighborRepository = new Mock<ILteNeighborCellRepository>();

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            cellRepository.Setup(x => x.GetAll()).Returns(new List<Cell>
            {
                new Cell {ENodebId = 501250, SectorId = 48, Pci = 111, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501250, SectorId = 49, Pci = 112, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501250, SectorId = 50, Pci = 113, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501251, SectorId = 48, Pci = 23, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501252, SectorId = 48, Pci = 189, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501252, SectorId = 49, Pci = 190, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501252, SectorId = 50, Pci = 191, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501253, SectorId = 50, Pci = 213, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501254, SectorId = 50, Pci = 264, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501255, SectorId = 48, Pci = 28, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501256, SectorId = 48, Pci = 90, Longtitute = 112, Lattitute = 23},
                new Cell {ENodebId = 501257, SectorId = 48, Pci = 387, Longtitute = 112, Lattitute = 23},
            }.AsQueryable());
            cellRepository.Setup(x => x.GetAllList()).Returns(cellRepository.Object.GetAll().ToList);
            neighborRepository.SetupGet(x => x.NearestPciCells).Returns(new List<NearestPciCell>
            {
                new NearestPciCell {CellId = 501250, SectorId = 48, NearestCellId = 501250, NearestSectorId = 49, Pci = 112},
                new NearestPciCell {CellId = 501250, SectorId = 48, NearestCellId = 501250, NearestSectorId = 50, Pci = 113},
                new NearestPciCell {CellId = 501250, SectorId = 49, NearestCellId = 501250, NearestSectorId = 48, Pci = 111},
                new NearestPciCell {CellId = 501250, SectorId = 49, NearestCellId = 501250, NearestSectorId = 50, Pci = 113},
                new NearestPciCell {CellId = 501250, SectorId = 50, NearestCellId = 501250, NearestSectorId = 48, Pci = 111},
                new NearestPciCell {CellId = 501250, SectorId = 50, NearestCellId = 501250, NearestSectorId = 50, Pci = 113},
                new NearestPciCell {CellId = 501250, SectorId = 50, NearestCellId = 501252, NearestSectorId = 48, Pci = 189},
                new NearestPciCell {CellId = 501250, SectorId = 50, NearestCellId = 501252, NearestSectorId = 49, Pci = 190},
                new NearestPciCell {CellId = 501250, SectorId = 50, NearestCellId = 501252, NearestSectorId = 50, Pci = 192}
            }.AsQueryable());
        }

        [SetUp]
        public void SetUp()
        {
            importer = new MroFilesImporter(cellRepository.Object, neighborRepository.Object);
        }

        [Test]
        public void Test_EmptyFiles()
        {
            string mrsFileName = Path.Combine(testDirectory, "FDD-LTE_MRO_ZTE_OMC440601_503387_20150713044500.xml");
            importer.Import(new[] { mrsFileName }, x => new MroRecordSet(new StreamReader(x)));
            Assert.IsNotNull(importer);
            Assert.AreEqual(importer.InterferenceStats.Count, 0);
            Assert.AreEqual(importer.RsrpTaStatList.Count, 0);
        }

        [Test]
        public void Test_NotEmptyFiles()
        {
            string mrsFileName = Path.Combine(testDirectory, "FDD-LTE_MRO_ZTE_OMC1_501250_20150713183000.xml");
            importer.Import(new[] { mrsFileName }, x => new MroRecordSet(new StreamReader(x)));
            Assert.IsNotNull(importer);
            Assert.AreEqual(importer.InterferenceStats.Count, 1, "interference");
            Assert.AreEqual(importer.RsrpTaStatList.Count, 9, "rsrp-ta");
        }

        [Test]
        public void Test_TwoFiles()
        {
            string mrsFileName1 = Path.Combine(testDirectory, "FDD-LTE_MRO_ZTE_OMC1_501250_20150713183000.xml");
            string mrsFileName2 = Path.Combine(testDirectory, "FDD-LTE_MRO_ZTE_OMC1_501250_20150713190000.xml");
            importer.Import(new[] { mrsFileName1, mrsFileName2 }, 
                x => new MroRecordSet(new StreamReader(x)));
            Assert.IsNotNull(importer);
            Assert.AreEqual(importer.InterferenceStats.Count, 3, "interference");
            Assert.AreEqual(importer.RsrpTaStatList.Count, 13, "rsrp-ta");
        }
    }
}
