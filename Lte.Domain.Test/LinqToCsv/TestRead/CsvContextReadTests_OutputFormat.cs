using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.LinqToCsv.Mapper;
using Lte.Domain.Regular;
using Lte.Domain.Test.LinqToCsv.Product;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.TestRead
{
    [TestFixture]
    public class CsvContextReadTests_OutputFormat : Test
    {
        [Test]
        public void GoodFileUsingOutputFormatForParsingDatesCharUSEnglish()
        {
            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = ';',
                FirstLineHasColumnNames = false,
                UseOutputFormatForParsingCsvValue = true,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            string testInput =
                "AAAAAAAA;052308" + Environment.NewLine +
                "BBBBBBBB;051212" + Environment.NewLine +
                "CCCCCCCC;122308";

            var expected = new[] {
                new ProductDataParsingOutputFormat
                {
                    Name = "AAAAAAAA", StartDate = new DateTime(2008, 5, 23),
                },
                new ProductDataParsingOutputFormat {
                    Name = "BBBBBBBB", StartDate = new DateTime(2012, 5, 12), 
                },
                new ProductDataParsingOutputFormat {
                    Name = "CCCCCCCC",  StartDate = new DateTime(2008, 12, 23),
                }
            };

            AssertRead(testInput, fileDescription_namesUs, expected);
        }

        [Test]
        public void GoodFileNoSeparatorCharUseOutputFormatForParsingUSEnglish()
        {
            // Arrange

            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                NoSeparatorChar = true,
                UseOutputFormatForParsingCsvValue = true,
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            const string testInput = @"AAAAAAAA34.18405/23/08
BBBBBBBB10.31105/12/12
CCCCCCCC12.00012/23/08";

            var expected = new[] {
                new ProductDataCharLength
                {
                    Name = "AAAAAAAA", Weight = 34.184, StartDate = new DateTime(2008, 5, 23),
                },
                new ProductDataCharLength {
                    Name = "BBBBBBBB", Weight = 10.311, StartDate = new DateTime(2012, 5, 12), 
                },
                new ProductDataCharLength {
                    Name = "CCCCCCCC", Weight = 12.000, StartDate = new DateTime(2008, 12, 23),
                }
            };

            // Act and Assert

            AssertRead(testInput, fileDescription_namesUs, expected);
        }

        [Test]
        public void GoodFileNoSeparatorCharUSEnglish()
        {
            // Arrange

            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                NoSeparatorChar = true,
                UseOutputFormatForParsingCsvValue = false,
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            const string testInput = @"AAAAAAAA34.18405/23/08
BBBBBBBB10.31105/12/12
CCCCCCCC12.00012/23/08";

            var expected = new[] {
                new ProductDataCharLength
                {
                    Name = "AAAAAAAA", Weight = 34.184, StartDate = new DateTime(2008, 5, 23),
                },
                new ProductDataCharLength {
                    Name = "BBBBBBBB", Weight = 10.311, StartDate = new DateTime(2012, 5, 12), 
                },
                new ProductDataCharLength {
                    Name = "CCCCCCCC", Weight = 12.000, StartDate = new DateTime(2008, 12, 23),
                }
            };

            // Act and Assert
            FileDataAccess dataAccess = new FileDataAccess(testInput.GetStreamReader(), fileDescription_namesUs);
            RowReader<ProductDataCharLength> reader = dataAccess.ReadDataPreparation<ProductDataCharLength>(null);
            dataAccess.Row = new DataRow();
            FieldMapperReading<ProductDataCharLength> fm = new FieldMapperReading<ProductDataCharLength>(
                fileDescription_namesUs, null, false);
            List<int> charLengths = fm.GetCharLengths();
            Assert.AreEqual(charLengths.Count, 3);
            bool firstRow = true;
            List<ProductDataCharLength> actual=new List<ProductDataCharLength>();
            while (dataAccess.Cs.ReadRow(dataAccess.Row, charLengths))
            {
                if ((dataAccess.Row.Count == 1) && ((dataAccess.Row[0].Value == null) 
                    || (string.IsNullOrEmpty(dataAccess.Row[0].Value.Trim()))))
                {
                    continue;
                }

                bool readingResult = reader.ReadingOneFieldRow(fm, dataAccess.Row, firstRow);

                if (readingResult) { actual.Add(reader.Obj); }
                firstRow = false;
            }
            AssertCollectionsEqual(actual, expected);
        }
    }
}
