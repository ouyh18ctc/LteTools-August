using System.Collections.Generic;
using Lte.Parameters.Concrete;
using NUnit.Framework;
using System.Data;
using Moq;

namespace Lte.Parameters.Test.Import
{
    [TestFixture]
    public class ImportExcelListTest
    {
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();
        private ImportClass myObject;

        private void MockMultiElements()
        {
            mockReader.Setup(x => x.Read()).Returns(
                () => mockReader.Object.Depth < 2).Callback(
                () => { int depth = mockReader.Object.Depth + 1; mockReader.Setup(x => x.Depth).Returns(depth); });
            mockReader.Setup(x => x.GetName(1)).Returns("Name1");
            mockReader.Setup(x => x.GetValue(1)).Returns(1);
            mockReader.Setup(x => x.GetName(2)).Returns("Name2");
            mockReader.Setup(x => x.GetValue(2)).Returns(2);
        }

        [SetUp]
        public void SetUp()
        {
            mockReader.Setup(x => x.Depth).Returns(0);
            mockReader.Setup(x => x.GetName(0)).Returns("Name0");
            mockReader.Setup(x => x.GetValue(0)).Returns(0);
        }

        [Test]
        public void TestImportExcelList()
        {
            myObject=new ImportClass(mockReader.Object);
            myObject.Import();
            Assert.IsNotNull(myObject);
            Assert.AreEqual(myObject.Name, "Name0");
            Assert.AreEqual(myObject.Value, 0);
        }

        [Test]
        public void TestImportExcelList_MultiElements()
        {
            MockMultiElements();
            myObject=new ImportClass(mockReader.Object);
            myObject.Import();
            Assert.IsNotNull(myObject);
            Assert.AreEqual(myObject.Name, "Name0");
            Assert.AreEqual(myObject.Value, 0);
            Assert.IsTrue(mockReader.Object.Read());
            Assert.AreEqual(mockReader.Object.Depth, 1);
            myObject=new ImportClass(mockReader.Object);
            myObject.Import();
            Assert.IsNotNull(myObject);
            Assert.AreEqual(myObject.Name, "Name1");
            Assert.AreEqual(myObject.Value, 1);
            Assert.IsTrue(mockReader.Object.Read());
            Assert.AreEqual(mockReader.Object.Depth, 2);
            myObject=new ImportClass(mockReader.Object);
            myObject.Import();
            Assert.IsNotNull(myObject);
            Assert.AreEqual(myObject.Name, "Name2");
            Assert.AreEqual(myObject.Value, 2);
            Assert.IsFalse(mockReader.Object.Read());
            Assert.AreEqual(mockReader.Object.Depth, 3);
        }

        [TestCase(2, new[] { "aaa", "bbb" }, new[] { 111, 222 })]
        [TestCase(3, new[] { "aaa", "bbb", "cc" }, new[] { 111, 222, -60 })]
        [TestCase(4, new[] { "aaa", "ee7", "bbb", "cc" }, new[] { 111, 78, 222, -60 })]
        public void TestImportExcelList_ImportDataTable(int rows, string[] values_1, int[] values_2)
        {
            DataTable dataTable = new DataTable();
            List<ColumnImportClass> importList = new List<ColumnImportClass>();

            dataTable.Columns.Add("Column1", typeof(string));
            dataTable.Columns.Add("Column2", typeof(int));
            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["Column1"] = values_1[i];
                dr["Column2"] = values_2[i];
                dataTable.Rows.Add(dr);
            }

            ImportExcelListService<ColumnImportClass> service =
                new ImportExcelListService<ColumnImportClass>(importList, dataTable);
            service.Import();
            Assert.IsNotNull(importList);
            Assert.AreEqual(importList.Count, rows);
            for (int i = 0; i < rows; i++)
            {
                Assert.AreEqual(importList[i].Column1, values_1[i]);
                Assert.AreEqual(importList[i].Column2, values_2[i]);
            }
        }
}
}
