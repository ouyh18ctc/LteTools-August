using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses;
using Remotion.Data.Linq.Clauses.Expressions;
using Remotion.Data.Linq.Clauses.ResultOperators;
using Remotion.Data.Linq.Collections;
using Remotion.Data.Linq.Parsing;

namespace Lte.Domain.LinqToExcel.Entities
{
    internal class SqlGeneratorQueryModelVisitor : QueryModelVisitorBase
    {
        public SqlParts SqlStatement { get; protected set; }
        private readonly ExcelQueryArgs _args;

        internal SqlGeneratorQueryModelVisitor(ExcelQueryArgs args)
        {
            _args = args;
            SqlStatement = new SqlParts();
            SqlStatement.Table = (String.IsNullOrEmpty(_args.StartRange)) ?
                !String.IsNullOrEmpty(_args.NamedRangeName) && String.IsNullOrEmpty(_args.WorksheetName) ?
                string.Format("[{0}]",
                    _args.NamedRangeName) :
                string.Format("[{0}${1}]",
                    _args.WorksheetName, _args.NamedRangeName) :
                string.Format("[{0}${1}:{2}]",
                    _args.WorksheetName, _args.StartRange, _args.EndRange);

            if (!string.IsNullOrEmpty(_args.WorksheetName) && _args.WorksheetName.ToLower().EndsWith(".csv"))
                SqlStatement.Table = SqlStatement.Table.Replace("$]", "]");
        }

        public override void VisitGroupJoinClause(GroupJoinClause groupJoinClause, QueryModel queryModel, int index)
        {
            throw new NotSupportedException("LinqToExcel does not provide support for group join");
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            throw new NotSupportedException("LinqToExcel does not provide support for the Join() method");
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);

            if (queryModel.MainFromClause.ItemType.Name == "IGrouping`2")
                throw new NotSupportedException("LinqToExcel does not provide support for the Group() method");
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            var where = new WhereClauseExpressionTreeVisitor(queryModel.MainFromClause.ItemType, _args.ColumnMappings);
            where.Visit(whereClause.Predicate);
            SqlStatement.Where = where.WhereClause;
            SqlStatement.Parameters = where.Params;
            SqlStatement.ColumnNamesUsed.AddRange(where.ColumnNamesUsed);

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            //Affects SQL result operators
            if (resultOperator is TakeResultOperator)
            {
                var take = resultOperator as TakeResultOperator;
                SqlStatement.Aggregate = string.Format("TOP {0} *", take.Count);
            }
            else if (resultOperator is AverageResultOperator)
                UpdateAggregate(queryModel, "AVG");
            else if (resultOperator is CountResultOperator)
                SqlStatement.Aggregate = "COUNT(*)";
            else if (resultOperator is LongCountResultOperator)
                SqlStatement.Aggregate = "COUNT(*)";
            else if (resultOperator is FirstResultOperator)
                SqlStatement.Aggregate = "TOP 1 *";
            else if (resultOperator is MaxResultOperator)
                UpdateAggregate(queryModel, "MAX");
            else if (resultOperator is MinResultOperator)
                UpdateAggregate(queryModel, "MIN");
            else if (resultOperator is SumResultOperator)
                UpdateAggregate(queryModel, "SUM");
            else if (resultOperator is DistinctResultOperator)
                ProcessDistinctAggregate(queryModel);

            //Not supported result operators
            else if (resultOperator is ContainsResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the Contains() method");
            else if (resultOperator is DefaultIfEmptyResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the DefaultIfEmpty() method");
            else if (resultOperator is ExceptResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the Except() method");
            else if (resultOperator is GroupResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the Group() method");
            else if (resultOperator is IntersectResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the Intersect() method");
            else if (resultOperator is OfTypeResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the OfType() method");
            else if (resultOperator is SingleResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the Single() method. Use the First() method instead");
            else if (resultOperator is UnionResultOperator)
                throw new NotSupportedException("LinqToExcel does not provide support for the Union() method");

            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        protected override void VisitBodyClauses(ObservableCollection<IBodyClause> bodyClauses, QueryModel queryModel)
        {
            var orderClause = bodyClauses
                .FirstOrDefault(x => x.GetType() == typeof(OrderByClause))
                as OrderByClause;

            if (orderClause != null)
            {
                var columnName = "";
                var exp = orderClause.Orderings.First().Expression;
                if (exp is MemberExpression)
                {
                    var mExp = exp as MemberExpression;
                    columnName = (_args.ColumnMappings.ContainsKey(mExp.Member.Name)) ?
                        _args.ColumnMappings[mExp.Member.Name] :
                        mExp.Member.Name;
                }
                else if (exp is MethodCallExpression)
                {
                    //row["ColumnName"] is being used in order by statement
                    columnName = ((MethodCallExpression)exp).Arguments.First()
                        .ToString().Replace("\"", "");
                }

                SqlStatement.OrderBy = columnName;
                SqlStatement.ColumnNamesUsed.Add(columnName);
                var orderDirection = orderClause.Orderings.First().OrderingDirection;
                SqlStatement.OrderByAsc = (orderDirection == OrderingDirection.Asc);
            }
            base.VisitBodyClauses(bodyClauses, queryModel);
        }

        protected void UpdateAggregate(QueryModel queryModel, string aggregateName)
        {
            var columnName = GetResultColumnName(queryModel);
            SqlStatement.Aggregate = string.Format("{0}({1})",
                aggregateName,
                columnName);
            SqlStatement.ColumnNamesUsed.Add(columnName);
        }

        protected void ProcessDistinctAggregate(QueryModel queryModel)
        {
            if (queryModel.SelectClause.Selector is MemberExpression)
                UpdateAggregate(queryModel, "DISTINCT");
            else
                throw new NotSupportedException(
                    "LinqToExcel only provides support for the Distinct() method when it's mapped to a class and a single property is selected. [e.g. (from row in excel.Worksheet<Person>() select row.FirstName).Distinct()]");
        }

        private string GetResultColumnName(QueryModel queryModel)
        {
            var mExp = queryModel.SelectClause.Selector as MemberExpression;
            return 
                mExp != null && (_args.ColumnMappings != null && _args.ColumnMappings.ContainsKey(mExp.Member.Name)) ?
                _args.ColumnMappings[mExp.Member.Name] :
                mExp.Member.Name;
        }
    }

    public class ProjectorBuildingExpressionTreeVisitor : ExpressionTreeVisitor
    {
        // This is the generic ResultObjectMapping.GetObject<T>() method we'll use to obtain a queried object for an IQuerySource.
        private static readonly MethodInfo s_getObjectGenericMethodDefinition 
            = typeof(ResultObjectMapping).GetMethod("GetObject");

        // Call this method to get the projector. T is the type of the result (after the projection).
        public static Func<ResultObjectMapping, T> BuildProjector<T>(Expression selectExpression)
        {
            // This is the parameter of the delegat we're building. It's the ResultObjectMapping, which holds all the input data needed for the projection.
            var resultItemParameter = Expression.Parameter(typeof(ResultObjectMapping), "resultItem");

            // The visitor gives us the projector's body. It simply replaces all QuerySourceReferenceExpressions with calls to ResultObjectMapping.GetObject<T>().
            var visitor = new ProjectorBuildingExpressionTreeVisitor(resultItemParameter);
            var body = visitor.VisitExpression(selectExpression);

            // Construct a LambdaExpression from parameter and body and compile it into a delegate.
            var projector = Expression.Lambda<Func<ResultObjectMapping, T>>(body, resultItemParameter);
            return projector.Compile();
        }

        private readonly ParameterExpression _resultItemParameter;

        private ProjectorBuildingExpressionTreeVisitor(ParameterExpression resultItemParameter)
        {
            _resultItemParameter = resultItemParameter;
        }

        /// <summary>
        /// Substitute generic parameter "T" of ResultObjectMapping.GetObject<T/>() 
        /// with type of query source item, then return a call to that method
        /// with the query source referenced by the expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitQuerySourceReferenceExpression(QuerySourceReferenceExpression expression)
        { 
            var getObjectMethod = s_getObjectGenericMethodDefinition.MakeGenericMethod(expression.Type);
            return Expression.Call(_resultItemParameter, getObjectMethod, 
                Expression.Constant(expression.ReferencedQuerySource));
        }

        protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
        {
            throw new NotSupportedException("This provider does not support subqueries in the select projection.");
        }
    }
}
