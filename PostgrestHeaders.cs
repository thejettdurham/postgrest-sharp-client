using RestSharp;

namespace Postgrest.Client
{
    class PostgrestHeaders
    {
        public static HttpHeader ContentType => new HttpHeader
        {
            Name = "Content-Type",
            Value = "application/json"
        };

        public static HttpHeader AcceptCsv => new HttpHeader
        {
            Name = "Accept",
            Value = "text/csv"
        };

        public static HttpHeader SingularResponse => new HttpHeader
        {
            Name = "Prefer",
            Value = "plurality=singular"
        };
    }
}
