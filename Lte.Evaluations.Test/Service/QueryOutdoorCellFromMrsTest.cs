using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using NUnit.Framework;
using Moq;

namespace Lte.Evaluations.Test.Service
{
    [TestFixture]
    public class QueryOutdoorCellFromMrsTest
    {
        private readonly Mock<IENodebRepository> mockENodebRepository = new Mock<IENodebRepository>();
        private readonly Mock<ICellRepository> mockCellRepository = new Mock<ICellRepository>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            mockENodebRepository.Setup(x => x.GetAll()).Returns(new List<ENodeb>
            {
                new ENodeb {ENodebId = 1, Name = "E-1"},
                new ENodeb {ENodebId = 2, Name = "E-2"}
            }.AsQueryable());
            mockENodebRepository.Setup(x => x.GetAllList()).Returns(mockENodebRepository.Object.GetAll().ToList());
            mockENodebRepository.Setup(x => x.Count()).Returns(mockENodebRepository.Object.GetAll().Count());
            mockENodebRepository.Setup(x => x.GetAllWithIds(It.IsAny<IEnumerable<int>>())).Returns<IEnumerable<int>>(
                ids => (from id in ids
                    join Entities in mockENodebRepository.Object.GetAll()
                        on id equals Entities.ENodebId
                    select Entities).ToList());
            mockCellRepository.Setup(x => x.GetAll()).Returns(new List<Cell>
            {
                new Cell {ENodebId = 1, SectorId = 0, Height = 10},
                new Cell {ENodebId = 1, SectorId = 1, Height = 10},
                new Cell {ENodebId = 1, SectorId = 2, Height = 10},
                new Cell {ENodebId = 2, SectorId = 0, Height = 10},
                new Cell {ENodebId = 2, SectorId = 1, Height = 10},
                new Cell {ENodebId = 2, SectorId = 2, Height = 10}
            }.AsQueryable());
            mockCellRepository.Setup(x => x.GetAllList()).Returns(mockCellRepository.Object.GetAll().ToList());
        }

        [Test]
        public void Test_EmptyList()
        {
            RutraceStatContainer.MrsStats = new List<MrsCellDateView>();

            IEnumerable<EvaluationOutdoorCell> resultList = RutraceStatContainer.QueryOutdoorCellsFromMrs(
                mockENodebRepository.Object, mockCellRepository.Object);
            Assert.AreEqual(resultList.Count(), 0);
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        public void Test_OneEntity_Mathed(int eNodebId, byte sectorId)
        {
            RutraceStatContainer.MrsStats = new List<MrsCellDateView>
            {
                new MrsCellDateView {CellId = eNodebId, SectorId = sectorId}
            };

            IEnumerable<EvaluationOutdoorCell> resultList = RutraceStatContainer.QueryOutdoorCellsFromMrs(
                mockENodebRepository.Object, mockCellRepository.Object);
            Assert.AreEqual(resultList.Count(), 1);
            Assert.AreEqual(resultList.ElementAt(0).Height, 10);
            Assert.AreEqual(resultList.ElementAt(0).CellName, "E-" + eNodebId + "-" + sectorId);
        }

        [TestCase(5, 0)]
        [TestCase(1, 4)]
        [TestCase(3, 2)]
        [TestCase(32, 0)]
        [TestCase(2, 14)]
        [TestCase(21, 20)]
        public void Test_OneEntity_NotMathed(int eNodebId, byte sectorId)
        {
            RutraceStatContainer.MrsStats = new List<MrsCellDateView>
            {
                new MrsCellDateView {CellId = eNodebId, SectorId = sectorId}
            };

            IEnumerable<EvaluationOutdoorCell> resultList = RutraceStatContainer.QueryOutdoorCellsFromMrs(
                mockENodebRepository.Object, mockCellRepository.Object);
            Assert.AreEqual(resultList.Count(), 0);
        }

        [TestCase(new[]{1, 1}, new byte[]{0, 4})]
        [TestCase(new[]{1, 3}, new byte[]{1, 1})]
        [TestCase(new[]{1, 6}, new byte[]{2, 7})]
        [TestCase(new[]{2, 3}, new byte[]{0, 0})]
        public void Test_OneEntity_FirstMathed(int[] eNodebIds, byte[] sectorIds)
        {
            RutraceStatContainer.MrsStats = new List<MrsCellDateView>
            {
                new MrsCellDateView {CellId = eNodebIds[0], SectorId = sectorIds[0]},
                new MrsCellDateView {CellId = eNodebIds[1], SectorId = sectorIds[1]}
            };

            IEnumerable<EvaluationOutdoorCell> resultList = RutraceStatContainer.QueryOutdoorCellsFromMrs(
                mockENodebRepository.Object, mockCellRepository.Object);
            Assert.AreEqual(resultList.Count(), 1);
            Assert.AreEqual(resultList.ElementAt(0).Height, 10);
            Assert.AreEqual(resultList.ElementAt(0).CellName, "E-" + eNodebIds[0] + "-" + sectorIds[0]);
        }
    }
}
