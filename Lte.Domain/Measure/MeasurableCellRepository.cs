using System;
using System.Collections.Generic;
using System.Linq;

namespace Lte.Domain.Measure
{
    public class MeasurableCellRepository
    {
        public IList<MeasurableCell> CellList { get; set; }

        private const short _maxMeasurableCells = 40;

        private const double Eps = 1E-6;

        public MeasurableCellRepository()
        {
            CellList = new List<MeasurableCell>();
        }

        public void GenerateMeasurableCellList(ILinkBudget<double> budget, ComparableCell[] compCells,
            MeasurePoint point)
        {
            CellList.Clear();
            int count = Math.Min(compCells.Length, _maxMeasurableCells);
            for (int i = 0; i < count; i++)
            {
                MeasurableCell c = new MeasurableCell(compCells[i], point, budget);
                c.CalculateRsrp();
                CellList.Add(c);
            }
        }

        public void GenerateMeasurableCellList(ComparableCell[] compCells, MeasurePoint point)
        {
            CellList.Clear();
            int count = Math.Min(compCells.Length, _maxMeasurableCells);
            for (int i = 0; i < count; i++)
            {
                MeasurableCell c = new MeasurableCell(compCells[i], point);
                c.CalculateRsrp();
                CellList.Add(c);
            }
        }

        public MeasurableCell CalculateStrongestCell()
        {
            if (CellList.Count == 0)
            {
                return null;
            }
            double maxRsrp = CellList.Max(x => x.ReceivedRsrp);
            return CellList.FirstOrDefault(x => Math.Abs(x.ReceivedRsrp - maxRsrp) < Eps);
        }

    }
}
