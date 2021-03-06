﻿using RestSharp;
using RestSharp.Authenticators;

namespace Postgrest.Client
{
    /// <summary>
    /// Implements RestSharp's IAuthenticator interface for authenticating requests in a PostgrestClient.
    /// </summary>
    public class PostgrestAuthenticator : IAuthenticator
    {
        private HttpHeader AuthHeader { get; }

        public PostgrestAuthenticator(HttpHeader auth)
        {
            AuthHeader = auth;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader(AuthHeader.Name, AuthHeader.Value);
        }
    }
}
