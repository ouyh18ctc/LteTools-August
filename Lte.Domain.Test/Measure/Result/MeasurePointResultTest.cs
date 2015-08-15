using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Result
{
    [TestFixture]
    public class MeasurePointResultTest
    {
        private MeasurePointResult result = new StubMeasurePointResult();
        private List<MeasurableCell> _cellList = new List<MeasurableCell>();

        [SetUp]
        public void TestInitialize()
        {
            result.StrongestCell = new MeasurableCell
            {
                ReceivedRsrp = -80,
                Cell = new ComparableCell { PciModx = 101 }
            };
        }

        [Test]
        public void TestUpdateSameModInterference_EmptyList()
        {
            IEnumerable<MeasurableCell> sameModInterference = result.UpdateSameModInterference(_cellList);
            Assert.IsNotNull(sameModInterference);
            Assert.AreEqual(sameModInterference.Count(), 0);
            Assert.AreEqual(result.SameModInterferenceLevel, double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, double.MinValue);
        }

        [Test]
        public void TestUpdateDifferentModInterference_EmptyList()
        {
            IEnumerable<MeasurableCell> diffModInterference = result.UpdateDifferentModInterference(_cellList);
            Assert.IsNotNull(diffModInterference);
            Assert.AreEqual(diffModInterference.Count(), 0);
            Assert.AreEqual(result.DifferentModInterferenceLevel, double.MinValue);
            Assert.AreEqual(result.SameModInterferenceLevel, double.MinValue);
        }

        [Test]
        public void TestUpdateSameModInterference_OneCell_CellList()
        {
            _cellList = StubMeasurePointResult.CellListOneSameModCell;
            IEnumerable<MeasurableCell> sameModInterference = result.UpdateSameModInterference(_cellList);
            Assert.IsNotNull(sameModInterference);
            Assert.AreEqual(sameModInterference.Count(), 1);
            Assert.AreEqual(result.SameModInterferenceLevel, -90);
            _cellList = StubMeasurePointResult.CellListOneDiffModCell;
            sameModInterference = result.UpdateSameModInterference(_cellList);
            Assert.IsNotNull(sameModInterference);
            Assert.AreEqual(sameModInterference.Count(), 0);
        }

        [Test]
        public void TestUpdateDifferentModInterference_OneCell_CellList()
        {
            _cellList = StubMeasurePointResult.CellListOneSameModCell;
            IEnumerable<MeasurableCell> diffModInterference = result.UpdateDifferentModInterference(_cellList);
            Assert.IsNotNull(diffModInterference);
            Assert.AreEqual(diffModInterference.Count(), 0);
            Assert.AreEqual(result.DifferentModInterferenceLevel, double.MinValue);
            _cellList = StubMeasurePointResult.CellListOneDiffModCell;
            diffModInterference = result.UpdateDifferentModInterference(_cellList);
            Assert.IsNotNull(diffModInterference);
            Assert.AreEqual(diffModInterference.Count(), 1);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -90);
        }

        [Test]
        public void TestUpdateSameModInterference_TwoCells()
        {
            _cellList = StubMeasurePointResult.CellListTwoSameModCells;
            IEnumerable<MeasurableCell> sameModInterference = result.UpdateSameModInterference(_cellList);
            Assert.IsNotNull(sameModInterference);
            Assert.AreEqual(sameModInterference.Count(), 2);
            Assert.AreEqual(result.SameModInterferenceLevel, -86.9897, 1E-6);
            _cellList = StubMeasurePointResult.CellListTwoDiffModCells;
            sameModInterference = result.UpdateSameModInterference(_cellList);
            Assert.IsNotNull(sameModInterference);
            Assert.AreEqual(sameModInterference.Count(), 0);
            _cellList = StubMeasurePointResult.CellListOneSameModCellOneDiffModCell;
            sameModInterference = result.UpdateSameModInterference(_cellList);
            Assert.IsNotNull(sameModInterference);
            Assert.AreEqual(sameModInterference.Count(), 1);
            Assert.AreEqual(result.SameModInterferenceLevel, -90);
        }

        [Test]
        public void TestUpdateDifferentModInterference_TwoCells()
        {
            _cellList = StubMeasurePointResult.CellListTwoSameModCells;
            IEnumerable<MeasurableCell> diffModInterference = result.UpdateDifferentModInterference(_cellList);
            Assert.IsNotNull(diffModInterference);
            Assert.AreEqual(diffModInterference.Count(), 0);
            Assert.AreEqual(result.DifferentModInterferenceLevel, double.MinValue);
            _cellList = StubMeasurePointResult.CellListTwoDiffModCells;
            diffModInterference = result.UpdateDifferentModInterference(_cellList);
            Assert.IsNotNull(diffModInterference);
            Assert.AreEqual(diffModInterference.Count(), 2);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -86.9897, 1E-6);
            _cellList = StubMeasurePointResult.CellListOneSameModCellOneDiffModCell;
            diffModInterference = result.UpdateDifferentModInterference(_cellList);
            Assert.IsNotNull(diffModInterference);
            Assert.AreEqual(diffModInterference.Count(), 1);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -90);
        }
    }
}
