using System;
using System.Collections.Generic;

namespace Postgrest.Client
{
    public class PostgrestOrdering
    {
        private const string OrderKeyword = "order";

        public string ColumnToOrderBy { get; private set; }
        public bool? IsAscending { get; set; }
        public bool? NullsFirst { get; set; }

        public PostgrestOrdering(string columnName)
        {
            ColumnToOrderBy = columnName;
        }

        public static KeyValuePair<string, string> BuildOrderParameter(List<PostgrestOrdering> orderings)
        {
            throw new NotImplementedException();
        } 
    }
}