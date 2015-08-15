using System;
using System.Globalization;
using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Test;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.Product
{
    public class ProductData : IAssertable<ProductData>
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

        [CsvColumn(FieldIndex = 2, OutputFormat = "d")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        
        private DateTime launchTime;

        [CsvColumn(FieldIndex = 3, OutputFormat = "dd MMM HH:mm:ss")]
        public DateTime LaunchTime
        {
            get { return launchTime; }
            set { launchTime = value; }
        }

        // Can use both fields and properties
        [CsvColumn(FieldIndex = 4, CanBeNull = false, OutputFormat = "#,000.000")]
        public double Weight { get; set; }

        // Following field has no CsvColumn attribute.
        // So will be ignored when library is told to only use data with CsvColumn attribute.
        private int nbrAvailable;

        public int NbrAvailable
        {
            get { return nbrAvailable; }
            set { nbrAvailable = value; }
        }

        // Ok to have gaps in FieldIndex order
        
        private string shopsAvailable;

        [CsvColumn(FieldIndex = 10)]
        public string ShopsAvailable
        {
            get { return shopsAvailable; }
            set { shopsAvailable = value; }
        }

        // Override field name, so in data files, this field is known as "code" instead of "hexProductCode"
        // This field contains hexadecimal numbers, without leading 0x.
        // This requires setting the NumberStyle property to NumberStyles.HexNumber.
        // Don't forget to import the namespace System.Globalization
        
        private int hexProductCode;

        [CsvColumn(Name = "Code", FieldIndex = 11, NumberStyle = NumberStyles.HexNumber)]
        public int HexProductCode
        {
            get { return hexProductCode; }
            set { hexProductCode = value; }
        }

        private string unusedField;

        public string UnusedField
        {
            get { return unusedField; }
            set { unusedField = value; }
        }

        // FieldIndex order higher then that of next field.
        // So this field will come AFTER next field in the actual data file
        
        private bool onsale;

        [CsvColumn(FieldIndex = 16)]
        public bool Onsale
        {
            get { return onsale; }
            set { onsale = value; }
        }

        // Override field name, so in data files, this field is known as "price" instead of "retailPrice"
        // OutputFormat uses the same codes as the standard ToString method (search MSDN). Format "C" is for currency.
        [CsvColumn(Name = "Price", FieldIndex = 14, CanBeNull = false, OutputFormat = "C")]
        public decimal RetailPrice { get; set; }

        
        private string description;

        [CsvColumn(FieldIndex = 30)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public void AssertEqual(ProductData other)
        {
            Assert.AreNotEqual(other, null);

            Assert.AreEqual(other.Name, name, "name");
            Assert.AreEqual(other.StartDate, startDate, "startDate");
            Assert.AreEqual(other.LaunchTime, launchTime, "launchTime");
            Assert.AreEqual(other.Weight, Weight, "weight");
            Assert.AreEqual(other.NbrAvailable, nbrAvailable, "nbrAvailable");
            Assert.AreEqual(other.ShopsAvailable, shopsAvailable, "shopsAvailable");
            Assert.AreEqual(other.HexProductCode, hexProductCode, "hexProductCode");
            Assert.AreEqual(other.Onsale, onsale, "onsale");
            Assert.AreEqual(other.RetailPrice, RetailPrice, "retailPrice");
            Assert.AreEqual(Utils.NormalizeString(other.description), Utils.NormalizeString(description), "description");
        }
    }
}
