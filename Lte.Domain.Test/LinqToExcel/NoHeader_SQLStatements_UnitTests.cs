using System;
using System.Data.OleDb;
using System.Linq;
using Lte.Domain.LinqToExcel;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToExcel
{
    [TestFixture]
    public class NoHeader_SQLStatements_UnitTests : SQLLogStatements_Helper
    {
        private ExcelQueryFactory _factory;

        [TestFixtureSetUp]
        public void fs()
        {
            InstantiateLogger();
        }

        [SetUp]
        public void s()
        {
            _factory = new ExcelQueryFactory("");
            ClearLogEvents();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "Cannot use WorksheetRangeNoHeader on csv files")]
        public void range_csv_file_throws_exception()
        {
            var csvFile = @"C:\ExcelFiles\NoHeaderRange.csv";

            var excel = new ExcelQueryFactory(csvFile);
            var companies = (from c in excel.WorksheetRangeNoHeader("B9", "E15")
                             select c).ToList();
        }

        [Test]
        public void where_clause()
        {
            var warnerCompany = from c in _factory.WorksheetNoHeader()
                               where c[1] == "Bugs Bunny"
                               select c;

            try { warnerCompany.GetEnumerator(); }
            catch (OleDbException) { }

            var expectedSQL = "SELECT * FROM [Sheet1$] WHERE (F2 = ?)";
            Assert.AreEqual(expectedSQL, GetSQLStatement());
        }

        [Test]
        public void where_is_null()
        {
            var warnerCompany = from c in _factory.WorksheetNoHeader()
                                where c[1] == null
                                select c;

            try { warnerCompany.GetEnumerator(); }
            catch (OleDbException) { }

            var expectedSQL = "SELECT * FROM [Sheet1$] WHERE (F2 IS NULL)";
            Assert.AreEqual(expectedSQL, GetSQLStatement());
        }
    }
}
