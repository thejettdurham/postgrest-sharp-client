using System;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestRequest : RestRequest
    {
        public static PostgrestRequest Create<T>(string table, T newData) where T: PostgrestModel
        {
            var request = new PostgrestRequest(table, Method.POST);
            request.AddBody(newData.GetMinimalJson());
            return request;
        }

        public static PostgrestRequest Read(string table)
        {
            return new PostgrestRequest(table, Method.GET);
        }

        public static PostgrestRequest ReadAsCsv(string table)
        {
            var request = Read(table);
            request.AddHeader(PostgrestHeaders.AcceptCsv);
            return request;
        }

        public static PostgrestRequest ReadSingle(string table)
        {
            var request = Read(table);
            request.AddHeader(PostgrestHeaders.SingularResponse);
            return request;
        }

        public static PostgrestRequest ReadSingleAsCsv(string table)
        {
            var request = ReadAsCsv(table);
            request.AddHeader(PostgrestHeaders.SingularResponse);
            return request;
        }

        public static PostgrestRequest Update<T>(string table, T updateData) where T: PostgrestModel
        {
            var request = new PostgrestRequest(table, Method.PATCH);
            request.AddBody(updateData.GetMinimalJson());
            // Add primary key filter
            return request;
        }

        public static PostgrestRequest Delete<T>(string table, T deleteData) where T: PostgrestModel
        {
            var request = new PostgrestRequest(table, Method.DELETE);
            // Add primary key filter
            return request;
        }

        public static PostgrestRequest Procedure(string procedureName, object args)
        {
            throw new NotImplementedException();
        }

        private PostgrestRequest(string route, Method method) : base(route, method)
        {
            
        }

        public IRestRequest AddHeader(HttpHeader header)
        {
            return AddHeader(header.Name, header.Value);
        }
    }
}
