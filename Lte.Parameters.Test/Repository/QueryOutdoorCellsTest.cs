using System.Collections.Generic;
using Lte.Parameters.Service.Public;
using Lte.Parameters.Test.Repository.CellRepository;
using NUnit.Framework;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Test.Repository
{
    [TestFixture]
    public class QueryOutdoorCellsTest : CellRepositoryTestConfig
    {
        private readonly List<ENodeb> eNodebs = new List<ENodeb>{
            new ENodeb{ ENodebId=1, Name="E-1" },
            new ENodeb{ ENodebId=2, Name="E-2" }
        };

        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        private List<EvaluationOutdoorCell> QueryOutdoorCells()
        {
            return repository.Object.Query(eNodebs);
        }

        [Test]
        public void TestQueryOutdoorCells_OneCell()
        {
            List<EvaluationOutdoorCell> outdoorCells = QueryOutdoorCells();
            Assert.IsNotNull(outdoorCells);
            Assert.AreEqual(outdoorCells.Count, 1);
            Assert.AreEqual(outdoorCells[0].CellName, "E-1-0");
            Assert.AreEqual(outdoorCells[0].Azimuth, 55);
            Assert.AreEqual(outdoorCells[0].RsPower, 15.2);
        }

        [Test]
        public void TestQueryOutdoorCells_AddOneInvalidCell()
        {
            repository.Object.Insert(new Cell
            {
                ENodebId = 1,
                SectorId = 2,
                Height = 0,
                Azimuth = 67
            });
            List<EvaluationOutdoorCell> outdoorCells = QueryOutdoorCells();
            Assert.AreEqual(outdoorCells.Count, 1);
        }

        [Test]
        public void TestQueryOutdoorCells_AddOneValidCell()
        {
            repository.Object.Insert(new Cell
            {
                ENodebId = 1,
                SectorId = 2,
                Height = 20,
                Azimuth = 67
            });
            List<EvaluationOutdoorCell> outdoorCells = QueryOutdoorCells();
            Assert.AreEqual(outdoorCells.Count, 2);
            Assert.AreEqual(outdoorCells[1].CellName, "E-1-2");
            Assert.AreEqual(outdoorCells[1].Azimuth, 67);
        }

        [Test]
        public void TestQueryOutdoorCells_AddOneValidCell_DifferentENodeb()
        {
            repository.Object.Insert(new Cell
            {
                ENodebId = 2,
                SectorId = 2,
                Height = 20,
                Azimuth = 67
            });
            List<EvaluationOutdoorCell> outdoorCells = QueryOutdoorCells();
            Assert.AreEqual(outdoorCells.Count, 2);
            Assert.AreEqual(outdoorCells[1].CellName, "E-2-2");
            Assert.AreEqual(outdoorCells[1].Azimuth, 67);
        }
    }
}
