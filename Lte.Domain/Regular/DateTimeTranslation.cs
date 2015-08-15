using System;
using System.Text.RegularExpressions;

namespace Lte.Domain.Regular
{
    /// <summary>
    /// 主要功能：定义一些基本的日期时间转换函数
    /// </summary>
    /// <remarks>
    /// 作者：欧阳晖
    /// 日期：2013年4月8日
    /// 修改记录：
    /// 2013年5月4日，增加转换时间到秒的操作
    /// </remarks>
    public static class DateTimeTranslation
    {
        /// <summary>
        /// 主要功能：根据字符串格式生成日期变量
        /// </summary>
        /// <remarks>
        /// 作者：欧阳晖
        /// 日期：2013年4月11日
        /// </remarks>
        /// <param name="date">yyyymmdd的日期格式</param>
        /// <returns>生成的日期变量</returns>
        public static DateTime GetDate(this string date)
        {
            if (!date.IsValidDateString()) throw new ArgumentOutOfRangeException();
            int year = Convert.ToInt32(date.Substring(0, 4));
            int month = Convert.ToInt32(date.Substring(4, 2));
            int day = Convert.ToInt32(date.Substring(6, 2));
            return new DateTime(year, month, day);
        }

        public static DateTime GetDateExtend(this string date)
        {
            while (date.Length >= 8)
            {
                if (!date.Substring(0, 8).IsValidDateString())
                {
                    date = date.Substring(1);
                    continue;
                }
                int year = Convert.ToInt32(date.Substring(0, 4));
                int month = Convert.ToInt32(date.Substring(4, 2));
                int day = Convert.ToInt32(date.Substring(6, 2));
                return new DateTime(year, month, day);
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// 主要功能：根据字符串格式生成日期时间变量
        /// </summary>
        /// <remarks>
        /// 作者：欧阳晖
        /// 日期：2013年4月11日
        /// </remarks>
        /// <param name="time">yyyymmddHHmm的日期时间格式</param>
        /// <returns>日期时间变量（精确到分钟）</returns>
        public static DateTime GetTime(this string time)
        {
            if (time.Substring(0, 8).IsValidDateString())
            {
                int year = Convert.ToInt32(time.Substring(0, 4));
                int month = Convert.ToInt32(time.Substring(4, 2));
                int day = Convert.ToInt32(time.Substring(6, 2));
                int hour = Convert.ToInt32(time.Substring(8, 2));
                int minute = Convert.ToInt32(time.Substring(10, 2));
                return new DateTime(year, month, day, hour, minute, 0);
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 主要功能：判断一个字符串是否为合法的日期字符串
        /// </summary>
        /// <remarks>
        /// 作者：欧阳晖
        /// 日期：2013年4月26日
        /// </remarks>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static bool IsValidDateString(this string dateString)
        {
            string testString = "^((((1[6-9]|[2-9]\\d)\\d{2})(0[13578]|1[02])(0[1-9]|[12]\\d|3[01]))|"
                + "(((1[6-9]|[2-9]\\d)\\d{2})(0[13456789]|1[012])(0[1-9]|[12]\\d|30))|"
                + "(((1[6-9]|[2-9]\\d)\\d{2})02(0[1-9]|1\\d|2[0-8]))|"
                + "(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|"
                + "((16|[2468][048]|[3579][26])00))0229))$";
            Regex regexText = new Regex(testString);
            return regexText.IsMatch(dateString);
        }

        /// <summary>
        /// 主要功能：获得精确到秒的日期时间变量
        /// </summary>
        /// <remarks>
        /// 作者：欧阳晖
        /// 日期：2013年5月4日
        /// </remarks>
        /// <param name="time">yyyymmddHHmmss的日期时间格式</param>
        /// <returns>精确到秒的日期时间变量</returns>
        public static DateTime GetTimeSecond(this string time)
        {
            if (time.Substring(0, 8).IsValidDateString())
            {
                int year = Convert.ToInt32(time.Substring(0, 4));
                int month = Convert.ToInt32(time.Substring(4, 2));
                int day = Convert.ToInt32(time.Substring(6, 2));
                int hour = Convert.ToInt32(time.Substring(8, 2));
                int minute = Convert.ToInt32(time.Substring(10, 2));
                int second = Convert.ToInt32(time.Substring(12, 2));
                return new DateTime(year, month, day, hour, minute, second);
            }
            return DateTime.Now;
        }
    }
}
