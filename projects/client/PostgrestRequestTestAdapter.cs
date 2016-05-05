using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgrest.Client
{
    /// <summary>
    /// An adapter class to allow the testing of protected/internal methods without making them public
    /// </summary>
    public class PostgrestRequestTestAdapter
    {
        public PostgrestRequest Request { get; }

        public PostgrestRequestTestAdapter(PostgrestRequest req)
        {
            Request = req;
        }

        public PostgrestRequest PrepareRequest()
        {
            Request.PrepareRequest();
            return Request;
        }
    }
}
