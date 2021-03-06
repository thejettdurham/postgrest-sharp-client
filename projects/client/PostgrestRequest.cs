﻿using System;
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

        /// <summary>
        /// Encode response data as CSV instead of JSON
        /// </summary>
        public bool ReadAsCsv { get; set; }

        /// <summary>
        /// Send as CSV instead of JSON
        /// </summary>
        public bool WriteAsCsv { get; set; }

        /// <summary>
        /// Return a single JSON object instead of an array.
        /// </summary>
        public bool AsSingular { get; set; }

        /// <summary>
        /// Don't return the total row count in a response header
        /// </summary>
        public bool SupressCount { get; set; }
        
        /// <summary>
        /// For Create and Update requests: return the created/modified object(s) in the response
        /// </summary>
        public bool ReturnNewData { get; set; }

        /// <summary>
        /// Data to create or update
        /// </summary>
        public PostgrestModel Data { get; set; }

        /// <summary>
        /// JSON-serializable arguments for Procedure requests
        /// </summary>
        public object ProcedureArgs { get; set; }

        /// <summary>
        /// Row filters to shape the response objects
        /// </summary>
        public List<PostgrestFilter> RowFilters { get; set; }

        // TODO: Better Support Foreign Entity Embedding
        // TODO: Better Support For Type Coercion on Column Filter
        /// <summary>
        /// Column Filters to shape the response objects
        /// </summary>
        public List<string> ColumnFilters { get; set; }

        // TODO: Support json_col

        /// <summary>
        /// Ordering expressions to shape the response objects
        /// </summary>
        public List<PostgrestOrdering> Orderings { get; set; }

        /// <summary>
        /// A pair of integers to limit the number of returned objects. The lower bound is required, but the upper bound can be null.
        /// </summary>
        public Tuple<int, int?> LimitRange { get; set; }

        public PostgrestRequest(string route, PostgrestRequestType rType)
        {
            Resource = route;
            RequestType = rType;
        }

        /// <summary>
        /// Constructor to clone a request into a new object. Useful for composing complex requests from simpler ones.
        /// </summary>
        /// <param name="baseRequest"></param>
        public PostgrestRequest(PostgrestRequest baseRequest) : this(baseRequest.Resource, baseRequest.RequestType)
        {
            ReadAsCsv = baseRequest.ReadAsCsv;
            WriteAsCsv = baseRequest.WriteAsCsv;
            AsSingular = baseRequest.AsSingular;
            SupressCount = baseRequest.SupressCount;
            ReturnNewData = baseRequest.ReturnNewData;
            Data = baseRequest.Data;
            ProcedureArgs = baseRequest.ProcedureArgs;
            RowFilters = baseRequest.RowFilters;
            ColumnFilters = baseRequest.ColumnFilters;
            Orderings = baseRequest.Orderings;
            LimitRange = baseRequest.LimitRange;
        }

        #region Generated Equality Members
        protected bool Equals(PostgrestRequest other) => Resource == other.Resource && RequestType == other.RequestType && ReadAsCsv == other.ReadAsCsv && WriteAsCsv == other.WriteAsCsv && AsSingular == other.AsSingular && SupressCount == other.SupressCount && ReturnNewData == other.ReturnNewData && Equals(Data, other.Data) && Equals(ProcedureArgs, other.ProcedureArgs) && Equals(RowFilters, other.RowFilters) && Equals(ColumnFilters, other.ColumnFilters) && Equals(Orderings, other.Orderings) && Equals(LimitRange, other.LimitRange);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PostgrestRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) RequestType;
                hashCode = (hashCode*397) ^ ReadAsCsv.GetHashCode();
                hashCode = (hashCode*397) ^ WriteAsCsv.GetHashCode();
                hashCode = (hashCode*397) ^ AsSingular.GetHashCode();
                hashCode = (hashCode*397) ^ SupressCount.GetHashCode();
                hashCode = (hashCode*397) ^ ReturnNewData.GetHashCode();
                hashCode = (hashCode*397) ^ (Data?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (ProcedureArgs?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (RowFilters?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (ColumnFilters?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (Orderings?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (LimitRange?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        #endregion

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
                        AddParameter(PostgrestHeaders.JsonContentType.Value, JsonConvert.SerializeObject(ProcedureArgs), ParameterType.RequestBody);
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
                RowFilters?.ForEach(f =>
                {
                    AddQueryParameter(f.ColumnName, f.FilterExpression);
                });
            }
            
            switch (RequestType)
            {
                case PostgrestRequestType.Read:
                    Method = Method.GET;
                    PrepareReadRequest();
                    break;

                case PostgrestRequestType.Delete:
                    Method = Method.DELETE;
                    break;

                case PostgrestRequestType.Create:
                    Method = Method.POST;
                    if (ReturnNewData) PrepareVolatileRequestToReturnData();
                    string serializedData;

                    if (WriteAsCsv)
                    {
                        AddHeader(PostgrestHeaders.SendCsv);
                        serializedData = Data.ToCsv();
                        AddParameter(PostgrestHeaders.SendCsv.Value, serializedData, ParameterType.RequestBody);
                    }
                    else
                    {
                        serializedData = Data.ToJson();
                        AddParameter(PostgrestHeaders.JsonContentType.Value, serializedData, ParameterType.RequestBody);
                    }

                    
                    break;           

                case PostgrestRequestType.Update:
                    Method = Method.PATCH;
                    if (ReturnNewData) PrepareVolatileRequestToReturnData();
                    AddParameter(PostgrestHeaders.JsonContentType.Value, Data.ToMinimalJson(), ParameterType.RequestBody);
                    break;
            }
        }

        private void PrepareReadRequest()
        {
            if (ReadAsCsv) AddHeader(PostgrestHeaders.AcceptCsv);
            if (AsSingular) AddHeader(PostgrestHeaders.SingularResponse);
            if (SupressCount) AddHeader(PostgrestHeaders.SuppressCount);

            if (ColumnFilters != null && ColumnFilters.Count > 0)
            {
                AddQueryParameter(ColumnFilterKeyword, string.Join(ColumnFilterSeparator, ColumnFilters));
            }

            if (Orderings != null && Orderings.Count > 0)
            {
                var orderParam = PostgrestOrdering.BuildOrderParameter(Orderings);

                AddQueryParameter(orderParam.Key, orderParam.Value);
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
            if (RowFilters == null || RowFilters.Count == 0) throw new InvalidOperationException("Row Filters must be supplied for " + RequestType + " requests.");
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
