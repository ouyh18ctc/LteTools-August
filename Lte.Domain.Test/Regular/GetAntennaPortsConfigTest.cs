using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class GetAntennaPortsConfigTest
    {
        [Test]
        public void TestGetAntennaPortsConfig()
        {
            Assert.AreEqual(("2t2r").GetAntennaPortsConfig(), AntennaPortsConfigure.Antenna2T2R);
            Assert.AreEqual(("2T4R").GetAntennaPortsConfig(), AntennaPortsConfigure.Antenna2T4R);
            Assert.AreEqual(("1T1R").GetAntennaPortsConfig(), AntennaPortsConfigure.Antenna1T1R);
        }
    }
}
