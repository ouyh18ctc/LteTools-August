using System;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Test.LinqToCsv.Product;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.TestRead
{
    [TestFixture]
    public class CsvContextReadTests_UseFieldIndex : Test
    {
        [Test]
        public void GoodFileCommaDelimitedUseFieldIndexForReadingDataCharUSEnglish()
        {
            // Arrange

            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = ',',
                IgnoreUnknownColumns = true,
                UseFieldIndexForReadingData = true,
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            string testInput =
    "AAAAAAAA,__,34.184,05/23/08" + Environment.NewLine +
    "BBBBBBBB,__,10.311,05/12/12" + Environment.NewLine +
    "CCCCCCCC,__,12.000,12/23/08";

            var expected = new[] {
                new ProductDataSpecificFieldIndex
                {
                    Name = "AAAAAAAA", Weight = 34.184, StartDate = new DateTime(2008, 5, 23),
                },
                new ProductDataSpecificFieldIndex {
                    Name = "BBBBBBBB", Weight = 10.311, StartDate = new DateTime(2012, 5, 12), 
                },
                new ProductDataSpecificFieldIndex {
                    Name = "CCCCCCCC", Weight = 12.000, StartDate = new DateTime(2008, 12, 23),
                }
            };
            AssertRead(testInput, fileDescription_namesUs, expected);
        }

        [Test]
        public void GoodFileCommaDelimitedUseFieldIndexForReadingDataCharUseOutputFormatForParsingUSEnglish()
        {
            // Arrange

            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = ',',
                IgnoreUnknownColumns = true,
                UseOutputFormatForParsingCsvValue = true,

                UseFieldIndexForReadingData = true,
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            string testInput =
    "AAAAAAAA,__,34.184,05/23/08" + Environment.NewLine +
    "BBBBBBBB,__,10.311,05/12/12" + Environment.NewLine +
    "CCCCCCCC,__,12.000,12/23/08";

            var expected = new[] {
                new ProductDataSpecificFieldIndex
                {
                    Name = "AAAAAAAA", Weight = 34.184, StartDate = new DateTime(2008, 5, 23),
                },
                new ProductDataSpecificFieldIndex {
                    Name = "BBBBBBBB", Weight = 10.311, StartDate = new DateTime(2012, 5, 12), 
                },
                new ProductDataSpecificFieldIndex {
                    Name = "CCCCCCCC", Weight = 12.000, StartDate = new DateTime(2008, 12, 23),
                }
            };

            // Act and Assert

            AssertRead(testInput, fileDescription_namesUs, expected);
        }
    }
}
