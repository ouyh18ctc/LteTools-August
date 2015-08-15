using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Lte.Domain.LinqToExcel.Service;
using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses;

namespace Lte.Domain.LinqToExcel.Entities
{
    public class ExcelQueryArgs
    {
        public string FileName { get; set; }
        internal ExcelDatabaseEngine DatabaseEngine { get; set; }
        public string WorksheetName { get; set; }
        internal int? WorksheetIndex { get; set; }
        public Dictionary<string, string> ColumnMappings { get; set; }
        internal Dictionary<string, Func<string, object>> Transformations { get; private set; }
        public string NamedRangeName { get; set; }
        internal string StartRange { get; set; }
        internal string EndRange { get; set; }
        public bool NoHeader { get; set; }
        internal StrictMappingType? StrictMapping { get; set; }
        public bool ReadOnly { get; set; }
        internal bool UsePersistentConnection { get; set; }
        internal OleDbConnection PersistentConnection { get; set; }
        internal TrimSpacesType TrimSpaces { get; set; }

        public ExcelQueryArgs()
            : this(new ExcelQueryConstructorArgs { DatabaseEngine = ExcelUtilities.DefaultDatabaseEngine() })
        { }

        public ExcelQueryArgs(ExcelQueryConstructorArgs args)
        {
            FileName = args.FileName;
            DatabaseEngine = args.DatabaseEngine;
            ColumnMappings = args.ColumnMappings ?? new Dictionary<string, string>();
            Transformations = args.Transformations ?? new Dictionary<string, Func<string, object>>();
            StrictMapping = args.StrictMapping ?? StrictMappingType.None;
            UsePersistentConnection = args.UsePersistentConnection;
            TrimSpaces = args.TrimSpaces;
            ReadOnly = args.ReadOnly;
        }

        public override string ToString()
        {
            var columnMappingsString = new StringBuilder();
            foreach (var kvp in ColumnMappings)
                columnMappingsString.AppendFormat("[{0} = '{1}'] ", kvp.Key, kvp.Value);
            var transformationsString = string.Join(", ", Transformations.Keys.ToArray());

            return string.Format("FileName: '{0}'; WorksheetName: '{1}'; WorksheetIndex: {2}; StartRange: {3}; EndRange: {4}; Named Range: {11}; NoHeader: {5}; ColumnMappings: {6}; Transformations: {7}, StrictMapping: {8}, UsePersistentConnection: {9}, TrimSpaces: {10}",
                FileName, WorksheetName, WorksheetIndex, StartRange, EndRange, NoHeader, columnMappingsString, transformationsString, StrictMapping, UsePersistentConnection, TrimSpaces, NamedRangeName);
        }
    }

    public class ExcelQueryConstructorArgs
    {
        internal string FileName { get; set; }
        internal ExcelDatabaseEngine DatabaseEngine { get; set; }
        internal Dictionary<string, string> ColumnMappings { get; set; }
        internal Dictionary<string, Func<string, object>> Transformations { get; set; }
        internal StrictMappingType? StrictMapping { get; set; }
        internal bool UsePersistentConnection { get; set; }
        internal TrimSpacesType TrimSpaces { get; set; }
        internal bool ReadOnly { get; set; }
    }

    public class ExcelQueryable<T> : QueryableBase<T>
    {
        private static IQueryExecutor CreateExecutor(ExcelQueryArgs args)
        {
            return new ExcelQueryExecutor(args);
        }

        public ExcelQueryable(ExcelQueryArgs args)
            : base(CreateExecutor(args))
        {
            foreach (var property in typeof(T).GetProperties())
            {
                ExcelColumnAttribute att = (ExcelColumnAttribute)Attribute.GetCustomAttribute(property, typeof(ExcelColumnAttribute));
                if (att != null && !args.ColumnMappings.ContainsKey(property.Name))
                {
                    args.ColumnMappings.Add(property.Name, att.ColumnName);
                }
            }
        }

        public ExcelQueryable(IQueryProvider provider, Expression expression)
            : base(provider, expression)
        { }
    }

    public class SqlParts
    {
        public string Aggregate { get; set; }
        public string Table { get; set; }
        public string Where { get; set; }
        public IEnumerable<OleDbParameter> Parameters { get; set; }
        public string OrderBy { get; set; }
        public bool OrderByAsc { get; set; }
        public List<string> ColumnNamesUsed { get; set; }

        public SqlParts()
        {
            Aggregate = "*";
            Parameters = new List<OleDbParameter>();
            OrderByAsc = true;
            ColumnNamesUsed = new List<string>();
        }

        public static implicit operator string(SqlParts sql)
        {
            return sql.ToString();
        }

        public override string ToString()
        {
            var sql = new StringBuilder();
            sql.AppendFormat("SELECT {0} FROM {1}",
                Aggregate,
                Table);
            if (!String.IsNullOrEmpty(Where))
                sql.AppendFormat(" WHERE {0}", Where);
            if (!String.IsNullOrEmpty(OrderBy))
            {
                var asc = (OrderByAsc) ? "ASC" : "DESC";
                sql.AppendFormat(" ORDER BY [{0}] {1}",
                    OrderBy,
                    asc);
            }
            return sql.ToString();
        }
    }

    public class ResultObjectMapping
    {
        private readonly Dictionary<IQuerySource, object> _resultObjectsBySource = new Dictionary<IQuerySource, object>();

        public ResultObjectMapping(IQuerySource querySource, object resultObject)
        {
            Add(querySource, resultObject);
        }

        public void Add(IQuerySource querySource, object resultObject)
        {
            _resultObjectsBySource.Add(querySource, resultObject);
        }

        public T GetObject<T>(IQuerySource source)
        {
            return (T)_resultObjectsBySource[source];
        }

        public IEnumerator<KeyValuePair<IQuerySource, object>> GetEnumerator()
        {
            return _resultObjectsBySource.GetEnumerator();
        }
    }
}
