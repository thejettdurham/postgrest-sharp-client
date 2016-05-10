using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace Postgrest.Client
{
    public class PostgrestResponse
    {
        private readonly IRestResponse _baseResponse;

        public PostgrestResponse(IRestResponse resp)
        {
            _baseResponse = resp;
        }
        /// <summary>
        /// Peforms response validation, then attempts to deserialize the response body into the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataIfValid<T>()
        {
            WasValid();

            return JsonConvert.DeserializeObject<T>(_baseResponse.Content);
        }

        /// <summary>
        /// Performs response validation, then returns the response body as a string.
        /// </summary>
        /// <returns></returns>
        public string GetContentIfValid()
        {
            WasValid();

            return _baseResponse.Content;
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
            if (_baseResponse.ErrorException != null) throw new PostgrestRequestFailedException(_baseResponse.ErrorMessage ?? "Could not communicate with API server", _baseResponse.ErrorException);
            if (_baseResponse.ResponseStatus != ResponseStatus.Completed) throw new PostgrestRequestFailedException("Failed to complete request to API server", _baseResponse.ErrorException);
            if (_baseResponse.ErrorMessage != null) throw new PostgrestRequestFailedException(_baseResponse.ErrorMessage, _baseResponse.ErrorException);
        }

        /// <summary>
        /// Throws a PostgrestErrorException if the HTTP status code of the response suggests an error. The error object from the API server is included with this exception.
        /// </summary>
        private void StatusCodeIsValid()
        {
            if (!(_baseResponse.StatusCode >= HttpStatusCode.OK && _baseResponse.StatusCode < HttpStatusCode.Ambiguous))
            {
                throw new PostgrestErrorException(JsonConvert.DeserializeObject<PostgrestError>(_baseResponse.Content));
            }
        }
    }
}
