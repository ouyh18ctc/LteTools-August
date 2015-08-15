using System.IO;
using System.Linq;
using System.Text;
using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.LinqToCsv.Mapper;
using Lte.Domain.LinqToCsv.StreamDef;
using Lte.Domain.Test.LinqToCsv.Product;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.TestRead
{
    [TestFixture]
    public class CsvContextReadTest
    {
        private CsvFileDescription _description;
        private string _input;
        private FileDataAccess _dataAccess;
        private StreamReader _sr;
        private CsvStream _cs;
        private IDataRow _row;
        private FieldMapperReading<Person> _fm;
        private AggregatedException _ae;
        private RowReader<Person> _reader;

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
1,John,Doe,15,Washington";

            byte[] stringAsByteArray = Encoding.UTF8.GetBytes(_input);
            Stream stream = new MemoryStream(stringAsByteArray);
            _sr = new StreamReader(stream, Encoding.UTF8);
            _dataAccess = new FileDataAccess(_sr, _description);
            
            _cs = new CsvStream(_sr, null, _description.SeparatorChar,
                _description.IgnoreTrailingSeparatorChar);
            _row = new DataRow();
            _fm = new FieldMapperReading<Person>(_description, null, false);
            _ae = new AggregatedException(typeof(Person).ToString(), null, _description.MaximumNbrExceptions);
            _reader = new RowReader<Person>(_description, _ae);
        }

        [Test]
        public void TestCsvContext_FileDataAccess()
        {
            Assert.AreEqual(_fm.NameToInfo.Count, 3);
            Assert.IsNotNull(_dataAccess);
            Assert.AreEqual(_fm.NameToInfo.ElementAt(0).Key, "Name");
            Assert.AreEqual(_fm.NameToInfo["Name"].Name, "Name");
            Assert.AreEqual(_fm.NameToInfo["Last Name"].Name, "Last Name");
            Assert.AreEqual(_fm.NameToInfo["Age"].Name, "Age");
        }

        [Test]
        public void TestCsvContext_CsvStream()
        {   
            bool result = _cs.ReadRow(_row);
            Assert.IsTrue(result);
            Assert.IsNotNull(_row);
            Assert.AreEqual(_row.Count, 5);
            Assert.AreEqual(_row[0].LineNbr, 1);
            Assert.AreEqual(_row[0].Value, "Id");
            Assert.AreEqual(_row[1].Value, "Name");
            Assert.AreEqual(_row[2].Value, "Last Name");
            Assert.AreEqual(_row[3].Value, "Age");
            Assert.AreEqual(_row[4].Value, "City");
        }

        [Test]
        public void TestCsvContext_ReadingOneRow()
        {
            _cs.ReadRow(_row);
            bool readingResult = _reader.ReadingOneFieldRow(_fm, _row, true);
            Assert.IsFalse(readingResult);
            Assert.AreEqual(_fm.FieldIndexInfo.IndexToInfo.Length, 3);
            Assert.AreEqual(_fm.FieldIndexInfo.IndexToInfo[0].Name, "Name");
            Assert.AreEqual(_fm.FieldIndexInfo.IndexToInfo[1].Name, "Last Name");
            Assert.AreEqual(_fm.FieldIndexInfo.IndexToInfo[2].Name, "Age");
        }

        [Test]
        public void TestCsvContext_ReadingSecondRow()
        {
            _cs.ReadRow(_row);
            _reader.ReadingOneFieldRow(_fm, _row, true);
            _cs.ReadRow(_row);
            Person person = _fm.ReadObject(_row, _ae);
            Assert.IsNotNull(person);
            Assert.AreEqual(person.Age, 15);
        }
    }
}
