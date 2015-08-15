using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class DateTimeTranslationTest
    {
        [TestCase("20020708","2002-7-8")]
        [TestCase("20130715","2013-07-15")]
        [TestCase("20120809","2012-08-9")]
        public void Test_GetDate(string input, string expected)
        {
            DateTime result = input.GetDate();
            Assert.AreEqual(result, DateTime.Parse(expected));
        }

        [TestCase("200207081")]
        [TestCase("pre20120809")]
        public void Test_GetDate_InvalidFormat(string input)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => input.GetDate());
        }

        [TestCase("20020708", "2002-7-8")]
        [TestCase("20130715", "2013-07-15")]
        [TestCase("20120809", "2012-08-9")]
        [TestCase("200207081", "2002-7-8")]
        [TestCase("pre20120809", "2012-8-9")]
        [TestCase("佛山20150427_重叠覆盖小区详情_20150430113941", "2015-4-27")]
        public void Test_GetDateExtend(string input, string expected)
        {
            DateTime result = input.GetDateExtend();
            Assert.AreEqual(result, DateTime.Parse(expected));
        }

        [TestCase("201302")]
        [TestCase("0o2012-0302")]
        [TestCase("trr-20131313")]
        public void Test_GetDateExtend_InvalidFormat(string input)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => input.GetDateExtend());
        }
    }
}
