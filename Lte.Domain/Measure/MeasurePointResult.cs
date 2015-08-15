using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;

namespace Lte.Domain.Measure
{
    public interface IMeasurePointResult
    {
        IEnumerable<MeasurableCell> CalculateDifferentModInterferences(IList<MeasurableCell> cellList);

        void CalculateInterference(IList<MeasurableCell> cellList, double trafficLoad);

        IEnumerable<MeasurableCell> CalculateSameModInterferences(IList<MeasurableCell> cellList);

        double DifferentModInterferenceLevel { get; set; }

        double NominalSinr { get; set; }

        double SameModInterferenceLevel { get; set; }

        MeasurableCell StrongestCell { get; set; }

        MeasurableCell StrongestInterference { get; set; }

        double TotalInterferencePower { get; set; }

        IEnumerable<MeasurableCell> UpdateDifferentModInterference(IList<MeasurableCell> cellList);

        IEnumerable<MeasurableCell> UpdateSameModInterference(IList<MeasurableCell> cellList);

        void UpdateTotalInterference(double trafficLoad,
            IEnumerable<MeasurableCell> rsInterference, IEnumerable<MeasurableCell> trafficInterference);
    }

    public abstract class MeasurePointResult : IMeasurePointResult
    {
        public MeasurableCell StrongestCell { get; set; }

        public MeasurableCell StrongestInterference { get; set; }

        public double SameModInterferenceLevel { get; set; }

        public double DifferentModInterferenceLevel { get; set; }

        public double TotalInterferencePower { get; set; }

        public double NominalSinr { get; set; }

        private const double Eps = 1E-6;

        public MeasurePointResult()
        {
            SameModInterferenceLevel = double.MinValue;
            DifferentModInterferenceLevel = double.MinValue;
            TotalInterferencePower = double.MinValue;
            NominalSinr = double.MinValue;
        }

        public void CalculateInterference(IList<MeasurableCell> cellList, double trafficLoad)
        {
            IEnumerable<MeasurableCell> rsInterferences = UpdateSameModInterference(cellList);

            IEnumerable<MeasurableCell> trafficInterference = UpdateDifferentModInterference(cellList);

            UpdateTotalInterference(trafficLoad, rsInterferences, trafficInterference);
        }

        public IEnumerable<MeasurableCell> UpdateSameModInterference(IList<MeasurableCell> cellList)
        {
            IEnumerable<MeasurableCell> rsInterferences = CalculateSameModInterferences(cellList);

            if (rsInterferences != null && rsInterferences.Any())
            {
                double secondRsrp = rsInterferences.Max(x => x.ReceivedRsrp);
                StrongestInterference = rsInterferences.FirstOrDefault(x => Math.Abs(x.ReceivedRsrp - secondRsrp) < Eps);
                SameModInterferenceLevel = rsInterferences.SumOfPowerLevel(x => x.ReceivedRsrp);
            }
            else
            {
                StrongestInterference = null;
                SameModInterferenceLevel = Double.MinValue;
            }
            return rsInterferences;
        }

        public IEnumerable<MeasurableCell> UpdateDifferentModInterference(IList<MeasurableCell> cellList)
        {
            IEnumerable<MeasurableCell> trafficInterference = CalculateDifferentModInterferences(cellList);

            DifferentModInterferenceLevel = (trafficInterference != null && trafficInterference.Any()) ?
                trafficInterference.SumOfPowerLevel(x => x.ReceivedRsrp) :
                DifferentModInterferenceLevel = Double.MinValue;

            return trafficInterference;
        }

        public void UpdateTotalInterference(double trafficLoad,
            IEnumerable<MeasurableCell> rsInterference, IEnumerable<MeasurableCell> trafficInterference)
        {
            double loadModifier = 10 * Math.Log10(trafficLoad);
            IEnumerable<double> sameModInterferenceLevelList
                = (rsInterference != null) ? rsInterference.Select(x => x.ReceivedRsrp) : null;
            IEnumerable<double> differentModInterferenceLevelList
                = (trafficInterference != null) ? trafficInterference.Select(x => x.ReceivedRsrp + loadModifier) : null;

            IEnumerable<double> concatInterferenceLevelList
                = (sameModInterferenceLevelList == null) ? differentModInterferenceLevelList :
                ((differentModInterferenceLevelList == null) ? sameModInterferenceLevelList
                : sameModInterferenceLevelList.Concat(differentModInterferenceLevelList));

            TotalInterferencePower = (concatInterferenceLevelList == null) ? Double.MinValue :
                concatInterferenceLevelList.SumOfPowerLevel(x => x);
        }

        public abstract IEnumerable<MeasurableCell> CalculateSameModInterferences(IList<MeasurableCell> cellList);

        public abstract IEnumerable<MeasurableCell> CalculateDifferentModInterferences(IList<MeasurableCell> cellList);
    }

    public class MfMeasurePointResult : MeasurePointResult
    {
        public override IEnumerable<MeasurableCell> CalculateSameModInterferences(IList<MeasurableCell> cellList)
        {
            if (cellList.Count == 0) { return null; }
            if (StrongestCell == null) { return null; }

            IEnumerable<MeasurableCell> interferences = cellList.Where(x => x != StrongestCell
                && x.Cell != null && x.Cell.Cell.Frequency == StrongestCell.Cell.Cell.Frequency
                && x.Cell.PciModx == StrongestCell.Cell.PciModx);

            return interferences;
        }

        public override IEnumerable<MeasurableCell> CalculateDifferentModInterferences(IList<MeasurableCell> cellList)
        {
            if (cellList.Count == 0) { return null; }
            if (StrongestCell == null) { return null; }

            IEnumerable<MeasurableCell> interferences
                = cellList.Where(x => x.Cell != null && x.Cell.Cell.Frequency == StrongestCell.Cell.Cell.Frequency
                    && x.Cell.PciModx != StrongestCell.Cell.PciModx);
            return interferences;
        }
    }

    public class SfMeasurePointResult : MeasurePointResult
    {
        public override IEnumerable<MeasurableCell> CalculateSameModInterferences(IList<MeasurableCell> cellList)
        {
            if (cellList.Count == 0) { return null; }
            if (StrongestCell == null) { return null; }

            IEnumerable<MeasurableCell> interferences = cellList.Where(x => x != StrongestCell);

            return interferences.Where(x => x.Cell != null && x.Cell.PciModx == StrongestCell.Cell.PciModx);
        }

        public override IEnumerable<MeasurableCell> CalculateDifferentModInterferences(IList<MeasurableCell> cellList)
        {
            if (cellList.Count == 0 || StrongestCell == null) { return null; }

            IEnumerable<MeasurableCell> interferences
                = cellList.Where(x => x.Cell != null && x.Cell.PciModx != StrongestCell.Cell.PciModx);
            return interferences;
        }
    }
}
