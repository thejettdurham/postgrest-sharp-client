using System;
using System.Collections.Generic;
using RestSharp;

namespace Postgrest.Client
{
    public abstract class PostgrestClient : RestClient
    {
        private static HttpHeader _authHeader;

        public abstract string AuthToken { get; }
        public override abstract Uri BaseUrl { get; }
        public abstract List<HttpHeader> ExtraHeaders { get; set; } 

        public HttpHeader AuthHeader
        {
            get
            {
                if (_authHeader != null) return _authHeader;

                _authHeader = new HttpHeader
                {
                    Name = "Authorization",
                    Value = "Bearer " + AuthToken
                };

                return _authHeader;
            }
        }

        protected PostgrestClient()
        {
            Authenticator = new PostgrestAuthenticator(AuthHeader);
        }

        /// <summary>
        /// Executes the request, checks validity, deserializes the response content as JSON into type T, and returns it.
        /// </summary>
        /// <typeparam name="T">A type deserializable by JSON.NET</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T ExecuteAndGetData<T>(PostgrestRequest request)
        {
            return ExecuteRaw(request).GetDataIfValid<T>();
        }

        /// <summary>
        /// Executes the request, checks validity, and returns the response content as a string.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string ExecuteAndGetContent(PostgrestRequest request)
        {
            return ExecuteRaw(request).GetContentIfValid();
        }

        /// <summary>
        /// Executes the request and checks validity. Any errors found in the response will throw an exception
        /// </summary>
        /// <param name="request"></param>
        public void ExecuteAndCheckValidity(PostgrestRequest request)
        {
            ExecuteRaw(request).WasValid();
        }

        /// <summary>
        /// Executes the request and returns the response without any further validity checking.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PostgrestResponse ExecuteRaw(PostgrestRequest request)
        {
            ExtraHeaders?.ForEach(header => request.AddHeader(header));
            request.PrepareRequest();
            return (PostgrestResponse)Execute(request);
        }
    }
}
