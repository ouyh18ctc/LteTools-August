using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;

namespace Lte.Domain.Measure
{
    public class ComparableCell : IComparable
    {
        public IOutdoorCell Cell { get; set; }

        public double Distance { get; set; }

        public double AzimuthAngle { get; set; }

        public byte PciModx { get; set; }

        public ILinkBudget<double> Budget { get; set; }

        private const double Eps = 1E-6;

        public ComparableCell()
        { 
        }

        public ComparableCell(IGeoPoint<double> point, IOutdoorCell cell, IList<ILinkBudget<double>> budgetList,
            IBroadcastModel model, byte pciModx = 0)
        {
            SetupComparableCell(point, cell);
            ILinkBudget<double> budget = budgetList.FirstOrDefault(
                x => Math.Abs(x.TransmitPower - cell.RsPower) < Eps
                && Math.Abs(x.AntennaGain - cell.AntennaGain) < Eps);
            if (budget == null)
            {
                budget = new LinkBudget(model, cell.RsPower, cell.AntennaGain);
                budgetList.Add(budget);
            }

            Budget = budget;
            PciModx = pciModx;
        }

        public ComparableCell(IGeoPoint<double> point, IOutdoorCell cell, IList<ILinkBudget<double>> budgetList,
            byte modBase = 3)
        {
            SetupComparableCell(point, cell);
            ILinkBudget<double> budget = budgetList.FirstOrDefault(
                x => Math.Abs(x.TransmitPower - cell.RsPower) < Eps
                && Math.Abs(x.AntennaGain - cell.AntennaGain) < Eps 
                && x.Model.Earfcn == cell.Frequency);
            if (budget == null)
            {
                budget = new LinkBudget(cell);
                budgetList.Add(budget);
            }
            Budget = budget;
            PciModx = (byte)(cell.Pci % modBase);
        }

        public ComparableCell(IGeoPoint<double> point, IOutdoorCell cell, byte pciModx = 0)
        {
            SetupComparableCell(point, cell);
            Budget = new LinkBudget(cell);
            PciModx = pciModx;
        }

        public ComparableCell(double distance, double azimuthAngle)
        {
            Distance = distance;
            AzimuthAngle = azimuthAngle;
        }

        public int CompareTo(object other)
        {
            if (!(other is ComparableCell)) { throw new ArgumentException(); }
            double thisMetric = MetricCalculate();
            double otherMetric = (other as ComparableCell).MetricCalculate();
            return (thisMetric >= otherMetric) ? 1 : -1;
        }

        public void SetAzimuthAngle(IGeoPoint<double> point, double value)
        {
            Cell.Azimuth = value;
            AzimuthAngle = Cell.AngleFromCellAzimuth(point);
        }

        public double AzimuthFactor(HorizontalProperty property = null)
        {
            property = property ?? HorizontalProperty.DefaultProperty;
            return property.CalculateFactor(Math.Abs(AzimuthAngle));
        }

        public void SetupComparableCell(IGeoPoint<double> point, IOutdoorCell cell)
        {
            Cell = cell;
            Distance = point.SimpleDistance(cell);
            AzimuthAngle = cell.AngleFromCellAzimuth(point);
        }

        public double CalculateReceivedRsrp(ILinkBudget<double> budget, double tiltFactor)
        {
            return budget.CalculateReceivedPower(Distance, Cell.Height) - AzimuthFactor() - tiltFactor;
        }

        protected double MetricCalculate(HorizontalProperty property = null,
            DistanceAzimuthMetric metric = null)
        {
            metric = metric ?? DistanceAzimuthMetric.Default;
            return metric.Calculate(Distance, AzimuthFactor(property));
        }
    }
}
