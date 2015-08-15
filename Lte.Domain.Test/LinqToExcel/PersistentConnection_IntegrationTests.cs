using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Domain.LinqToExcel;
using Lte.Domain.LinqToExcel.Entities;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToExcel
{
	[TestFixture]
	public class PersistentConnection_IntegrationTests
	{
		private IExcelQueryFactory _factory;
	    private string excelFileName;

		[TestFixtureSetUp]
		public void fs()
		{
			string testDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
			excelFileName = Path.Combine(excelFilesDirectory, "Companies.xlsm");

		}

        [SetUp]
	    public void Setup()
	    {
			_factory = new ExcelQueryFactory(excelFileName);
            _factory.UsePersistentConnection = true;
	    }

	    [Test]
		public void WorksheetRangeNoHeader_returns_7_companies()
		{
		    List<ExcelRowNoHeader> companies =
		        (from c in _factory.WorksheetRangeNoHeader("A2", "D8", "Sheet1")
		            select c).ToList();

			Assert.AreEqual(7, companies.Count);
		}

		[TestCase(500)]
		public void WorksheetRange_can_query_sheet_times_on_same_connection(int times)
		{
		    int totalRows = 0;

			for (int i = 0; i < times; i++)
			{
			    List<ExcelRow> rows = (from cm in _factory.WorksheetRange("A1", "D8")
			        select cm).ToList();

			    totalRows += rows.Count;
			}

		    Assert.AreEqual((times*7), totalRows);
		}

        [Test]
        public void WorksheetRangeNoHeader_can_query()
        {
            List<ExcelRow> companies =
                (from c in _factory.WorksheetRange("A1", "D8")
                 select c).ToList();

            Assert.AreEqual(7, companies.Count);
        }

		[TestFixtureTearDown]
		public void td()
		{
			//dispose of the factory (and persistent connection)
			_factory.Dispose();
		}
	}
}
