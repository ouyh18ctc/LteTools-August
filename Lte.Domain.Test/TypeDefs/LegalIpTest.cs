using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.TypeDefs
{
    [TestFixture]
    public class LegalIpTest
    {
        [Test]
        public void TestLegalIp_AllZeros_Legal()
        {
            const string address = "0.0.0.0";
            Assert.IsTrue(address.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_AllOnes_Legal()
        {
            const string address = "1.1.1.1";
            Assert.IsTrue(address.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_All127_Legal()
        {
            const string address = "127.127.127.127";
            Assert.IsTrue(address.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_All255_Legal()
        {
            string address = "255.255.255.255";
            Assert.IsTrue(address.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_LocalMachine_Legal()
        {
            string address = "127.0.0.1";
            Assert.IsTrue(address.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_AType_Legal()
        {
            string address = "10.17.165.100";
            string address1 = "10.17.0.128";
            string address2 = "10.16.128.33";
            Assert.IsTrue(address.IsLegalIp());
            Assert.IsTrue(address1.IsLegalIp());
            Assert.IsTrue(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_BType_Legal()
        {
            string address = "172.16.165.100";
            string address1 = "172.154.1.255";
            string address2 = "172.183.128.201";
            Assert.IsTrue(address.IsLegalIp());
            Assert.IsTrue(address1.IsLegalIp());
            Assert.IsTrue(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_CType_Legal()
        {
            string address = "192.168.202.177";
            string address1 = "192.170.1.23";
            string address2 = "192.183.22.201";
            Assert.IsTrue(address.IsLegalIp());
            Assert.IsTrue(address1.IsLegalIp());
            Assert.IsTrue(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_Multicast_Legal()
        {
            string address = "224.0.0.2";
            string address1 = "224.2.1.211";
            string address2 = "224.0.35.21";
            Assert.IsTrue(address.IsLegalIp());
            Assert.IsTrue(address1.IsLegalIp());
            Assert.IsTrue(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_TooManyDigits_Illegal()
        {
            string address = "12.33.45.2.1";
            string address1 = "0.0.12.37.2.9";
            string address2 = "233.67.90.87.56";
            Assert.IsFalse(address.IsLegalIp());
            Assert.IsFalse(address1.IsLegalIp());
            Assert.IsFalse(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_TooLessDigits_Illegal()
        {
            string address = "12.33.45";
            string address1 = "0.0";
            string address2 = "233.67";
            Assert.IsFalse(address.IsLegalIp());
            Assert.IsFalse(address1.IsLegalIp());
            Assert.IsFalse(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_TooLargeDigits_Illegal()
        {
            string address = "12.33.45.256";
            string address1 = "1000.0.12.37";
            string address2 = "301.67.90.87";
            Assert.IsFalse(address.IsLegalIp());
            Assert.IsFalse(address1.IsLegalIp());
            Assert.IsFalse(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_TooLargeDigits2_Illegal()
        {
            string address = "12.331.45.252";
            string address1 = "1001.0.12.371";
            string address2 = "30.67.900.87";
            Assert.IsFalse(address.IsLegalIp());
            Assert.IsFalse(address1.IsLegalIp());
            Assert.IsFalse(address2.IsLegalIp());
        }

        [Test]
        public void TestLegalIp_OtherChars_Illegal()
        {
            string address = "12a.33.45.2e1";
            string address1 = "0-0.12.37.2";
            string address2 = "233.6_7.90.87.56";
            Assert.IsFalse(address.IsLegalIp());
            Assert.IsFalse(address1.IsLegalIp());
            Assert.IsFalse(address2.IsLegalIp());
        }
    }
}
