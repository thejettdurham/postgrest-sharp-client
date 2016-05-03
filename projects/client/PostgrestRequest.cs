using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestRequest : RestRequest
    {
        private const string StoredProcedurePrefix = "rpc/";
        private const string ColumnFilterKeyword = "select";
        private const string ColumnFilterSeparator = ",";
        private const string RangeHeaderName = "Range";
        private const string RangeHeaderValueSeparator = "-";
        
        /// <summary>
        /// Defines the over-arching behavior of the request
        /// </summary>
        public PostgrestRequestType RequestType { get; }
        public bool ReadAsCsv { get; set; }
        public bool WriteAsCsv { get; set; }
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

        public PostgrestRequest(string route, PostgrestRequestType rType)
        {
            Resource = route;
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
        protected internal void PrepareRequest()
        {
            // Handle the oddball requests and any potential runtime errors first
            switch (RequestType)
            {
                case PostgrestRequestType.Schema:
                    Method = Method.OPTIONS;
                    return;

                case PostgrestRequestType.Procedure:
                    Method = Method.POST;
                    Resource = Resource.Insert(0, StoredProcedurePrefix);
                    if (ProcedureArgs != null)
                    {
                        AddBody(JsonConvert.SerializeObject(ProcedureArgs));
                    }
                    return;
                
                case PostgrestRequestType.Create:
                    ErrorIfNoData();
                    break;

                case PostgrestRequestType.Update:
                    ErrorIfNoRowFilters();
                    ErrorIfNoData();
                    break;

                case PostgrestRequestType.Delete:
                    ErrorIfNoRowFilters();
                    break;
            }

            if (RequestType != PostgrestRequestType.Create)
            {
                RowFilters.ForEach(f =>
                {
                    AddQueryParameter(f.ColumnName, f.FilterExpression);
                });
            }
            
            switch (RequestType)
            {
                case PostgrestRequestType.Create:
                    Method = Method.POST;
                    if (ReturnNewData) PrepareVolatileRequestToReturnData();
                    string serialized;

                    if (WriteAsCsv)
                    {
                        AddHeader(PostgrestHeaders.SendCsv);
                        serialized = Data.Csv;
                    }
                    else
                    {
                        AddHeader(PostgrestHeaders.JsonContentType);
                        serialized = Data.Json;
                    }

                    AddParameter("theBody", serialized, ParameterType.RequestBody);
                    break;

                case PostgrestRequestType.Read:
                    Method = Method.GET;
                    PrepareReadRequest();
                    break;

                case PostgrestRequestType.Update:
                    Method = Method.PATCH;
                    if (ReturnNewData) PrepareVolatileRequestToReturnData();
                    AddParameter("theBody", Data.MinimalJson, ParameterType.RequestBody);
                    break;

                case PostgrestRequestType.Delete:
                    Method = Method.DELETE;
                    break;
            }
        }

        private void PrepareReadRequest()
        {
            if (ReadAsCsv) AddHeader(PostgrestHeaders.AcceptCsv);
            if (AsSingular) AddHeader(PostgrestHeaders.SingularResponse);
            if (SupressCount) AddHeader(PostgrestHeaders.SuppressCount);

            if (ColumnFilters != null)
            {
                AddQueryParameter(ColumnFilterKeyword, string.Join(ColumnFilterSeparator, ColumnFilters));
            }

            if (Orderings != null)
            {
                var orderParam = PostgrestOrdering.BuildOrderParameter(Orderings);

                AddQueryParameter(orderParam.Item1, orderParam.Item2);
            }

            if (LimitRange != null)
            {
                AddHeader(PostgrestHeaders.RangeUnit);
                AddHeader(RangeHeaderName,
                    LimitRange.Item1 + RangeHeaderValueSeparator + (LimitRange.Item2?.ToString() ?? ""));
            }
        }

        private void PrepareVolatileRequestToReturnData()
        {
            AddHeader(PostgrestHeaders.ReturnRepresentation);
            PrepareReadRequest();
        }

        private void ErrorIfNoRowFilters()
        {
            if (RowFilters == null) throw new InvalidOperationException("Row Filters must be supplied for " + RequestType + " requests.");
        }

        private void ErrorIfNoData()
        {
            if (Data == null) throw new InvalidOperationException("Data must be supplied for " + RequestType + " requests.");
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
}
