using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class HexOperationsTest
    {
        [Test]
        public void TestHexStringToInt()
        {
            Assert.AreEqual(("A").HexStringToInt(), 10);
            Assert.AreEqual(("12B").HexStringToInt(), 299);
            Assert.AreEqual(("0012B").HexStringToInt(), 299);
        }

        [Test]
        public void TestHex_GenerateMask()
        {
            Assert.AreEqual(HexOperations.GenerateMask(1), 1);
            Assert.AreEqual(HexOperations.GenerateMask(3), 7);
        }

        [Test]
        public void TestHex_GetFieldContent_FullLength()
        {
            const string hexString = "105A2C33";
            Assert.AreEqual(hexString.GetFieldContent(), 0);
            Assert.AreEqual(hexString.GetFieldContent(length: 4), 1);
            Assert.AreEqual(hexString.GetFieldContent(1, 4), 2);
            Assert.AreEqual(hexString.GetFieldContent(10, 12), 1675);
        }

        [Test]
        public void TestHex_GetFieldContent_ShortLength()
        {
            const string hexString = "733E5";
            Assert.AreEqual(hexString.GetFieldContent(), 0);
            Assert.AreEqual(hexString.GetFieldContent(length: 4), 7);
            Assert.AreEqual(hexString.GetFieldContent(3, 4), 9);
        }

        [Test]
        public void TestHex_GetFieldContent_RRCConnectionRelease()
        {
            const string signal = "2a02";
            Assert.AreEqual(signal.GetFieldContent(1, 4), 5);
            Assert.AreEqual(signal.GetFieldContent(5, 2), 1);
            Assert.AreEqual(signal.GetFieldContent(7), 0);
            Assert.AreEqual(signal.GetFieldContent(13, 2), 1);
        }
    }
}
