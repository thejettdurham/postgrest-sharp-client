using System.Collections.Generic;

namespace Postgrest.Client
{
    public sealed class PostgrestFilter
    {
        private const string FilterExpressionSeparator = ".";
        private const string FilterNegator = "not";

        private readonly PostgrestFilterOperation _filterOperation;
        private readonly string _filterCondition;

        private static readonly Dictionary<PostgrestFilterOperation, string> OperationsToExpressions = new Dictionary
            <PostgrestFilterOperation, string>
        {
            {PostgrestFilterOperation.EqualTo, "eq"},
            {PostgrestFilterOperation.GreaterThan, "gt"},
            {PostgrestFilterOperation.LessThan, "lt"},
            {PostgrestFilterOperation.GreaterThanOrEqualTo, "gte"},
            {PostgrestFilterOperation.LessThanOrEqualTo, "lte"},
            {PostgrestFilterOperation.Like, "like"},
            {PostgrestFilterOperation.ILike, "ilike"},
            {PostgrestFilterOperation.FullText, "@@"},
            {PostgrestFilterOperation.Is, "is"},
            {PostgrestFilterOperation.In, "in"}
        };

        /// <summary>
        /// The column to which the filter is applied
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Generates the Filter expression for use in a request's Query Parameter
        /// </summary>
        public string FilterExpression => ((Negate) ? FilterNegator + FilterExpressionSeparator : "") + OperationsToExpressions[_filterOperation] + FilterExpressionSeparator + _filterCondition;

        /// <summary>
        /// Inverts the logic of the filter
        /// </summary>
        public bool Negate { get; set; }

        public PostgrestFilter(string columnName, PostgrestFilterOperation filterOperation, string filterCondition)
        {
            ColumnName = columnName;
            _filterOperation = filterOperation;
            _filterCondition = filterCondition;
        }
    }

    public enum PostgrestFilterOperation
    {
        EqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        Like,
        ILike,
        FullText,
        Is,
        In
    }
}
