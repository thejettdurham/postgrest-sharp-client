using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestResponse : RestResponse
    {
        public T GetDataIfValid<T>()
        {
            WasValid();

            return JsonConvert.DeserializeObject<T>(Content);
        }

        public void WasValid()
        {
            NoErrorsInRequest();
            StatusCodeIsValid();
        }

        private void NoErrorsInRequest()
        {
            if (ErrorException != null) throw new PostgrestRequestFailedException(ErrorMessage ?? "Could not communicate with API server", ErrorException);
            if (ResponseStatus != ResponseStatus.Completed) throw new PostgrestRequestFailedException("Failed to complete request to API server", ErrorException);
            if (ErrorMessage != null) throw new PostgrestRequestFailedException(ErrorMessage, ErrorException);
        }

        private void StatusCodeIsValid()
        {
            if (!(StatusCode >= HttpStatusCode.OK && StatusCode < HttpStatusCode.Ambiguous))
            {
                throw new PostgrestErrorException(JsonConvert.DeserializeObject<PostgrestError>(Content));
            }
        }
    }
}
