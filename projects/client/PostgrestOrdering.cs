using System;
using System.Collections.Generic;

namespace Postgrest.Client
{
    public class PostgrestOrdering
    {
        private const string OrderKeyword = "order";
        private const string AscendingKeyword = "asc";
        private const string DescendingKeyword = "desc";
        private const string NullsFirstKeyword = "nullsfirst";
        private const string NullsLastKeyword = "nullslast";
        private const string InternalExpressionSeparator = ".";
        private const string ExternalExpressionSeparator = ",";

        public string ColumnToOrderBy { get; private set; }
        public bool? IsAscending { get; set; }
        public bool? NullsFirst { get; set; }

        public string OrderExpression
        {
            get
            {
                var expression = ColumnToOrderBy;
                if (IsAscending != null)
                {
                    expression += InternalExpressionSeparator + ((IsAscending.Value) ? AscendingKeyword : DescendingKeyword);
                }

                if (NullsFirst != null)
                {
                    expression += InternalExpressionSeparator +
                                  ((NullsFirst.Value) ? NullsFirstKeyword : NullsLastKeyword);
                }

                return expression;
            }
        }

        public PostgrestOrdering(string columnName)
        {
            ColumnToOrderBy = columnName;
        }
        public static Tuple<string, string> BuildOrderParameter(List<PostgrestOrdering> orderings)
        {
            var orderExpressions = new List<string>();

            orderings.ForEach(o =>
            {
                orderExpressions.Add(o.OrderExpression);
            });

            return Tuple.Create(OrderKeyword, string.Join(ExternalExpressionSeparator, orderExpressions));
        } 
    }
}