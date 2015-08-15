using System;
using System.Collections.Generic;
using Lte.Domain.LinqToExcel.Entities;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToExcel
{
    [TestFixture]
    public class RowTest
    {
        IDictionary<string, int> _columnMappings;
        IList<ExcelCell> _cells;

        [SetUp]
        public void s()
        {
            _columnMappings = new Dictionary<string, int>();
            _columnMappings["Name"] = 0;
            _columnMappings["Favorite Sport"] = 1;
            _columnMappings["Age"] = 2;

            _cells = new List<ExcelCell>();
            _cells.Add(new ExcelCell("Paul"));
            _cells.Add(new ExcelCell("Ultimate Frisbee"));
            _cells.Add(new ExcelCell(25));
        }

        [Test]
        public void index_maps_cells_correctly()
        {
            var row = new ExcelRow(_cells, _columnMappings);
            Assert.AreEqual(_cells[0], row[0]);
            Assert.AreEqual(_cells[1], row[1]);
            Assert.AreEqual(_cells[2], row[2]);
        }

        [Test]
        public void column_name_index_maps_cells_correctly()
        {
            var row = new ExcelRow(_cells, _columnMappings);
            Assert.AreEqual(_cells[0], row["Name"]);
            Assert.AreEqual(_cells[1], row["Favorite Sport"]);
            Assert.AreEqual(_cells[2], row["Age"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "'First Name' column name does not exist. Valid column names are 'Name', 'Favorite Sport', 'Age'")]
        public void invalid_column_name_index_throws_argument_exception()
        {
            var newRow = new ExcelRow(_cells, _columnMappings);
            var temp = newRow["First Name"];
        }
    }
}
