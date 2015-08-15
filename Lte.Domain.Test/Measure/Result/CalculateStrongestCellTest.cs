using System.Collections.Generic;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Result
{
    [TestFixture]
    public class CalculateStrongestCellTest
    {
        private IMeasurePointResult result = new SfMeasurePointResult();
        private List<MeasurableCell> cellList = new List<MeasurableCell>();

        [Test]
        public void TestStrongestCell_EmptyList()
        {
            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = cellList
            };
            result.StrongestCell = repository.CalculateStrongestCell();
            Assert.IsNull(result.StrongestCell);
        }

        [Test]
        public void TestStrongestCell_OneElement()
        {
            MeasurableCell mcell = new MeasurableCell();
            mcell.ReceivedRsrp = 1.2345;
            cellList.Add(mcell);
            Assert.AreEqual(cellList.Count, 1);

            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = cellList
            };
            result.StrongestCell = repository.CalculateStrongestCell();
            Assert.IsNotNull(result.StrongestCell);
            Assert.AreEqual(result.StrongestCell.ReceivedRsrp, 1.2345);
        }

        [Test]
        public void TestStrongestCell_TwoElements()
        {
            MeasurableCell mcell1 = new MeasurableCell();
            MeasurableCell mcell2 = new MeasurableCell();
            mcell1.ReceivedRsrp = 1.2345;
            mcell2.ReceivedRsrp = 2.2345;
            cellList.Add(mcell1);
            cellList.Add(mcell2);

            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = cellList
            };
            result.StrongestCell = repository.CalculateStrongestCell();
            Assert.AreSame(result.StrongestCell, mcell2);
            Assert.AreEqual(result.StrongestCell.ReceivedRsrp, 2.2345);
        }

        [Test]
        public void TestStrongestCell_ThreeElements()
        {
            MeasurableCell mcell1 = new MeasurableCell();
            MeasurableCell mcell2 = new MeasurableCell();
            MeasurableCell mcell3 = new MeasurableCell();
            mcell1.ReceivedRsrp = 1.2345;
            mcell2.ReceivedRsrp = 2.2345;
            mcell3.ReceivedRsrp = 1.5345;
            cellList.Add(mcell1);
            cellList.Add(mcell2);
            cellList.Add(mcell3);

            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = cellList
            };
            result.StrongestCell = repository.CalculateStrongestCell();
            Assert.AreSame(result.StrongestCell, mcell2);
            Assert.AreEqual(result.StrongestCell.ReceivedRsrp, 2.2345);
        }
    }
}
