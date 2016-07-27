using System;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestErrorException : Exception
    {
        public PostgrestError Error { get; private set; }
        public IRestResponse ApiResponse { get; set; }


        public PostgrestErrorException(PostgrestError err) : base("API returned an error")
        {
            Error = err;
        }
    }
}
