using System;
using System.Collections.Generic;
using System.Linq;

namespace Lte.Domain.Regular
{
    public static class ExtendedMath
    {
        private const double Eps = 1E-6;

        public static double DbToPower(this double dbValue)
        {
            return Math.Pow(10, dbValue / 10);
        }

        public static double SumOfPowerLevel<T>(this IEnumerable<T> levelSet, Func<T, double> getLevel)
        {
            IEnumerable<T> enumerable = levelSet as T[] ?? levelSet.ToArray();
            if (!enumerable.Any()) { return Double.MinValue; }
            if (enumerable.Count() == 1) { return getLevel(enumerable.ElementAt(0)); }
            return 10 * Math.Log10(enumerable.Sum(x => Math.Pow(10, getLevel(x) / 10)));
        }

        public static double Ceiling(double source, byte dec)
        {
            return DecPowerNegative(Math.Ceiling(DecPowerPositive(source, dec)), dec);
        }

        public static double Floor(double source, byte dec)
        {
            return DecPowerNegative(Math.Floor(DecPowerPositive(source, dec)), dec);
        }

        public static double Round(double source, byte dec)
        {
            return DecPowerNegative(Math.Round(DecPowerPositive(source, dec)), dec);
        }

        public static double DecPowerNegative(double result, byte dec)
        {
            for (byte i = 0; i < dec; i++)
            {
                result /= 10;
            }
            return result;
        }

        public static double DecPowerPositive(double source, byte dec)
        {
            for (byte i = 0; i < dec; i++)
            {
                source *= 10;
            }
            return source;
        }

        public static double AverageIgnoreZeros<T>(this IEnumerable<T> source, Func<T, double> property)
            where T : class
        {
            IEnumerable<T> validParts = source.Where(x => Math.Abs(property(x)) > Eps);
            return (!validParts.Any()) ? 0 : validParts.Average(property);
        }
    }
}
