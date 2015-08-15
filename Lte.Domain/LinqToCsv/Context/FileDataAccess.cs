using System.Collections.Generic;
using System.IO;
using Lte.Domain.LinqToCsv.Mapper;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.LinqToCsv.StreamDef;

namespace Lte.Domain.LinqToCsv.Context
{
    public class FileDataAccess
    {
        private StreamReader _stream;

        private CsvFileDescription fileDescription;

        public CsvFileDescription FileDescription
        {
            get { return fileDescription; }
        }

        public IDataRow Row { get; set; }

        public CsvStream Cs { get; private set; }

        public AggregatedException Ae { get; private set; }

        public FileDataAccess(StreamReader stream, CsvFileDescription fileDescription)
        {
            _stream = stream;
            this.fileDescription = fileDescription;
            Cs = new CsvStream(stream, null, fileDescription.SeparatorChar,
                fileDescription.IgnoreTrailingSeparatorChar);
        }

        public IEnumerable<T> ReadData<T>(string fileName) where T : class, new()
        {
            RowReader<T> reader = ReadDataPreparation<T>(fileName);
            if (typeof(IDataRow).IsAssignableFrom(typeof(T)))
            {                
                Row = new T() as IDataRow;
                return ReadRawData(reader, fileName);
            }
            Row = new DataRow();
            return ReadFieldData(reader, fileName);
        }

        public RowReader<T> ReadDataPreparation<T>(string fileName) where T : class, new()
        {
            RewindStream(fileName);
            Ae =
                new AggregatedException(typeof(T).ToString(), fileName, fileDescription.MaximumNbrExceptions);
            return new RowReader<T>(fileDescription, Ae);
        }

        public IEnumerable<T> ReadRawData<T>(RowReader<T> reader, string fileName) where T : class, new()
        {
            bool firstRow = true;
            try
            {
                while (Cs.ReadRow(Row))
                {
                    if ((Row.Count == 1) && ((Row[0].Value == null) || (string.IsNullOrEmpty(Row[0].Value.Trim()))))
                    {
                        continue;
                    }

                    bool readingResult = reader.ReadingOneRawRow(Row, firstRow);

                    if (readingResult) { yield return reader.Obj; }
                    firstRow = false;
                }
            }
            finally
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    _stream.Close();
                }

                Ae.ThrowIfExceptionsStored();
            }

        }

        public IEnumerable<T> ReadFieldData<T>(RowReader<T> reader, string fileName) where T : class, new()
        {
            FieldMapperReading<T> fm = new FieldMapperReading<T>(fileDescription, fileName, false);
            List<int> charLengths = fm.GetCharLengths();
            return ReadFieldDataRows(reader, fileName, fm, charLengths);
        }

        public IEnumerable<T> ReadFieldDataRows<T>(RowReader<T> reader, string fileName,
            FieldMapperReading<T> fm, List<int> charLengths) where T : class, new()
        {
            bool firstRow = true;

            try
            {
                while (Cs.ReadRow(Row, charLengths))
                {
                    if ((Row.Count == 1) && ((Row[0].Value == null) || (string.IsNullOrEmpty(Row[0].Value.Trim()))))
                    {
                        continue;
                    }

                    bool readingResult = reader.ReadingOneFieldRow(fm, Row, firstRow);

                    if (readingResult) { yield return reader.Obj; }
                    firstRow = false;
                }
            }
            finally
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    _stream.Close();
                }

                Ae.ThrowIfExceptionsStored();
            }
        }

        public void RewindStream(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                _stream = new StreamReader(fileName, fileDescription.TextEncoding,
                    fileDescription.DetectEncodingFromByteOrderMarks);
            }
            else
            {
                if ((_stream == null) || (!_stream.BaseStream.CanSeek))
                {
                    throw new BadStreamException();
                }

                _stream.BaseStream.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
