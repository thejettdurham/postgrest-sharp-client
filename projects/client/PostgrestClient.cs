using System.Collections.Generic;
using RestSharp;

namespace Postgrest.Client
{
    public abstract class PostgrestClient : RestClient
    {
        private static HttpHeader _authHeader;

        public abstract string AuthToken { get; }
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

        protected PostgrestClient(string location) : base(location)
        {
            Authenticator = new PostgrestAuthenticator(AuthHeader);
        }

        public T Execute<T>(PostgrestRequest postgrestRequest)
        {
            PrepareGeneralRequest(postgrestRequest);
            return ((PostgrestResponse)Execute(postgrestRequest)).GetDataIfValid<T>();
        }

        public void ExecuteNonQuery(PostgrestRequest postgrestRequest)
        {
            PrepareGeneralRequest(postgrestRequest);
            ((PostgrestResponse)Execute(postgrestRequest)).WasValid();
        }

        /// <summary>
        /// Adds authorization and implementation-specific headers to incoming requests before execution
        /// </summary>
        /// <param name="request"></param>
        private void PrepareGeneralRequest(PostgrestRequest request)
        {
            request.AddHeader(PostgrestHeaders.ContentType);
            ExtraHeaders?.ForEach(header => request.AddHeader(header.Name, header.Value));
        }
    }
}
