using Lte.Parameters.Entities;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class ImportENodebExcelInfoTest
    {
        private readonly ENodeb eNodeb = new ENodeb();
        private readonly ENodebExcel eNodebInfo = new ENodebExcel
        {
            ENodebId = 1,
            Name = "First eNodeb",
            Factory = "Huawei",
            Gateway = new IpAddress("10.17.165.100"),
            Ip = new IpAddress("10.17.165.121")
        };

        [Test]
        public void TestImportENodeb_Original()
        {
            eNodeb.Import(eNodebInfo);
            Assert.AreEqual(eNodeb.ENodebId, 1);
            Assert.AreEqual(eNodeb.Name, "First eNodeb");
            Assert.AreEqual(eNodeb.Factory, "Huawei");
            Assert.AreEqual(eNodeb.GatewayIp.AddressString, "10.17.165.100");
            Assert.AreEqual(eNodeb.Ip.AddressString, "10.17.165.121");
        }

        [Test]
        public void TestImportENodeb_OtherInfos()
        {
            eNodebInfo.Longtitute = 113.2879;
            eNodebInfo.Lattitute = 22.9788;
            eNodebInfo.DivisionDuplex = "TDD";
            eNodebInfo.PlanNum = "FSL1122";
            eNodeb.Import(eNodebInfo);
            Assert.AreEqual(eNodeb.Longtitute, 113.2879);
            Assert.AreEqual(eNodeb.Lattitute, 22.9788);
            Assert.AreEqual(eNodeb.PlanNum, "FSL1122");
            Assert.IsFalse(eNodeb.IsFdd);
        }
    }
}
