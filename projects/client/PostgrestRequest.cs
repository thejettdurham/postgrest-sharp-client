using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestRequest : RestRequest
    {
        private const string StoredProcedurePrefix = "rpc/";
        private const string ColumnFilterKeyword = "select";
        

        //public string Route { get; private set; }
        public PostgrestRequestType RequestType { get; }
        public bool AsCsv { get; set; } // Read, (Create, Update if ReturnNewData)
        public bool AsSingular { get; set; } // Read
        public bool SupressCount { get; set; } // Read
        public bool ReturnNewData { get; set; } // Create, Update
        public PostgrestModel Data { get; set; } // Create, Update, Delete
        public object ProcedureArgs { get; set; }
        public List<PostgrestFilter> RowFilters { get; set; }

        // TODO: Better Support Foreign Entity Embedding
        // TODO: Better Support For Type Coercion on Column Filter
        public List<string> ColumnFilters { get; set; } // Read, Create, Update (if ReturnNewData)

        // TODO: Support json_col
       
        public List<PostgrestOrdering> Orderings { get; set; } // Read, Create, Update (if ReturnNewData)
        public Tuple<int, int?> LimitRange { get; set; } // Read

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
            // Handle the oddball requests first
            switch (RequestType)
            {
                case PostgrestRequestType.Schema:
                    Method = Method.OPTIONS;
                    return;

                case PostgrestRequestType.Procedure:
                    PrepareProcedureRequest();
                    return;

                default:
                    RowFilters.ForEach(f =>
                    {
                        AddQueryParameter(f.ColumnName, f.FilterExpression);
                    });
                    break;
            }

            switch (RequestType)
            {
                case PostgrestRequestType.Read:
                    break;
            }
        }

        private void PrepareProcedureRequest()
        {
            Method = Method.POST;
            Resource = Resource.Insert(0, StoredProcedurePrefix);
            AddBody(JsonConvert.SerializeObject(ProcedureArgs));
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
