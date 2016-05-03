using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgrest.Client
{
    public sealed class PostgrestFilter
    {
        private readonly string _filterName;
        private readonly string _filterCondition;

        public bool Negate { get; set; }

        private PostgrestFilter(string name)
        {
            _filterName = name;
        }

        private PostgrestFilter(string name, string condition)
        {
            _filterName = name;
            _filterCondition = condition;
        }

        public string ToFilterExpression()
        {
            return ((Negate) ? "not." : "") + _filterName + "." + _filterCondition;
        }

        public static PostgrestFilter EqualTo(string condition)
        {
            return new PostgrestFilter("eq", condition);
        } 

        public static PostgrestFilter GREATER_THAN = new PostgrestFilter("gt");
        public static PostgrestFilter LESS_THAN = new PostgrestFilter("lt");
        public static PostgrestFilter GREATER_THAN_OR_EQUAL = new PostgrestFilter("gte");
        public static PostgrestFilter LESS_THAN_OR_EQUAL = new PostgrestFilter("lte");
        public static PostgrestFilter LIKE = new PostgrestFilter("like");
        public static PostgrestFilter ILIKE = new PostgrestFilter("ilike");
        public static PostgrestFilter FULL_TEXT = new PostgrestFilter("@@");
        public static PostgrestFilter IS = new PostgrestFilter("is");
        public static PostgrestFilter IN = new PostgrestFilter("in");
    }
}
