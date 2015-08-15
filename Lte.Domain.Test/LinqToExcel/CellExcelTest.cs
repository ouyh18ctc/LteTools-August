using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.LinqToExcel.Entities;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToExcel
{
    [TestFixture]
    public class CellExcelTest
    {
        [Test]
        public void Cell_implicitly_converts_to_string()
        {
            var newCell = new ExcelCell("some value");
            Assert.IsTrue("some value" == newCell);
        }

        [Test]
        public void Constructor_sets_cell_value()
        {
            var newCell = new ExcelCell("hello");
            Assert.AreEqual("hello", newCell.Value);
        }

        [Test]
        public void As_converts_cell_value_type_to_generic_argument_type()
        {
            var newCell = new ExcelCell("2");
            Assert.AreEqual(2, newCell.Cast<int>());
            Assert.AreEqual(typeof(int), newCell.Cast<int>().GetType());
        }

        [Test]
        public void As_returns_default_generic_value_when_value_is_null()
        {
            var newCell = new ExcelCell(null);
            Assert.AreEqual(0, newCell.Cast<int>());
        }

        [Test]
        public void ValueAs_returns_default_generic_value_when_value_is_DBNull()
        {
            var newCell = new ExcelCell(DBNull.Value);
            Assert.AreEqual(0, newCell.Cast<int>());
        }

        [Test]
        public void to_string_returns_value_as_string()
        {
            var newCell = new ExcelCell("hello");
            Assert.AreEqual("hello", newCell.ToString());
        }
    }
}
