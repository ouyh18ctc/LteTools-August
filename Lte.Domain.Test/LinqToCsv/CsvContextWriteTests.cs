using System;
using System.Collections.Generic;
using System.Text;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Test.LinqToCsv.Product;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv
{
    [TestFixture]
    public class CsvContextWriteTests : Test
    {
        [Test]
        public void GoodFileCommaDelimitedNamesInFirstLineNLnl()
        {
            // Arrange

            List<ProductData> dataRows_Test = new List<ProductData>
            {
                new ProductData
                {
                    RetailPrice = 4.59M,
                    Name = "Wooden toy",
                    StartDate = new DateTime(2008, 2, 1),
                    NbrAvailable = 67
                },
                new ProductData
                {
                    Onsale = true,
                    Weight = 4.03,
                    ShopsAvailable = "Ashfield",
                    Description = ""
                },
                new ProductData
                {
                    Name = "Metal box",
                    LaunchTime = new DateTime(2009, 11, 5, 4, 50, 0),
                    Description = "Great\nproduct"
                }
            };

            CsvFileDescription fileDescription_namesNl2 = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false,
                TextEncoding = Encoding.Unicode,
                FileCultureName = "nl-Nl" // default is the current culture
            };

            string expected =
@"Name,StartDate,LaunchTime,Weight,ShopsAvailable,Code,Price,Onsale,Description,NbrAvailable,UnusedField
Wooden toy,1-2-2008,01 jan 00:00:00,""000,000"",,0,""€ 4,59"",False,,67,
,1-1-0001,01 jan 00:00:00,""004,030"",Ashfield,0,""€ 0,00"",True,"""",0,
Metal box,1-1-0001,05 nov 04:50:00,""000,000"",,0,""€ 0,00"",False,""Great
product"",0,
";

            // Act and Assert

            AssertWrite(dataRows_Test, fileDescription_namesNl2, expected);
        }
    }
}
