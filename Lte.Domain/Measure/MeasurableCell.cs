using System;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Regular;

namespace Lte.Domain.Measure
{
    public class MeasurableCell
    {
        public ComparableCell Cell { get; set; }

        public double TiltAngle { get; private set; }

        public ILinkBudget<double> Budget { get; private set; }

        public double ReceivedRsrp { get; set; }

        public MeasurableCell() { Cell = new ComparableCell(); }

        public MeasurableCell(ComparableCell cell, IGeoPoint<double> point, ILinkBudget<double> budget)
        {
            Cell = cell;
            Budget = budget;
            TiltAngle = cell.Cell.AngleFromCellTilt(point);
        }

        public MeasurableCell(ComparableCell cell, IGeoPoint<double> point)
        {
            Cell = cell;
            Budget = cell.Budget;
            TiltAngle = cell.Cell.AngleFromCellTilt(point);
        }

        public MeasurableCell(IGeoPoint<double> point, IOutdoorCell cell, byte modBase = 3) :
            this(new ComparableCell(point, cell, (byte)(cell.Pci % modBase)), point)
        { 
        }

        public void CalculateRsrp()
        {
            ReceivedRsrp = Cell.CalculateReceivedRsrp(Budget, TiltFactor());
        }

        public double TiltFactor(VerticalProperty property = null)
        {
            property = property ?? VerticalProperty.DefaultProperty;
            return property.CalculateFactor(TiltAngle);
        }

        public double DistanceInMeter 
        { 
            get { return Cell.Distance * 1000; }
        }

        public string CellName
        {
            get { return Cell.Cell.CellName; }
        }

        public byte PciModx
        {
            get { return Cell.PciModx; }
        }
    }

    public class MeasurePlanCell
    {
        public IOutdoorCell Cell
        { get; private set; }

        public string CellName
        {
            get { return Cell.CellName; }
        }

        public byte PciModx
        { get; private set; }

        public double ReceivePower
        { get; private set; }

        public double Rsrp
        {
            get { return 10 * Math.Log10(ReceivePower); }
        }

        public MeasurePlanCell() { }

        public MeasurePlanCell(MeasurableCell cell)
        {
            Cell = cell.Cell.Cell;
            PciModx = cell.Cell.PciModx;
            ReceivePower = cell.ReceivedRsrp.DbToPower();
        }

        public void UpdateRsrpPower(MeasurableCell mCell)
        {
            ReceivePower += mCell.ReceivedRsrp.DbToPower();
        }
    }
}
