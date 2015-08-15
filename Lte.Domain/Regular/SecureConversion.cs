using System;
using System.Globalization;
using System.Linq.Expressions;
using Moq;

namespace Lte.Domain.Regular
{
    public static class SecureConversion
    {
        public static byte ConvertToByte(this string text, byte defaultValue)
        {
            byte returnValue;
            return byte.TryParse(text, out returnValue) ? returnValue : defaultValue;
        }

        public static short ConvertToShort(this string text, short defaultValue)
        {
            short returnValue;
            return short.TryParse(text, out returnValue) ? returnValue : defaultValue;
        }

        public static int ConvertToInt(this string text, int defaultValue)
        {
            int returnValue;
            return int.TryParse(text, out returnValue) ? returnValue : defaultValue;
        }

        public static long ConvertToLong(this string text, long defaultValue)
        {
            long returnValue;
            return long.TryParse(text, out returnValue) ? returnValue : defaultValue;
        }

        public static double ConvertToDouble(this string text, double defaultValue)
        {
            double returnValue;
            return double.TryParse(text, out returnValue) ? returnValue : defaultValue;
        }

        public static DateTime ConvertToDateTime(this string text, DateTime defaultValue)
        {
            DateTime returnValue;
            return DateTime.TryParse(text, out returnValue) ? returnValue : defaultValue;
        }
    }

    public static class MockOperations
    {
        public static void BindGetAndSetAttributes<TObject, TValue>(this Mock<TObject> mock,
            Expression<Func<TObject, TValue>> getter, Action<TObject, TValue> setter)
            where TObject : class
        {
            mock.SetupSet(x => setter(x, It.IsAny<TValue>())).Callback<TValue>(
                a => mock.SetupGet(getter).Returns(a));
        }
    }

    public static class HexOperations
    {
        public static int HexStringToInt(this string hexString)
        {
            if (hexString.Length > 8) { throw new ArgumentOutOfRangeException("hexString"); }
            return Int32.Parse(hexString, NumberStyles.HexNumber);
        }

        public static int GenerateMask(byte length)
        {
            int result = 1;
            for (byte index = 1; index < length; index++)
            {
                result |= 1 << index;
            }
            return result;
        }

        public static int GetFieldContent(this string hexString, byte position = 0, byte length = 1)
        {
            int number = hexString.HexStringToInt();
            return (number >> (hexString.Length * 4 - position - length)) & GenerateMask(length);
        }

        public static byte GetLastByte(this int source)
        {
            return (byte)(source & GenerateMask(8));
        }
    }
}
