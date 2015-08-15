using Lte.Domain.Test.Ipv4;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Domain.Test.TypeDefs
{
    [TestFixture]
    public class IpAddressTest
    {
        [Test]
        public void TestIpAddress_GetString()
        {
            IIpAddress address = new StubIpAddress()
            {
                IpByte1 = 0,
                IpByte2 = 1,
                IpByte3 = 27,
                IpByte4 = 255
            };
            Assert.AreEqual(address.GetString(), "0.1.27.255");
            address.IpByte2 = 201;
            Assert.AreEqual(address.GetString(), "0.201.27.255");
        }

        [Test]
        public void TestIpAddress_GetAddress()
        {
            IIpAddress address = new StubIpAddress();
            Assert.AreEqual(address.IpByte1, 0);
            Assert.AreEqual(address.IpByte2, 0);
            Assert.AreEqual(address.IpByte3, 0);
            Assert.AreEqual(address.IpByte4, 0);
            bool result = address.SetAddress("2.201.23.255");
            Assert.IsTrue(result);
            Assert.AreEqual(address.IpByte1, 2);
            Assert.AreEqual(address.IpByte2, 201);
            Assert.AreEqual(address.IpByte3, 23);
            Assert.AreEqual(address.IpByte4, 255);
            result = address.SetAddress("17.17");
            Assert.IsFalse(result);
            Assert.AreEqual(address.IpByte1, 2);
            Assert.AreEqual(address.IpByte2, 201);
            Assert.AreEqual(address.IpByte3, 23);
            Assert.AreEqual(address.IpByte4, 255);
        }

        [Test]
        public void TestIpAddress_AddressValue()
        {
            Assert.AreEqual((new IpAddress("0.0.0.0")).AddressValue, 0);
            Assert.AreEqual((new IpAddress("0.0.0.1")).AddressValue, 1);
            Assert.AreEqual((new IpAddress("0.0.1.0")).AddressValue, 256);
            Assert.AreEqual((new IpAddress("0.1.0.0")).AddressValue, 65536);
        }

        [Test]
        public void TestIpAddress_SetAddressValue()
        {
            IpAddress ipAddress = new IpAddress()
            {
                AddressValue = 3 * 256 * 256 * 256 + 2 * 256 * 256 + 7 * 256 + 6
            };
            Assert.AreEqual(ipAddress.GetString(), "3.2.7.6");

            for (int i = -1; i > -128888888; i -= 254)
            {
                ipAddress = new IpAddress() { AddressValue = i };
                Assert.AreEqual(ipAddress.AddressValue, i);
            }
        }
    }
}
