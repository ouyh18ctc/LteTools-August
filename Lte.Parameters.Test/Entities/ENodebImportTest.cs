using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class ENodebImportTest
    {
        private readonly ENodebExcel eNodebExcel = new ENodebExcel
        {
            Name = "eNodeb-excel",
            ENodebId = 1,
            Address = "address 1"
        };

        private ENodeb eNodeb;

        private void Initialize()
        {
            eNodeb = new ENodeb
            {
                Name = "eNodeb",
                ENodebId = 2,
                Address = "address 2",
                TownId = 10
            };
        }

        [Test]
        public void Test_Default()
        {
            Initialize();
            eNodeb.Import(eNodebExcel);
            Assert.AreEqual(eNodeb.Name, "eNodeb-excel");
            Assert.AreEqual(eNodeb.ENodebId, 1);
            Assert.AreEqual(eNodeb.Address, "address 1");
            Assert.AreEqual(eNodeb.TownId, -1);
        }

        [Test]
        public void Test_UpdateTownId2()
        {
            Initialize();
            eNodeb.Import(eNodebExcel, 2);
            Assert.AreEqual(eNodeb.Name, "eNodeb-excel");
            Assert.AreEqual(eNodeb.ENodebId, 1);
            Assert.AreEqual(eNodeb.Address, "address 1");
            Assert.AreEqual(eNodeb.TownId, 2);
        }

        [Test]
        public void Test_UpdateTownId2_InvariantENodebId()
        {
            Initialize();
            eNodeb.Import(eNodebExcel, 2, false);
            Assert.AreEqual(eNodeb.Name, "eNodeb-excel");
            Assert.AreEqual(eNodeb.ENodebId, 2);
            Assert.AreEqual(eNodeb.Address, "address 1");
            Assert.AreEqual(eNodeb.TownId, 2);
        }
    }
}
