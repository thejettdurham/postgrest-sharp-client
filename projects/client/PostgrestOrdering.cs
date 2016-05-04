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

        /// <summary>
        /// Column to which the ordering applies
        /// </summary>
        public string ColumnToOrderBy { get; }

        /// <summary>
        /// Specifies an ordering direction. Not setting this property does not add a direction to the request, delegating to the API server for default ordering direction.
        /// </summary>
        public bool? IsAscending { get; set; }

        /// <summary>
        /// Specifies an ordering preference on null values. Not setting this property does not add a preference to the request, delegating to the API server for default ordering preference.
        /// </summary>
        public bool? NullsFirst { get; set; }

        /// <summary>
        /// Builds the ordering expression for use in a query parameter.
        /// </summary>
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

                if (expression != null) return expression;
                throw new NullReferenceException("OrderExpression cannot be null");
            }
        }

        public PostgrestOrdering(string columnName)
        {
            ColumnToOrderBy = columnName;
        }

        /// <summary>
        /// Converts a list of ordering objects to a single header key-value pair
        /// </summary>
        /// <param name="orderings"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> BuildOrderParameter(List<PostgrestOrdering> orderings)
        {
            var orderExpressions = new List<string>();

            orderings.ForEach(o =>
            {
                orderExpressions.Add(o.OrderExpression);
            });

            return new KeyValuePair<string, string>(OrderKeyword, string.Join(ExternalExpressionSeparator, orderExpressions));
        } 
    }
}