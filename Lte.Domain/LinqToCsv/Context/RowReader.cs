using Lte.Domain.LinqToCsv.Mapper;
using Lte.Domain.LinqToCsv.Description;

namespace Lte.Domain.LinqToCsv.Context
{
    public class RowReader<T>
        where T : class, new()
    {
        public T Obj { get; private set; }

        private readonly CsvFileDescription _description;
        private readonly AggregatedException _ae;

        public RowReader(CsvFileDescription description, AggregatedException ae)
        { 
            Obj = default(T);
            _description = description;
            _ae = ae;
        }

        public bool ReadingOneRawRow(IDataRow row, bool firstRow)
        {
            if (firstRow && _description.FirstLineHasColumnNames)
            {
                return false;
            }
            try
            {
                Obj = row as T;
            }
            catch (AggregatedException)
            {
                throw;
            }
            catch (System.Exception e)
            {
                _ae.AddException(e);
            }
            return true;
        }

        public bool ReadingOneFieldRow(FieldMapperReading<T> fm, IDataRow row, bool firstRow)
        {
            if (firstRow && _description.FirstLineHasColumnNames)
            {
                fm.ReadNames(row); 
                return false;
            }
            Obj = fm.ReadObject(row, _ae);
            return true;
        }

    }
}
