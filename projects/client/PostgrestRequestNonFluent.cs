using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestRequestNonFluent : RestRequest
    {
        private const string StoredProcedurePrefix = "rpc/";
        private const string ColumnFilterKeyword = "select";
        private const string OrderKeyword = "order";

        public string Route { get; private set; }
        public PostgrestRequestType RequestType { get; private set; }
        public bool AsCsv { get; set; }
        public bool AsSingular { get; set; }
        public bool SupressCount { get; set; }
        public bool ReturnNewData { get; set; }
        public PostgrestModel Data { get; set; }
        public object ProcedureArgs { get; set; }
        public List<PostgrestFilter> RowFilters { get; set; }

        // TODO: Better Support Foreign Entity Embedding
        // TODO: Better Support For Type Coercion on Column Filter
        public List<string> ColumnFilters { get; set; }

        // TODO: Support json_col
       
        public List<PostgrestOrdering> Orderings { get; set; }
        public Tuple<int, int?> LimitRange { get; set; }

        public PostgrestRequestNonFluent(string route, PostgrestRequestType rType)
        {
            Route = route;
            RequestType = rType;
        }

        /// <summary>
        /// A simple extension method to allow direct addition of an HttpHeader to an IRestRequest.
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public IRestRequest AddHeader(HttpHeader header)
        {
            return AddHeader(header.Name, header.Value);
        }

        /// <summary>
        /// Used by a PostgrestClient to convert the internal state of the PostgrestRequest into headers and parameters
        /// </summary>
        protected void PrepareRequest()
        {
            
        }
    }

    public enum PostgrestRequestType
    {
        Create,
        Read,
        Update,
        Delete,
        Schema,
        Procedure
    }

    public class PostgrestOrdering
    {
        public string ColumnToOrderBy { get; private set; }
        public bool? IsAscending { get; set; }
        public bool? NullsFirst { get; set; }

        public PostgrestOrdering(string columnName)
        {
            ColumnToOrderBy = columnName;
        }
    }
}
