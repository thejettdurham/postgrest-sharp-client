using System;

namespace Postgrest.Client
{
    public class PostgrestRequestFailedException : Exception
    {
        public PostgrestRequestFailedException(string message, Exception requestException) : base(message, requestException)
        {
            
        }
    }
}
