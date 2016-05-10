using System;
using System.Collections.Generic;
using RestSharp;

namespace Postgrest.Client
{
    /// <summary>
    /// A thin abstract wrapper around RestSharp's RestClient that implements most boilerplate from a Postgrest client implementation
    /// </summary>
    public abstract class PostgrestClient : RestClient
    {
        private static HttpHeader _authHeader;

        /// <summary>
        /// Subclasses must implment this property to instruct the RestClient how to authenticate to the API.
        /// If the Token is null, no Authentication will be supplied with any requests on the client.
        /// By design, this getter will only be ran once in the lifetime of the client instance to build the AuthHeader singleton.
        /// </summary>
        protected abstract string AuthToken { get; }

        /// <summary>
        /// A Postgrest client implementation has exactly one BaseUrl. As such, subclasses must define it.
        /// </summary>
        public abstract override Uri BaseUrl { get; }

        /// <summary>
        /// An optional list of headers to attach to all requests sent through the client. Can be null to specify no extra headers.
        /// </summary>
        public List<HttpHeader> ExtraHeaders { get; set; } 

        /// <summary>
        /// Returns an instance of the Authentication header used by this class's authenticator.
        /// </summary>
        private HttpHeader AuthHeader
        {
            get
            {
                if (AuthToken == null) return null;
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
            // -- Checking BaseUrl here will force an exception to bubble up if the subclass fails to implement it or implements it with a null value.
            // -- Much better than letting a non-well-formed client object exist.
            // ReSharper disable once VirtualMemberCallInConstructor
            if (BaseUrl != null && AuthHeader != null)
            {
                Authenticator = new PostgrestAuthenticator(AuthHeader);
            }
        }

        /// <summary>
        /// Executes the request, checks response validity, deserializes the response content as JSON into type T, and returns it.
        /// </summary>
        /// <typeparam name="T">A type deserializable by JSON.NET</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T ExecuteAndGetData<T>(PostgrestRequest request)
        {
            return ExecuteRaw(request).GetDataIfValid<T>();
        }

        /// <summary>
        /// Executes the request, checks response validity, and returns the response content as a string.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string ExecuteAndGetContent(PostgrestRequest request)
        {
            return ExecuteRaw(request).GetContentIfValid();
        }

        /// <summary>
        /// Executes the request, checks repsonse validity, and returns the response. Any errors found in the response will throw an exception
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PostgrestResponse ExecuteAndValidate(PostgrestRequest request)
        {
            var response = ExecuteRaw(request);
            response.WasValid();
            return response;
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
            return new PostgrestResponse(Execute(request));
        }
    }
}
