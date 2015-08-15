using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class SecureConversionTest
    {
        [Test]
        public void TestSecureConversion()
        {
            Assert.AreEqual(("2").ConvertToByte(1), 2);
            Assert.AreEqual(("10000").ConvertToByte(1), 1);
            Assert.AreEqual(("-2").ConvertToInt(0), -2);
            Assert.AreEqual(("-33a").ConvertToInt(0), 0);
            Assert.AreEqual(("2.3345").ConvertToDouble(3), 2.3345);
            Assert.AreEqual(("2.33.45").ConvertToDouble(3), 3);
        }
    }
}
