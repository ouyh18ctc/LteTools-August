using System.Collections.Generic;
using System.IO;
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
    public class CsvContextReadTestsUnknownColumns : Test
    {
        private CsvFileDescription _description;
        private string _input;
        private StreamReader _stream;
        private FileDataAccess _dataAccess;
        private RowReader<Person> _reader;

        public Person[] Expected { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            _description = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                IgnoreUnknownColumns = true,
            };
            _input =
@"Id,Name,Last Name,Age,City
1,John,Doe,15,Washington
2,Jane,Doe,20,New York
";
            Expected = new[]
                {
                    new Person
                        {
                            Name = "John",
                            LastName = "Doe",
                            Age = 15
                        },
                    new Person
                        {
                            Name = "Jane",
                            LastName = "Doe",
                            Age = 20
                        }
                };
            _stream = _input.GetStreamReader();
            _dataAccess = new FileDataAccess(_stream, _description);
            _reader = _dataAccess.ReadDataPreparation<Person>(null);
            _dataAccess.Row = new DataRow();
        }

        [Test]
        public void FileWithUnknownColumns_ShouldDiscardColumns_BasicParameters()
        {
            Assert.IsNotNull(_dataAccess);
            Assert.IsNotNull(_stream);
            Assert.AreEqual(_dataAccess.FileDescription, _description);
        }

        [Test]
        public void FileWithUnknownColumns_ShouldDiscardColumns_Concise()
        {
            List<Person> actual = _dataAccess.ReadFieldData(_reader, null).ToList();
            Assert.AreEqual(actual.Count, 2);
            Assert.AreEqual(actual[0].Age, 15);
            Assert.AreEqual(actual[1].Name, "Jane");
        }

        [Test]
        public void FileWithUnknownColumns_ShouldDiscardColumns_BasicParameters2()
        {
            FieldMapperReading<Person> fm = new FieldMapperReading<Person>(_description, null, false);
            List<int> charLengths = fm.GetCharLengths();
            Assert.IsNull(charLengths);

            Assert.IsNotNull(_dataAccess.Row);
            Assert.AreEqual(_dataAccess.Row.Count, 0);
          
        }
            
        [Test]
        public void FileWithUnknownColumns_ShouldDiscardColumns_ReadRow()
        {
            bool result = _dataAccess.Cs.ReadRow(_dataAccess.Row);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void FileWithUnknownColumns_ShouldDiscardColumns_ReadFieldData()
        {
            FieldMapperReading<Person> fm = new FieldMapperReading<Person>(_description, null, false);
            List<int> charLengths = fm.GetCharLengths();

            List<Person> actual 
                = _dataAccess.ReadFieldDataRows(_reader, null, fm, charLengths).ToList();

            Assert.AreEqual(actual.Count, 2);
            Assert.AreEqual(actual[0].Age, 15);
            Assert.AreEqual(actual[1].Name, "Jane");
        }
    }
}
