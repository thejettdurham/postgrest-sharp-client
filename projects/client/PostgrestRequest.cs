using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestRequest : RestRequest
    {
        private const string StoredProcedurePrefix = "rpc/";
        protected const string ColumnFilterKeyword = "select";

        protected PostgrestRequest(string route, Method method) : base(route, method)
        {

        }

        public IRestRequest AddHeader(HttpHeader header)
        {
            return AddHeader(header.Name, header.Value);
        }

        #region Static Builder Methods
        public static PostgrestReadRequest Read(string table)
        {
            return new PostgrestReadRequest(table);
        }

        public static PostgrestRequest Create<T>(string table, T newData) where T : PostgrestModel
        {
            var request = new PostgrestRequest(table, Method.POST);
            request.AddBody(newData.MinimalJson);
            return request;
        }

        public static PostgrestRequest Update<T>(string table, T updateData) where T: PostgrestModel
        {
            var request = new PostgrestRequest(table, Method.PATCH);
            request.AddBody(updateData.MinimalJson);
            // Add primary key filter
            return request;
        }

        public static PostgrestRequest Delete<T>(string table, T deleteData) where T: PostgrestModel
        {
            var request = new PostgrestRequest(table, Method.DELETE);
            // Add primary key filter
            return request;
        }

        // TODO: More specific typing for procedure arguments?
        public static PostgrestRequest Procedure(string procedureName, object args)
        {
            var request = new PostgrestRequest(StoredProcedurePrefix + procedureName, Method.POST);
            request.AddJsonBody(args);
            return request;
        }

        #endregion

        public PostgrestRequest Where(string colName, PostgrestFilter filter)
        {
            AddQueryParameter(colName, filter.ToString());
            return this;
        }

    }

    public class PostgrestReadRequest : PostgrestRequest
    {
        protected internal PostgrestReadRequest(string route) : base(route, Method.GET)
        {

        }

        public PostgrestReadRequest AsCsv()
        {
            AddHeader(PostgrestHeaders.AcceptCsv);
            return this;
        }

        public PostgrestReadRequest Singular()
        {
            AddHeader(PostgrestHeaders.SingularResponse);
            return this;
        }

        // TODO: Support Embedding Foreign Elements
        public PostgrestReadRequest SelectColumns(IEnumerable<string> columns)
        {
            AddQueryParameter(ColumnFilterKeyword, string.Join(",", columns));
            return this;
        }
    }
}
