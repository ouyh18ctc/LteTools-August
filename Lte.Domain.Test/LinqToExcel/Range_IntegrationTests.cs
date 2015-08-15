using System;
using System.IO;
using System.Linq;
using Lte.Domain.LinqToExcel;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToExcel
{
    [TestFixture]
    public class Range_IntegrationTests
    {
        ExcelQueryFactory _factory;
        string _excelFileName;

        [TestFixtureSetUp]
        public void fs()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "Companies.xls");
        }

        [SetUp]
        public void s()
        {
            _factory = new ExcelQueryFactory(_excelFileName);
        }

        [Test]
        public void use_sheetData_and_worksheetIndex()
        {
            var companies = from c in _factory.WorksheetRange<Company>("C6", "F13", 4)
                            select c;

            Assert.AreEqual(7, companies.Count(), "Count");
            Assert.AreEqual("ACME", companies.First().Name, "First Company Name");
        }

        [Test]
        public void use_row_and_worksheetIndex()
        {
            var companies = from c in _factory.WorksheetRange("c6", "f13", 4)
                            select c;

            Assert.AreEqual(7, companies.Count(), "Count");
            Assert.AreEqual("Ontario Systems", companies.Last()["Name"].ToString(), "Last Company Name");
        }

        [Test]
        public void use_row_where_null()
        {
            var factory = new ExcelQueryFactory(_excelFileName + "x");
            var companies = from c in factory.WorksheetRange("A1", "D4", "NullCells")
                            where c["EmployeeCount"] == null
                            select c;

            Assert.AreEqual(2, companies.Count(), "Count");
        }

        [Test]
        public void use_row_no_header_where_null()
        {
            var factory = new ExcelQueryFactory(_excelFileName + "x");
            var companies = from c in factory.WorksheetRangeNoHeader("A1", "D4", "NullCells")
                            where c[2] == null
                            select c;

            Assert.AreEqual(2, companies.Count(), "Count");
        }
    }
}
