using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Postgrest.Client
{
    public sealed class PostgrestFilter
    {
        private readonly PostgrestFilterOperation _filterOperation;
        private readonly string _filterCondition;
        private readonly string _columnName;

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

        public bool Negate { get; set; }

        public HttpHeader Header => new HttpHeader
        {
            Name = _columnName,
            Value = ToFilterExpression()
        };

        public PostgrestFilter(string columnName, PostgrestFilterOperation filterOperation, string filterCondition)
        {
            _columnName = columnName;
            _filterOperation = filterOperation;
            _filterCondition = filterCondition;
        }

        private string ToFilterExpression()
        {
            return ((Negate) ? "not." : "") + OperationsToExpressions[_filterOperation] + "." + _filterCondition;
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
