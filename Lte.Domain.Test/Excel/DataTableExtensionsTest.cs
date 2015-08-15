using Lte.Domain.Regular;
using System.Data;
using System.Collections.Generic;
using NUnit.Framework;

namespace Lte.Domain.Test.Excel
{
    [TestFixture]
    public class DataTableExtensionsTest
    {
        private readonly List<ColumnClass> myColumnList = new List<ColumnClass>();

        [SetUp]
        public void TestInitialize()
        {
            myColumnList.Add(new ColumnClass
            {
                FirstField = 1,
                SecondField = 1.1,
                NoAttributeField = 2
            });
            myColumnList.Add(new ColumnClass
            {
                FirstField = 2,
                SecondField = 2.1,
                NoAttributeField = 3
            });
            myColumnList.Add(new ColumnClass
            {
                FirstField = 3,
                SecondField = 3.1,
                NoAttributeField = 4
            });
        }

        [Test]
        public void TestDataTableExtensions()
        {
            DataTable myTable = myColumnList.ListToDataTable("myTable");
            Assert.IsNotNull(myTable);
            Assert.AreEqual(myTable.Rows.Count, 3);
            Assert.AreEqual(myTable.Columns.Count, 3);
            Assert.AreEqual(myTable.TableName, "myTable");
            Assert.AreEqual(myTable.Rows[0]["FirstField"], 1);
            Assert.AreEqual(myTable.Rows[1]["SecondField"], 2.1);
            Assert.AreEqual(myTable.Rows[2]["NoAttributeField"], 4);
        }
    }
}
