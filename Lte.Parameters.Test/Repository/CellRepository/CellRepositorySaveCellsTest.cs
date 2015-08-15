using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository.CellRepository
{
    [TestFixture]
    public class CellRepositorySaveCellsTest : CellRepositoryTestConfig
    {
        private List<CellExcel> cellInfos;

        [SetUp]
        public void SetUp()
        {
            Initialize();
            eNodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb> 
            {
                new ENodeb
                {
                    ENodebId = 1,
                    Name = "FoshanHuafo",
                    Longtitute = 112.3344,
                    Lattitute = 22.7788
                }
            }.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());

            cellInfos = new List<CellExcel>
            {
                new CellExcel
                {
                    ENodebId = 1,
                    SectorId = 1,
                    IsIndoor = "否  ",
                    Frequency = 1750,
                    BandClass = 1,
                    Height = 40,
                    Azimuth = 35,
                    AntennaGain = 17.5,
                    MTilt = 4,
                    ETilt = 7,
                    RsPower = 16.2,
                    TransmitReceive = "2T4R"
                },
                new CellExcel
                {
                    ENodebId = 1,
                    SectorId = 2,
                    IsIndoor = "是",
                    Frequency = 1750,
                    BandClass = 1,
                    Height = 33,
                    Azimuth = 65,
                    AntennaGain = 18.5,
                    MTilt = 8,
                    ETilt = 2,
                    RsPower = 15.2,
                    TransmitReceive = "4T4R"
                }
            };
        }

        [Test]
        public void TestCellRepositorySaveCells_FirstCell_ENodebExist_CellNotExist()
        {
            Assert.AreEqual(repository.Object.Count(), 1);
            Assert.AreEqual(SaveCells(cellInfos), 2);
            Assert.AreEqual(repository.Object.Count(), 3);
            Assert.IsTrue(repository.Object.GetAll().ElementAt(1).IsOutdoor);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).AntennaPorts, AntennaPortsConfigure.Antenna2T4R);
        }

        [Test]
        public void TestCellRepositorySaveCellInfos_FirstCell_ENodebExist_CellNotExist()
        {
            int[] result = SaveCellInfos(cellInfos);
            Assert.AreEqual(result[0], 2);
            Assert.AreEqual(result[1], 0);
            Assert.AreEqual(repository.Object.Count(), 3);
            Assert.IsTrue(repository.Object.GetAll().ElementAt(1).IsOutdoor);
            Assert.AreEqual(repository.Object.GetAll().ElementAt(1).AntennaPorts, AntennaPortsConfigure.Antenna2T4R);
        }

        [Test]
        public void TestCellRepositorySaveCells_FirstCell_ENodebNotExist()
        {
            cellInfos[0].ENodebId = 2;
            Assert.AreEqual(SaveCells(cellInfos), 1);
            Assert.AreEqual(repository.Object.Count(), 2);
        }

        [Test]
        public void TestCellRepositorySaveCellInfos_FirstCell_ENodebNotExist()
        {
            cellInfos[0].ENodebId = 2;
            int[] result = SaveCellInfos(cellInfos);
            Assert.AreEqual(result[0], 1);
            Assert.AreEqual(result[1], 0);
            Assert.AreEqual(repository.Object.Count(), 2);
        }

        [Test]
        public void TestCellRepositorySaveCells_FirstCell_ENodebExist_CellExist()
        {
            cellInfos[0].SectorId = 0;
            Assert.AreEqual(SaveCells(cellInfos), 1);
            Assert.AreEqual(repository.Object.Count(), 2);
        }

        [Test]
        public void TestCellRepositorySaveCellInfos_FirstCell_ENodebExist_CellExist()
        {
            cellInfos[0].SectorId = 0;
            int[] result = SaveCellInfos(cellInfos);
            Assert.AreEqual(result[0], 1);
            Assert.AreEqual(result[1], 0);
            Assert.AreEqual(repository.Object.Count(), 2);
        }
    }
}
