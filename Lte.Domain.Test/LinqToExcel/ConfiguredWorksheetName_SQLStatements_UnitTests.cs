using System.Data.OleDb;
using System.Linq;
using Lte.Domain.LinqToExcel;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToExcel
{
    [TestFixture]
    public class ConfiguredWorksheetName_SQLStatements_UnitTests : SQLLogStatements_Helper
    {
        [TestFixtureSetUp]
        public void fs()
        {
            InstantiateLogger();
        }

        [SetUp]
        public void Setup()
        {
            ClearLogEvents();
        }

        [Test]
        public void table_name_in_sql_statement_matches_configured_table_name()
        {
            var companies = from c in ExcelQueryFactory.Worksheet<Company>("Company Worksheet", "")
                            select c;

            try { companies.GetEnumerator(); }
            catch (OleDbException) { }
            const string expectedSql = "SELECT * FROM [Company Worksheet$]";
            Assert.AreEqual(expectedSql, GetSQLStatement());
        }
    }
}
