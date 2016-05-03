using System;
using System.Collections.Generic;
using RestSharp;

namespace Postgrest.Client
{
    public class SimplePostgrestClient : PostgrestClient
    {
        public override string AuthToken => "blablah";
        public override Uri BaseUrl => new Uri("");
        public override List<HttpHeader> ExtraHeaders { get; set; }
    }
}
