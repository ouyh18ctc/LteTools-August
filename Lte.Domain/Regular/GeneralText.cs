using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Regular
{
    public static class GeneralText
    {
        public static bool IsValidString(this string inputText, string regularExpression)
        {
            return (String.IsNullOrEmpty(regularExpression)) || (Regex.IsMatch(inputText, regularExpression));
        }

        public static int StringListLength(this IEnumerable<string> stringList)
        {
            return stringList.Sum(s => s.Length);
        }

        public static IEnumerable<int> GetFieldWidth(this IEnumerable<string> stringList, int totalWidth)
        {
            IEnumerable<string> enumerable = stringList as string[] ?? stringList.ToArray();
            int totalLength = enumerable.StringListLength();
            return enumerable.Select(s => s.Length * totalWidth / totalLength);
        }

        public static IEnumerable<int> GetFieldWidth(this IEnumerable<int> itemList, int totalWidth)
        {
            IEnumerable<int> enumerable = itemList as int[] ?? itemList.ToArray();
            int totalLength = enumerable.Sum();
            return enumerable.Select(s => s * totalWidth / totalLength);
        }

        public static bool MatchKeyWordInLine(this string sLine, string keyWord)
        {
            return sLine.IndexOf(keyWord, StringComparison.Ordinal) != -1;
        }

        public static string[] GetSplittedFields(this string line)
        {
            return line.Split(new[] { '=', ',', '\"', ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetSplittedFields(this string line, char splitter)
        {
            return line.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetSplittedFields(this string line, string splitter)
        {
            return line.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetSubStringInFirstPairOfChars(this string line, char first, char second)
        {
            int index1 = line.IndexOf(first);
            int index2 = line.IndexOf(second);
            if (index2 == -1) { index2 = line.Length; }
            string ipData = line.Substring(index1 + 1, index2 - index1 - 1);
            return ipData;
        }

        public static string GetSubStringInFirstBracket(this string line)
        {
            return line.GetSubStringInFirstPairOfChars('(', ')');
        }

        public static int[] GenerateIpDataDigits(this string ipData, string errorTest)
        {
            int[] digits = new int[6];
            string[] buf = ipData.GetSplittedFields(',');
            if (buf.Length < 6) { throw new IOException("Too few fields"); }
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    int digit = Int32.Parse(buf[i]);
                    if ((digit < 0) || (digit > 255)) { throw new IOException("At least one digit is negative number or two large"); }
                    digits[i] = digit;
                }
                catch
                { throw new IOException(errorTest); }
            }
            return digits;
        }

        public static StreamReader GetStreamReader(this string source)
        {
            byte[] stringAsByteArray = Encoding.UTF8.GetBytes(source);
            Stream stream = new MemoryStream(stringAsByteArray);

            var streamReader = new StreamReader(stream, Encoding.UTF8);
            return streamReader;
        }

        public static AntennaPortsConfigure GetAntennaPortsConfig(this string description)
        {
            switch (description.ToUpper().Trim())
            { 
                case "2T2R":
                    return AntennaPortsConfigure.Antenna2T2R;
                case "1T1R":
                    return AntennaPortsConfigure.Antenna1T1R;
                case "2T8R":
                    return AntennaPortsConfigure.Antenna2T8R;
                case "4T4R":
                    return AntennaPortsConfigure.Antenna4T4R;
                default:
                    return AntennaPortsConfigure.Antenna2T4R;
            }
        }

        public static bool IsAllUserRecordFileName(this string fileName)
        {
            const string regexText = "^\\d{1}_CHR_\\d{1}_\\d{14}$";
            const string regexText2 = "^\\d{2}_CHR_\\d{1}_\\d{14}$";
            return Regex.IsMatch(fileName, regexText) || Regex.IsMatch(fileName, regexText2);
        }

        public static string RetrieveFileNameBody(this string fullName)
        {
            int dotLocation = fullName.LastIndexOf(".", StringComparison.Ordinal);
            return (dotLocation < 0) ? fullName : fullName.Substring(fullName.LastIndexOf("\\",
                StringComparison.Ordinal) + 1,
                dotLocation - fullName.LastIndexOf("\\", StringComparison.Ordinal) - 1);
        }
    }

    public static class RegularChecking
    {
        public static bool IsLegalImsi(this string imsi)
        {
            const string regexText = "^46003\\d{10}$";

            return Regex.IsMatch(imsi, regexText);
        }

        public static bool IsLegalEsn(this string esn)
        {
            const string regexText = "^[0-9a-fA-F]{14}$";

            return Regex.IsMatch(esn, regexText);
        }

        public static bool IsLegalIp(this string ip)
        {
            const string regexText = "^((?:(?:25[0-5]|2[0-4]\\d|((1\\d{2})|([1-9]?\\d)))\\.){3}" +
                                     "(?:25[0-5]|2[0-4]\\d|((1\\d{2})|([1-9]?\\d))))$";

            return Regex.IsMatch(ip, regexText);
        }

        public static int StringMatchTimes(this string objectString, string matchString)
        {
            return Regex.Matches(objectString, matchString).Count;
        }

        public static int? StringToInt(this string text)
        {
            try
            {
                return Convert.ToInt32(text);
            }
            catch
            {
                return null;
            }
        }

        public static short? StringToShort(this string text)
        {
            try
            {
                return Convert.ToInt16(text);
            }
            catch
            {
                return null;
            }
        }
    }
}
