using System;
using Lte.Domain.Geo;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Measure
{
    public interface ILinkBudget<out T>
    {
        T TransmitPower { get; }

        T AntennaGain { get; }

        IBroadcastModel Model { get; }
    }

    public static class LinkBudgetOperations
    {
        public static double CalculateReceivedPower(this ILinkBudget<double> budget, double distance, double antennaHeight)
        {
            return budget.TransmitPower + budget.AntennaGain - budget.Model.CalculatePathLoss(distance, antennaHeight);
        }

    }

    public class LinkBudget : ILinkBudget<double>
    {
        public double TransmitPower { get; private set; }

        public double AntennaGain { get; private set; }

        public IBroadcastModel Model { get; private set; }

        public LinkBudget(IBroadcastModel model, double power = 15.2, double gain =18)
        {
            model = model ?? BroadcastModel.DefaultModel;
            Model = model;
            TransmitPower = power;
            AntennaGain = gain;
        }

        public LinkBudget(IOutdoorCell cell, UrbanType utype = UrbanType.Large)
            : this(new BroadcastModel(cell.Frequency, utype), cell.RsPower, cell.AntennaGain)
        { 
        }
    }

    public interface IBroadcastModel
    {
        void SetFrequencyBand(FrequencyBandType type);
        void SetKvalue(UrbanType utype);

        double K1 { get; }
        double K4 { get; }
        double Frequency { get; }
        UrbanType UrbanType { get; }
        int Earfcn { get; }
    }

    public class BroadcastModel : IBroadcastModel
    {
        public double K1 { get; private set; }

        public double K4 { get; private set; }

        public double Frequency { get; private set; }

        public int Earfcn
        {
            get { return Frequency.GetEarfcn(); }
        }

        private UrbanType _urbanType;

        public UrbanType UrbanType
        {
            get { return _urbanType; }
        }

        public static readonly IBroadcastModel DefaultModel = new BroadcastModel();

        public BroadcastModel(FrequencyBandType ftype = FrequencyBandType.Downlink2100, UrbanType utype = UrbanType.Large)
        {

            SetFrequencyBand(ftype);
            _urbanType = utype;
            SetKvalue(utype);
        }

        public BroadcastModel(int fcn, UrbanType utype = UrbanType.Large)
        {
            SetEarfcn(fcn);
            _urbanType = utype;
            SetKvalue(utype);
        }

        public void SetKvalue(UrbanType utype)
        {
            _urbanType = utype;
            switch (utype)
            {
                case UrbanType.Middle:
                case UrbanType.Large:
                    K1 = 69.55;
                    K4 = 44.9;
                    break;
                default:
                    K1 = 85.83;
                    K4 = 60;
                    break;
            }
        }

        public void SetFrequencyBand(FrequencyBandType type)
        {
            switch (type)
            {
                case FrequencyBandType.Downlink2100:
                    Frequency = 2120;
                    break;
                case FrequencyBandType.Uplink2100:
                    Frequency = 1930;
                    break;
                case FrequencyBandType.Downlink1800:
                    Frequency = 1860;
                    break;
                case FrequencyBandType.Uplink1800:
                    Frequency = 1765;
                    break;
                case FrequencyBandType.Tdd2600:
                    Frequency = 2645;
                    break;
                default:
                    Frequency = 2120;
                    break;
            }
        }

        public void SetEarfcn(int fcn)
        {
            Frequency = fcn.GetFrequency();
        }
    }

    public static class BroadcastModelOperations
    {
        private const double K2 = 26.16;
        private const double K3 = -13.82;
        private const double K5 = -6.55;
        private const double DiffractionLoss = 0.8;
        private const double ClutterLoss = 0;

        public static void Validate(double distanceInKilometer, double baseHeight, double mobileHeight)
        {
            const double eps = 1E-6;
            if (distanceInKilometer <= eps) throw new ArgumentOutOfRangeException("distanceInKilometer");
            if (baseHeight <= eps * 100) throw new ArgumentOutOfRangeException("baseHeight");
            if (mobileHeight <= eps * 100) throw new ArgumentOutOfRangeException("mobileHeight");
        }

        private static double CalculateModifiedFactor(this IBroadcastModel model, double mobileHeight, UrbanType urbanType)
        {
            switch (urbanType)
            {
                case UrbanType.Middle:
                    return (1.1 * Math.Log10(model.Frequency) - 0.7) * mobileHeight - 1.56 * Math.Log10(model.Frequency) + 0.8;
                case UrbanType.Large:
                    return 8.29 * Math.Log10(1.54 * model.Frequency) * Math.Log10(1.54 * model.Frequency) - 1.1;
                default:
                    return 3.2 * Math.Log10(11.75 * model.Frequency) * Math.Log10(11.75 * model.Frequency) - 4.97;
            }
        }

        public static double CalculatePathLoss(this IBroadcastModel model, double distanceInKilometer,
            double baseHeight, double mobileHeight = 1.5)
        {
            Validate(distanceInKilometer, baseHeight, mobileHeight);
            return model.K1 + K2 * Math.Log10(model.Frequency) + K3 * Math.Log10(baseHeight)
                + Math.Log10(model.CalculateModifiedFactor(mobileHeight, model.UrbanType))
                + (model.K4 + K5 * Math.Log10(baseHeight)) * Math.Log10(distanceInKilometer)
                + DiffractionLoss + ClutterLoss;
        }
    }
}
