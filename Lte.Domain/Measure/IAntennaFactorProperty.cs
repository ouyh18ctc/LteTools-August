using System;

namespace Lte.Domain.Measure
{
    public interface IAntennaFactorProperty
    {
        double CalculateFactor(double parameter);
    }

    public class HorizontalProperty : IAntennaFactorProperty
    {
        private readonly double _quadraicCoefficient = 0;
        private readonly double _linearCoefficient = 0;
        private readonly double _maxValue = 0;

        public static readonly HorizontalProperty DefaultProperty = new HorizontalProperty();

        public HorizontalProperty(double halfPowerAzimuth = 32, double frontBackRatio = 30)
        {
            if (halfPowerAzimuth <= 0) { throw new ArgumentOutOfRangeException(); }
            _quadraicCoefficient = Math.Max(
                (frontBackRatio - 270 / halfPowerAzimuth) / (8100 - 90 * halfPowerAzimuth), 0);
            _linearCoefficient = 3 / halfPowerAzimuth - _quadraicCoefficient * halfPowerAzimuth;
            _maxValue = frontBackRatio;
        }

        public double CalculateFactor(double parameter)
        {
            return Math.Min(_quadraicCoefficient * parameter * parameter
                + _linearCoefficient * parameter, _maxValue);
        }
    }

    public class VerticalProperty : IAntennaFactorProperty
    {
        private readonly double _coefficient;
        private readonly double _maxValue;

        public static readonly VerticalProperty DefaultProperty = new VerticalProperty();

        public VerticalProperty(double halfTiltAngle = 7, double maxValue = 30)
        {
            _coefficient = 3 / halfTiltAngle;
            _maxValue = maxValue;
        }

        public double CalculateFactor(double parameter)
        {
            return Math.Min(_coefficient * parameter, _maxValue);
        }
    }

    public class DistanceAzimuthMetric
    {
        private readonly double _moment = 35;
        private readonly double _minimumMetric = -10;

        public static readonly DistanceAzimuthMetric Default = new DistanceAzimuthMetric();

        public DistanceAzimuthMetric(double moment = 35, double minimumMetric = -70)
        {
            _moment = moment;
            _minimumMetric = minimumMetric;
        }

        public double Calculate(double distance, double azimuthFactor)
        {
            return Math.Max(_moment * Math.Log10(distance) + azimuthFactor, _minimumMetric);
        }
    }
}
