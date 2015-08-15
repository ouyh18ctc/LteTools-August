using System;
using Lte.Parameters.Entities;
using System.Reflection;
using NUnit.Framework;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class CellExcelAttributesTest
    {
        private readonly CellExcel cellInfo = new CellExcel();
        private PropertyInfo[] properties;

        [SetUp]
        public void TestInitialize()
        {
            cellInfo.AntennaGain = 17.5;
            cellInfo.AntennaInfo = "aabb";
            cellInfo.ENodebId = 3344;
            cellInfo.SectorId = 2;
            cellInfo.Pci = 101;
            properties = (typeof(CellExcel)).GetProperties();
        }

        [Test]
        public void TestCellExcelAttributes()
        {
            Assert.AreEqual(properties.Length, 20);
            Attribute attribute = Attribute.GetCustomAttribute(properties[0], typeof(LteExcelColumnAttribute));
            Assert.IsNotNull(attribute);
            Assert.AreEqual((attribute as SimpleExcelColumnAttribute).Name, "eNodeB ID");
            Assert.AreEqual((attribute as SimpleExcelColumnAttribute).DefaultValue, "1");
            Assert.AreEqual(properties[0].PropertyType.Name, "Int32");
            Assert.AreEqual(cellInfo.ENodebId, 3344);
            properties[0].SetValue(cellInfo, 2233);
            Assert.AreEqual(cellInfo.ENodebId, 2233);
        }
    }
}
