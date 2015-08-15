﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using Lte.Domain.LinqToExcel.Entities;
using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses.ResultOperators;

namespace Lte.Domain.LinqToExcel.Service
{
    public class ExcelQueryExecutor : IQueryExecutor
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExcelQueryArgs _args;

        public ExcelQueryExecutor(ExcelQueryArgs args)
        {
            ValidateArgs(args);
            _args = args;

            if (_log.IsDebugEnabled)
                _log.DebugFormat("Connection String: {0}", ExcelUtilities.GetConnection(args).ConnectionString);

            GetWorksheetName();
        }

        private void ValidateArgs(ExcelQueryArgs args)
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("ExcelQueryArgs = {0}", args);

            if (args.FileName == null)
                throw new ArgumentNullException("args", "FileName property cannot be null.");

            if (!String.IsNullOrEmpty(args.StartRange) &&
                !Regex.Match(args.StartRange, "^[a-zA-Z]{1,3}[0-9]{1,7}$").Success)
                throw new ArgumentException(string.Format(
                    "StartRange argument '{0}' is invalid format for cell name", args.StartRange));

            if (!String.IsNullOrEmpty(args.EndRange) &&
                !Regex.Match(args.EndRange, "^[a-zA-Z]{1,3}[0-9]{1,7}$").Success)
                throw new ArgumentException(string.Format(
                    "EndRange argument '{0}' is invalid format for cell name", args.EndRange));

            if (args.NoHeader &&
                !String.IsNullOrEmpty(args.StartRange) &&
                args.FileName.ToLower().Contains(".csv"))
                throw new ArgumentException("Cannot use WorksheetRangeNoHeader on csv files");
        }

        /// <summary>
        /// Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
        /// </summary>
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteSingle<T>(queryModel, false);
        }

        /// <summary>
        /// Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        /// </summary>
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            var results = ExecuteCollection<T>(queryModel);

            foreach (var resultOperator in queryModel.ResultOperators)
            {
                if (resultOperator is LastResultOperator)
                    return results.LastOrDefault();
            }

            return (returnDefaultWhenEmpty) ?
                results.FirstOrDefault() :
                results.First();
        }

        /// <summary>
        /// Executes a query with a collection result.
        /// </summary>
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var sql = GetSqlStatement(queryModel);
            LogSqlStatement(sql);

            var objectResults = GetDataResults(sql, queryModel);
            var projector = GetSelectProjector<T>(objectResults.FirstOrDefault(), queryModel);
            var returnResults = objectResults.Cast(projector);

            foreach (var resultOperator in queryModel.ResultOperators)
            {
                if (resultOperator is ReverseResultOperator)
                    returnResults = returnResults.Reverse();
                if (resultOperator is SkipResultOperator)
                    returnResults = returnResults.Skip(resultOperator.Cast<SkipResultOperator>().GetConstantCount());
            }

            return returnResults;
        }

        protected Func<object, T> GetSelectProjector<T>(object firstResult, QueryModel queryModel)
        {
            Func<object, T> projector = result => result.Cast<T>();
            if (!ShouldBuildResultObjectMapping<T>(firstResult, queryModel)) return projector;
            var proj = ProjectorBuildingExpressionTreeVisitor.BuildProjector<T>(queryModel.SelectClause.Selector);
            projector = result => proj(new ResultObjectMapping(queryModel.MainFromClause, result));
            return projector;
        }

        protected bool ShouldBuildResultObjectMapping<T>(object firstResult, QueryModel queryModel)
        {
            var ignoredResultOperators = new List<Type>
            {
                typeof (MaxResultOperator),
                typeof (CountResultOperator),
                typeof (LongCountResultOperator),
                typeof (MinResultOperator),
                typeof (SumResultOperator)
            };

            return (firstResult != null &&
                    firstResult.GetType() != typeof(T) &&
                    !queryModel.ResultOperators.Any(x => ignoredResultOperators.Contains(x.GetType())));
        }

        protected SqlParts GetSqlStatement(QueryModel queryModel)
        {
            var sqlVisitor = new SqlGeneratorQueryModelVisitor(_args);
            sqlVisitor.VisitQueryModel(queryModel);
            return sqlVisitor.SqlStatement;
        }

        private void GetWorksheetName()
        {
            if (_args.FileName.ToLower().EndsWith("csv"))
                _args.WorksheetName = Path.GetFileName(_args.FileName);
            else if (_args.WorksheetIndex.HasValue)
            {
                var worksheetNames = ExcelUtilities.GetWorksheetNames(_args);
                if (_args.WorksheetIndex.Value < worksheetNames.Count())
                    _args.WorksheetName = worksheetNames.ElementAt(_args.WorksheetIndex.Value);
                else
                    throw new DataException("Worksheet Index Out of Range");
            }
            else if (String.IsNullOrEmpty(_args.WorksheetName) && String.IsNullOrEmpty(_args.NamedRangeName))
            {
                _args.WorksheetName = "Sheet1";
            }
        }

        /// <summary>
        /// Executes the sql query and returns the data results
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryModel">Linq query model</param>
        protected IEnumerable<object> GetDataResults(SqlParts sql, QueryModel queryModel)
        {
            IEnumerable<object> results;
            OleDbDataReader data = null;

            var conn = ExcelUtilities.GetConnection(_args);
            var command = conn.CreateCommand();
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                command.CommandText = sql.ToString();
                command.Parameters.AddRange(sql.Parameters.ToArray());
                try { data = command.ExecuteReader(); }
                catch (OleDbException e)
                {
                    if (e.Message.Contains(_args.WorksheetName))
                        throw new DataException(
                            string.Format(
                                "'{0}' is not a valid worksheet name in file {3}. Valid worksheet names are: '{1}'. Error received: {2}",
                                _args.WorksheetName,
                                string.Join("', '", ExcelUtilities.GetWorksheetNames(_args.FileName).ToArray()),
                                e.Message, _args.FileName), e);
                    if (!CheckIfInvalidColumnNameUsed(sql))
                        throw;
                }

                var columns = ExcelUtilities.GetColumnNames(data);
                LogColumnMappingWarnings(columns);
                if (columns.Count() == 1 && columns.First() == "Expr1000")
                    results = GetScalarResults(data);
                else if (queryModel.MainFromClause.ItemType == typeof(ExcelRow))
                    results = GetRowResults(data, columns);
                else if (queryModel.MainFromClause.ItemType == typeof(ExcelRowNoHeader))
                    results = GetRowNoHeaderResults(data);
                else
                    results = GetTypeResults(data, columns, queryModel);
            }
            finally
            {
                command.Dispose();

                if (!_args.UsePersistentConnection)
                {
                    conn.Dispose();
                    _args.PersistentConnection = null;
                }
            }

            return results;
        }

        /// <summary>
        /// Logs a warning for any property to column mappings that do not exist in the excel worksheet
        /// </summary>
        /// <param name="columns">List of columns in the worksheet</param>
        private void LogColumnMappingWarnings(IEnumerable<string> columns)
        {
            foreach (var kvp in _args.ColumnMappings)
            {
                if (!columns.Contains(kvp.Value))
                {
                    _log.WarnFormat("'{0}' column that is mapped to the '{1}' property does not exist in the '{2}' worksheet",
                        kvp.Value, kvp.Key, _args.WorksheetName);
                }
            }
        }

        private bool CheckIfInvalidColumnNameUsed(SqlParts sql)
        {
            var usedColumns = sql.ColumnNamesUsed;
            var tableColumns = ExcelUtilities.GetColumnNames(_args.WorksheetName, _args.NamedRangeName, _args.FileName);
            foreach (var column in usedColumns)
            {
                if (!tableColumns.Contains(column))
                {
                    throw new DataException(string.Format(
                        "'{0}' is not a valid column name. " +
                        "Valid column names are: '{1}'",
                        column,
                        string.Join("', '", tableColumns.ToArray())));
                }
            }
            return false;
        }

        private IEnumerable<object> GetRowResults(IDataReader data, IEnumerable<string> columns)
        {
            var results = new List<object>();
            var columnIndexMapping = new Dictionary<string, int>();
            for (var i = 0; i < columns.Count(); i++)
                columnIndexMapping[columns.ElementAt(i)] = i;

            while (data.Read())
            {
                IList<ExcelCell> cells = new List<ExcelCell>();
                for (var i = 0; i < columns.Count(); i++)
                {
                    var value = data[i];
                    value = TrimStringValue(value);
                    cells.Add(new ExcelCell(value));
                }
                results.CallMethod("Add", new ExcelRow(cells, columnIndexMapping));
            }
            return results.AsEnumerable();
        }

        private IEnumerable<object> GetRowNoHeaderResults(OleDbDataReader data)
        {
            var results = new List<object>();
            while (data.Read())
            {
                IList<ExcelCell> cells = new List<ExcelCell>();
                for (var i = 0; i < data.FieldCount; i++)
                {
                    var value = data[i];
                    value = TrimStringValue(value);
                    cells.Add(new ExcelCell(value));
                }
                results.CallMethod("Add", new ExcelRowNoHeader(cells));
            }
            return results.AsEnumerable();
        }

        private IEnumerable<object> GetTypeResults(IDataReader data, IEnumerable<string> columns, QueryModel queryModel)
        {
            var results = new List<object>();
            var fromType = queryModel.MainFromClause.ItemType;
            var props = fromType.GetProperties();
            if (_args.StrictMapping != null && _args.StrictMapping.Value != StrictMappingType.None)
                ConfirmStrictMapping(columns, props, _args.StrictMapping.Value);

            while (data.Read())
            {
                var result = Activator.CreateInstance(fromType);
                foreach (var prop in props)
                {
                    var columnName = (_args.ColumnMappings.ContainsKey(prop.Name)) ?
                        _args.ColumnMappings[prop.Name] :
                        prop.Name;
                    if (columns.Contains(columnName))
                    {
                        var value = GetColumnValue(data, columnName, prop.Name).Cast(prop.PropertyType);
                        value = TrimStringValue(value);
                        result.SetProperty(prop.Name, value);
                    }
                }
                results.Add(result);
            }
            return results.AsEnumerable();
        }

        /// <summary>
        /// Trims leading and trailing spaces, based on the value of _args.TrimSpaces
        /// </summary>
        /// <param name="value">Input string value</param>
        /// <returns>Trimmed string value</returns>
        private object TrimStringValue(object value)
        {
            if (value == null || value.GetType() != typeof(string))
                return value;

            switch (_args.TrimSpaces)
            {
                case TrimSpacesType.Start:
                    return ((string)value).TrimStart();
                case TrimSpacesType.End:
                    return ((string)value).TrimEnd();
                case TrimSpacesType.Both:
                    return ((string)value).Trim();
                default:
                    return value;
            }
        }

        private void ConfirmStrictMapping(IEnumerable<string> columns, 
            PropertyInfo[] properties, StrictMappingType strictMappingType)
        {
            var propertyNames = properties.Select(x => x.Name);
            if (strictMappingType == StrictMappingType.ClassStrict || strictMappingType == StrictMappingType.Both)
            {
                foreach (var propertyName in propertyNames)
                {
                    if (!columns.Contains(propertyName) && PropertyIsNotMapped(propertyName))
                        throw new StrictMappingException("'{0}' property is not mapped to a column", propertyName);
                }
            }

            if (strictMappingType == StrictMappingType.WorksheetStrict || strictMappingType == StrictMappingType.Both)
            {
                foreach (var column in columns)
                {
                    if (!propertyNames.Contains(column) && ColumnIsNotMapped(column))
                        throw new StrictMappingException("'{0}' column is not mapped to a property", column);
                }
            }
        }

        private bool PropertyIsNotMapped(string propertyName)
        {
            return !_args.ColumnMappings.Keys.Contains(propertyName);
        }

        private bool ColumnIsNotMapped(string columnName)
        {
            return !_args.ColumnMappings.Values.Contains(columnName);
        }

        private object GetColumnValue(IDataRecord data, string columnName, string propertyName)
        {
            //Perform the property transformation if there is one
            return (_args.Transformations.ContainsKey(propertyName)) ?
                _args.Transformations[propertyName](data[columnName].ToString()) :
                data[columnName];
        }

        private IEnumerable<object> GetScalarResults(IDataReader data)
        {
            data.Read();
            return new List<object> { data[0] };
        }

        private void LogSqlStatement(SqlParts sqlParts)
        {
            if (_log.IsDebugEnabled)
            {
                var logMessage = new StringBuilder();
                logMessage.AppendFormat("{0};", sqlParts);
                for (var i = 0; i < sqlParts.Parameters.Count(); i++)
                {
                    var paramValue = sqlParts.Parameters.ElementAt(i).Value.ToString();
                    var paramMessage = string.Format(" p{0} = '{1}';",
                        i, sqlParts.Parameters.ElementAt(i).Value);

                    if (paramValue.IsNumber())
                        paramMessage = paramMessage.Replace("'", "");
                    logMessage.Append(paramMessage);
                }

                var sqlLog = LogManager.GetLogger("LinqToExcel.SQL");
                sqlLog.Debug(logMessage.ToString());
            }
        }
    }
}
