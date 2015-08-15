using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Measure;

namespace Lte.Domain.Test.Measure.Result
{
    public class StubMeasurePointResult : MeasurePointResult
    {
        public override IEnumerable<MeasurableCell> CalculateDifferentModInterferences(IList<MeasurableCell> cellList)
        {
            return cellList.Where(x => x.PciModx == 102);
        }

        public override IEnumerable<MeasurableCell> CalculateSameModInterferences(IList<MeasurableCell> cellList)
        {
            return cellList.Where(x => x.PciModx == 101);
        }

        private static List<MeasurableCell> cellList_OneSameModCell = new List<MeasurableCell>{
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 101 },
                ReceivedRsrp = -90 }
        };

        public static List<MeasurableCell> CellListOneSameModCell
        {
            get { return cellList_OneSameModCell; }
        }

        private static List<MeasurableCell> cellList_OneDiffModCell = new List<MeasurableCell>{
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 102 },
                ReceivedRsrp = -90 }
        };

        public static List<MeasurableCell> CellListOneDiffModCell
        {
            get { return cellList_OneDiffModCell; }
        }

        private static List<MeasurableCell> cellList_TwoSameModCells = new List<MeasurableCell>{
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 101 },
                ReceivedRsrp = -90 },
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 101 },
                ReceivedRsrp = -90 }
        };

        public static List<MeasurableCell> CellListTwoSameModCells
        {
            get { return cellList_TwoSameModCells; }
        }

        private static List<MeasurableCell> cellList_TwoDiffModCells = new List<MeasurableCell>{
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 102 },
                ReceivedRsrp = -90 },
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 102 },
                ReceivedRsrp = -90 }
        };

        public static List<MeasurableCell> CellListTwoDiffModCells
        {
            get { return cellList_TwoDiffModCells; }
        }

        private static List<MeasurableCell> cellList_OneSameModCell_OneDiffModCell = new List<MeasurableCell>{
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 101 },
                ReceivedRsrp = -90 },
            new MeasurableCell {
                Cell = new ComparableCell { PciModx = 102 },
                ReceivedRsrp = -90 }
        };

        public static List<MeasurableCell> CellListOneSameModCellOneDiffModCell
        {
            get { return cellList_OneSameModCell_OneDiffModCell; }
        }
    }
}
