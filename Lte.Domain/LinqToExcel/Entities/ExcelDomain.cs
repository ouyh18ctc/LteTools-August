using System;
using System.Collections.Generic;
using System.Linq;

namespace Lte.Domain.LinqToExcel.Entities
{
    public class ExcelCell
    {
        public object Value { get; private set; }

        public ExcelCell(object value)
        {
            Value = value;
        }

        public T Cast<T>()
        {
            return (Value == null || Value is DBNull) ?
                default(T) :
                (T)Convert.ChangeType(Value, typeof(T));
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator string(ExcelCell cell)
        {
            return cell.ToString();
        }
    }

    public class ExcelRow : List<ExcelCell>
    {
        IDictionary<string, int> _columnIndexMapping;

        public ExcelRow() :
            this(new List<ExcelCell>(), new Dictionary<string, int>())
        { }

        public ExcelRow(IList<ExcelCell> cells, IDictionary<string, int> columnIndexMapping)
        {
            for (int i = 0; i < cells.Count; i++) Insert(i, cells[i]);
            _columnIndexMapping = columnIndexMapping;
        }

        public ExcelCell this[string columnName]
        {
            get
            {
                if (!_columnIndexMapping.ContainsKey(columnName))
                    throw new ArgumentException(string.Format(
                        "'{0}' column name does not exist. Valid column names are '{1}'",
                        columnName, string.Join("', '", _columnIndexMapping.Keys.ToArray())));
                return base[_columnIndexMapping[columnName]];
            }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return _columnIndexMapping.Keys; }
        }
    }

    public class ExcelRowNoHeader : List<ExcelCell>
    {
        public ExcelRowNoHeader(IEnumerable<ExcelCell> cells)
        {
            AddRange(cells);
        }
    }

    public class StrictMappingException : Exception
    {
        public StrictMappingException(string message)
            : base(message)
        { }

        public StrictMappingException(string formatMessage, params object[] args)
            : base(string.Format(formatMessage, args))
        { }
    }

    public enum ExcelDatabaseEngine
    {
        Jet,
        Ace
    }

    /// <summary>
    /// Class property and worksheet mapping enforcemment type.
    /// </summary>
    public enum StrictMappingType
    {
        /// <summary>
        /// All worksheet columns must map to a class property; all class properties must map to a worksheet columm.
        /// </summary>
        Both,

        /// <summary>
        /// All class properties must map to a worksheet column; other worksheet columns are ignored.
        /// </summary>
        ClassStrict,

        /// <summary>
        /// No checks are made to enforce worksheet column or class property mappings.
        /// </summary>
        None,

        /// <summary>
        /// All worksheet columns must map to a class property; other class properties are ignored.
        /// </summary>
        WorksheetStrict
    }

    /// <summary>
    /// Indicates how to treat leading and trailing spaces in string values.
    /// </summary>
    public enum TrimSpacesType
    {
        /// <summary>
        /// Do not perform any trimming.
        /// </summary>
        None,

        /// <summary>
        /// Trim leading spaces from strings.
        /// </summary>
        Start,

        /// <summary>
        /// Trim trailing spaces from strings.
        /// </summary>
        End,

        /// <summary>
        /// Trim leading and trailing spaces from strings. 
        /// </summary>
        Both
    }
}
