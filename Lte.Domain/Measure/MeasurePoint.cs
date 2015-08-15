using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;

namespace Lte.Domain.Measure
{
    /// <summary>
    /// 仿真程序的核心，定义一个测量点
    /// </summary>
    /// <remarks>日期：2014年4月2日</remarks>
    public class MeasurePoint : IGeoPoint<double>, IGeoPointReadonly<double>
    {
        public double Longtitute { get; set; }

        public double Lattitute { get; set; }

        public IMeasurePointResult Result { get; set; }

        public MeasurableCellRepository CellRepository { get; private set; }

        public ComparableCell ComparableCellAt(int i)
        {
            return (i >= CellRepository.CellList.Count) ? 
                null : 
                CellRepository.CellList.ElementAt(i).Cell;
        }

        public double ReceivedRsrpAt(int i)
        {
            return (i >= CellRepository.CellList.Count) ? 
                Double.MinValue : CellRepository.CellList.ElementAt(i).ReceivedRsrp;
        }

        public MeasurePoint()
        { 
            Result = new MfMeasurePointResult();
            CellRepository = new MeasurableCellRepository();
        }

        public MeasurePoint(IGeoPoint<double> position) : this()
        {
            Longtitute = position.Longtitute;
            Lattitute = position.Lattitute;
        }

        public void ImportCells(IEnumerable<IOutdoorCell> cells, ILinkBudget<double> budget)
        {
            ComparableCell[] compCells = GenerateComaparbleCellList(cells);
            CellRepository.GenerateMeasurableCellList(budget, compCells, this);
        }

        public void ImportCells(IEnumerable<IOutdoorCell> cells, IList<ILinkBudget<double>> budgetList,
            IBroadcastModel model, byte modBase = 3)
        {
            ComparableCell[] compCells = GenerateComaparbleCellList(cells, budgetList, model, modBase);
            CellRepository.GenerateMeasurableCellList(compCells, this);
        }

        public void ImportCells(IEnumerable<IOutdoorCell> cells, IList<ILinkBudget<double>> budgetList,
            byte modBase = 3)
        {
            ComparableCell[] compCells = GenerateComaparbleCellList(cells, budgetList, modBase);
            CellRepository.GenerateMeasurableCellList(compCells, this);
        }

        private ComparableCell[] GenerateComaparbleCellList(IEnumerable<IOutdoorCell> cells)
        {
            ComparableCell[] compCells = cells.Select(x => new ComparableCell(this, x)).ToArray();
            Array.Sort(compCells);
            return compCells;
        }

        public ComparableCell[] GenerateComaparbleCellList(IEnumerable<IOutdoorCell> cells,
            IList<ILinkBudget<double>> budgetList, IBroadcastModel model, byte modBase = 3)
        {
            ComparableCell[] compCells 
                = cells.Select(x => new ComparableCell(this, x, budgetList, model, (byte)(x.Pci % modBase))).ToArray();
            Array.Sort(compCells);
            return compCells;
        }

        private ComparableCell[] GenerateComaparbleCellList(IEnumerable<IOutdoorCell> cells,
            IList<ILinkBudget<double>> budgetList, byte modBase = 3)
        {
            ComparableCell[] compCells
                = cells.Select(x => new ComparableCell(this, x, budgetList, modBase)).ToArray();
            Array.Sort(compCells);
            return compCells;
        }

        public void CalculatePerformance(double trafficLoad)
        {
            Result.StrongestCell = CellRepository.CalculateStrongestCell();

            Result.CalculateInterference(CellRepository.CellList, trafficLoad);
            Result.NominalSinr = Math.Min(Result.StrongestCell.ReceivedRsrp - Result.TotalInterferencePower, 100);
        }

        public MeasurePlanCellRelation GenerateMeasurePlanCellRelation(double trafficLoad)
        {
            MeasurePlanCellRelation mpcRelation = new MeasurePlanCellRelation
            {
                MainCell = new MeasurePlanCell(Result.StrongestCell),
                InterferenceCells = new List<MeasurePlanCell>(),
                TrafficLoad = trafficLoad,
                CoverPoints = 1
            };

            foreach (MeasurableCell mcell 
                in CellRepository.CellList.Where(x => x != Result.StrongestCell))
            {
                mpcRelation.InterferenceCells.Add(new MeasurePlanCell(mcell));
            }

            return mpcRelation;
        }
    }
}
