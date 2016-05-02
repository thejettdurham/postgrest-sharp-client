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

        public string GetContentIfValid()
        {
            WasValid();

            return Content;
        }

        /// <summary>
        /// Throws an exception if any error was found in the response.
        /// </summary>
        public void WasValid()
        {
            NoErrorsInRequest();
            StatusCodeIsValid();
        }
        
        /// <summary>
        /// Throws a PostgrestRequestFailedException if any transport error is found in the response.
        /// </summary>
        private void NoErrorsInRequest()
        {
            if (ErrorException != null) throw new PostgrestRequestFailedException(ErrorMessage ?? "Could not communicate with API server", ErrorException);
            if (ResponseStatus != ResponseStatus.Completed) throw new PostgrestRequestFailedException("Failed to complete request to API server", ErrorException);
            if (ErrorMessage != null) throw new PostgrestRequestFailedException(ErrorMessage, ErrorException);
        }

        /// <summary>
        /// Throws a PostgrestErrorException if the HTTP status code of the response suggests an error. The error object from the API server is included with this exception.
        /// </summary>
        private void StatusCodeIsValid()
        {
            if (!(StatusCode >= HttpStatusCode.OK && StatusCode < HttpStatusCode.Ambiguous))
            {
                throw new PostgrestErrorException(JsonConvert.DeserializeObject<PostgrestError>(Content));
            }
        }
    }
}
