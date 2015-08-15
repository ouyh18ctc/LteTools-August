using System.Data;
using Lte.Parameters.Entities;
using Lte.Parameters.Test.Entities;
using NUnit.Framework;
using Moq;
using System;
using System.Reflection;
using System.Linq;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class ENodebExcelConstructionTest
    {
        private ENodebExcel eNodebExcel = new ENodebExcel();
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();

        [SetUp]
        public void SetUp()
        {
            Tuple<string, string>[] contents =
            {
                new Tuple<string,string>("地市","Foshan"),
                new Tuple<string,string>("CELL_ID","1"),
                new Tuple<string,string>("eNodeB ID","3344"),
                new Tuple<string,string>("经度","112.123"),
                new Tuple<string,string>("纬度","23.456"),
                new Tuple<string,string>("区域","Chancheng"),
                new Tuple<string,string>("镇区","Zumiao"),
                new Tuple<string,string>("eNodeBName","My Bts")
            };
            mockReader.MockReaderContents(contents);
        }

        [Test]
        public void TestENodebExcelConstruction_BasicParameters()
        {
            PropertyInfo[] properties = (typeof(ENodebExcel)).GetProperties();
            Assert.AreEqual(properties.Length, 16);
            PropertyInfo property = properties.FirstOrDefault(x => x.Name == "Gateway");
            Assert.IsNotNull(property);
            Assert.AreEqual(property.PropertyType.Name, "IpAddress");
            eNodebExcel = new ENodebExcel(mockReader.Object);
            eNodebExcel.Import();
            Assert.IsNotNull(eNodebExcel);
            Assert.AreEqual(eNodebExcel.CityName, "Foshan");
            Assert.AreEqual(eNodebExcel.DistrictName, "Chancheng");
            Assert.AreEqual(eNodebExcel.TownName, "Zumiao");
            Assert.AreEqual(eNodebExcel.Name, "My Bts");
            Assert.AreEqual(eNodebExcel.ENodebId, 3344);
            Assert.AreEqual(eNodebExcel.Longtitute, 112.123);
            Assert.AreEqual(eNodebExcel.Lattitute, 23.456);
        }

        [Test]
        public void TestENodebExcelContruction_Gateway()
        {
            mockReader.Setup(x => x.GetName(4)).Returns("IP");
            mockReader.Setup(x => x.GetName(5)).Returns("网关");
            mockReader.Setup(x => x.GetValue(4)).Returns("10.17.165.27");
            mockReader.Setup(x => x.GetValue(5)).Returns("10.17.165.100");
            eNodebExcel = new ENodebExcel(mockReader.Object);
            eNodebExcel.Import();
            Assert.IsNotNull(eNodebExcel);
            Assert.AreEqual(eNodebExcel.Ip.AddressString, "10.17.165.27");
            Assert.AreEqual(eNodebExcel.Gateway.AddressString, "10.17.165.100");
        }
    }
}
