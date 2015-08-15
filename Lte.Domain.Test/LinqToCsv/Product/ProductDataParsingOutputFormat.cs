using System;
using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Test;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.Product
{
    public class ProductDataParsingOutputFormat : IAssertable<ProductDataParsingOutputFormat>
    {
        
        private string name;

        [CsvColumn(FieldIndex = 1)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // OutputFormat uses the same codes as the standard ToString method (search MSDN).
        
        private DateTime startDate;

        [CsvColumn(FieldIndex = 2, OutputFormat = "MMddyy")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

#pragma warning restore 0169, 0414, 0649

        public void AssertEqual(ProductDataParsingOutputFormat other)
        {
            Assert.AreNotEqual(other, null);

            Assert.AreEqual(other.Name, name, "name");
            Assert.AreEqual(other.StartDate, startDate, "startDate");
        }
    }
}
