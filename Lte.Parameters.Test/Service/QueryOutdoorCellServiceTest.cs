using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;
using NUnit.Framework;
using Moq;

namespace Lte.Parameters.Test.Service
{
    [TestFixture]
    public class ByENodebListQueryOutdoorCellServiceTest
    {
        private readonly Mock<ICellRepository> repository = new Mock<ICellRepository>();
        private IEnumerable<ENodeb> eNodebs;

        [SetUp]
        public void SetUp()
        {
            eNodebs = new List<ENodeb>
            {
                new ENodeb {ENodebId = 1, Name = "E-1"},
                new ENodeb {ENodebId = 2, Name = "E-2"},
                new ENodeb {ENodebId = 3, Name = "E-3"}
            };
        }

        [TestCase(1, new[] { 1 }, new byte[] { 1 }, 1)]
        [TestCase(2, new[] { 1, 1 }, new byte[] { 1, 2 }, 2)]
        public void Test(int cells, int[] eNodebIds, byte[] sectorIds, int expectedLength)
        {
            List<Cell> cellList = new List<Cell>();
            for (int i = 0; i < cells; i++)
            {
                cellList.Add(new Cell {ENodebId = eNodebIds[i], SectorId = sectorIds[i], Height = 30});
            }
            repository.Setup(x => x.GetAll()).Returns(cellList.AsQueryable());
            repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
            repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
            List<EvaluationOutdoorCell> outdoorCells = repository.Object.Query(eNodebs);
            Assert.AreEqual(outdoorCells.Count, expectedLength);
        }
    }
}
