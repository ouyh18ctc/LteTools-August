using System;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Test.LinqToCsv.Product;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.TestRead
{
    [TestFixture]
    public class CsvContextReadTests_Delimited : Test
    {
        [Test]
        public void GoodFileCommaDelimitedNamesInFirstLineUSEnglish()
        {
            // Arrange

            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = ',', // default is ','
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            const string testInput = @"Name,        Weight,       StartDate, LaunchTime,               NbrAvailable,Onsale,ShopsAvailable,    Code,  Price,    Description
moonbuggy,   34.184,       5/23/08,   5-May-2009 4:11 pm,       1205,        true,  ""Paris, New York"", 1F,    $540.12,  newly launched product
""mouse trap"",45E-5,        1/2/1985,  ""7 August 1988, 0:00 am"", ""4,030"",     FALSE, ""This field has
a newline"", 100, ""$78,300"", ""This field has quotes(""""), and
two newlines
and a quoted """"string""""""
dog house,    ""45,230,990"",29 Feb 2004, ,                  -56,        True,"""",                  FF10, ""12,008""";

            var expected = new[] {
                new ProductData {
                    Name = "moonbuggy", Weight = 34.184, StartDate = new DateTime(2008, 5, 23), 
                    LaunchTime = new DateTime(2009, 5, 5, 16, 11, 0),
                    NbrAvailable = 1205, Onsale = true, ShopsAvailable = "Paris, New York", 
                    HexProductCode = 31, RetailPrice = 540.12M,
                    Description = "newly launched product"
                },
                new ProductData {
                    Name = "mouse trap", Weight = 45E-5, StartDate = new DateTime(1985, 1, 2), 
                    LaunchTime = new DateTime(1988, 8, 7, 0, 0, 0),
                    NbrAvailable = 4030, Onsale = false, ShopsAvailable = @"This field has
a newline", HexProductCode = 256, RetailPrice = 78300M,
                    Description = @"This field has quotes(""), and
two newlines
and a quoted ""string"""
                },
                new ProductData {
                    Name = "dog house", Weight = 45230990, StartDate = new DateTime(2004, 2, 29), 
                    LaunchTime = default(DateTime),
                    NbrAvailable = -56, Onsale = true, ShopsAvailable = "", 
                    HexProductCode = 65296, RetailPrice = 12008M,
                    Description = null
                }
            };

            // Act and Assert

            AssertRead(testInput, fileDescription_namesUs, expected);
        }

        [Test]
        public void GoodFileTabDelimitedNoNamesInFirstLineNLnl()
        {
            // Arrange

            CsvFileDescription fileDescription_nonamesNl = new CsvFileDescription
            {
                SeparatorChar = '\t', // tab character
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,
                FileCultureName = "nl-NL" // default is the current culture
            };

            const string testInput = "moonbuggy\t       23/5/08\t   5-Mei-2009 16:11 pm\t   34.184\t  \"Paris, New York\"\t 1F\t    €540,12\t        true\t  newly launched product\r\n\"mouse trap\"\t        2/1/1985\t  \"7 Augustus 1988\t 0:00\"\t45E-5\t \"This field has\r\na newline\"\t 100\t \"€78.300\"\t     FALSE\t \"This field has quotes(\"\"), and\r\ntwo newlines\r\nand a quoted \"\"string\"\"\"\r\ndog house\t29 Feb 2004\t \t    \"45.230.990\"\t\"\"\t                  FF10\t \"12.008\"\t        True";
            var expected = new[] {
                new ProductData {
                    Name = "moonbuggy", Weight = 34184, StartDate = new DateTime(2008, 5, 23), 
                    LaunchTime = new DateTime(2009, 5, 5, 16, 11, 0),
                    NbrAvailable = 0, Onsale = true, ShopsAvailable = "Paris, New York", 
                    HexProductCode = 31, RetailPrice = 540.12M,
                    Description = "newly launched product"
                },
                new ProductData {
                    Name = "mouse trap", Weight = 45E-5, StartDate = new DateTime(1985, 1, 2), 
                    LaunchTime = new DateTime(1988, 8, 7, 0, 0, 0),
                    NbrAvailable = 0, Onsale = false, ShopsAvailable = @"This field has
a newline", HexProductCode = 256, RetailPrice = 78300M,
                    Description = @"This field has quotes(""), and
two newlines
and a quoted ""string"""
                },
                new ProductData {
                    Name = "dog house", Weight = 45230990, StartDate = new DateTime(2004, 2, 29), 
                    LaunchTime = default(DateTime),
                    NbrAvailable = 0, Onsale = true, ShopsAvailable = "", 
                    HexProductCode = 65296, RetailPrice = 12008M,
                    Description = null
                }
            };

            // Act and Assert
            AssertRead(Utils.NormalizeString(testInput), fileDescription_nonamesNl, expected);
        }

        [Test]
        public void GoodFileCommaDelimitedWithTrailingSeparatorChars()
        {
            // Arrange

            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = ',', // default is ','
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false, // default is false
                FileCultureName = "en-US", // default is the current culture
                IgnoreTrailingSeparatorChar = true
            };

            const string testInput = @"Name,        Weight,       StartDate, LaunchTime,               NbrAvailable,Onsale,ShopsAvailable,    Code,  Price,    Description,
moonbuggy,   34.184,       5/23/08,   5-May-2009 4:11 pm,       1205,        true,  ""Paris, New York"", 1F,    $540.12,  newly launched product,
""mouse trap"",45E-5,        1/2/1985,  ""7 August 1988, 0:00 am"", ""4,030"",     FALSE, ""This field has
a newline"", 100, ""$78,300"", ""This field has quotes(""""), and
two newlines
and a quoted """"string""""""
dog house,    ""45,230,990"",29 Feb 2004, ,                  -56,        True,"""",                  FF10, ""12,008"",";

            var expected = new[] {
                new ProductData {
                    Name = "moonbuggy", Weight = 34.184, StartDate = new DateTime(2008, 5, 23), 
                    LaunchTime = new DateTime(2009, 5, 5, 16, 11, 0),
                    NbrAvailable = 1205, Onsale = true, ShopsAvailable = "Paris, New York", 
                    HexProductCode = 31, RetailPrice = 540.12M,
                    Description = "newly launched product"
                },
                new ProductData {
                    Name = "mouse trap", Weight = 45E-5, StartDate = new DateTime(1985, 1, 2), 
                    LaunchTime = new DateTime(1988, 8, 7, 0, 0, 0),
                    NbrAvailable = 4030, Onsale = false, ShopsAvailable = @"This field has
a newline", HexProductCode = 256, RetailPrice = 78300M,
                    Description = @"This field has quotes(""), and
two newlines
and a quoted ""string"""
                },
                new ProductData {
                    Name = "dog house", Weight = 45230990, StartDate = new DateTime(2004, 2, 29), 
                    LaunchTime = default(DateTime),
                    NbrAvailable = -56, Onsale = true, ShopsAvailable = "", 
                    HexProductCode = 65296, RetailPrice = 12008M,
                    Description = null
                }
            };

            // Act and Assert

            AssertRead(testInput, fileDescription_namesUs, expected);
        }
    }
}
