namespace Postgrest.Client
{
    public class PostgrestError
    {
        public string Hint { get; set; }
        public string Details { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
