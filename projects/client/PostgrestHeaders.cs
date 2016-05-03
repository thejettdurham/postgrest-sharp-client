using RestSharp;

namespace Postgrest.Client
{
    class PostgrestHeaders
    {
        public static HttpHeader JsonContentType => new HttpHeader
        {
            Name = "Content-Type",
            Value = "application/json"
        };

        public static HttpHeader AcceptCsv => new HttpHeader
        {
            Name = "Accept",
            Value = "text/csv"
        };

        public static HttpHeader SendCsv => new HttpHeader
        {
            Name = "Content-Type",
            Value = "text/csv"
        };

        public static HttpHeader ReturnRepresentation => new HttpHeader
        {
            Name = "Prefer",
            Value = "return=representation"
        };

        public static HttpHeader SingularResponse => new HttpHeader
        {
            Name = "Prefer",
            Value = "plurality=singular"
        };

        public static HttpHeader SuppressCount => new HttpHeader
        {
            Name = "Prefer",
            Value = "count=none"
        };

        public static HttpHeader RangeUnit => new HttpHeader
        {
            Name = "Range-Unit",
            Value = "items"
        };
    }
}
