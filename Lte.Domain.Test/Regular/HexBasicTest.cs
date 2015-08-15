using System;
using System.Globalization;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class HexBasicTest
    {
        [Test]
        public void TestParseHexString()
        {
            Assert.AreEqual(Int32.Parse("8E2", NumberStyles.HexNumber), 2274);
            Assert.AreEqual(Int32.Parse("8E26", NumberStyles.HexNumber), 36390);
            Assert.AreEqual(Int32.Parse("8E2671", NumberStyles.HexNumber), 9315953);
            Assert.AreEqual(Int32.Parse("8E2671AB", NumberStyles.HexNumber), -1910083157);
            Assert.AreEqual(Int64.Parse("00002A02", NumberStyles.HexNumber), 10754);
        }

        [Test]
        public void TestHexNumberShift()
        {
            int a = 1 << 15 | 1 << 14 | 1 << 13 | 1 << 12 | 1 << 11;
            Assert.AreEqual((10754 & a) >> 11, 5);
        }
    }
}
