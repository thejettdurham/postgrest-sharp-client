using System;

namespace Postgrest.Client.Sample
{
    public class SimplePostgrestClient : PostgrestClient
    {
        // Specifying a null token here informs the client to provide no authentication.
        protected override string AuthToken => null;
        public override Uri BaseUrl => new Uri("https://testapi.com");

    }
}
