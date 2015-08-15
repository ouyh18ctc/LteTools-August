using System;
using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Test;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.Product
{
    public class ProductDataCharLength : IAssertable<ProductDataCharLength>
    {
        
        private string name;

        [CsvColumn(FieldIndex = 1, CharLength = 8)]
        public string Name
        {
            private get { return name; }
            set { name = value; }
        }

        // OutputFormat uses the same codes as the standard ToString method (search MSDN).
        
        private DateTime startDate;

        [CsvColumn(FieldIndex = 3, OutputFormat = "MM/dd/yy", CharLength = 8)]
        public DateTime StartDate
        {
            private get { return startDate; }
            set { startDate = value; }
        }

        // Can use both fields and properties
        [CsvColumn(FieldIndex = 2, CanBeNull = false, CharLength = 6)]
        public double Weight { private get; set; }

        public void AssertEqual(ProductDataCharLength other)
        {
            Assert.AreNotEqual(other, null);

            Assert.AreEqual(other.Name, name, "name");
            Assert.AreEqual(other.StartDate, startDate, "startDate");
            Assert.AreEqual(other.Weight, Weight, "weight");
        }
    }
}
