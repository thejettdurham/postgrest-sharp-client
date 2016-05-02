using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
