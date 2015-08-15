using System;
using Lte.Domain.LinqToCsv;

namespace Lte.Domain.Test.LinqToCsv.Product
{
    public class ProductData_MissingFieldIndex
    {
        [CsvColumn(FieldIndex = 1)]
        public string name;

        // CsvOutputFormat uses the same codes as the standard ToString method (search MSDN).
        [CsvColumn(FieldIndex = 2, OutputFormat = "d")]
        public DateTime startDate;

        // ########## missing index below

        [CsvColumn(CanBeNull = false, OutputFormat = "dd MMM HH:mm:ss")]
        public DateTime launchTime;

        // Can use both fields and properties
        [CsvColumn(FieldIndex = 4, OutputFormat = "#,000.000")]
        public double weight { get; set; }

        // Following field has no CsvColumn attribute.
        // So will be ignored when library is told to only use data with CsvColumn attribute.
        public int nbrAvailable;

        // Ok to have gaps in FieldIndex order
        [CsvColumn(FieldIndex = 10)]
        public string shopsAvailable;
    }
}
