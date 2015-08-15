using System.Collections.Generic;
using System.Linq;

namespace Lte.Domain.Measure
{
    public class MeasurePlanCellRelation
    {
        public MeasurePlanCell MainCell
        { get; set; }

        public IList<MeasurePlanCell> InterferenceCells
        { get; set; }

        public double TrafficLoad
        { get; set; }

        public int CoverPoints
        { get; set; }

        public void ImportMeasurePoint(MeasurePoint mPoint)
        {
            if (MainCell.Cell != mPoint.Result.StrongestCell.Cell.Cell) { return; }
            MainCell.UpdateRsrpPower(mPoint.Result.StrongestCell);
            CoverPoints++;

            foreach (MeasurableCell mcell 
                in mPoint.CellRepository.CellList.Where(x => x != mPoint.Result.StrongestCell))
            {
                MeasurePlanCell mpCell = InterferenceCells.FirstOrDefault(
                    x => x.Cell == mcell.Cell.Cell);

                if (mpCell != null)
                {
                    mpCell.UpdateRsrpPower(mcell);
                }
                else
                {
                    InterferenceCells.Add(new MeasurePlanCell(mcell));
                }
            }
        }
    }
}
