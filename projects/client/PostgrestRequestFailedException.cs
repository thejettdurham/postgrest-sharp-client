using System;

namespace Postgrest.Client
{
    class PostgrestRequestFailedException : Exception
    {
        public PostgrestRequestFailedException(string message, Exception requestException) : base(message, requestException)
        {
            
        }
    }
}
